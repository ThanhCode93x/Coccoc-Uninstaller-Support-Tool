# 🦆 Coccoc Uninstaller Support Tool

A lightweight Windows Forms tool to help users **uninstall CocCoc Browser** completely and permanently — including registry traces, services, and leftover files.

---

## ✨ Features

- ✅ Auto-detect CocCoc installation via Registry
- ✅ Uninstall CocCoc using its own `setup.exe --uninstall`
- ✅ Kill all CocCoc-related processes
- ✅ Remove all registry traces (HKCU, HKLM, MuiCache)
- ✅ Stop and delete CocCoc Windows Services
- ✅ Delete all leftover files and folders
- ✅ Remove Desktop and Start Menu shortcuts
- ✅ Runs as Administrator automatically
- ✅ Displays current OS name and architecture

---

## 🖥️ Requirements

- Windows 7 / 8 / 8.1 / 10 / 11
- x64 architecture
- No .NET runtime required (self-contained)

---

## 🚀 Usage

1. Download `Coccoc Uninstaller Support Tool.exe`
2. Run as Administrator *(the app will request it automatically)*
3. Click **Uninstall** to remove CocCoc using its own uninstaller
4. Click **Remove Remnants** to clean all leftover files and registry entries

---

## 🔧 Build from source

```bash
# Clone the repository
git clone https://github.com/yourusername/frmcocuni.git
cd frmcocuni

# Build
dotnet build -c Release

# Publish as single .exe
dotnet publish -c Release
```

Output: `bin\Release\net8.0-windows\win-x64\publish\Coccoc Uninstaller Support Tool.exe`

---

## 🛠️ Tech Stack

- **Language:** C# 12.0
- **Framework:** .NET 8 Windows Forms
- **Target:** Windows x64 (self-contained single EXE)

---

## ☕ Support

If this tool helped you, consider buying me a coffee:

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/thanhnguyen150993)

---

## 📄 License

MIT License — free to use and modify.
