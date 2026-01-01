using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Unity.Input
{
    public class InputService
    {
        private readonly Camera mainCamera;
        private readonly InputActions actions;

        public InputAction LeftClick { get; }
        public InputAction RightClick { get; }
        public InputAction MousePosition { get; }

        public InputService(Camera mainCamera)
        {
            Debug.Log($"[InputService] Created");
            this.mainCamera = mainCamera;

            actions = new InputActions();
            actions.Enable();

            LeftClick = actions.Gameplay.LeftClick;
            RightClick = actions.Gameplay.RightClick;
            MousePosition = actions.Gameplay.MousePosition;
        }

        public void Dispose()
        {
            if (actions != null)
            {
                actions.Disable();
                actions.Dispose();
            }
        }

        public Vector2 GetGridPosition(Vector2 screenPosition)
        {
            return mainCamera.ScreenToWorldPoint(screenPosition);
        }
    }
}