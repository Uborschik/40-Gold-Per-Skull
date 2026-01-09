using Cysharp.Threading.Tasks;
using Game.Combat.Application.Events;
using Game.Combat.Application.Turns;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Combat.Infrastructure.View;
using Game.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;
using Utils;

public class PlayerTurnState : ITurnState
{
    public TurnType Type => TurnType.Player;
    private readonly IEventBus events;
    private readonly CellRegistry cellRegistry;
    private readonly UnitRegistry unitRegistry;

    private UniTaskCompletionSource turnEndAwaiter;

    private Unit currentUnit;

    public PlayerTurnState(IEventBus events, CellRegistry cellRegistry, UnitRegistry unitRegistry)
    {
        this.events = events;
        this.cellRegistry = cellRegistry;
        this.unitRegistry = unitRegistry;
    }

    public UniTask Enter(Unit unit)
    {
        currentUnit = unit;
        turnEndAwaiter = new UniTaskCompletionSource();

        Debug.Log($"[PlayerTurn] Начало хода {unit.Name}");
        events.Publish(new TurnStarted(unit, Type));

        var movableArea = CalculateMovableArea(unit);
        events.Publish(new ShowMoveableAreaRequested(movableArea));

        return UniTask.CompletedTask;
    }

    public async UniTask Process()
    {
        await turnEndAwaiter.Task;

        Debug.Log("[PlayerTurn] Игрок закончил ход вручную");
    }

    public void Exit()
    {
        events.Publish(new HideMoveableAreaRequested());
        events.Publish(new TurnEnded(currentUnit, Type));
        turnEndAwaiter?.TrySetCanceled();
    }

    public void OnPlayerAction()
    {
        if (currentUnit.TrySpendAction())
        {
            Debug.Log($"[PlayerTurn] Действие выполнено. Осталось AP: {currentUnit.ActionPoints.Current}");
        }
        else
        {
            Debug.LogWarning("[PlayerTurn] Нет AP для действия!");
        }
    }

    public void OnEndTurnRequested()
    {
        Debug.Log("[PlayerTurn] Получен запрос на завершение хода");
        turnEndAwaiter?.TrySetResult();
    }

    private HashSet<Vector2Int> CalculateMovableArea(Unit unit)
    {
        var movableArea = new HashSet<Vector2Int>();
        var startPos = unit.Position.ToInt();
        float maxCost = unit.Speed;

        var queue = new Queue<(Vector2Int pos, float cost)>();
        var costs = new Dictionary<Vector2Int, float>();

        queue.Enqueue((startPos, 0f));
        costs[startPos] = 0f;

        while (queue.Count > 0)
        {
            var (currentPos, currentCost) = queue.Dequeue();

            if (currentCost >= maxCost) continue;

            foreach (var direction in Directions2D.eightDirections)
            {
                var nextPos = currentPos + direction;

                var cell = cellRegistry.GetCell(nextPos);
                if (cell == null || !cell.IsWalkable) continue;
                if (unitRegistry.TryGetUnit(nextPos, out _)) continue;

                float stepCost = (Mathf.Abs(direction.x) == 1 && Mathf.Abs(direction.y) == 1)
                    ? 1.5f
                    : 1.0f;

                float newCost = currentCost + stepCost;

                if (costs.TryGetValue(nextPos, out float existingCost) && newCost >= existingCost)
                    continue;

                costs[nextPos] = newCost;
                queue.Enqueue((nextPos, newCost));

                if (nextPos != startPos)
                {
                    movableArea.Add(nextPos);
                }
            }
        }

        return movableArea;
    }
}