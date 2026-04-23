using System;
using CoreAssembly;
using DefaultNamespace.Core.Model;
using UnityEngine;

namespace Framework.QFramework
{
    public class QFrameworkInstance : MonoBehaviour, ICanGetModel, ICanGetSystem, ICanRegisterEvent, ICanSendEvent,
        ICanGetUtility, ICanSendCommand
    {

        //
        public static IInputModel InputModel => Instance._inputModel ??= Instance.GetModel<IInputModel>();
        private IInputModel _inputModel;

        
        private static QFrameworkInstance _instance;

        public static QFrameworkInstance Instance
        {
            get
            {
                _instance = FindObjectOfType<QFrameworkInstance>();
                if (_instance == null)
                {
                    _instance = new GameObject("QFramework").AddComponent<QFrameworkInstance>();
                }

                return _instance;
            }
        }
        
        public void Init()
        {
            GetArchitecture(); // 如果没有，那就生成与注册
        }
        
        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}