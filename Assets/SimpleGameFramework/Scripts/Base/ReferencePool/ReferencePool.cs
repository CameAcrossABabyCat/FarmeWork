using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{
    /// <summary>
    /// 引用池
    /// </summary>
    public static class ReferencePool
    {
        /// <summary>
        /// 引用集合的字典
        /// </summary>
        private static Dictionary<string, ReferenceCollection> s_Reference = new Dictionary<string, ReferenceCollection>();

        /// <summary>
        /// 获取引用池的数量
        /// </summary>
        public static int Count
        {
            get
            {
                return s_Reference.Count;
            }
        }

        /// <summary>
        /// 获取引用集合
        /// </summary>
        private static ReferenceCollection GetReference(string fullName)
        {
            ReferenceCollection reference = null;

            lock (s_Reference)
            {
                if (!s_Reference.TryGetValue(fullName, out reference))
                {
                    reference = new ReferenceCollection();
                    s_Reference.Add(fullName, reference);
                }
            }
            return reference;
        }

        /// <summary>
        /// 清除所有引用集合
        /// </summary>
        public static void ClearAll()
        {
            lock (s_Reference)
            {
                foreach (var item in s_Reference)
                {
                    item.Value.RemoveAll();
                }
                s_Reference.Clear();
            }
        }

        /// <summary>
        /// 像引用集合中追加指定数量的引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="count">追加数量</param>
        public static void Add<T>(int count) where T : class, IReference, new()
        {
            GetReference(typeof(T).FullName).Add<T>(count);
        }

        /// <summary>
        /// 从引用集合中移除指定数量的引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="count">移除数量</param>
        public static void Remove<T>(int count) where T : class, IReference
        {
            GetReference(typeof(T).FullName).Remove<T>(count);
        }

        /// <summary>
        /// 从引用集合中移除所有的引用
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        public static void RemoveAll<T>() where T : class, IReference
        {
            GetReference(typeof(T).FullName).RemoveAll();
        }

        /// <summary>
        /// 从引用集合获取引用
        /// </summary>
        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReference(typeof(T).FullName).Acquire<T>();
        }

        // <summary>
        /// 从引用集合获取引用
        /// </summary>
        public static IReference Acquire(Type referenceType)
        {
            return GetReference(referenceType.FullName).Acquire(referenceType);
        }

        /// <summary>
        /// 将引用归还引用集合
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="reference">引用</param>
        public static void Release<T>(T reference) where T : class, IReference
        {
            if (reference == null)
                Debug.Log("要归还的引用为空");

            GetReference(typeof(T).FullName).Release(reference);
        }
    }
}