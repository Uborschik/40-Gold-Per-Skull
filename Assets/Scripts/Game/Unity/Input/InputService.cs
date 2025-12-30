using Game.Core.Combat.Grid;
using Game.Unity.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Unity.Input
{
    public class InputService
    {
        private readonly Camera mainCamera;
        private readonly CellRegistry cellRegistry;
        private readonly InputActions actions;

        public InputAction LeftClick { get; }
        public InputAction RightClick { get; }

        public InputService(Camera mainCamera, CellRegistry cellRegistry)
        {
            Debug.Log($"[InputService] Created");
            this.mainCamera = mainCamera;
            this.cellRegistry = cellRegistry;

            actions = new InputActions();
            actions.Enable();

            LeftClick = actions.Gameplay.LeftClick;
            RightClick = actions.Gameplay.RightClick;
        }

        public void Dispose()
        {
            if (actions != null)
            {
                actions.Disable();
                actions.Dispose();
            }
        }

        public bool TryGetGridPosition(out Position2Int position)
        {
            var screenPosition = Mouse.current.position.ReadValue();
            var gridPosition = mainCamera.ScreenToWorldPoint(screenPosition).ToPosition();

            if (cellRegistry.IsValid(gridPosition))
            {
                position = gridPosition;
                return true;
            }

            position = default;
            return false;
        }
    }
}