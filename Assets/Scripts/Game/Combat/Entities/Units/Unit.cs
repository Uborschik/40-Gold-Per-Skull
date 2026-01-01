using System;
using UnityEngine;

namespace Game.Combat.Entities.Units
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Unit : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        public Stats Stats { get; }
        public Team Team { get; private set; }
        public Vector2Int Position { get; private set; }
        public bool IsAlive { get; private set; } = true;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetTeam(Team team) => Team = team;
        public void SetPosition(Vector2Int position) => Position = position;
        public void SetIsAlive(bool isAlive) => IsAlive = isAlive;

        internal void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}