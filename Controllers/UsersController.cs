using Gamewo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gamewo.Controllers;
[Route("api/[controller]")]
[ApiController]

public class UsersController : ControllerBase {
	private readonly UsersRepository _repository;

	public UsersController(IConfiguration configuration) {
		_repository = new UsersRepository(configuration.GetConnectionString("Gamewo"));
	}

	//=============================
	//==Development Only CRUD API==
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


	//Other Commands

	[HttpPost("AccountCreate")]
	public async Task<string> AccountCreate(Users users) {
		return await _repository.AccountCreate(users);
	}


	[HttpPost("AccountLogin")]
	public async Task<string> AccountLogin(Users users) {
		return await _repository.AccountLogin(users);
	}

}
