using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace Gamewo.Models;

public class SolutionsRepository {

	private readonly IDbConnection _db;

	public SolutionsRepository(string connectionString) {
		_db = new MySqlConnection(connectionString);
	}

	//=============================
	//==Development Only CRUD API==
#if DEBUG
	public async Task<int> Create(Solutions solutions) {
		return await _db.ExecuteAsync("INSERT INTO solutions (description, attachedfile) VALUES (@Description, @AttachedFile)", solutions);
	}

	public async Task<IEnumerable<Solutions>> GetAll() {
		return await _db.QueryAsync<Solutions>("SELECT * FROM solutions");
	}

	public async Task<Solutions> GetById(int id) {
		return await _db.QueryFirstOrDefaultAsync<Solutions>("SELECT * FROM solutions WHERE solutionid = @SolutionId", new { SolutionId = id });
	}

	public async Task<int> Update(Solutions solutions) {
		return await _db.ExecuteAsync("UPDATE solutions SET description = @Description, attachedfile = @AttachedFile WHERE solutionid = @SolutionId", solutions);
	}

	public async Task<int> Delete(int id) {
		return await _db.ExecuteAsync("DELETE FROM solutions WHERE solutionid = @SolutionId", new { SolutionId = id });
	}
#endif


	//Other Commands

	public async Task<IEnumerable<Solutions>> GetNewest(int id) {
		return await _db.QueryAsync<Solutions>("SELECT * FROM solutions ORDER BY solutionid DESC LIMIT @Val", new { val = id });
	}

	public async Task<string> PostSolutionToTask(Solutions solution, int taskId) {
		_db.ExecuteAsync("INSERT INTO solutions (description, attachedfile) VALUES (@Description, @AttachedFile)", solution);
		int solutionIndex = Convert.ToInt32(GetLastSolutionId());
		_db.ExecuteAsync("INSERT INTO tasksolutions (taskid, solutionid) VALUES (@TaskId, @SolutionId)", new { TaskId = taskId, SolutionId = solutionIndex });

		return "Solution posted";
	}

	public async Task<int> GetLastSolutionId() {
		Solutions sol = await _db.QueryFirstOrDefaultAsync<Solutions>("SELECT last_insert_id() FROM information_schema.TABLES WHERE TABLE_SCHEMA = \"gamewo\" AND TABLE_NAME = \"solutions\"");
		return sol.SolutionId;
	}

}
