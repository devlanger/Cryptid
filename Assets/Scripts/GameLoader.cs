using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public GameStartSettings gameStartSettings;

    private void Awake()
    {
        StartNewGame(gameStartSettings);
    }

    public void StartNewGame(GameStartSettings settings)
    {
        GameState state = new GameState();

        for (int i = 0; i < settings.players; i++)
        {
            string playerId = System.Guid.NewGuid().ToString();

            UnitsController.Instance.SpawnUnit(new UnitSpawnSettings()
            {
                ownerId = playerId,
                type = UnitType.PLAYER,
                spawnPoint = new Vector2Int(i * 2 - (settings.players / 2), 0)
            });
            
            state.players[playerId] = new Player(playerId);
        }

        state.CurrentPlayerIndex = 0;
        state.CurrentPlayerId = state.players.Values.ToList()[0].Id;

        GameController.Instance.gameState = state;
    }

    public void LoadGame()
    {

    }
}

[System.Serializable]
public class GameStartSettings
{
    public int players = 3;
}