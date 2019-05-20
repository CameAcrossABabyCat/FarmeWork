using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{
    /// <summary>
    /// 事件池
    /// </summary>
    public class EventPool<T> where T : GlobalEventArgs
    {
        /// <summary>
        /// 事件结点
        /// </summary>
        private class Event
        {
            /// <summary>
            /// 事件发送者
            /// </summary>
            public object Sender { get; private set; }

            /// <summary>
            /// 事件参数
            /// </summary>
            public T EventArgs { get; private set; }

            public Event(object sender, T e)
            {
                Sender = sender;
                EventArgs = e;
            }
        }

        /// <summary>
        /// 事件码与对应处理方法的字典
        /// </summary>
        private Dictionary<int, EventHandler<T>> m_EventHandlers;

        /// <summary>
        /// 事件结点队列
        /// </summary>
        private Queue<Event> m_Events;


        public EventPool()
        {
            m_EventHandlers = new Dictionary<int, EventHandler<T>>();
            m_Events = new Queue<Event>();
        }


        public bool Check(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                Debug.Log("事件处理方法为空!");
                return false;
            }
            EventHandler<T> temp = null;
            if (!m_EventHandlers.TryGetValue(id, out temp) || temp == null)
                return false;
            //遍历委托里的所有方法
            foreach (EventHandler<T> i in temp.GetInvocationList())
            {
                if (i == handler)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                Debug.Log("事件方法为空,无法订阅!");
                return;
            }
            EventHandler<T> eventHander = null;
            //检查是否获取处理方法失败或获取到的为空
            if (!m_EventHandlers.TryGetValue(id, out eventHander) || eventHander == null)
            {
                m_EventHandlers[id] = handler;
            }
            else if(Check(id,handler))//不为空检查是否已经重复
            {
                Debug.Log("要订阅事件的处理方法已经存在!");
            }
            else
            {
                eventHander += handler;
                m_EventHandlers[id] = eventHander;
            }
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler">需要取消订阅的事件</param>
        public void Unsubscribe(int id,EventHandler<T> handler)
        {
            if (handler == null)
            {
                Debug.Log("事件方法为空,无法取消订阅!");
                return;
            }
            if(m_EventHandlers.ContainsKey(id))
            {
                m_EventHandlers[id] -= handler;
            }
        }

        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="sender">事件来源</param>
        /// <param name="e">事件参数</param>
        private void HandleEvent(object sender,T e)
        {
            //尝试获取事件的处理方法
            int eventId = e.Id;
            EventHandler<T> handlers = null;
            if(m_EventHandlers.TryGetValue(eventId,out handlers))
            {
                if (handlers != null)
                {
                    handlers(sender, e);
                }
                else
                    Debug.Log("事件没有对应的处理方法:" + eventId);
            }
            //向引用池归还事件引用
            ReferencePool.Release(e);
        }

        /// <summary>
        /// 事件池轮询(用于处理线程安全的事件)
        /// </summary>
        /// <param name="elapseSecounds"></param>
        /// <param name="realElapseSecounds"></param>
        public void Update(float elapseSecounds,float realElapseSecounds)
        {
            while (m_Events.Count>0)
            {
                Event e = null;
                lock (m_Events)
                {
                    e = m_Events.Dequeue();
                }
                //从封装的Event中取出事件数据并进行处理
                HandleEvent(e.Sender, e.EventArgs);
            }
        }

        /// <summary>
        /// 抛出事件(线程安全)
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void Fire(object sender,T e)
        {
            //将事件源和事件参数封装为Event加入队列
            Event eventNode = new Event(sender, e);
            lock (m_Events)
            {
                m_Events.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 抛出事件(线程不安全)
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void FireNow(object sender,T e)
        {
            HandleEvent(sender, e);
        }

        /// <summary>
        /// 清理事件
        /// </summary>
        public void Clear()
        {
            lock (m_Events)
            {
                m_Events.Clear();
            }
        }

        /// <summary>
        /// 关闭并清理事件池
        /// </summary>
        public void Shutdown()
        {
            Clear();
            m_EventHandlers.Clear();
        }
    }
}