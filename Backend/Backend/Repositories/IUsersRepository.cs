using Backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Repositories
{
    public interface IUsersRepository
    {
        Task<List<Player>> GetAllUsers();
        Task<Player> GetPlayer(Guid id);

        /// <summary>
        /// Retrieves User Data from the database.
        /// </summary>
        /// <param name="name">User's nickname</param>
        /// <returns></returns>
        public string GetUserData(string name);
        public void GetUserData();
        Task<Player> UpdatePlayer(Player player);
    }
}
