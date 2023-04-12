using System.Configuration;
using System.Data;
using System.Threading.Tasks;
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
		Users user = new Users();
		user = await _db.QueryFirstOrDefaultAsync<Users>("SELECT * FROM users WHERE userid = @UserId", tasks);
		if (user.Tokens >= tasks.Tokens) {
			user.Tokens -= tasks.Tokens;
			tasks.Finished = false;
			await _db.ExecuteAsync("UPDATE users SET tokens = @Tokens WHERE userid = @UserId", user);
			return await _db.ExecuteAsync("INSERT INTO tasks (userid, title, description, attachedfile, finished, tokens) VALUES (@UserId, @Title, @Description, @AttachedFile, @Finished, @Tokens)", tasks);
		}
		else return 0;
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
		return await _db.ExecuteAsync("DELETE FROM tasks WHERE taskid = @TaskId", new { TaskId = id });
	}
#endif


	//Other Commands
	public async Task<IEnumerable<Tasks>> GetNewestTasks(int id) {
		return await _db.QueryAsync<Tasks>("SELECT * FROM tasks ORDER BY taskid DESC LIMIT @Val", new { Val = id });
	}

	public async Task<int> SelectSolution(Tasks tasks) {
		tasks.Finished = true;
		Users users = new Users();
		users.UserId = tasks.UserId;
		users.Tokens += tasks.Tokens;
		await _db.ExecuteAsync("UPDATE users SET tokens = @Tokens WHERE userid = @UserId", users);
		return await _db.ExecuteAsync("UPDATE tasks SET finished = @Finished, solutionid = @SolutionId WHERE taskid = @TaskId", tasks);
	}

	public async Task<IEnumerable<Solutions>> GetAllSolutionsForTaskId(int id){
		return await _db.QueryAsync<Solutions>("SELECT * FROM solutions WHERE taskid = @TaskId", new { TaskId = id });
	}

}
