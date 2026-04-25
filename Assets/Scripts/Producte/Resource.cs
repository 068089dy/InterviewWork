using System;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class Resource : MonoBehaviour
    {
        private ResourcePlacer placer;

        public float duration = 0.5f;

        private Vector3 startPos;

        private float timer;
        private bool isMoving;

        public void SetPlacer(ResourcePlacer placer)
        {
            this.placer = placer;

            startPos = transform.position;

            timer = 0f;
            isMoving = true;
        }

        private void Update()
        {
            if (!isMoving) return;
            
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            Vector3 tPos = Vector3.Lerp(startPos, placer.transform.position, t);
            transform.position = tPos;
            
            if (t >= 1f)
            {
                isMoving = false;
                transform.SetParent(placer.transform);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }

        private void OnDisable()
        {
            placer = null;
            timer = 0f;
            isMoving = false;
        }
    }
}