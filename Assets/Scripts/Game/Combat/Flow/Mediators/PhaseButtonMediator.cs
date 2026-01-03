using Game.Combat.Application.Notifications;
using Game.Combat.Flow.Commands;
using Game.Combat.Flow.Phases;
using UnityEngine.UI;

namespace Game.Combat.Flow.Mediators
{
    public class PhaseButtonMediator : INotifyPhaseChanged
    {
        private readonly Button button;
        private readonly ICommandFactory commandFactory;

        public PhaseButtonMediator(Button button, ICommandFactory commandFactory)
        {
            this.button = button;
            this.commandFactory = commandFactory;

            button.onClick.AddListener(OnClick);
        }

        private ICommand currentCommand;

        void INotifyPhaseChanged.PhaseEntered(ICombatPhase phase)
        {
            switch (phase)
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

        void INotifyPhaseChanged.PhaseExited(ICombatPhase phase)
        {
            // Опционально: можно сохранить состояние
        }

        private void OnClick()
        {
            currentCommand?.Execute();
        }

        public void Dispose()
        {
            button.onClick.RemoveListener(OnClick);
        }
    }
}