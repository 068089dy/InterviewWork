using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.SO;
using Lean.Pool;
using UnityEngine;

public class ResourceStack
{
    public ResourceType Type;
    public GameObject placer;
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

    public bool Add(ResourceType type, GameObject placer)
    {
        if (IsFull) return false;
        resources.Add(new ResourceStack()
        {
            Type = type,
            placer = placer
        });
        placer.transform.position = GetStackPosition(resources.Count-1); // 计算新资源的位置（可用于动画）
        placer.transform.localRotation = Quaternion.identity;
        return true;
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
            if (resources[i].placer != null)
            {
                resources[i].placer.transform.position = GetStackPosition(i);
            }
        }
    }
    
    public bool RemoveAndReturnPos(ResourceType type, out Vector3 pos)
    {
        pos = Vector3.one;
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].Type == type)
            {
                var stack = resources[i];
                
                if (stack.placer != null)
                {
                    pos = stack.placer.transform.position;
                    stack.placer.GetComponent<ResourcePlacer>().Recycle();
                }

                resources.RemoveAt(i);
                    
                // 重新整理堆叠位置
                RefreshPositions();

                return true;
            }
        }

        return false;
    }

    public bool Remove(ResourceType type)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].Type == type)
            {
                var stack = resources[i];
                
                if (stack.placer != null)
                {
                    stack.placer.GetComponent<ResourcePlacer>().Recycle();
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
