using Gamewo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamewo.Controllers;
[Route("api/[controller]")]
[ApiController]

public class UsersController : ControllerBase {
	private readonly UsersRepository _repository;

	public UsersController(IConfiguration configuration) {
		_repository = new UsersRepository(configuration.GetConnectionString("Gamewo"));
	}

#if DEBUG
	[HttpPost]
	public async Task<int> Create(Users users) {
		return await _repository.Create(users);
	}

	[HttpGet]
	public async Task<IEnumerable<Users>> GetAllUsers() {
		return await _repository.GetAllUsers();
	}

	[HttpGet("{id}")]
	public async Task<Users> GetUserById(int id) {
		return await _repository.GetUserById(id);
	}

	[HttpPut("{id}")]
	public async Task<int> UpdateUser(int id, Users users) {
		users.UserId = id;
		return await _repository.UpdateUser(users);
	}

	[HttpDelete("{id}")]
	public async Task<int> DeleteUser(int id) {
		return await _repository.DeleteUser(id);
	}
#endif

	[HttpGet("GetMostActiveUsers{id}")]
	public async Task<IEnumerable<Users>> GetMostActiveUsers(int id) {
		if (id == 0)
			id = 10;
		return await _repository.GetMostActiveUsers(id);
	}

}
