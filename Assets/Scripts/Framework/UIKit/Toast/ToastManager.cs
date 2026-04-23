using System;
using System.Collections.Generic;
using CoreAssembly;
using Framework;
using FrameWork;
using UnityEngine;

namespace CommonCom
{
    public class ToastManager: MyMonoSingletonSingleScene<ToastManager>
    {
        // string prefabPath = UIPath.UI_Toast;
        private List<UIToast> _toasts = new List<UIToast>();
        public UIToast toastPrefab;

        public void Start()
        {
            // base.Init();
            // toastPrefab = ResMgr.Instance.LoadAsset<GameObject>(prefabPath).GetComponent<UIToast>();
        }

        public void CreateToast(string text, float duration)
        {
            try
            {
                var toast = GameObject.Instantiate(toastPrefab);
                UIRoot.Instance.AddChild(toast, UI_LAYER.TOAST);
                _toasts.Insert(0, toast);
                // _toasts.Add(toast);
                toast.Show(new UIToastData()
                {
                    text = text,
                    duration = duration
                }, delegate { _toasts.Remove(toast); });
                ToastMove(0.2f);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Toast Error:" + e.StackTrace);
            }
        }
        
        void ToastMove(float speed) {
            for (int i = 0; i < _toasts.Count; i++)
            {
                _toasts[i].MoveUp(speed,i+1);
            }
        }

        

        public void Hide()
        {
            
        }

    }
}