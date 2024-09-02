using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "InGameMenuInfo",menuName = "InGameMenuInfos")]
    public class InGameMenuInfos : ScriptableObject
    {
        public List<InGameMenuInfo> list = new List<InGameMenuInfo>();
    }
    [System.Serializable]
    public class InGameMenuInfo
    {
        public string SceneName;
        public string WorldName;
    [Multiline]
        public string WorldDescrption;
        public Sprite WorldSprite;
    }
