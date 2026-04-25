using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.SO
{
    public enum ResourceType
    {
        N1,
        N2,
        N3,
        None,
    }
    
    [CreateAssetMenu(fileName="ResConfig",menuName="配置/资源集合")]
    public class ResSOConfig: SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "ID", ValueLabel = "预设")]
        public Dictionary<ResourceType, GameObject> prefabs = new Dictionary<ResourceType, GameObject>();
    }
}