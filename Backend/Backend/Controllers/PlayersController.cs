using Backend.Data;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private IUsersRepository _usersRepository;

        public PlayersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Player>>> GetPlayers()
        {
            return Ok(await _usersRepository.GetAllUsers());
        }

        [HttpPut]
        public async Task<ActionResult<List<Player>>> UpdatePlayer(Player player)
        {
            var dbPlayer = await _usersRepository.GetPlayer(player.Id);

            if(dbPlayer == null)
            {
                return BadRequest("Couldn't find a player with given id.");
            }

            await _usersRepository.UpdatePlayer(player);

            return Ok(await _usersRepository.GetAllUsers());
        }
    }
}