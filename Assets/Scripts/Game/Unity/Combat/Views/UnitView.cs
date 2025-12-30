using Game.Core.Combat.Events;
using Game.Core.Combat.Grid;
using Game.Core.Combat.Units;
using Game.Unity.Utils;
using NUnit.Framework.Internal;
using UnityEngine;
using VContainer;

namespace Game.Unity.Combat.Views
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class UnitView : MonoBehaviour
    {
        [Inject] private readonly EventStream eventStream;

        private UnitID id;
        private UnitTeam team;
        private Transform matrix;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            matrix = GetComponent<Transform>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            eventStream.Subscribe<UnitMovedEvent>(UpdatePosition);
        }

        private void OnDisable()
        {
            eventStream.Unsubscribe<UnitMovedEvent>(UpdatePosition);
        }

        public void Initialize(UnitID id, Position2Int position, UnitTeam team)
        {
            this.id = id;
            this.team = team;

            matrix.position = position.ToCenter2();
        }

        public void UpdatePosition(UnitMovedEvent e)
        {
            if (e.UnitId.Equals(id))
                matrix.position = e.To.ToCenter2();
        }
    }
}