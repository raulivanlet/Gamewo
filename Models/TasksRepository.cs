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
		return await _db.ExecuteAsync("INSERT INTO tasks (taskid, userid, title, description, attachedfile, finished, tokens) VALUES (@TaskId, @UserId, @Title, @Description, @AttachedFile , @Finished, @Tokens)", tasks);
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
	public async Task<IEnumerable<Tasks>> GetNewest(int id) {
		//@Id", new { Id = id })
		//return await _db.QueryAsync<Tasks>("SELECT * FROM tasks ORDER BY id DESC LIMIT 10");
		return await _db.QueryAsync<Tasks>("SELECT * FROM tasks ORDER BY @Val DESC LIMIT 10", new { val = id });
	}

}
