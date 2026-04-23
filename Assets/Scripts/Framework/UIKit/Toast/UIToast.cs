using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CommonCom
{
    public class UIToastData
    {
        public string text;
        public float duration;
    }
    public class UIToast: UIBase
    {
        public CanvasGroup canvasGroup;
        public Text text;
        public void Show(UIToastData data, Action callback)
        {
            text.text = data.text;
            var tween = DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, 0.1f).OnComplete((() =>
            {
                DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, data.duration).OnComplete((() =>
                {
                    DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, 0.2f).OnComplete((() =>
                    {
                        if (callback != null)
                        {
                            callback();
                        }
                        Destroy(gameObject);
                    })).SetUpdate(true);
                })).SetUpdate(true);
            })).SetUpdate(true);
            tween.OnKill(() => tween = null).SetUpdate(true);
        }

        //堆叠向上移动
        public void MoveUp(float speed, int targetPos)
        {
            try
            {
                transform.DOLocalMoveY(targetPos * 80, speed).SetUpdate(true);
            }
            catch (Exception e)
            {
                
            }
        }
    }
}