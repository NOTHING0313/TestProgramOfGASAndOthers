using System;
using Unity.VisualScripting;

namespace GAS
{
    [Flags]
    public enum TaskStatus
    {
        None = 0,
        Unstarted = 1,
        Running = 0b10,
        Succeeded = 0b100,
        Failed = 0b1000
    }
    public static class TaskStatusExtension
    {
        /// <summary>
        /// 扩展方法，用于检测是否当前已经完成Ability
        /// </summary>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public static bool IsFinished(this TaskStatus taskStatus)
        {
            return taskStatus == TaskStatus.Succeeded || taskStatus == TaskStatus.Failed;
        }
    }
}
