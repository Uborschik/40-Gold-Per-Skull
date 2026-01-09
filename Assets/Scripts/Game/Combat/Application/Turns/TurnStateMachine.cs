using Cysharp.Threading.Tasks;
using Game.Combat.Application.Events;
using Game.Combat.Core.Entities;
using Game.Combat.Entities.Units;
using System;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;
using VContainer;

namespace Game.Combat.Application.Turns
{
    public class TurnStateMachine
    {
        private readonly IEventBus events;
        private readonly IObjectResolver resolver;

        private ITurnState currentState;
        private CancellationTokenSource cts;

        private bool isProcessing = false;

        public ITurnState CurrentState => currentState;

        public TurnStateMachine(IEventBus events, IObjectResolver resolver)
        {
            this.events = events;
            this.resolver = resolver;
        }

        public void StartTurn(Unit unit)
        {
            if (isProcessing) return;

            isProcessing = true;

            EndTurn();

            currentState = unit.Team == Team.Player
                ? resolver.Resolve<PlayerTurnState>()
                : resolver.Resolve<AITurnState>();

            cts = new CancellationTokenSource();

            RunTurnAsync(unit, cts.Token).Forget();
        }

        private async UniTask RunTurnAsync(Unit unit, CancellationToken token)
        {
            try
            {
                await currentState.Enter(unit);
                await currentState.Process();
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"[TurnStateMachine] Ход {unit.Name} отменен");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[TurnStateMachine] Ошибка: {ex}");
            }
            finally
            {
                Exit();
                isProcessing = false;
            }

            await UniTask.Yield();

            if (unit != null)
            {
                events.Publish(new TurnStateCompleted(unit));
            }
        }

        public void EndTurn()
        {
            if (cts == null) return;

            cts.Cancel();
            cts.Dispose();
            cts = null;
        }

        private void Exit()
        {
            currentState?.Exit();
            currentState = null;
        }

        public void Dispose()
        {
            EndTurn();
        }
    }
}