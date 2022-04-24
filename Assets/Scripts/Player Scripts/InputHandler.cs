using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J
{
    public class InputHandler : MonoBehaviour
    {
        PlayerInputActions playerInputActions;
        PlayerController playerController;

        Vector2 movementInput;
        Vector2 cameraInput;

        public float vertical;
        public float horizontal;
        public float moveAmount;
        public float cameraX;
        public float cameraY;
        
        public bool rb_input;
        public bool rt_input;
        
        public bool b_input;
        public bool rollFlag;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            if (playerInputActions == null)
            {
                playerInputActions = new PlayerInputActions(); 
                playerInputActions.PlayerMovement.Movement.performed += playerInputActions => movementInput = playerInputActions.ReadValue<Vector2>();
                playerInputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            playerInputActions.Enable();
        }

        private void OnDisable()
        {
            playerInputActions.Disable();
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            TickInput(delta);
        }

        private void FixedUpdate()
        {
            float fdelta = Time.fixedDeltaTime;

            if (playerController.cameraHandler != null)
            {
                playerController.DoCamera(fdelta);
            }
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            cameraX = cameraInput.x;
            cameraY = cameraInput.y;

            playerController.DoMovement(delta);
        }

        public void RollInput(float delta)
        {
            b_input = playerInputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (b_input)
            {
                rollFlag = true;
                playerController.HandleRoll();
            }
        }
  
        private void AttackInput()
        {
            playerInputActions.PlayerActions.LightAttack.performed += i => rb_input = true;
            playerInputActions.PlayerActions.HeavyAttack.performed += i => rt_input = true;

            // rb is righ hand
            if (rb_input)
            {
                playerController.HandleAttack("light");
            }

            if (rt_input)
            {
                playerController.HandleAttack("heavy");
            }
        }

        private void LateUpdate()
        {
            rb_input = false;
            rt_input = false;
        }
    }

}