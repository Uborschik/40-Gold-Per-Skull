using Game.Combat.Application.Events;
using Game.Combat.Flow.Phases;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.Systems;
using System;
using UnityEngine;

namespace Game.Combat.Flow
{
    public class CombatFlow : IDisposable
    {
        private readonly IEventBus events;
        private readonly InputSelector selector;
        private readonly TurnSystem turnSystem;
        private readonly ICombatPhase[] phases;
        private int currentIndex;

        public CombatFlow(IEventBus events, InputSelector selector, TurnSystem turnSystem, ICombatPhase[] phases)
        {
            this.events = events;
            this.selector = selector;
            this.turnSystem = turnSystem;
            this.phases = phases;
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
            if (phase is CombatPhase) turnSystem.StartNewRound();
            events.Publish(new PhaseChanged(phase, true));
        }

        private void ExitPhase(ICombatPhase phase)
        {
            phase.Exit();
            events.Publish(new PhaseChanged(phase, false));
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