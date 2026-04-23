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
        private void OnTriggerStay(Collider other)
        {
            var storage = other.GetComponent<Storage>();

            if (storage == null) return;

            // 从输出仓库拿
            if (!backpack.IsFull && !storage.IsEmpty && storage.storageType == StorageType.Output)
            {
                StartCoroutine(TakeResource(storage));
            }

            // 往输入仓库放
            if (!backpack.IsEmpty && !storage.IsFull && storage.storageType == StorageType.Input)
            {
                StartCoroutine(PutResource(storage));
            }
        }

        private bool isBusy = false;
        IEnumerator PutResource(Storage storage)
        {
            isBusy = true;

            // 1️⃣ 从背包拿一个
            GameObject cube = null;
            ResourceType resourceType = 0;
            foreach (var type in storage.consumeTypes)
            {
                cube = backpack.RemoveTop(type);
                resourceType = type;
                if (cube)
                    break;
            }
            
            if (cube == null)
            {
                isBusy = false;
                yield break;
            }

            // 2️⃣ 先从背包脱离
            cube.transform.SetParent(null);

            // 3️⃣ 目标位置（仓库下一个格子）
            Vector3 target = storage.GetNextWorldPos();

            // 4️⃣ 飞过去（角色 → 仓库）
            yield return MoveAnim.Move(
                cube.transform,
                cube.transform.position,
                target,
                0.3f
            );

            // 5️⃣ 加入仓库（并自动排队）
            yield return storage.Add(resourceType, cube);

            // 6️⃣ 可选：加一点节奏（避免太快）
            yield return new WaitForSeconds(0.1f);

            isBusy = false;
        }
        
        IEnumerator TakeResource(Storage storage)
        {
            var type = storage.Resources.First().Type;

            storage.Remove(type);

            GameObject cube = LeanPool.Spawn(resSoConfig.prefabs[type]);

            yield return MoveAnim.Move(
                cube.transform,
                storage.transform.position,
                transform.position,
                0.3f
            );

            cube.transform.rotation = transform.rotation;
            backpack.Add(type, cube);

            // 挂到角色背后（堆叠）
            //cube.transform.SetParent(backpack.transform);
        }
    }
}