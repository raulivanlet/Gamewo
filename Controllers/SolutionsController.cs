using Gamewo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gamewo.Controllers;

[Route("api/[controller]")]
[ApiController]

public class SolutionsController : ControllerBase {
	private readonly SolutionsRepository _repository;

	public SolutionsController(IConfiguration configuration) {
		_repository = new SolutionsRepository(configuration.GetConnectionString("Gamewo"));
	}

	[HttpPost]
	public async Task<int> Create(Solutions solutions) {
		return await _repository.Create(solutions);
	}

	[HttpGet("GetNewestSolution")]
	public async Task<IEnumerable<Solutions>> GetNewest() {
		return await _repository.GetNewest(10);
	}

	[HttpGet("GetNewestSolution{id}")]
	public async Task<IEnumerable<Solutions>> GetNewest(int id) {
		if (id == 0)
			id = 10;
		return await _repository.GetNewest(id);
	}

#if DEBUG
	[HttpGet]
	public async Task<IEnumerable<Solutions>> GetAll() {
		return await _repository.GetAll();
	}

	[HttpGet("{id}")]
	public async Task<Solutions> GetById(int id) {
		return await _repository.GetById(id);
	}

	[HttpPut("{id}")]
	public async Task<int> Update(int id, Solutions solutions) {
		solutions.UserId = id;
		return await _repository.Update(solutions);
	}

	[HttpDelete("{id}")]
	public async Task<int> Delete(int id) {
		return await _repository.Delete(id);
	}
#endif
}