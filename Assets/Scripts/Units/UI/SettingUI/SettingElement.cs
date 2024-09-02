using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SettingUI
{
    public abstract class SettingElement : MonoBehaviour
    {
        public string LableName;
        public string saveKey { get; protected set; }
        public abstract void Init(string LableName);
        public abstract void Init(string LableName,Color color);
        public abstract void Init(string LableName, Color color, string SaveKey, float InitValue, float StartIndex = 0, float Multiplier = 100);
        public abstract void Save();

    }
}
