using UnityEngine;

namespace DefaultNamespace.Character
{
    [ExecuteInEditMode]
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField]
        private Transform target;

        [Header("Offset")]
        public Vector3 offset = new Vector3(0, 3, -6);

        [Header("Smooth")]
        public float followSpeed = 10f;

        
        void LateUpdate()
        {
            if (!target) return;

            float dt = Time.unscaledDeltaTime;
            Vector3 targetPos = target.position + offset;
            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                followSpeed * dt
            );
            transform.LookAt(target);
        }
    }
}