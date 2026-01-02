using System;
using UnityEngine;

namespace Game.Combat.Entities.Units
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Unit : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        public Stats Stats { get; private set; }
        public Team Team { get; private set; }
        public Vector2 Position => transform.position;
        public bool IsAlive { get; private set; } = true;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(Stats stats, Team team, Vector2 position)
        {
            Stats = stats;
            Team = team;
            transform.position = position;
        }

        public void SetTeam(Team team) => Team = team;
        public void SetPosition(Vector2 position) => transform.position = position;
        public void SetOrder(int order) => spriteRenderer.sortingOrder = order;
        public void SetIsAlive(bool isAlive) => IsAlive = isAlive;
    }
}