using Game.Combat.Application.Events;
using Game.Combat.Flow;
using Game.Combat.Flow.Commands;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Game.Combat.UI.View
{
    public class PhaseButton : MonoBehaviour
    {
        [Inject] private readonly IEventBus events;
        [Inject] private readonly ICommandFactory commandFactory;

        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI btnText;

        private Action<PhaseChanged> SetSettings;

        private ICommand currentCommand;

        private void Awake()
        {
            SetSettings = OnSetSettings;
        }

        private void OnEnable()
        {
            events.Subscribe(SetSettings);
            button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            events.Unsubscribe(SetSettings);
            button.onClick.RemoveListener(OnClick);
        }

        public void OnSetSettings(PhaseChanged evt)
        {
            switch (evt.Phase)
            {
                case Phase.Placement:
                    currentCommand = commandFactory.CreateEndPhaseCommand();
                    btnText.text = "Start Combat";
                    break;

                case Phase.Combat:
                    currentCommand = commandFactory.CreateEndTurnCommand();
                    btnText.text = "End Turn";
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