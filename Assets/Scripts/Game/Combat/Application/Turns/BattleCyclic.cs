using Cysharp.Threading.Tasks;
using Game.Combat.Application.Events;
using Game.Combat.Core.Entities;
using Game.Combat.Entities.Units;
using Game.Combat.Infrastructure.TurnOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Combat.Application.Turns
{
    public class BattleCyclic : IDisposable
    {
        private readonly IEventBus events;
        private readonly TurnQueueFactory turnQueue;
        private readonly TurnStateMachine turnStateMachine;
        private readonly UnitRegistry unitRegistry;

        private List<Unit> order = new();
        private int currentIndex;

        public Unit CurrentUnit => order[currentIndex];
        public bool IsPlayerTurn => CurrentUnit?.Team == Team.Player;

        public BattleCyclic(IEventBus events, TurnQueueFactory turnQueue, TurnStateMachine turnStateMachine, UnitRegistry unitRegistry)
        {
            this.events = events;
            this.turnQueue = turnQueue;
            this.turnStateMachine = turnStateMachine;
            this.unitRegistry = unitRegistry;

            events.Subscribe<TurnStateCompleted>(OnTurnCompleted);
        }

        public void Dispose()
        {
            events.Unsubscribe<TurnStateCompleted>(OnTurnCompleted);
        }

        public void InitializeBattle()
        {
            var aliveUnits = unitRegistry.GetAllAliveUnits().ToList();

            if (aliveUnits.Count == 0)
            {
                return;
            }

            order = turnQueue.Create(aliveUnits);
            turnStateMachine.StartTurn(CurrentUnit);
        }

        public void RequestEndPlayerTurn()
        {
            if (!IsPlayerTurn)
            {
                Debug.LogWarning("[BattleCyclic] Попытка завершить не игрока!");
                return;
            }

            if (turnStateMachine.CurrentState is PlayerTurnState playerState)
            {
                playerState.OnEndTurnRequested();
            }
        }

        private void OnTurnCompleted(TurnStateCompleted evt)
        {
            Debug.Log($"[BattleCyclic] Ход {evt.Unit.Name} завершен автоматически");

            UniTask.Void(async () =>
            {
                await UniTask.Yield();
                EndCurrentTurn();
            });
        }

        private void EndCurrentTurn()
        {
            turnStateMachine.EndTurn();

            currentIndex = (currentIndex + 1) % order.Count;

            while (!CurrentUnit.IsAlive)
            {
                currentIndex = (currentIndex + 1) % order.Count;
            }

            turnStateMachine.StartTurn(CurrentUnit);
        }

        private void DebugOrder()
        {
            foreach (var unit in order)
            {
                Debug.Log($"[CombatTurnController] {unit.Name}: {unit.Stats.Dexterity}");
            }
        }
    }
}