using System;
using System.Collections;
using System.Collections.Generic;
using CommonCom;
using DefaultNamespace.SO;
using Lean.Pool;
using UnityEngine;

namespace DefaultNamespace
{
    public class Building : MonoBehaviour
    {
        public Storage inputStorage;
        public Storage outputStorage;

        [Header("生产间隔/冷却")]
        public float produceInterval = 2f;
        private float timer;

        public ResourceType produceType;
        public List<ResourceType> consumeTypes;

        public System.Action<string> OnStopReason; // UI提示
        public ResSOConfig resSoConfig;

        private void Start()
        {
            if (inputStorage)
            {
                inputStorage.storageType = StorageType.Input;
                inputStorage.consumeTypes = consumeTypes;
            }

            if (outputStorage)
                outputStorage.storageType = StorageType.Output;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= produceInterval)
            {
                timer = 0;
                TryProduce();
            }
        }

        void TryProduce()
        {
            // 1. 输出满
            if (outputStorage.IsFull)
            {
                OnStopReason?.Invoke($"{name} 停产：输出仓库已满");
                ToastManager.Instance.CreateToast($"{name} 停产：输出仓库已满", 1f);
                return;
            }

            // 2. 输入不足
            foreach (var type in consumeTypes)
            {
                if (!inputStorage.Has(type))
                {
                    OnStopReason?.Invoke($"{name} 停产：缺少 {type}");
                    ToastManager.Instance.CreateToast($"{name} 停产：缺少 {type}", 1f);
                    return;
                }
            }

            // 3. 消耗资源
            foreach (var type in consumeTypes)
            {
                inputStorage.Remove(type);
            }

            // 4. 生产资源（带动画）
            StartCoroutine(ProduceAnimation(resSoConfig.prefabs[produceType]));
        }
        
        IEnumerator ProduceAnimation(GameObject resourcePrefab)
        {
            // 对象池生成
            GameObject cube = LeanPool.Spawn(resourcePrefab);

            Vector3 start = transform.position;
            Vector3 end = outputStorage.transform.position;

            yield return MoveAnim.Move(cube.transform, start, end, 0.5f);

            outputStorage.Add(produceType, cube);
        }
    }
    
    
}