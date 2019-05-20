using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework
{
    /// <summary>
    /// 数据结点管理器
    /// </summary>
    public class DataNodeManager : ManagerBase
    {
        private static readonly string[] s_EmptyStringArray = new string[] { };

        /// <summary>
        /// 根结点
        /// </summary>
        public DataNode Root { get; private set; }

        /// <summary>
        /// 根结点名称
        /// </summary>
        private const string RootName = "<Root>";


        public DataNodeManager()
        {
            Root = new DataNode(RootName, null);
        }

        /// <summary>
        /// 数据结点路径切分
        /// </summary>
        /// <param name="path">要切分的结点路径</param>
        /// <returns></returns>
        private static string[] GetSplitPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return s_EmptyStringArray;

            return path.Split(DataNode.s_PathSplit, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 获取数据结点
        /// </summary>
        /// <param name="path">相当于node的查找路径</param>
        /// <param name="node">查找路径的起始点</param>
        /// <returns></returns>
        public DataNode GetNode(string path, DataNode node = null)
        {
            DataNode current = node ?? Root;

            var splitPath = GetSplitPath(path);
            foreach (var child in splitPath)
            {
                current = current.GetChild(child);
                if (current == null)
                    return null;
            }
            return current;
        }

        /// <summary>
        /// 获取或添加数据结点
        /// </summary>
        /// <param name="path">相当于node的查找或添加路径</param>
        /// <param name="node">查找或添加路径的起始点</param>
        /// <returns></returns>
        public DataNode GetAndAddNode(string path, DataNode node = null)
        {
            DataNode current = node ?? Root;

            var splitPath = GetSplitPath(path);
            foreach (var child in splitPath)
            {
                current = current.GetAndAddChild(child);
            }
            return current;
        }


        public void RemoveDataNode(string path, DataNode node = null)
        {
            DataNode current = node ?? Root;
            DataNode parent = current.Parent;

            var splitPath = GetSplitPath(path);
            foreach (var child in splitPath)
            {
                parent = current;
                current = current.GetChild(child);
                if (current == null)
                    return;
            }
            if (parent != null)
                parent.RemoveChild(current.Name);
        }

        /// <summary>
        /// 根据数据类型获取数据结点的数据
        /// </summary>
        /// <typeparam name="T">要获取的数据类型</typeparam>
        /// <param name="path">要获取的路径</param>
        /// <param name="node">要获取的起点</param>
        /// <returns></returns>
        public T GetData<T>(string path, DataNode node = null)
        {
            var current = GetNode(path, node);
            if (current == null)
            {
                Debug.Log("要获取数据的结点不存在:" + path);
                return default(T);
            }

            return current.GetData<T>();
        }

        /// <summary>
        /// 设置数据结点的数据
        /// </summary>
        /// <param name="path">设置的数据路径</param>
        /// <param name="data">设置的数据</param>
        /// <param name="node">设置的数据起点</param>
        public void SetData(string path, object data, DataNode node = null)
        {
            var current = GetAndAddNode(path, node);
            current.SetData(data);
        }

        public override void Init()
        {

        }

        public override void Shutdown()
        {
            Root.Clear();
            Root = null;
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }
    }
}