using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameManager
{

    public class SingletonPlant<T> where T : class, new()
    {
        private static T _singleton;

        private static readonly object _syslock = new object();

        public static T Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new T();
                    return _singleton;
                }
                return _singleton;
            }
        }
    }
}