using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{
    /// <summary>
    /// 流程管理器
    /// </summary>
    public class ProcedureManager : ManagerBase
    {
        /// <summary>
        /// 状态机管理器
        /// </summary>
        private FsmManager m_FsmManager;

        /// <summary>
        /// 流程的状态机
        /// </summary>
        private Fsm<ProcedureManager> m_ProcedureFsm;

        /// <summary>
        /// 所有流程的列表
        /// </summary>
        private List<ProcedureBase> m_Procedures;

        /// <summary>
        /// 入口流程
        /// </summary>
        private ProcedureBase m_EntranceProcedure;

        /// <summary>
        /// 当前流程
        /// </summary>
        public ProcedureBase CurrentProcedure
        {
            get
            {
                if (m_ProcedureFsm == null)
                    Debug.Log("流程状态机为空,无法获取当前流程");
                return (ProcedureBase)m_ProcedureFsm.CurrentState;
            }
        }

        public override int Priority
        {
            get
            {
                return -10;
            }
        }

        public ProcedureManager()
        {
            m_FsmManager = FrameworkEntry.Instance.GetManager<FsmManager>();
            m_ProcedureFsm = null;
            m_Procedures = new List<ProcedureBase>();
        }

        /// <summary>
        /// 添加流程
        /// </summary>
        public void AddProcedure(ProcedureBase procedure)
        {
            if (procedure == null)
                Debug.Log("要添加的流程为空");
            m_Procedures.Add(procedure);
        }

        /// <summary>
        /// 设置入口流程
        /// </summary>
        public void SetEntranceProcedure(ProcedureBase procedure)
        {
            m_EntranceProcedure = procedure;
        }

        /// <summary>
        /// 创建流程状态机
        /// </summary>
        public void CreateProceduresFsm()
        {
            m_ProcedureFsm = m_FsmManager.CreateFsm(this, "", m_Procedures.ToArray());

            if (m_EntranceProcedure == null)
            {
                Debug.Log("入口流程为空,无法开始流程");
                return;
            }

            m_ProcedureFsm.Start(m_EntranceProcedure.GetType());
        }

        /// <summary>
        /// 初始化模块
        /// </summary>
        public override void Init()
        {

        }

        /// <summary>
        /// 停止并清理模块
        /// </summary>
        public override void Shutdown()
        {

        }

        /// <summary>
        /// 轮询模块
        /// </summary>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }
    }
}