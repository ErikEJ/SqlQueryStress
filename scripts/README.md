# Scripts 資料夾

這個資料夾包含用於 SQLQueryStress 翻譯工作的 PowerShell 腳本。

## 腳本說明

### 檢查腳本

- `check_simple.ps1` - 簡單檢查未翻譯的文字
- `check_untranslated.ps1` - 完整檢查未翻譯的文字
- `find_untranslated.ps1` - 尋找未翻譯的文字

### 替換腳本

- `replace_simple.ps1` - 替換 FormMain 的硬編碼文字
- `replace_db_select.ps1` - 替換 DatabaseSelect 的硬編碼文字
- `replace_param_window.ps1` - 替換 ParamWindow 的硬編碼文字
- `replace_data_viewer.ps1` - 替換 DataViewer 的硬編碼文字
- `replace_text.ps1` - 原始的文字替換腳本

### 更新腳本

- `update_resources.ps1` - 更新 Resources.Designer.cs
- `update_db_resources.ps1` - 更新資料庫相關資源
- `update_final_resources.ps1` - 更新最終資源
- `update_missing_resources.ps1` - 更新遺漏的資源

## 建置與編譯

### 環境需求

- .NET 8.0 SDK
- PowerShell（Windows 內建）

### 編譯專案

#### 1. 使用命令提示字元或 PowerShell

```bash
# 切換到專案根目錄
cd e:\source\SqlQueryStress

# 編譯 Debug 版本
dotnet build ./src/SQLQueryStress/SQLQueryStress.csproj --configuration Debug

# 編譯 Release 版本
dotnet build ./src/SQLQueryStress/SQLQueryStress.csproj --configuration Release
```

#### 2. 編譯輸出位置

- Debug 版本：`src\SQLQueryStress\bin\Debug\net8.0-windows\`
- Release 版本：`src\SQLQueryStress\bin\Release\net8.0-windows\`

#### 3. 執行程式

```bash
# 執行 Debug 版本
src\SQLQueryStress\bin\Debug\net8.0-windows\SQLQueryStress.exe

# 執行 Release 版本
src\SQLQueryStress\bin\Release\net8.0-windows\SQLQueryStress.exe
```

### 注意事項

1. **檔案鎖定問題**：如果程式正在執行，Release 版本編譯可能會失敗。請先關閉正在執行的 SQLQueryStress 程式。

2. **語言切換**：
   - 繁體中文系統會自動顯示中文介面
   - 其他語言系統會顯示英文介面
   - 資源檔案位置：`src\SQLQueryStress\Properties\`

3. **開發環境**：
   - 可以使用 Visual Studio 2022 打開 `SQLQueryStress.sln`
   - 或使用 Visual Studio Code

## 使用方式

```powershell
# 檢查未翻譯的文字
powershell -ExecutionPolicy Bypass -File "scripts\check_simple.ps1"

# 執行替換（如需要）
powershell -ExecutionPolicy Bypass -File "scripts\replace_simple.ps1"
```

## 重要提醒

這些腳本是用於翻譯工作的輔助工具，不需要提交到原始碼庫。
