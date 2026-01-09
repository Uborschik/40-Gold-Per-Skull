using Game.Combat.Application.Events;
using Game.Combat.Flow.Phases;
using Game.Combat.Infrastructure.Input;
using System;
using UnityEngine;

namespace Game.Combat.Flow
{
    public enum Phase { None, Placement, Combat, Result }

    public class CombatFlow : IDisposable
    {
        public Phase CurrentPhase { get; private set; }

        private readonly IEventBus events;
        private readonly InputSelector selector;
        private readonly PlacementPhase placement;
        private readonly CombatPhase combat;

        public CombatFlow(IEventBus events, InputSelector selector, PlacementPhase placement, CombatPhase combat)
        {
            CurrentPhase = 0;

            this.events = events;
            this.selector = selector;
            this.placement = placement;
            this.combat = combat;

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
            Advance();
        }

        public void Advance()
        {
            GetCurrentPhase()?.Exit();

            CurrentPhase++;

            if (CurrentPhase > Phase.Result)
                CurrentPhase = Phase.Result;
            else
            {
                GetCurrentPhase()?.Enter();
                events.Publish(new PhaseChanged(CurrentPhase));
            }
        }

        public IPassivePhase GetCurrentPhase() => CurrentPhase switch
        {
            Phase.None => null,
            Phase.Placement => placement,
            Phase.Combat => combat,
            Phase.Result => null,
            _ => throw new Exception("Invalid phase")
        };

        private void OnHover(Vector2Int pos, HighlightType type)
        {
            if (GetCurrentPhase() is IInteractivePhase interactive)
                interactive.UpdateHover(pos, type);
        }

        private void OnClick(Vector2Int pos, SelectionType type)
        {
            if (GetCurrentPhase() is IInteractivePhase interactive)
                interactive.UpdateClick(pos, type);
        }

        private void OnReset()
        {
            if (GetCurrentPhase() is IInteractivePhase interactive)
                interactive.Reset();
        }
    }
}