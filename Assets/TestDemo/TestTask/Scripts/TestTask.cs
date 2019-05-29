using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTask : TaskBase
{
    int temp = 0;
    public override int SerialId
    {
        get
        {
            return 1;
        }
    }

    public override void StartTask()
    {
        Debug.Log("-------"+temp);
        temp++;
        Done = true;
    }
}

public class TestTask2 : TaskBase
{
    int temp = 0;
    public override int SerialId
    {
        get
        {
            return 1;
        }
    }

    public override void StartTask()
    {
        Debug.Log("-------2" + temp);

        temp++;
        Done = true;
    }
}

public class TestTask1 : TaskBase
{
    int temp = 0;
    public override int SerialId
    {
        get
        {
            return 1;
        }
    }

    public override void StartTask()
    {
        Debug.Log("-------1" + temp);
        temp++;
        Done = true;
    }
}