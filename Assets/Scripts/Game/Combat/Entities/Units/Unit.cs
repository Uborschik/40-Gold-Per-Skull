using Game.Combat.Core.Entities;
using UnityEngine;

namespace Game.Combat.Entities.Units
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Unit : MonoBehaviour, IMovableObject
    {
        private SpriteRenderer spriteRenderer;

        public string Name { get; private set; }
        public Health Health { get; private set; }
        public ActionPoints ActionPoints { get; private set; }
        public int AttackRange { get; internal set; }
        public int Speed { get; private set; }
        public Stats Stats { get; private set; }
        public Team Team { get; private set; }
        public Vector2 Position => transform.position;
        public bool IsAlive { get; private set; } = true;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(string name, Health health, ActionPoints actionPoints, int attackRange, int speed, Stats stats, Team team, Vector2 position)
        {
            Name = name;
            Health = health;
            ActionPoints = actionPoints;
            AttackRange = attackRange;
            Speed = speed;
            Stats = stats;
            Team = team;
            transform.position = position;
        }

        public void TakeDamage(int damage)
        {
            Health.TakeDamage(damage);

            if (Health.IsDead)
            {
                SetIsAlive(false);
                spriteRenderer.color = Color.gray;
            }
        }

        public bool TrySpendAction() => ActionPoints.Spend();
        public void RefreshActions() => ActionPoints.Refresh();

        public void SetTeam(Team team) => Team = team;
        public void SetPosition(Vector2 position) => transform.position = position;
        public void SetOrder(int order) => spriteRenderer.sortingOrder = order;
        public void SetIsAlive(bool isAlive) => IsAlive = isAlive;
    }
}