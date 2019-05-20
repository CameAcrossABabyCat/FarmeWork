using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{
    /// <summary>
    /// 数据结点
    /// </summary>
    public class DataNode
    {
        public static readonly DataNode[] m_EmptyArry = new DataNode[] { };
        public static readonly string[] s_PathSplit = new string[] { ".", "/", "\\" };

        /// <summary>
        /// 结点名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 结点全名
        /// </summary>
        public string FullName
        {
            get
            {
                return Parent == null ? Name : string.Format("{0}{1}{2}", Parent.FullName, s_PathSplit[0], Name);
            }
        }

        /// <summary>
        /// 结点数据
        /// </summary>
        private object m_Data;

        /// <summary>
        /// 父结点
        /// </summary>
        public DataNode Parent { get; private set; }

        /// <summary>
        /// 子结点
        /// </summary>
        private List<DataNode> m_Childs;

        /// <summary>
        /// 子结点数量
        /// </summary>
        public int ChildCount
        {
            get
            {
                return m_Childs != null ? m_Childs.Count : 0;
            }
        }

        public DataNode(string name, DataNode parent)
        {
            if (!IsValidName(name))
                Debug.Log("数据结点名字不合法:" + name);

            Name = name;
            m_Data = null;
            Parent = parent;
            m_Childs = null;
        }

        /// <summary>
        /// 检测数据结点名称是否合法
        /// </summary>
        /// <param name="name">要检测的数据结点名称</param>
        /// <returns>是否是合法的数据结点名称</returns>
        private static bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            foreach (var split in s_PathSplit)
            {
                if (name.Contains(split))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 获取结点数据
        /// </summary>
        public T GetData<T>()
        {
            return (T)m_Data;
        }

        /// <summary>
        /// 设置结点数据
        /// </summary>
        public void SetData(object data)
        {
            m_Data = data;
        }

        /// <summary>
        /// 根据索引获取子数据结点
        /// </summary>
        /// <param name="index">索引值</param>
        /// <returns>指定索引的子数据结点,如果越界则返回空</returns>
        public DataNode GetChild(int index)
        {
            return index >= m_Childs.Count || index < 0 ? null : m_Childs[index];
        }

        /// <summary>
        /// 根据名称获取子数据结点
        /// </summary>
        /// <param name="name">要获取数据结点的名称</param>
        public DataNode GetChild(string name)
        {
            if (!IsValidName(name))
                return null;
            if (m_Childs == null)
                return null;

            foreach (var child in m_Childs)
            {
                if (child.Name == name)
                    return child;
            }

            return null;
        }

        /// <summary>
        /// 根据名称获取或添加数据结点,如果目标子结点存在则返回否则新建目标结点后返回
        /// </summary>
        /// <param name="name">要添加或获取的数据结点名称</param>
        /// <returns></returns>
        public DataNode GetAndAddChild(string name)
        {
            var node = GetChild(name);
            if (node != null)
                return node;
            node = new DataNode(name, this);
            if (m_Childs == null)
                m_Childs = new List<DataNode>();

            m_Childs.Add(node);
            return node;
        }

        /// <summary>
        /// 根据索引移除子数据结点
        /// </summary>
        public void RemoveChild(int index)
        {
            var node = GetChild(index);
            if (node == null)
                return;
            node.Clear();
            m_Childs.Remove(node);
        }

        /// <summary>
        /// 根据名称移除子数据结点
        /// </summary>
        public void RemoveChild(string name)
        {
            var node = GetChild(name);
            if (node == null)
                return;
            node.Clear();
            m_Childs.Remove(node);
        }

        /// <summary>
        /// 移除当前数据结点的数据和子数据结点
        /// </summary>
        public void Clear()
        {
            m_Data = null;
            if (m_Childs != null)
            {
                foreach (var child in m_Childs)
                {
                    child.Clear();
                }
                m_Childs.Clear();
            }
        }
    }
}