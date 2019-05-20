using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEventMain : MonoBehaviour {

    private void Start()
    {
        //订阅事件
        FrameworkEntry.Instance.GetManager<EventManager>().Subscribe(1, EventTestMethod);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TestEvent e = ReferencePool.Acquire<TestEvent>();

            //派发事件
            FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.Fill("EventArgs"));
        }
    }


    /// <summary>
        /// 事件处理方法
        /// </summary>
    private void EventTestMethod(object sender, GlobalEventArgs e)
    {
        TestEvent args = e as TestEvent;
        Debug.Log(args.m_Name);
    }
}
