using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Backend.Logic;

namespace Backend.Data
{
    //public class GamePlayer
    //{
    //    public GamePlayer(int relationId, Guid gameId, Guid playerId, byte playerState)
    //    {
    //        RelationId = relationId;
    //        GameId = gameId;
    //        PlayerId = playerId;
    //        PlayerState = playerState;
    //    }
    //
    //    [Key]
    //    public int RelationId;
    //    public Guid GameId { get; set; }
    //    public Guid PlayerId { get; set; }
    //    public byte PlayerState { get; set; }
    //
    //
    //}
}


public enum PlayerGameState : byte
{
    INVITED = 1,
    ACCEPTED = 2
}