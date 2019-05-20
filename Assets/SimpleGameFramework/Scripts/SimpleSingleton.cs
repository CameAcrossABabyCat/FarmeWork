using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{
    public abstract class SimpleSingleton<T> : MonoBehaviour where T : SimpleSingleton<T>
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.Log("场景中的单例脚本数量>1:" + _instance.GetType().ToString());
                        return _instance;
                    }
                    if (_instance == null)
                    {
                        var instanceName = typeof(T).Name;
                        GameObject instanceGo = GameObject.Find(instanceName);
                        if (instanceGo == null)
                        {
                            instanceGo = new GameObject(instanceName);
                            DontDestroyOnLoad(instanceGo);
                            _instance = instanceGo.AddComponent<T>();
                            DontDestroyOnLoad(_instance);
                        }
                        else
                        {
                            //场景中已存在同名游戏物体时就打印提示
                            Debug.Log("场景中已存在单例脚本所挂载的游戏物体:" + instanceGo.name);
                        }
                    }
                }
                return _instance;
            }
        }

        void OnDestroy()
        {
            _instance = null;
        }
    }
}