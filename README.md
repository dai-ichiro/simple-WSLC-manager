# WSLC Controller

Windows Form アプリケーションから WSLC (WSL Container) のコンテナーを起動・停止・監視するためのアプリケーションです。

## 概要

指定した WSL コンテナー（例: `llamacpp`）を起動、停止したり、稼働状態をリアルタイムでモニタリングするための GUI ツールです。内部では次の 2 つのコマンドを実行します。

```
wslc start llamacpp
wslc stop llamacpp
```

## 特長・機能

- **コンテナー状態のリアルタイム監視**: 3秒ごとに `wslc ps`（既定では実行中のコンテナーのみ表示）の出力を確認し、稼働中 (Running) / 停止中 (Stopped) のステータスを画面に表示します。
- **ワンクリックでの起動・停止**:
  - **起動**: `wslc start <ContainerName>` を実行します。
  - **停止**: `wslc stop <ContainerName>` を実行します。
- **ターミナルの表示**: チェックを入れて起動すると、`wslc start -ai <ContainerName>` を実行します。
- **アプリ終了時の自動停止**: 右上の「×」や Alt+F4 でフォームを閉じる際、`wslc stop` の完了を待ってから閉じます（最大 120 秒）。タスクマネージャーでの強制終了やクラッシュ、電源断の場合は停止されません。

## 動作環境

- **OS**: Windows 11
- **フレームワーク**: .NET Framework 4.8
- **依存環境**: WSLC (`wslc.exe`) がインストールされ、対象コンテナーが作成済みであること

## 設定ファイル (`settings.json`)

実行ファイル (`WinForm.exe`) と同じフォルダに `settings.json` を配置して動作を設定します。

```json
{
  "ContainerName": "llamacpp"
}
```

### 設定項目

- **`ContainerName`**: 操作対象のコンテナー名（例: `llamacpp`）

## 使用方法

1. `settings.json` にコンテナー名を指定します。
2. アプリケーションを実行します。
3. **「起動 (Start)」** ボタンをクリックするとコンテナーが起動します。
4. **「停止 (Stop)」** ボタンをクリックするとコンテナーが停止します。

## 注意事項

- 状態判定は `wslc ps` の最終列がコンテナー名であることを前提にしたテキスト解析です。wslc の出力形式が変わると判定が外れる可能性があります。
- 起動時の `wslc start` は 15 秒以内に完了しなくても待機を打ち切り、続けて exec を実行します。

## Building

### PowerShell

#### Debug build

```powershell
&"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" WinForm.csproj -p:Configuration=Debug
```

#### Release build

```powershell
&"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" WinForm.csproj -p:Configuration=Release
```
