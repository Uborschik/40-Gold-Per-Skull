using Game.Core.Combat.Grid;
using Game.Unity.Input;
using Game.Unity.Utils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Game.Unity.Combat.Views
{
    public enum SelectionState
    {
        Hidden,
        Active,
        Invalid
    }

    public class HoverObjectView : MonoBehaviour, IGridSelector
    {
        public event Action<Position2Int> Select;

        [Inject] private readonly InputService inputService;
        [Inject] private readonly CellRegistry cellRegistry;

        [Header("Highlight Settings")]
        [SerializeField] private Color highlightActiveColor = Color.cyan;
        [SerializeField] private Color highlightInvalidColor = Color.red;
        [SerializeField] private Transform highlightTransform;
        [SerializeField] private SpriteRenderer highlightSpriteRenderer;

        [Header("Selection Settings")]
        [SerializeField] private Color selectionActiveColor = Color.cyan;
        [SerializeField] private Color selectionInvalidColor = Color.red;
        [SerializeField] private Transform selectionTransform;
        [SerializeField] private SpriteRenderer selectionSpriteRenderer;

        public void Initialize()
        {
            Debug.Log($"[HoverObjectView] Created");

            HighlightHide();
            SelectionHide();
        }

        private void OnEnable()
        {
            inputService.LeftClick.canceled += OnGridClick;
        }

        private void OnDisable()
        {
            inputService.LeftClick.canceled -= OnGridClick;
        }

        private void FixedUpdate()
        {
            OnGridHover();
        }

        private void OnGridHover()
        {
            if (!inputService.TryGetGridPosition(out var position)) return;

            highlightTransform.position = position.ToCenter2();

            if (cellRegistry.GetCell(position).IsBlocked)
            {
                ShowInvalid();
                return;
            }

            ShowValid();
        }

        private void ShowValid()
        {
            highlightSpriteRenderer.enabled = true;
            highlightSpriteRenderer.color = Color.green;
        }

        private void ShowInvalid()
        {
            highlightSpriteRenderer.enabled = true;
            highlightSpriteRenderer.color = Color.red;
        }

        private void OnGridClick(InputAction.CallbackContext context)
        {
            if (!inputService.TryGetGridPosition(out var position)) return;

            selectionTransform.position = position.ToCenter2();

            if (cellRegistry.GetCell(position).IsWalkable)
            {
                SetSprite(SelectionState.Active);
                Select?.Invoke(position);
                return;
            }

            SetSprite(SelectionState.Invalid);
            Select?.Invoke(position);
        }

        private void SetSprite(SelectionState state)
        {
            selectionSpriteRenderer.enabled = state != SelectionState.Hidden;
            selectionSpriteRenderer.color = state == SelectionState.Active ? selectionActiveColor : selectionInvalidColor;
        }

        private void SelectionHide() => selectionSpriteRenderer.enabled = false;

        private void HighlightHide() => highlightSpriteRenderer.enabled = false;
    }
}
