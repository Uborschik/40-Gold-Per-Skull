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
        if (!unit.IsAlive || unit.Speed <= 0)
            return new HashSet<Vector2Int>();

        var startPos = unit.Position.ToInt();
        var radius = unit.Speed + 0.5f;
        var radiusSq = radius * radius;

        var top = Mathf.CeilToInt(startPos.y + radius);
        var right = Mathf.FloorToInt(startPos.x + radius);
        var bottom = Mathf.CeilToInt(startPos.y - radius);
        var left = Mathf.FloorToInt(startPos.x - radius);


        var movableArea = new HashSet<Vector2Int>();

        for (int y = bottom; y <= top; y++)
        {
            for (int x = left; x <= right; x++)
            {
                if (x == startPos.x && y == startPos.y) continue;

                var dx = x - startPos.x;
                var dy = y - startPos.y;

                if (dx * dx + dy * dy > radiusSq) continue;

                var pos = new Vector2Int(x, y);
                var cell = cellRegistry.GetCell(pos);

                if (cell == null || !cell.IsWalkable) continue;

                movableArea.Add(pos);
            }
        }

        return movableArea;
    }
}