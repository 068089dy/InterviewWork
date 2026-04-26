using System;
using System.Collections;
using System.Collections.Generic;
using CommonCom;
using DefaultNamespace.SO;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;

namespace DefaultNamespace
{
    enum StopReason
    {
        None,
        OutputFull,
        LackResource
    }
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
        public ResourcePlacer placerPrefab;
        
        private string lastStopReason = null;
        private bool isStopped = false;

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
            // 1. Full
            if (outputStorage.IsFull)
            {
                StopWithReason($"{name} Production halt: The output warehouse is full.");
                return;
            }

            // 2. Halt
            foreach (var type in consumeTypes)
            {
                if (!inputStorage.Has(type))
                {
                    StopWithReason($"{name} Production halt: Lack of {type}");
                    return;
                }
            }
            
            isStopped = false;
            lastStopReason = null;

            // 3. Input Animation
            foreach (var type in consumeTypes)
            {
                Vector3 pos1 = Vector3.zero;
                
                if (!inputStorage.RemoveAndReturnPos(type, out pos1))
                {
                    continue;
                }
                // Move Animation
                var cube = LeanPool.Spawn(resSoConfig.prefabs[type]);
                cube.transform.position = pos1;
                var duration = cube.GetComponent<Resource>().duration;
                cube.transform.DOMove(transform.position, duration).OnComplete(() =>
                {
                    LeanPool.Despawn(cube);
                });
            }

            // 4. Output Animation
            ProduceAnimation(resSoConfig.prefabs[produceType]);
        }

        void StopWithReason(string reason)
        {
            // repeated reason doesn't stack
            if (isStopped && lastStopReason == reason)
                return;

            isStopped = true;
            lastStopReason = reason;

            OnStopReason?.Invoke(reason);
            ToastManager.Instance.CreateToast(reason, 3f);
        }
        
        void ProduceAnimation(GameObject resourcePrefab)
        {
            ResourcePlacer placer = Instantiate(placerPrefab);
            GameObject cube = LeanPool.Spawn(resourcePrefab);
            cube.transform.position = transform.position;
            placer.PlaceResource(cube);

            outputStorage.Add(produceType, placer.gameObject);
        }
    }
    
    
}