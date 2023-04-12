using Gamewo.Models;
using Microsoft.AspNetCore.Authorization;
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
	public async Task<int> Update(Tasks tasks) {
		return await _repository.Update(tasks);
	}

	[HttpDelete("{id}")]
	public async Task<int> Delete(int id) {
		return await _repository.Delete(id);
	}
#endif

	//Other Commands

	[HttpGet("GetNewestTasks{id}")]
	public async Task<IEnumerable<Tasks>> GetNewestTasks(int id) {
		return await _repository.GetNewestTasks(id);
	}

	[HttpPut("ChooseSolution")]
	public async Task<int> SelectSolution(Tasks tasks) {
		return await _repository.SelectSolution(tasks);
	}


	[HttpGet("GetAllSolutionsForTaskId{id}")]
	public async Task<IEnumerable<Solutions>> GetAllSolutionsForTaskId(int id) {
		return await _repository.GetAllSolutionsForTaskId(id);
	}

}
