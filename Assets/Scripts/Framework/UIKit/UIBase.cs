using System;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace CommonCom
{

    public class UIBase: MonoBehaviour
    {
        [NonSerialized]
        public string m_ViewName;
        public Button CloseBtn;
        

        public void setViewName(string name = null)
        {
            m_ViewName = name;
        }
        public virtual void Show(object arg = null)
        {
            if (CloseBtn)
            {
                CloseBtn.onClick.AddListener(delegate
                {
                    Close();
                });
            }
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            else
            {
                // 移动到最前
                gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount-1);
            }
            OnShow(arg);
        }

        
        public virtual void Hide(object arg = null)
        {
            OnHide(arg);
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        public void Close(object arg = null)
        {
            // UIManager.Instance.Close(this, arg);
            //UIManager.Instance.Back(this, arg);
        }
        
        protected virtual void OnShow(object arg = null)
        {

        }

        protected virtual void OnHide(object arg = null)
        {

        }

        protected virtual void SetLayer(UI_LAYER layer = UI_LAYER.UI)
        {
            var layerRoot = UIRoot.Instance.FindUIRootLayer(layer);
            if (layerRoot)
            {
                transform.SetParent(layerRoot.transform);
            }
        }
        protected int order = 0;
        
        protected virtual void SetZOrder(int zOrder, UI_LAYER layer = UI_LAYER.UI)
        {
            if (transform.parent == null || transform.parent != UIRoot.Instance.FindUIRootLayer(layer).transform)
                return;
            order = zOrder;
            List<UIBase> _list = new List<UIBase>();
            if (zOrder >= transform.parent.childCount - 1)
            {
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    var child = transform.parent.GetChild(i);
                    var uiPanel = child.GetComponent<UIBase>();
                    if (uiPanel && uiPanel.GetZOrder() > zOrder)
                        _list.Add(uiPanel);
                }

                if (_list.Count > 0)
                {
                    _list.Sort((a, b) =>
                    {
                        return a.GetZOrder() - b.GetZOrder();
                    });
                    var _order = _list[0].transform.GetSiblingIndex();// - 1;
                    if(_order <= 0)
                        transform.SetSiblingIndex(_order+1);
                    else
                        transform.SetSiblingIndex(_order);

                    foreach(var p in _list)
                    {
                        p.transform.SetSiblingIndex(p.transform.GetSiblingIndex() + 1);
                    }
                    return;
                }
            }
            transform.SetSiblingIndex(zOrder);
        }
        
        protected virtual int GetZOrder()
        {
            if (order != 0)
                return order;
            else
                return transform.GetSiblingIndex();
        }
        
    }
}