using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTaskMain : MonoBehaviour
{

    private TaskAgentBase _temp;

    private TestTask _tempBase01;
    private TestTask1 _tempBase02;
    private TestTask2 _tempBase03;

    // Use this for initialization
    void Start()
    {
        _temp = new TaskAgentBase();
        _tempBase01 = new TestTask();
        _tempBase02 = new TestTask1();
        _tempBase03 = new TestTask2();
    }

    // Update is called once per frame
    void Update()
    { 
        if(Input.GetMouseButtonDown(0))
        {
            FrameworkEntry.Instance.GetManager<TaskManager>().AddAgent(_temp);
            FrameworkEntry.Instance.GetManager<TaskManager>().AddTask(_tempBase01);
            FrameworkEntry.Instance.GetManager<TaskManager>().AddTask(_tempBase02);
            FrameworkEntry.Instance.GetManager<TaskManager>().AddTask(_tempBase03);
        }
    }
}
