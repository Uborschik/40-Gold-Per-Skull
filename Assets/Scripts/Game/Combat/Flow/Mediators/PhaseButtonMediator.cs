using Game.Combat.Application.Events;
using Game.Combat.Flow.Commands;
using Game.Combat.Flow.Phases;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Combat.Flow.Mediators
{
    public class PhaseButtonMediator : IEventListener<PhaseChanged>
    {
        private readonly IEventBus events;
        private readonly Button button;
        private readonly ICommandFactory commandFactory;

        private ICommand currentCommand;

        public PhaseButtonMediator(IEventBus events, Button button, ICommandFactory commandFactory)
        {
            this.events = events;
            this.button = button;
            this.commandFactory = commandFactory;

            events.Subscribe(this);
            button.onClick.AddListener(OnClick);
        }

        public void Dispose()
        {
            events.Unsubscribe(this);
            button.onClick.RemoveListener(OnClick);
        }

        public void OnEvent(PhaseChanged evt)
        {
            switch (evt.Phase)
            {
                case PlacementPhase:
                    currentCommand = commandFactory.CreateEndPhaseCommand();
                    button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Start Combat";
                    break;

                case CombatPhase:
                    currentCommand = commandFactory.CreateEndTurnCommand();
                    button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "End Turn";
                    break;

                default:
                    currentCommand = null;
                    button.gameObject.SetActive(false);
                    break;
            }
        }

        private void OnClick()
        {
            currentCommand?.Execute();
        }
    }
}