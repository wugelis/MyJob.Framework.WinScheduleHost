using Mxic.Framework.ScheduleJob.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mxic.Framework.WinScheduleHost
{
    public class JobProcess
    {
		#region MenualResetEvent

        //當 Main thread 停止時重設的事件.
		ManualResetEvent _EventStop;

		//當 Worker thread 停止時重設的事件.
		ManualResetEvent _EventStopped;

		#endregion

		MainForm _form;

        //private JobData _job = null;
        private MethodInfo _jobMethod = null;
        private object _targetJobInstance = null;

		#region Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventStop"></param>
        /// <param name="eventStopped"></param>
        /// <param name="form">Mainform</param>
        /// <param name="JobMethod"></param>
        /// <param name="TargetJobInstance"></param>
		public JobProcess(ManualResetEvent eventStop, 
			ManualResetEvent eventStopped,
			MainForm form,
            MethodInfo JobMethod, 
            object TargetJobInstance)
		{
			_EventStop = eventStop;
			_EventStopped = eventStopped;
			_form = form;
            _jobMethod = JobMethod;
            _targetJobInstance = TargetJobInstance;
		}

		// Function runs in worker thread and emulates long process.
		public void Run()
		{
			Thread.Sleep(500);

			// 在 Window Form 產生一個非同步的 Thread 呼叫.
			_form.Invoke(_form.m_DelegateInvokeJob, new Object[] { (object)_jobMethod, _targetJobInstance });

			// 檢查 Thread 是否被停止.
			if ( _EventStop.WaitOne(0, true) )
			{
				// clean-up operations may be placed here
				// inform main thread that this thread stopped
				_EventStopped.Set();

				return;
			}

            // 當 Thread 完成後，在 Window Form 產生一個 Finished 的 Thread 呼叫.
			_form.Invoke(_form.m_DelegateThreadFinished, null);
		}
		#endregion
    }
}
