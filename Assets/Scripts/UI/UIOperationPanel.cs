using Framework.QFramework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace.UI
{
    public class UIOperationPanel : MonoBehaviour
    {
        public Joystick moveJoystick;
        
        InputAction move;

        private void Start()
        {
            
        }

        void OnEnable()
        {
            move = new InputAction(
                "Move",
                InputActionType.Value,
                "<Gamepad>/leftStick"
            );

            // 键盘 WASD
            move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");

            move.Enable();
            
        }

        void OnDisable()
        {
            move.Disable();
        }
        
        private void Update()
        {
            Vector2 input = move.ReadValue<Vector2>();
            float h = input.x;
            float v = input.y;
            if (math.abs(h) > 0 || math.abs(v) > 0)
            {
                moveJoystick.gameObject.SetActive(false);
            }
            else
            {
                moveJoystick.gameObject.SetActive(true);
                h = moveJoystick.Direction.x;
                v = moveJoystick.Direction.y;
            }
        
            QFrameworkInstance.InputModel.MoveDirection = new Vector2(h, v);
        }
    }
}