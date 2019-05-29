

namespace SimpleGameFramework
{
    /// <summary>
    /// 关于TaskBase的任务代理
    /// </summary>
    public class TaskAgentBase : ITaskAgent<TaskBase>
    {
        /// <summary>
        /// 任务
        /// </summary>
        public TaskBase Task
        {
            get
            {
                return new TaskBase();
            }
        }

        /// <summary>
        /// 初始化任务代理
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// 停止正在处理的任务并重置任务代理
        /// </summary>
        public void Reset()
        {

        }

        /// <summary>
        /// 关闭并清理任务代理
        /// </summary>
        public void Shutdown()
        {

        }

        /// <summary>
        /// 任务轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        /// <summary>
        /// 任务开始
        /// </summary>
        /// <param name="task"></param>
        public void Start(TaskBase task)
        {
            task.StartTask();
        }
    }
}