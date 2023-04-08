using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace Gamewo.Models;

public class TasksRepository {

	private readonly IDbConnection _db;

	public TasksRepository(string connectionString) {
		_db = new MySqlConnection(connectionString);
	}

	//=============================
	//==Development Only CRUD API==
#if DEBUG
	public async Task<int> Create(Tasks tasks) {
		return await _db.ExecuteAsync("INSERT INTO tasks (title, description, attachedfile) VALUES (@Title, @Description, @AttachedFile)", tasks);
	}

	public async Task<IEnumerable<Tasks>> GetAll() {
		return await _db.QueryAsync<Tasks>("SELECT * FROM tasks");
	}

	public async Task<Tasks> GetById(int id) {
		return await _db.QueryFirstOrDefaultAsync<Tasks>("SELECT * FROM tasks WHERE taskid = @TaskId", new { TaskId = id });
	}

	public async Task<int> Update(Tasks tasks) {
		return await _db.ExecuteAsync("UPDATE tasks SET title = @Title, description = @Description, attachedfile = @AttachedFile, finished = @Finished, tokens = @Tokens WHERE taskid = @TaskId", tasks);
	}

	public async Task<int> Delete(int id) {
		return await _db.ExecuteAsync("DELETE FROM tasks WHERE taskid = @TaskId", new { Id = id });
	}
#endif


	//Other Commands
	public async Task<IEnumerable<Tasks>> GetNewestTasks(int id) {
		return await _db.QueryAsync<Tasks>("SELECT * FROM tasks ORDER BY taskid DESC LIMIT @Val", new { Val = id });
	}

	public async Task<IEnumerable<Tasks>> GetTaskSolutions(int id) {
		IEnumerable<Tasks> solid = await _db.QueryAsync<Tasks>("SELECT * FROM tasksolutions WHERE taskid = @TaskId", new { TaskId = id });
		IEnumerable<Tasks> solutions = null;

		foreach (Tasks task in solid)
			solutions.Append(await GetById(task.TaskId));

		return solutions;
	}

}
