using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[System.Serializable]
public class GameStartSettings
{
    public bool IsOnline = false;
    public List<Player> Players { get; set; }

    [System.Serializable]
    public class Player
    {
        public string playerId;
    }
}
