using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{

    /// <summary>
    /// 任务池
    /// </summary>
    /// <typeparam name="T">任务类型</typeparam>
    public class TaskPool<T> where T : ITask
    {
        /// <summary>
        /// 可用的任务代理
        /// </summary>
        private Stack<ITaskAgent<T>> m_FreeAgents;

        /// <summary>
        /// 工作中的任务代理
        /// </summary>
        private LinkedList<ITaskAgent<T>> m_WorkingAgents;

        /// <summary>
        /// 等待中的任务
        /// </summary>
        private LinkedList<T> m_WaitingTasks;

        /// <summary>
        /// 可用任务代理数量
        /// </summary>
        public int FreeAgentCount
        {
            get { return m_FreeAgents.Count; }
        }

        /// <summary>
        /// 工作中任务代理数量
        /// </summary>
        public int WorkingAgentCount
        {
            get { return m_WorkingAgents.Count; }
        }

        /// <summary>
        /// 等待中任务数量
        /// </summary>
        public int WaitingTaskCount
        {
            get { return m_WaitingTasks.Count; }
        }

        /// <summary>
        /// 任务代理总数量
        /// </summary>
        public int TotalAgentCount
        {
            get { return FreeAgentCount + WorkingAgentCount; }
        }


        public TaskPool()
        {
            m_FreeAgents = new Stack<ITaskAgent<T>>();
            m_WaitingTasks = new LinkedList<T>();
            m_WorkingAgents = new LinkedList<ITaskAgent<T>>();
        }

        /// <summary>
        /// 增加任务代理
        /// </summary>
        /// <param name="agent">要增加的任务代理</param>
        public void AddAgent(ITaskAgent<T> agent)
        {
            if (agent == null)
            {
                Debug.Log("要增加的任务代理为空!");
                return;
            }
            agent.Initialize();
            m_FreeAgents.Push(agent);
        }

        /// <summary>
        /// 增加任务
        /// </summary>
        /// <param name="task">要增加的任务</param>
        public void AddTask(T task)
        {
            if (task == null)
            {
                Debug.Log("要增加的任务为空!");
                return;
            }
            m_WaitingTasks.AddLast(task);
        }

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="serialId">要移除任务的序列编号</param>
        /// <returns>被移除的任务</returns>
        public T RemoveTask(int serialId)
        {
            foreach (var wait in m_WaitingTasks)
            {
                if (wait.SerialId == serialId)
                {
                    m_WaitingTasks.Remove(wait);
                    return wait;
                }
            }

            foreach (var work in m_WorkingAgents)
            {
                if (work.Task.SerialId == serialId)
                {
                    work.Reset();
                    m_FreeAgents.Push(work);
                    m_WorkingAgents.Remove(work);
                    return work.Task;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 移除所有任务
        /// </summary>
        public void RemoveAllTask()
        {
            m_WaitingTasks.Clear();
            //重置所有工作中的任务代理
            foreach (var working in m_WorkingAgents)
            {
                working.Reset();
                m_FreeAgents.Push(working);
            }
            m_WorkingAgents.Clear();
        }

        /// <summary>
        /// 任务池轮询
        /// </summary>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            //获取第一个工作中的任务代理
            var current = m_WorkingAgents.First;
            while (current != null)
            {
                //如果当前工作中任务代理已经完成任务
                if (current.Value.Task.Done)
                {
                    //就让它重置并从工作中任务代理中移除
                    var next = current.Next;
                    current.Value.Reset();
                    m_FreeAgents.Push(current.Value);
                    m_WorkingAgents.Remove(current);
                    current = next;
                    continue;
                }
                //未完成就轮询任务代理
                current.Value.Update(elapseSeconds, realElapseSeconds);
                current = current.Next;
            }
            //有可用的任务代理并且有等待中的任务
            while (FreeAgentCount > 0 && WaitingTaskCount > 0)
            {
                //出栈一个任务代理
                var agent = m_FreeAgents.Pop();
                //添加到工作中任务代理
                var agentNode = m_WorkingAgents.AddLast(agent);

                //获得一个等待中的任务
                T task = m_WaitingTasks.First.Value;
                m_WaitingTasks.RemoveFirst();

                //开始处理这个任务
                agent.Start(task);

                if (task.Done)
                {
                    agent.Reset();
                    m_FreeAgents.Push(agent);
                    m_WorkingAgents.Remove(agentNode);
                }
            }
        }

        /// <summary>
        /// 关闭并清理任务池
        /// </summary>
        public void Shutdown()
        {
            while (FreeAgentCount>0)
            {
                m_FreeAgents.Pop().Shutdown();
            }

            foreach (var working in m_WorkingAgents)
            {
                working.Shutdown();
            }
            m_WorkingAgents.Clear();

            m_WaitingTasks.Clear();
        }
    }
}