using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{

    public abstract class GlobalEventArgs : EventArgs, IReference
    {
        /// <summary>
        /// 事件类型ID
        /// </summary>
        public abstract int Id { get; }

        public abstract void Clear();
    }
}