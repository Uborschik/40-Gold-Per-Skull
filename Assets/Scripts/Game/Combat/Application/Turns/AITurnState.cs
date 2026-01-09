using Cysharp.Threading.Tasks;
using Game.Combat.Application.Events;
using Game.Combat.Application.Turns;
using Game.Combat.Entities.Units;
using System;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;

public class AITurnState : ITurnState
{
    public TurnType Type => TurnType.AI;
    private readonly IEventBus events;
    private Unit currentUnit;

    private CancellationTokenSource actionCts;

    public AITurnState(IEventBus events)
    {
        this.events = events;
    }

    public void Dispose()
    {
        actionCts?.Dispose();
    }

    public UniTask Enter(Unit unit)
    {
        currentUnit = unit;
        actionCts = new CancellationTokenSource();
        Debug.Log($"[AITurn] Начало хода врага {unit.Name}");
        events.Publish(new TurnStarted(unit, Type));
        return UniTask.CompletedTask;
    }

    public async UniTask Process()
    {
        while (currentUnit.TrySpendAction())
        {
            Debug.Log($"[AITurn] {currentUnit.Name} выполняет действие");

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: actionCts.Token);

            if (actionCts.Token.IsCancellationRequested) break;
        }

        Debug.Log($"[AITurn] {currentUnit.Name} закончил действия");
    }

    public void Exit()
    {
        actionCts?.Cancel();
        events.Publish(new TurnEnded(currentUnit, Type));
    }
}