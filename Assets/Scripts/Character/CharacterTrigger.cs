using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.SO;
using Lean.Pool;
using UnityEngine;

namespace DefaultNamespace.Character
{
    public class CharacterTrigger : MonoBehaviour
    {
        public Backpack backpack;
        public ResSOConfig resSoConfig;
        public ResourcePlacer placerPrefab;
        private void OnTriggerStay(Collider other)
        {
            var storage = other.GetComponent<Storage>();

            if (storage == null) return;

            // 从输出仓库拿
            if (!backpack.IsFull && !storage.IsEmpty && storage.storageType == StorageType.Output)
            {
                TakeResource(storage);
            }

            // 往输入仓库放
            if (!backpack.IsEmpty && !storage.IsFull && storage.storageType == StorageType.Input)
            {
                PutResource(storage);
            }
        }

        private float lastPutTime = -10f;
        private float putCD = 0.03f;
        void PutResource(Storage storage)
        {
            if (lastPutTime + putCD > Time.time)
                return;
            if (storage.IsFull)
                return;

            // 从背包拿一个
            Vector3 pos = Vector3.zero;
            bool find = false;
            ResourceType resourceType = ResourceType.None;
            foreach (var type in storage.consumeTypes)
            {
                if (backpack.RemoveAndReturnPos(type, out pos))
                {
                    find = true;
                    resourceType = type;
                    break;
                }
            }
            
            if (!find)
            {
                return;
            }

            // 先从背包脱离
            var placer = Instantiate(placerPrefab);
            var cube = LeanPool.Spawn(resSoConfig.prefabs[resourceType]);
            cube.transform.position = pos;
            placer.PlaceResource(cube.gameObject);
            // 加入仓库（并自动排队）
            storage.Add(resourceType, placer.gameObject);
            lastPutTime = Time.time;
        }
        
        void TakeResource(Storage storage)
        {
            var type = storage.Resources.First().Type;
            var startPos = storage.Resources.First().placer.transform.position;
            storage.Remove(type);

            GameObject cube = LeanPool.Spawn(resSoConfig.prefabs[type]);
            var placer = LeanPool.Spawn(placerPrefab);
            backpack.Add(type, placer.gameObject);
            
            cube.transform.position = startPos;
            placer.PlaceResource(cube);

        }
    }
}