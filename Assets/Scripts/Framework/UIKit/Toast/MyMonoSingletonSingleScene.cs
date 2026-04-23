﻿using CoreAssembly;
 using UnityEditor;
 using UnityEngine;

/// <summary>
/// 此单例继承于Mono，绝大多情况下，都不需要使用此单例类型。请使用Singleton
/// 不需要手动挂载
/// </summary>
 namespace Framework
 {
     public class MyMonoSingletonSingleScene<T> : MonoBehaviour where T : MonoBehaviour
     {
         private static T _instance;

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
         protected static bool isGolbal = false;

         static MyMonoSingletonSingleScene()
         { 
             ApplicationIsQuitting = false;
         }

         public static T Instance
         {
             get
             {
                 if (ApplicationIsQuitting)
                 {
                     if (Debug.isDebugBuild)
                     {
                         Debug.LogWarning("[MySingleton] " + typeof(T) +
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
                         _instance = (T) FindObjectOfType(typeof(T));

                         if (FindObjectsOfType(typeof(T)).Length > 1)
                         {
                             if (Debug.isDebugBuild)
                             {
                                 Debug.LogWarning("[MySingleton] " + typeof(T).Name +
                                                  " should never be more than 1 in scene!");
                             }

                             return _instance;
                         }

                         // 场景中找不到就创建新物体挂载
                         if (_instance == null)
                         {
                             Debug.Log("未找到单例对象，正在创建");
                             // GameObject singletonObj = new GameObject();
                             // _instance = singletonObj.AddComponent<T>();
                             // singletonObj.name = typeof(T).Name;
                             //
                             // if (isGolbal && Application.isPlaying)
                             // {
                             //     //DontDestroyOnLoad(singletonObj);
                             // }

                             return _instance;
                         }
                         else
                         {
                             //DontDestroyOnLoad(_instance.gameObject);
                         }
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
         
         // ========== 新增：编辑器退出PlayMode时强制重置静态实例 ==========
#if UNITY_EDITOR
         private void OnEnable()
         {
             // 注册PlayMode状态回调
             EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
         }

         private void OnDisable()
         {
             // 注销回调，防止内存泄漏
             EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
         }

         private void OnPlayModeStateChanged(PlayModeStateChange state)
         {
             // 退出PlayMode时，强制把静态实例置为null
             if (state == PlayModeStateChange.ExitingPlayMode)
             {
                 _instance = null;
                 Debug.Log("CoroutineManager 静态实例已重置为null");
             }
         }
#endif
     }
 }

