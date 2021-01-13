namespace ExpandableButtons.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public static class TaskHelper
    {
        #region Exceptions

        public static Action<Exception> TraceException { get; set; }

        private static void HandlerException(Exception ex, bool isSilent = false, string callerMemberName = null)
        {
            if (ex is AggregateException)
            {
                if (!isSilent)
                {
                    DoInApplicationThread(() =>
                    {
                        Debug.WriteLine("Exception call by :" + callerMemberName);
                        if (ex.InnerException != null)
                        {
                            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                        }
                        else
                        {
                            throw ex;
                        }
                    });
                }
                else
                {
                    TraceException?.Invoke(ex);
                }
            }
            else
            {
                if (ex != null)
                {
                    if (!isSilent)
                    {
                        DoInApplicationThread(() => { throw ex; });
                    }
                    else
                    {
                        TraceException?.Invoke(ex);
                    }
                }
            }
        }

        #endregion

        #region Run

        public static void DoInApplicationThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        public static async Task DoInApplicationThreadAsync(Action action)
        {
            await Device.InvokeOnMainThreadAsync(action);
        }

        public static async Task DoInApplicationThreadAsync(Func<Task> funcTask)
        {
            await Task.Yield();
            DoInApplicationThread(async () => await funcTask());
        }


        public static void RunWithoutAwaitIntoAnotherThread(
            Func<Task> function,
            bool isSilentException = false,
            Action continueWithAction = null)
        {
            var task = Task.Run(function).ContinueWith(t =>
            {
                if (t.IsFaulted && !t.IsCanceled && t.Exception != null)
                {
                    HandlerException(t.Exception, isSilentException);
                }

                if (continueWithAction != null)
                {
                    DoInApplicationThread(continueWithAction);
                }
            }).ConfigureAwait(true);
        }

        public static void ExecuteWithoutAwait(
            this Task task,
            bool isSilentException = false,
            Action continueWithAction = null,
            [CallerMemberName] string callerMemberName = null)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted && !t.IsCanceled && t.Exception != null)
                {
                    HandlerException(t.Exception, isSilentException, callerMemberName);
                }

                if (continueWithAction != null)
                {
                    DoInApplicationThread(continueWithAction);
                }
            }).ConfigureAwait(true);
        }

        public static void ExecuteWithoutAwait(
            this Task task,
            Action<bool, Exception> continueWithAction,
            bool isSilentException = false)
        {
            task.ContinueWith(t =>
            {
                bool success = true;
                if (t.IsFaulted && !t.IsCanceled && t.Exception != null)
                {
                    success = false;
                    HandlerException(t.Exception, isSilentException);
                }

                if (continueWithAction != null)
                {
                    DoInApplicationThread(() => continueWithAction(success, t.Exception));
                }
            }).ConfigureAwait(true);
        }

        #endregion

        #region Run Dispose

        public static async Task ExecuteAndCheckIfDisposed(
            this Task task,
            Func<bool> isDisposed,
            string objectName = null,
            bool checkResult = true)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                if (isDisposed())
                {
                    throw new ObjectDisposedException(objectName, ex);
                }
                else
                {
                    throw;
                }
            }
            if (checkResult && isDisposed())
            {
                throw new ObjectDisposedException(objectName);
            }
        }

        public static async Task<T> ExecuteAndCheckIfDisposed<T>(
            this Task<T> task,
            Func<bool> isDisposed,
            string objectName = null,
            bool checkResult = true)
        {
            var result = default(T);
            try
            {
                result = await task;
            }
            catch (Exception ex)
            {
                if (isDisposed())
                {
                    throw new ObjectDisposedException(objectName, ex);
                }
                else
                {
                    throw;
                }
            }

            if (checkResult && isDisposed())
            {
                throw new ObjectDisposedException(objectName);
            }

            return result;
        }

        #endregion
    }
}
