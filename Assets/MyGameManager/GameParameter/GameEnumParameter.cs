using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameManager
{
    /// <summary>
    /// 游戏的流程枚举
    /// </summary>
    public enum GameProcedure
    {
        None,
        Start,
        Waiting,
        Playing,
        End,
    }
}