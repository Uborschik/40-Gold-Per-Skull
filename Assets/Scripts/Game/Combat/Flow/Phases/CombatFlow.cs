using Game.Combat.Entities.Selector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Combat.Phases
{
    public class CombatFlow : IDisposable
    {
        private readonly Selector selector;
        private readonly Button exitPhaseBtn;
        private readonly ICombatPhase[] phases;
        private int currentIndex;

        public CombatFlow(Selector selector, Button exitPhaseBtn, PlacementPhase placement, CombatPhase combat)
        {
            this.selector = selector;
            this.exitPhaseBtn = exitPhaseBtn;
            phases = new ICombatPhase[] { placement, combat };
            currentIndex = 0;

            selector.Hover += HoverTick;
            selector.Click += ClickTick;
            selector.Reset += OnReset;

            exitPhaseBtn.onClick.AddListener(Exit);
        }

        public void Dispose()
        {
            selector.Hover -= HoverTick;
            selector.Click -= ClickTick;
            selector.Reset += OnReset;

            exitPhaseBtn.onClick.RemoveAllListeners();
        }

        public void Start()
        {
            while (currentIndex < phases.Length && phases[currentIndex].IsComplete)
                currentIndex++;

            if (currentIndex < phases.Length)
                phases[currentIndex].Enter();
        }

        public void HoverTick(Vector2Int position, HighlightType type)
        {
            if (TryGetCurrentPhase(out var phase))
            {
                phase.UpdateHover(position, type);
            }
        }

        public void ClickTick(Vector2Int position, SelectionType type)
        {
            if (TryGetCurrentPhase(out var phase))
            {
                phase.UpdateClick(position, type);
            }
        }

        public void OnReset()
        {
            if (TryGetCurrentPhase(out var phase))
            {
                phase.Reset();
            }
        }

        private void Exit()
        {
            if (TryGetCurrentPhase(out var phase))
            {
                exitPhaseBtn.gameObject.SetActive(false);
                if (TryAdvancePhase(out var nextPhase))
                {
                    nextPhase.Enter();
                }
            }
        }

        private bool TryGetCurrentPhase(out ICombatPhase phase)
        {
            if (currentIndex >= phases.Length)
            {
                phase = null;
                return false;
            }

            phase = phases[currentIndex];

            if (!phase.IsComplete)
                return true;

            if (!TryAdvancePhase(out phase))
            {
                return false;
            }

            phase.Enter();
            return true;
        }

        private bool TryAdvancePhase(out ICombatPhase nextPhase)
        {
            if (currentIndex < phases.Length)
            {
                phases[currentIndex].Exit();
            }

            currentIndex++;

            if (currentIndex >= phases.Length)
            {
                nextPhase = null;
                return false;
            }

            nextPhase = phases[currentIndex];
            return true;
        }
    }
}