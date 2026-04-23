using System;
using System.Collections.Generic;
using CoreAssembly;
using UnityEngine;
using UnityEngine.UI;

namespace CommonCom
{
    public enum UI_LAYER : int
    {
        HUD = 0,
        UI,
        TIP,
        TOAST
    }
    public class UIRoot: MonoBehaviour
    {
        private List<GameObject> UILayer;

        public GameObject prepareGo;
        // public static UIRoot FindViewRoot()
        // {
        //     if (m_ViewRoot == null)
        //     {
        //         GameObject root = GameObject.Find("UIRoot");
        //         if (root != null)
        //         {
        //             m_ViewRoot = root.GetComponent<UIRoot>();
        //             return m_ViewRoot;
        //         }
        //         return null;
        //     }
        //     else
        //     {
        //         return m_ViewRoot;
        //     }
        // }
        
        public void Prepare(bool value)
        {
            if (prepareGo)
            {
                prepareGo.SetActive(value);
            }
        }


        public void AddChild(UIBase child, UI_LAYER layer = UI_LAYER.UI)
        {
            GameObject root = FindUIRootLayer(layer);
            // UIRoot root = FindViewRoot();
            if(root !=null && child != null)
            {
                child.transform.SetParent(root.transform, false);
            }
        }
        
        public GameObject FindUIRootLayer(UI_LAYER layer)
        {
            int i = (int)layer;
            if(UILayer != null && UILayer.Count > i)
            {
                return UILayer[i];
            }
            else
            {
                return null;
            }
        }
        
        private void Awake()
        {
            UILayer = new List<GameObject>();
            // 初始化UI层级
            foreach (int value in Enum.GetValues(typeof(UI_LAYER)))
            {
                GameObject obj = new GameObject();
                obj.name = Enum.GetName(typeof(UI_LAYER), value);
                obj.transform.SetParent(transform);
                RectTransform uiRect = obj.AddComponent<RectTransform>();
                uiRect.pivot = new Vector2(0.5f, 0.5f);
                uiRect.anchorMin = new Vector2(0, 0);
                uiRect.anchorMax = new Vector2(1, 1);
                uiRect.localScale = Vector3.one;
                uiRect.localPosition = Vector3.zero; // 注意PosZ也要设置，否则有可能会不显示
                uiRect.offsetMax = new Vector2(0, 0);
                uiRect.offsetMin = new Vector2(0, 0);
                UILayer.Add(obj);
            }

            if (prepareGo)
            {
                prepareGo.transform.SetAsLastSibling();
            }
        }

        #region MonoSingleton
        /*
         * 单例类
         */
        private static UIRoot _instance;

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// 程序是否正在退出
        /// </summary>
        protected static bool ApplicationIsQuitting { get; private set; }

        /// <summary>
        /// 是否为全局单例
        /// </summary>
        protected static bool isGolbal = true;

        static UIRoot()
        {
            ApplicationIsQuitting = false;
        }

        public static UIRoot Instance
        {
            get
            {
                if (ApplicationIsQuitting)
                {
                    if (Debug.isDebugBuild)
                    {
                        Debug.LogWarning("[MySingleton] " + typeof(UIRoot) +
                                         " already destroyed on application quit." +
                                         " Won't create again - returning null.");
                    }

                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // 先在场景中找寻
                        _instance = (UIRoot) FindObjectOfType(typeof(UIRoot));

                        if (FindObjectsOfType(typeof(UIRoot)).Length > 1)
                        {
                            if (Debug.isDebugBuild)
                            {
                                Debug.LogWarning("[MySingleton] " + typeof(UIRoot).Name +
                                                  " should never be more than 1 in scene!");
                            }

                            return _instance;
                        }

                        // 场景中找不到就创建新物体挂载
                        // if (_instance == null)
                        // {
                        //     var singletonObj = GameObject.Instantiate(ResMgr.Instance.LoadAsset<GameObject>(UIPath.UI_Root));
                        //     //GameObject singletonObj = new GameObject();
                        //     _instance = singletonObj.AddComponent<UIRoot>();
                        //     // singletonObj.name = typeof(UIRoot).Name;
                        //
                        //     if (isGolbal && Application.isPlaying)
                        //     {
                        //         DontDestroyOnLoad(_instance);
                        //     }
                        //
                        //     return _instance;
                        // }
                    }

                    return _instance;
                }
            }
        }

        /// <summary>
        /// 当工程运行结束，在退出时，不允许访问单例
        /// </summary>
        public void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }
        #endregion
    }
}