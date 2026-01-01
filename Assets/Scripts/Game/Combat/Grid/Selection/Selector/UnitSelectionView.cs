using UnityEngine;

namespace Game.Unity.Combat.Views
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class UnitSelectionView : MonoBehaviour
    {
        private Transform thisTransform;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            thisTransform = transform;
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer not found on " + gameObject.name, gameObject);
                return;
            }

            Debug.Log($"[UnitSelectionView] Created");
        }

        private void Start()
        {
            Hide();
        }

        public void Set(Vector2 position)
        {
            Show();
            thisTransform.position = position;
        }

        public void Show() => spriteRenderer.enabled = true;

        public void Hide() => spriteRenderer.enabled = false;
    }
}