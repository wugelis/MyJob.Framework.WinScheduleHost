# MyJob.Framework.WinScheduleHost

## 什麼是 MyJob ScheduleJob Services？
提供排程方式執行用 C# 撰寫的類別庫 DLL 的服務，並提供 Windows UI 介面設定排程執行的時間。

## 設計目標
以Win32服務基礎來開發，希望可提供可程式化界面，只要使用 Visual Studio 的類別庫專案以 C# 繼承 AbstractProcess 類別，並使用 Template Method 的設計模式，實作排程服務的方法，並編譯成 DLL 後新增至 MyJob ScheduleJob UI介面，設定好執行的時間，排程便能自動在設定的時間下運行。

## 詳細需求說明
ScheduleJob Service須以Win32服務方式運行，須具備與Windows 排程服務相同的功能，提供以：(月、週、日)三種方式設定排程執行的時間，且這三種方式都要可以設定週期性(反覆執行)、非週期性(一次性工作)的執行方式。
![alt tag](http://i.imgur.com/twuFqWR.jpg)

開發人員撰寫程式的方式只需要建立類別庫專案，並安裝預先包裝好的 NuGet Package 套件，即會自動安裝需要 References 的元件，與範本 CS 檔案，開發人員只需要實作BeforePrepare()、PrepareData()、ProcessData()、AfterProcess()..等方法，編譯成DLL複製到 ScheduleJob 的執行目錄下，或是提供一個介面供加入新的排程，加入後，自動將 DLL 複製到 MyJob Schedule Services 的執行目錄即可。

因此開發人員只需要專注在 ScheduleJob 要處理的商務邏輯，其他如錯誤處理 (Exception Handler)、紀錄(Log)、重試機制(ReTry)，等等，ScheduleJob Framewok 底層會自動處理，因此開發人員不必實作這個部分。

另外 MyJob ScheduleJob 必須提供一 Window UI 介面，讓ScheduleJob的管理員可在UI介面上新增、修改、刪除所有的ScheduleJob排程服務，並提供『手動執行』排程的功能，以便當有需要手動執行某些工作時可以使用。
![alt tag](http://i.imgur.com/sfhUWF1.jpg)

另外，MyJob ScheduleJob Services 也提供監控機制，提供 ScheduleJob 管理員可以輕易查看目前排程執行的狀況，正在執行的排程有幾個，閒置的排程有多少個，是否有執行過久的排程等等。
![alt tag](http://i.imgur.com/3mwIgKe.jpg)
