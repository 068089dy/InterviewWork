using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.SO;
using Lean.Pool;
using UnityEngine;

namespace DefaultNamespace.Character
{
    public class Backpack : MonoBehaviour
    {
        public int capacity = 10;
        // private List<ResourceType> items = new();
        // private List<GameObject> cubes = new();
        public int rowCount = 1;
        public int colCount = 3;
        public float spacing = 0.25f;
        public float height = 0.25f;

        private List<ResourceStack> items = new();
        public bool IsFull => items.Count >= capacity;
        public bool IsEmpty => items.Count <= 0;

        public bool Add(ResourceType type, GameObject cube)
        {
            if (IsFull) return false;
            items.Add(new ResourceStack()
            {
                Type = type,
                cube = cube
            });

            int index = items.Count - 1;
            Vector3 targetLocal = GetLocalStackPos(index);
            cube.transform.SetParent(transform);
            cube.transform.localPosition = targetLocal;
            cube.transform.localRotation = Quaternion.identity;
            
            return true;
        }
        
        Vector3 GetLocalStackPos(int index)
        {
            int layer = index / (rowCount*colCount);
            int inner = index % (rowCount*colCount);

            int row = inner / colCount;
            int col = inner % colCount;

            float centerOffset = (4 - 1) * spacing * 0.5f;

            return new Vector3(
                col * spacing ,
                layer * height,
                row * spacing 
            );
        }

        public bool Remove(ResourceType type)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Type == type)
                {
                    var stack = items[i];
                
                    if (stack.cube != null)
                    {
                        LeanPool.Despawn(stack.cube);
                    }

                    items.RemoveAt(i);

                    // 重新整理堆叠位置
                    Rearrange();

                    return true;
                }
            }

            return false;
        }
        
        public GameObject RemoveTop(ResourceType type)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Type == type)
                {
                    var stack = items[i];
                
                    // if (stack.cube != null)
                    // {
                    //     LeanPool.Despawn(stack.cube);
                    // }

                    

                    items.RemoveAt(i);

                    // 重新整理堆叠位置
                    Rearrange();

                    return stack.cube;
                }
            }

            return null;
        }

        // public List<ResourceType> GetAll() => items;
        
        void Rearrange()
        {
            for (int i = 0; i < items.Count; i++)
            {
                Vector3 target = GetLocalStackPos(i);

                StartCoroutine(MoveLocal(items[i].cube.transform, target, 0.2f));
            }
        }
        
        IEnumerator MoveLocal(Transform t, Vector3 target, float time)
        {
            Vector3 start = t.localPosition;

            float tVal = 0;
            while (tVal < 1)
            {
                tVal += Time.deltaTime / time;
                t.localPosition = Vector3.Lerp(start, target, tVal);
                yield return null;
            }

            t.localPosition = target;
        }
    }
    
    
}