using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[System.Serializable]
public class GameStartSettings
{
    public List<Player> Players { get; set; }

    [System.Serializable]
    public class Player
    {
        public string playerId;
    }
}
