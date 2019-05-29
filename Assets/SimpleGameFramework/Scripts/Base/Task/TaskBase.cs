using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{
    /// <summary>
    /// TaskBase任务
    /// </summary>
    public class TaskBase : ITask
    {

        public TaskBase()
        {

        }

        /// <summary>
        /// 任务Id,根据Id来判断移除任务
        /// </summary>
        public virtual int SerialId
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 任务是否完成,如果为false,不进行下面的任务
        /// </summary>
        public bool Done { get; protected set; }

        /// <summary>
        /// 任务开始的调用方法
        /// </summary>
        public virtual void StartTask()
        {
            Debug.Log("++++++");
        }

        /// <summary>
        /// 当任务被移除时调用
        /// </summary>
        public virtual void OnRemove()
        {
            Debug.Log(this.GetType().FullName+"被移除了");
        }
    }
}