using LitJson;
using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MyGameManager
{

    public class GameData : SingletonPlant<GameData>
    {
        public JsonData GameJsonData = null;

        public DataNodeManager DataManager;

        public GameData()
        {
            DataManager = FrameworkEntry.Instance.GetManager<DataNodeManager>();
        }
    }
}