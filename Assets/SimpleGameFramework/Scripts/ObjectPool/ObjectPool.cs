using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SimpleGameFramework
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool<T> : IObjectPool where T : ObjectBase
    {
        /// <summary>
        /// 释放对象筛选方法
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="candidateObjects">要筛选的对象集合</param>
        /// <param name="toReleaseCount">需要释放的对象数量</param>
        /// <param name="expireTime">对象过期参考时间</param>
        /// <returns>经筛选需要释放的对象集合</returns>
        public delegate LinkedList<T> ReleaseObjectFilterCallback<T>(LinkedList<T> candidateObjects, int toReleaseCount, DateTime expireTime) where T : ObjectBase;

        /// <summary>
        /// 对象池容量
        /// </summary>
        private int m_Capacity;

        /// <summary>
        /// 对象池对象过期时间
        /// </summary>
        private float m_ExpireTime;

        /// <summary>
        /// 对象名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 池对象的链表
        /// </summary>
        private LinkedList<ObjectBase> m_Objects;

        /// <summary>
        /// 池对象的类型
        /// </summary>
        public Type ObjectType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// 池对象的数量
        /// </summary>
        public int Count
        {
            get { return m_Objects.Count; }
        }

        /// <summary>
        /// 池对象是否可被多次获取
        /// </summary>
        public bool AllowMultiSpawn { get; private set; }

        /// <summary>
        /// 可释放的对象数量
        /// </summary>
        public int CanReleaseCount
        {
            get
            {
                return GetCanReleaseObjects().Count;
            }
        }

        /// <summary>
        /// 对象池自动释放可释放对象计时
        /// </summary>
        public float AutoReleaseTime { get; private set; }

        /// <summary>
        /// 对象池自动释放可释放对象的间隔时间
        /// </summary>
        public float AutoReleaseInterval { get; set; }

        /// <summary>
        /// 对象池容量
        /// </summary>
        public int Capacity
        {
            get
            {
                return m_Capacity;
            }
            set
            {
                if (value < 0)
                    Debug.Log("设置对象池容量小于0,无法设置!");
                if (m_Capacity == value)
                {
                    return;
                }
                m_Capacity = value;
            }
        }

        /// <summary>
        /// 池对象过期秒数
        /// </summary>
        public float ExpireTime
        {
            get
            {
                return m_ExpireTime;
            }
            set
            {
                if (value < 0)
                    Debug.Log("设置对象过期时间小于0,无法设置!");
                if (m_ExpireTime == value)
                    return;
                m_ExpireTime = value;
            }
        }


        public ObjectPool(string name, int capacity, float expireTime, bool allowMultiSpawn)
        {
            Name = name;
            m_Objects = new LinkedList<ObjectBase>();
            Capacity = capacity;
            AutoReleaseInterval = expireTime;
            ExpireTime = expireTime;
            AutoReleaseTime = 0;
            AllowMultiSpawn = allowMultiSpawn;
        }

        /// <summary>
        /// 检查对象
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <returns>要检查的对象是否存在</returns>
        public bool CanSpawn(string name)
        {
            foreach (var item in m_Objects)
            {
                if (item.Name != name)
                    continue;
                if (AllowMultiSpawn || !item.IsUseing)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 注册对象
        /// </summary>
        /// <param name="obj">要注册的对象</param>
        /// <param name="spawned">对象是否已被获取</param>
        public void Register(T obj, bool spawned = false)
        {
            if (obj == null)
            {
                Debug.Log("要放入对象池的对象为空:" + typeof(T).FullName);
                return;
            }
            //已被获取就让计数+1
            if (spawned)
            {
                obj.SpawnCount++;
            }
            m_Objects.AddLast(obj);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <returns>要获取的对象</returns>
        public T Spawn(string name = "")
        {
            foreach (var obj in m_Objects)
            {
                if (obj.Name != name)
                    continue;

                if (AllowMultiSpawn || !obj.IsUseing)
                {
                    Debug.Log("获取了对象" + typeof(T).FullName + "/" + obj.Name);
                    return obj.Spawn() as T;
                }
            }
            return null;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Unspawn(ObjectBase obj)
        {
            Unspawn(obj.Target);
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Unspawn(object target)
        {
            if (target == null)
                Debug.Log("需要回收的对象为空:" + typeof(object).FullName);

            foreach (var obj in m_Objects)
            {
                if (obj.Target == target)
                {
                    obj.Unspawn();
                    Debug.Log("对象被回收了:" + typeof(T).FullName + "/" + obj.Name);
                    return;
                }
            }
            Debug.Log("找不到要回收的对象:" + typeof(T).FullName);
        }

        /// <summary>
        /// 获取所有可以释放的对象列表
        /// </summary>
        private LinkedList<T> GetCanReleaseObjects()
        {
            LinkedList<T> canReleases = new LinkedList<T>();

            foreach (var obj in m_Objects)
            {
                if (obj.IsUseing)
                    continue;

                canReleases.AddLast(obj as T);
            }

            return canReleases;
        }

        /// <summary>
        /// 释放对象池中的可释放对象
        /// </summary>
        /// <param name="toReleaseCound">尝试可释放的对象数量</param>
        /// <param name="releaseObjectFilterCallback">释放对象筛选方法</param>
        public void Release(int toReleaseCound, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
        {
            //重置计时
            AutoReleaseTime = 0;
            if (toReleaseCound <= 0)
            {
                return;
            }
            //计算对象过期参考时间
            DateTime expireTime = DateTime.MinValue;
            if (m_ExpireTime < float.MinValue)
            {
                //过期参考时间=当前时间-过期秒数
                expireTime = DateTime.Now.AddSeconds(-m_ExpireTime);
            }
            //获取可释放的对象和实际要释放的对象
            LinkedList<T> canReleaseObjects = GetCanReleaseObjects();
            LinkedList<T> toReleaseObjects = releaseObjectFilterCallback(canReleaseObjects, toReleaseCound, expireTime);
            if (toReleaseObjects == null || toReleaseObjects.Count < -0)
            {
                return;
            }
            foreach (var obj in toReleaseObjects)
            {
                if (obj == null)
                    Debug.Log("无法释放空对象!");
                foreach (var item in m_Objects)
                {
                    if (item != obj)
                        continue;
                    m_Objects.Remove(obj);
                    obj.Release();
                    Debug.Log("对象被释放了:" + obj.Name);
                    break;
                }
            }
        }

        /// <summary>
        /// 默认的释放对象筛选方法(未被使用且过期的对象)
        /// </summary>
        private LinkedList<T> DefaultReleaseObjectFillterCallBack(LinkedList<T> candidateObjects, int toReleaseCount, DateTime expireTime)
        {
            var toReleaseObjects = new LinkedList<T>();
            if (expireTime > DateTime.MaxValue)
            {
                LinkedListNode<T> current = candidateObjects.First;
                while (current != null)
                {
                    if (current.Value.LastUseTime <= expireTime)
                    {
                        toReleaseObjects.AddLast(current.Value);
                        LinkedListNode<T> next = current.Next;
                        candidateObjects.Remove(current);
                        toReleaseCount--;
                        if (toReleaseCount <= 0)
                        {
                            return toReleaseObjects;
                        }
                        current = next;
                        continue;
                    }
                    current = current.Next;
                }
            }
            return toReleaseObjects;
        }

        /// <summary>
        /// 释放超出对象池容量的可释放对象
        /// </summary>
        public void Release()
        {
            Release(m_Objects.Count - m_Capacity, DefaultReleaseObjectFillterCallBack);
        }

        /// <summary>
        /// 释放指定数量的可释放对象
        /// </summary>
        /// <param name="toReleaseCount"></param>
        public void Release(int toReleaseCount)
        {
            Release(toReleaseCount, DefaultReleaseObjectFillterCallBack);
        }

        /// <summary>
        /// 释放对象池中所有为使用对象
        /// </summary>
        public void ReleaseAllUnused()
        {
            LinkedListNode<ObjectBase> current = m_Objects.First;
            while (current != null)
            {
                if (current.Value.IsUseing)
                {
                    current = current.Next;
                    continue;
                }
                LinkedListNode<ObjectBase> next = current.Next;
                m_Objects.Remove(current);
                current.Value.Release();
                Debug.Log("对象被释放了:" + current.Value.Name);
                current = next;
            }
        }

        /// <summary>
        /// 对象池的定时释放
        /// </summary>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            AutoReleaseTime += realElapseSeconds;
            if (AutoReleaseTime < AutoReleaseInterval)
                return;
            Release();
        }

        /// <summary>
        /// 清理对象池
        /// </summary>
        public void Shutdowm()
        {
            var current = m_Objects.First;

            while (current != null)
            {
                var next = current.Next;
                m_Objects.Remove(current);
                current.Value.Release();
                Debug.Log("对象被释放了:" + current.Value.Name);
                current = next;
            }
        }
    }
}