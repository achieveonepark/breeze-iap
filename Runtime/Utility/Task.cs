using System;
using System.Threading.Tasks;

namespace Achieve.BreezeIAP
{
    internal static class TaskExtension
    {
        public static async Task<T> Timeout<T>(this Task<T> task, TimeSpan time)
        {
            Task delayTask = Task.Delay(time);
            Task firstToFinish = await Task.WhenAny(task, delayTask);

            if (firstToFinish == delayTask)
            {
                throw new TimeoutException("The operation has timed out.");
            }

            return await task;
        }

        public static async Task Timeout(this Task task, TimeSpan time)
        {
            Task delayTask = Task.Delay(time);
            Task firstToFinish = await Task.WhenAny(task, delayTask);

            if (firstToFinish == delayTask)
            {
                throw new TimeoutException("The operation has timed out.");
            }

            await task;
        }
    }
}
