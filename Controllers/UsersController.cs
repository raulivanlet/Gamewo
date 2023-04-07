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
	public async Task<IEnumerable<Users>> GetAll() {
		return await _repository.GetAll();
	}

	[HttpGet("{id}")]
	public async Task<Users> GetById(int id) {
		return await _repository.GetById(id);
	}

	[HttpPut("{id}")]
	public async Task<int> Update(int id, Users users) {
		users.UserId = id;
		return await _repository.Update(users);
	}

	[HttpDelete("{id}")]
	public async Task<int> Delete(int id) {
		return await _repository.Delete(id);
	}
#endif


	//Other Commands



}
