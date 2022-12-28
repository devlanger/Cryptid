using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cryptid.Logic;

namespace Cryptid.Shared.Logic
{
    public class GameInstanceFactory
    {
        
        public static UnitState CreateUnitState(UnitSpawnSettings settings)
        {
            var state = new UnitState();
            state.unitId = System.Guid.NewGuid().ToString();
            state.ownerId = settings.ownerId;
            state.type = settings.type;
            state.posX = settings.spawnX;
            state.posZ = settings.spawnZ;
            state.health = 10;
            state.minDmg = 1;
            state.maxDmg = 2;

            if (state.type == UnitType.MONSTER)
            {
                //if(databaseController.Manager.GetUnit(settings.baseId, out var unitData))
                //{
                    state.health = 1;//unitData.health;
                    state.minDmg = 1;//unitData.minDamage;
                    state.maxDmg = 1;//unitData.maxDamage;
                //}
            }

            return state;
        }

        public static GameState StartNewGame(GameStartSettings settings)
        {
            GameState state = new GameState();

            for (int i = 0; i < settings.Players.Count; i++)
            {
                string playerId = settings.Players[i].playerId;

                UnitState unitState = CreateUnitState(new UnitSpawnSettings()
                {
                    ownerId = playerId,
                    type = UnitType.PLAYER,
                    spawnX = (i * 2 - (settings.Players.Count / 2)),
                    spawnZ = 0
                });

                unitState.health = 10;

                state.unitStates.Add(unitState.unitId, unitState);
                state.backpacks[playerId] = new ItemsContainer();
                state.players[playerId] = new Player(playerId);
            }

            for (int i = 0; i < 5; i++)
            {
                var unitState = CreateUnitState(new UnitSpawnSettings()
                {
                    baseId = 1,
                    ownerId = "",
                    type = UnitType.MONSTER,
                    spawnX = new Random().Next(-9, 9),
                    spawnZ = new Random().Next(7, 27)
                });

                unitState.health = 3;

                state.unitStates.Add(unitState.unitId, unitState);
            }

            for (int i = 0; i < 5; i++)
            {
                var unitState = CreateUnitState(new UnitSpawnSettings()
                {
                    baseId = 1,
                    ownerId = "",
                    type = UnitType.CHEST,
                    spawnX = new Random().Next(-9, 9),
                    spawnZ = new Random().Next(7, 27)
                });

                state.unitStates.Add(unitState.unitId, unitState);
            }

            state.CurrentPlayerIndex = 0;
            state.CurrentPlayerId = state.players.Values.ToList()[0].Id;

            return state;
        }
    }
}