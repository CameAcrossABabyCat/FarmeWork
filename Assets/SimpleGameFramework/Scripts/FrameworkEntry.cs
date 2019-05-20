using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{
    /// <summary>
    /// 框架入口,维护所有模块管理器
    /// </summary>
    public class FrameworkEntry : SimpleSingleton<FrameworkEntry>
    {
        /// <summary>
        /// 所以模块管理器的链表
        /// </summary>
        private LinkedList<ManagerBase> m_Managers = new LinkedList<ManagerBase>();


        public T GetManager<T>() where T : ManagerBase
        {
            Type managerType = typeof(T);
            foreach (var item in m_Managers)
            {
                if (item.GetType() == managerType)
                {
                    return item as T;
                }
            }
            return CreateManager(managerType) as T;
        }

        public ManagerBase CreateManager(Type managerType)
        {
            ManagerBase manager = (ManagerBase)Activator.CreateInstance(managerType);
            if (manager == null)
            {
                Debug.Log("模块管理器创建失败:" + manager.GetType().FullName);
            }

            LinkedListNode<ManagerBase> current = m_Managers.First;
            while (current != null)
            {
                if (manager.Priority > current.Value.Priority)
                    break;
                current = current.Next;
            }
            if (current != null)
            {
                m_Managers.AddBefore(current, manager);
            }
            else
                m_Managers.AddLast(manager);

            manager.Init();
            return manager;
        }

        private void Update()
        {
            foreach (var item in m_Managers)
            {
                item.Update(Time.deltaTime, Time.unscaledDeltaTime);
            }
        }

        private void OnDestroy()
        {
            //关闭并清理所有管理器
            for (LinkedListNode<ManagerBase> current = m_Managers.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }
            m_Managers.Clear();
        }
    }
}