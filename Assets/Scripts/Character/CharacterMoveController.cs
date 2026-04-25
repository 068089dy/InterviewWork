using DefaultNamespace.Core.Model;
using Framework.QFramework;
using UnityEngine;

namespace DefaultNamespace.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMoveController : MonoBehaviour
    {
        
        [Header("Move")]
        public float moveSpeed = 5f;

        public float rotateSpeed = 5f;

        [Header("Gravity")]
        public float gravity = -9.8f;
        public float groundStickForce = -2f;

        private CharacterController controller;
        private Vector3 velocity;

        private IInputModel _inputModel;

        private bool _canMove = true;
        void Awake()
        {
            controller = GetComponent<CharacterController>();
            _inputModel = QFrameworkInstance.InputModel;
        }

        void Update()
        {
            Move();
            ApplyGravity();
        }

        public void Move(Vector3 motion)
        {
            controller.Move(motion);
        }

        void Move()
        {
            if (!_canMove)
                return;
            float h = _inputModel.MoveDirection.x;
            float v = _inputModel.MoveDirection.y;
            Vector3 moveDir = new Vector3(h, 0, v);

            if (moveDir.sqrMagnitude > 0.0001f)
            {
                // 面向移动方向
                transform.forward = Vector3.Slerp(transform.forward,moveDir.normalized, Time.deltaTime*rotateSpeed);
            }

            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }

        void ApplyGravity()
        {
            if (controller.isGrounded)
            {
                if (velocity.y < 0)
                    velocity.y = groundStickForce; // 贴地
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }

            controller.Move(velocity * Time.deltaTime);
        }

        public void SetCanMove(bool value)
        {
            _canMove = value;
        }

        
    }
}