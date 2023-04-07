using Gamewo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gamewo.Controllers;
[Route("api/[controller]")]
[ApiController]


public class TasksController : ControllerBase {

	private readonly TasksRepository _repository;

	public TasksController(IConfiguration configuration) {
		_repository = new TasksRepository(configuration.GetConnectionString("Gamewo"));
	}

	//=============================
	//==Development Only CRUD API==
#if DEBUG
	[HttpPost]
	public async Task<int> Create(Tasks tasks) {
		return await _repository.Create(tasks);
	}

	[HttpGet]
	public async Task<IEnumerable<Tasks>> GetAll() {
		return await _repository.GetAll();
	}

	[HttpGet("{id}")]
	public async Task<Tasks> GetById(int id) {
		return await _repository.GetById(id);
	}

	[HttpPut("{id}")]
	public async Task<int> Update(int id, Tasks tasks) {
		tasks.TaskId = id;
		return await _repository.Update(tasks);
	}

	[HttpDelete("{id}")]
	public async Task<int> Delete(int id) {
		return await _repository.Delete(id);
	}
#endif

	//Other Commands


	[HttpGet("Newest{id}")]
	public async Task<IEnumerable<Tasks>> GetNewest(int id) {
		return await _repository.GetNewest(id);
	}


}
