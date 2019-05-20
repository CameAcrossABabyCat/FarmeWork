using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IObjectPool
    {
        /// <summary>
        /// 对象池名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 对象池对象类型
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// 对象池中对象的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 对象池中能被释放的对象的数量
        /// </summary>
        int CanReleaseCount { get; }

        /// <summary>
        /// 对象池自动释放可释放对象的间隔时间(隔几秒进行自动释放)
        /// </summary>
        float AutoReleaseInterval { get; set; }

        /// <summary>
        /// 对象池的容量
        /// </summary>
        int Capacity { get; set; }

        /// <summary>
        /// 对象池对象过期秒数(被回收几秒视为过期,需要被释放)
        /// </summary>
        float ExpireTime { get; set; }

        /// <summary>
        /// 释放超出对象池容量的可释放对象
        /// </summary>
        void Release();

        /// <summary>
        /// 释放指定数量的可释放对象
        /// </summary>
        /// <param name="toReleaseCount">藏尸释放对象的数量</param>
        void Release(int toReleaseCount);

        /// <summary>
        /// 释放对象池中所有未使用的对象
        /// </summary>
        void ReleaseAllUnused();

        /// <summary>
        /// 轮询对象
        /// </summary>
        void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 清理并关闭对象池
        /// </summary>
        void Shutdowm();
    }
}