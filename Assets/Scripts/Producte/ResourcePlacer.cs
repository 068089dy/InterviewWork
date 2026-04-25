using System;
using Lean.Pool;
using UnityEngine;

namespace DefaultNamespace
{
    public class ResourcePlacer : MonoBehaviour
    {
        public Resource Resource=> resource;
        private Resource resource;
        public void PlaceResource(GameObject resourceObj)
        {
            resource = resourceObj.GetComponent<Resource>();
            if (resource != null)
            {
                resource.SetPlacer(this);
            }
        }

        public void Recycle()
        {
            if (resource != null)
            {
                resource.transform.SetParent(null);
                LeanPool.Despawn(resource);
                resource = null;
            }
            LeanPool.Despawn(gameObject);
        }
    }
}