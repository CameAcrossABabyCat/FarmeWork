using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{

    public class TaskManager : ManagerBase
    {
        /// <summary>
        /// 任务池
        /// </summary>
        private TaskPool<TaskBase> m_TaskPool;

        public TaskManager()
        {
            m_TaskPool = new TaskPool<TaskBase>();
        }


        public override int Priority
        {
            get
            {
                return 110;
            }
        }

        /// <summary>
        /// 增加任务代理
        /// </summary>
        /// <param name="agent"></param>
        public void AddAgent(TaskAgentBase agent)
        {
            m_TaskPool.AddAgent(agent);
        }

        /// <summary>
        /// 增加任务
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(TaskBase task)
        {
            m_TaskPool.AddTask(task);
        }

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public ITask RemoveTask(int serialId)
        {
            return m_TaskPool.RemoveTask(serialId);
        }

        /// <summary>
        /// 移除所有任务
        /// </summary>
        public void RemoveAllTask()
        {
            m_TaskPool.RemoveAllTask();
        }

        public override void Init()
        {

        }

        public override void Shutdown()
        {
            m_TaskPool.Shutdown();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            m_TaskPool.Update(elapseSeconds, realElapseSeconds);
        }
    }
}