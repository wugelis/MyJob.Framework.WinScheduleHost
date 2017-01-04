# MyJob.Framework.WinScheduleHost

## 什麼是MyJob  ScheduleJob Services？
提供排程方式執行用C#撰寫的類別庫DLL的服務，並提供Windows UI介面設定排程執行的時間。

## 設計目標
以Win32服務基礎來開發，希望可提供可程式化界面，只要使用Visual Studio 2013的類別庫專案以C#繼承AbstractProcess類別，並使用Template Method的設計模式，實作排程服務的方法，並編譯成DLL後新增至ScheduleJob UI介面，設定好執行的時間，排程便能自動在設定的時間下運行。
