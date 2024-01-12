using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.Utilities
{
    public static class Logger
    {
        public static void WriteTask(Task task, string processName)
        {
            Write(task.IsCompletedSuccessfully, processName, task.Exception?.Message);
        }

        public static void WriteTask(UniTask<bool> uniTask, string processName)
        {
            Write(uniTask.Status == UniTaskStatus.Succeeded, processName, "UniTask get wrong.");
        }

        private static void Write(bool isSucceed, string processName, string exception = "")
        {
            string message = $"[{processName}] ";

            if (!isSucceed)
            {
                message += $"Failed, because: {exception}";
                message = message.GetColoredString(Color.red);
            }
            else
            {
                message += "Succeed!";
                message = message.GetColoredString(Color.green);
            }

            Debug.Log(message);
        }
    }
}