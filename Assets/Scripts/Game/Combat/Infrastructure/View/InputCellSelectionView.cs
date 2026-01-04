using Game.Combat.Core.Entities;
using Game.Combat.Infrastructure.Input;
using UnityEngine;

namespace Game.Combat.Infrastructure.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InputCellSelectionView : MonoBehaviour, IMovableObject
    {
        [SerializeField] private Color allyUnitColor = Color.white;
        [SerializeField] private Color enemyUnitColor = Color.orange;
        [SerializeField] private Color activeColor = Color.cyan;
        [SerializeField] private Color invalidColor = Color.red;

        private Transform thisTransform;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            thisTransform = transform;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Hide();
        }

        public void Set(Vector2 position, SelectionType type)
        {
            Show();
            SetPosition(position);

            switch (type)
            {
                case SelectionType.AllyUnit:
                    spriteRenderer.color = allyUnitColor;
                    break;
                case SelectionType.EnemyUnit:
                    spriteRenderer.color = enemyUnitColor;
                    break;
                case SelectionType.Free:
                    spriteRenderer.color = activeColor;
                    break;
                case SelectionType.Blocked:
                    spriteRenderer.color = invalidColor;
                    break;
            }
        }
        public void SetPosition(Vector2 position) => thisTransform.position = position;

        public void Show() => spriteRenderer.enabled = true;

        public void Hide() => spriteRenderer.enabled = false;
    }
}