using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.SO;
using Lean.Pool;
using UnityEngine;

public class ResourceStack
{
    public ResourceType Type;
    public GameObject cube;
}
public enum StorageType
{
    Input,
    Output
}
public class Storage : MonoBehaviour
{
    public StorageType storageType;
    public List<ResourceType> consumeTypes;
    [Header("容量")]
    public int capacity;
    [Header("方块间距")]
    public float spacing = 0.15f;
    [Header("每层高度")]
    public float layerHeight = 0.15f;
    public List<ResourceStack> Resources => resources;
    private List<ResourceStack> resources = new();

    public bool IsFull => resources.Count >= capacity;
    public bool IsEmpty => resources.Count == 0;

    public bool Add(ResourceType type, GameObject resCube)
    {
        if (IsFull) return false;
        resources.Add(new ResourceStack()
        {
            Type = type,
            cube = resCube
        });
        resCube.transform.position = GetStackPosition(resources.Count-1); // 计算新资源的位置（可用于动画）
        resCube.transform.localRotation = transform.rotation;
        return true;
    }
    
    public Vector3 GetNextWorldPos()
    {
        int index = resources.Count-1;

        return GetStackPosition(index);
    }
    
    public Vector3 GetStackPosition(int index)
    {
        int layer = index / 16;
        int innerIndex = index % 16;

        int row = innerIndex / 4;
        int col = innerIndex % 4;
        float centerOffset = (4 - 1) * spacing * 0.5f;

        Vector3 offset = new Vector3(
            col * spacing - centerOffset,
            layer * layerHeight,
            row * spacing - centerOffset
        );

        return transform.position + offset;
    }
    
    private void RefreshPositions()
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].cube != null)
            {
                resources[i].cube.transform.position = GetStackPosition(i);
            }
        }
    }

    public bool Remove(ResourceType type)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].Type == type)
            {
                var stack = resources[i];
                
                if (stack.cube != null)
                {
                    LeanPool.Despawn(stack.cube);
                }

                resources.RemoveAt(i);

                // 重新整理堆叠位置
                RefreshPositions();

                return true;
            }
        }

        return false;
    }

    public bool Has(ResourceType type, int count = 1)
    {
        return resources.Count(r => r.Type == type) >= count;
    }
    
    
    
}
