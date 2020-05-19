using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Animations.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="task">Task.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute.</param>
        public static void Forget(this Task task, Action<Exception> onException = null)
        {
            task.ContinueWith(x =>
            {
                if (x.Exception != null)
                {
                    var exception = x.Exception.InnerException ?? x.Exception;
                    onException?.Invoke(exception);
                    Debug.WriteLine(exception.ToString());
                }
            },
            TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
