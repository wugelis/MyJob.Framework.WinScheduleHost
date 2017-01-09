# MyJob.Framework.WinScheduleHost

## 什麼是 MyJob ScheduleJob Services？
提供排程方式執行用 C# 撰寫的類別庫 DLL 的服務，並提供 Windows UI 介面設定排程執行的時間。

## 設計目標
以Win32服務基礎來開發，希望可提供可程式化界面，只要使用 Visual Studio 的類別庫專案以 C# 繼承 AbstractProcess 類別，並使用 Template Method 的設計模式，實作排程服務的方法，並編譯成 DLL 後新增至 MyJob ScheduleJob UI介面，設定好執行的時間，排程便能自動在設定的時間下運行。

## 詳細需求說明
MyJob ScheduleJob Service 須以 Win32 服務方式運行，須具備與Windows 排程服務相同的功能，提供以：(月、週、日)三種方式設定排程執行的時間，且這三種方式都要可以設定週期性(反覆執行)、非週期性(一次性工作)的執行方式。
![alt tag](http://i.imgur.com/twuFqWR.jpg)

開發人員撰寫程式的方式只需要建立類別庫專案，並安裝預先包裝好的 NuGet Package 套件，即會自動安裝需要 References 的元件，與範本 CS 檔案，開發人員只需要實作BeforePrepare()、PrepareData()、ProcessData()、AfterProcess()..等方法，編譯成DLL複製到 ScheduleJob 的執行目錄下，或是提供一個介面供加入新的排程，加入後，自動將 DLL 複製到 MyJob Schedule Services 的執行目錄即可。

因此開發人員只需要專注在 ScheduleJob 要處理的商務邏輯，其他如錯誤處理 (Exception Handler)、紀錄(Log)、重試機制(ReTry)，等等，ScheduleJob Framewok 底層會自動處理，因此開發人員不必實作這個部分。

另外 MyJob ScheduleJob 必須提供一 Window UI 介面，讓ScheduleJob的管理員可在UI介面上新增、修改、刪除所有的ScheduleJob排程服務，並提供『手動執行』排程的功能，以便當有需要手動執行某些工作時可以使用。
![alt tag](http://i.imgur.com/aYHhbEV.jpg)

另外，MyJob ScheduleJob Services 也提供監控機制，提供 ScheduleJob 管理員可以輕易查看目前排程執行的狀況，正在執行的排程有幾個，閒置的排程有多少個，是否有執行過久的排程等等。
![alt tag](http://i.imgur.com/3mwIgKe.jpg)

## MyJob ScheduleJob Services 整個運作示意圖如下：
![alt tag](http://i.imgur.com/ofIRTDy.jpg)

## 各個元件的詳細說明
1.	MyJob.Framework.ScheduleHostService
主要ScheduleJob的Win32 Service，它也是發動執行Job主要的執行者。

2.	MyJob.Framework.ScheduleJob.Engine
為 ScheduleJob 的主要執行引擎，包含了主要Job的MainThread主執行緒、輪循機制，不斷詢問 MyJob.Framework.ScheduleJob.Store 是否有符合條件、可執行的Job。

一旦有Job被執行，系統會自動在MainThread中再起一個WorkerThread的背景執行緒以執行該Job。

本Engine也提供ReTry機制，也就是說，一旦服務執行失敗，Engine會自動進行ReTry，若ReTry 3次還是執行失敗便會放棄，ScheduleJob服務本身會記錄自身的LOG，此時維運人員可經由LOG查看原因，並加以排除。

本Engine也會在記憶體中維護一個RunningTable，每當有Job被執行，就可在 RunningTable 的 RunningJobStatuss 屬性中查詢該JobId的Status(執行狀態)

Job 的執行狀態有下列三種：	
● Idel (服務閒置狀態)

●	Starting (服務啟動中)

●	Running (服務執行中)


3.	MyJob.Framework.ScheduleJob.Core
為ScheduleJob的核心處理部分，包括：動態讀取DLL、讀取DLL內部的Class與動態執行DLL內部方法的實作均定義在此元件中。

4.	MyJob.Framework.ScheduleJob.Store
定義JobData儲存體、封裝了Job相關資料的存放方式，提供Engine的RunningJob讀取出有效可執行的Job。

5.	MyJob.Framework.ScheduleJobBase
定義Job DLL開發實作的方式、樣板。

6.	MyJob.Framework.WinScheduleUI
提供視覺化設定、新增、調整ScheduleJob的UI工具。
