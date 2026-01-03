using Game.Combat.Application.Notifications;
using Game.Combat.Flow.Phases;
using Game.Combat.Infrastructure.Input;
using System;
using UnityEngine;

namespace Game.Combat.Flow
{
    public class CombatFlow : IDisposable
    {
        private readonly InputSelector selector;
        private readonly ICombatPhase[] phases;
        private readonly INotifyPhaseChanged[] phaseListeners;
        private int currentIndex;

        public CombatFlow(
            InputSelector selector,
            ICombatPhase[] phases,
            INotifyPhaseChanged[] phaseListeners)
        {
            this.selector = selector;
            this.phases = phases;
            this.phaseListeners = phaseListeners;
            currentIndex = 0;

            selector.Hover += OnHover;
            selector.Click += OnClick;
            selector.Reset += OnReset;
        }

        public void Dispose()
        {
            selector.Hover -= OnHover;
            selector.Click -= OnClick;
            selector.Reset -= OnReset;
        }

        public void Start()
        {
            if (phases.Length == 0) return;

            currentIndex = 0;
            EnterPhase(phases[0]);
        }

        private void EnterPhase(ICombatPhase phase)
        {
            phase.Enter();
            foreach (var listener in phaseListeners)
                listener.PhaseEntered(phase);
        }

        private void ExitPhase(ICombatPhase phase)
        {
            phase.Exit();
            foreach (var listener in phaseListeners)
                listener.PhaseExited(phase);
        }

        private void OnHover(Vector2Int position, HighlightType type)
        {
            if (TryGetCurrentPhase(out var phase))
                phase.UpdateHover(position, type);
        }

        private void OnClick(Vector2Int position, SelectionType type)
        {
            if (TryGetCurrentPhase(out var phase))
                phase.UpdateClick(position, type);
        }

        private void OnReset()
        {
            if (TryGetCurrentPhase(out var phase))
                phase.Reset();
        }

        private bool TryGetCurrentPhase(out ICombatPhase phase)
        {
            phase = currentIndex < phases.Length ? phases[currentIndex] : null;
            return phase != null;
        }

        public void AdvancePhase()
        {
            if (!TryGetCurrentPhase(out var currentPhase)) return;

            ExitPhase(currentPhase);
            currentIndex++;

            if (TryGetCurrentPhase(out var nextPhase))
                EnterPhase(nextPhase);
        }
    }
}