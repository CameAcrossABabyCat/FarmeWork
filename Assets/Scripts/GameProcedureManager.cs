using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleGameFramework;
using MyGameManager;

/// <summary>
/// 游戏流程管理
/// </summary>
public class GameProcedureManager : MonoBehaviour
{
    private void Start()
    {
        //获得流程管理器
        var procedure = FrameworkEntry.Instance.GetManager<ProcedureManager>();
        OnGameWait gameWait = new OnGameWait();
        //注册流程
        procedure.AddProcedure(gameWait);
        //将start设为开始流程
        procedure.SetEntranceProcedure(gameWait);

        procedure.AddProcedure(new OnGameStart());
        procedure.AddProcedure(new OnGamePlay());
        procedure.AddProcedure(new OnGameEnd());
        //创建流程状态机
        procedure.CreateProceduresFsm();
    }
}