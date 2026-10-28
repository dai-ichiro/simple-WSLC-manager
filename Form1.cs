using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm
{
    public partial class Form1 : Form
    {
        private const string DefaultContainerName = "magnitude2";
        private const int PollIntervalMs = 3000;
        private const int StopTimeoutMs = 30000;

        private readonly string _containerName;
        private readonly Timer _pollTimer = new Timer { Interval = PollIntervalMs };
        private Process _attached;
        private bool _polling;

        public Form1()
        {
            InitializeComponent();
            _containerName = LoadContainerName();
            Text = _containerName;

            _pollTimer.Tick += async (s, e) => await PollAsync();
            _pollTimer.Start();
            Load += async (s, e) => await PollAsync();
        }

        // settings.json (実行ファイルと同じフォルダ) から ContainerName を読み込む
        private static string LoadContainerName()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
                if (File.Exists(path))
                {
                    var m = Regex.Match(File.ReadAllText(path),
                        @"""ContainerName""\s*:\s*""(?<name>[^""]+)""");
                    if (m.Success) return m.Groups["name"].Value;
                }
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            return DefaultContainerName;
        }

        private async Task PollAsync()
        {
            if (_polling) return;
            _polling = true;
            try
            {
                bool? running = await Task.Run(() => QueryRunning());
                if (running.HasValue && !IsDisposed) SetState(running.Value);
            }
            finally
            {
                _polling = false;
            }
        }

        // wslc ps の出力にコンテナ名が含まれていれば起動中。取得できなければ null
        private bool? QueryRunning()
        {
            try
            {
                var psi = new ProcessStartInfo("wslc", "ps")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                using (var proc = Process.Start(psi))
                {
                    var errTask = proc.StandardError.ReadToEndAsync();
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    if (proc.ExitCode != 0) return null;
                    return output.Split('\n').Any(line =>
                        Regex.Split(line.Trim(), @"\s+").Contains(_containerName));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            bool terminal = checkTerminal.Checked;
            string args = terminal ? "start -ai " + _containerName : "start " + _containerName;

            try
            {
                var psi = new ProcessStartInfo("wslc", args)
                {
                    UseShellExecute = terminal,
                    CreateNoWindow = !terminal,
                    WindowStyle = terminal ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden
                };

                if (terminal)
                {
                    // アタッチ起動: ターミナルが閉じる/コンテナが終了するまでプロセスが残る
                    _attached = Process.Start(psi);
                    SetState(true);
                }
                else
                {
                    using (var proc = Process.Start(psi))
                    {
                        proc.WaitForExit();
                        if (proc.ExitCode != 0)
                        {
                            MessageBox.Show(this, "コンテナの起動に失敗しました。", Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    SetState(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (StopContainer())
            {
                SetState(false);
            }
            else
            {
                MessageBox.Show(this, "コンテナの停止に失敗しました。", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // wslc stop を実行して終了を待つ。成功すれば true
        private bool StopContainer()
        {
            try
            {
                var psi = new ProcessStartInfo("wslc", "stop " + _containerName)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var proc = Process.Start(psi))
                {
                    if (!proc.WaitForExit(StopTimeoutMs))
                    {
                        try { proc.Kill(); } catch (InvalidOperationException) { }
                        return false;
                    }
                    return proc.ExitCode == 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 「×」ボタン等で閉じるとき、状態表示に関わらず確実にコンテナを停止する
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.Cancel) return;

            _pollTimer.Stop();
            Cursor = Cursors.WaitCursor;
            Enabled = false;

            bool stopped = StopContainer();

            if (_attached != null)
            {
                try { if (!_attached.HasExited) _attached.Kill(); }
                catch (Exception) { }
                _attached.Dispose();
                _attached = null;
            }

            if (!stopped && QueryRunning() == true)
            {
                var result = MessageBox.Show(this,
                    "コンテナの停止に失敗しました。このまま終了しますか?", Text,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    Enabled = true;
                    Cursor = Cursors.Default;
                    _pollTimer.Start();
                    e.Cancel = true;
                }
            }
        }

        private void SetState(bool running)
        {
            labelStatus.Text = running ? "起動中 (Running)" : "停止中 (Stopped)";
            labelStatus.ForeColor = running ? Color.Green : SystemColors.GrayText;
            buttonStart.Enabled = !running;
            buttonStop.Enabled = running;
            checkTerminal.Enabled = !running;
        }
    }
}
