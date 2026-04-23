using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class MoveAnim
    {
        public static IEnumerator Move(Transform obj, Vector3 start, Vector3 end, float time)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / time;
                obj.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
            obj.position = end;
        }
    }
}