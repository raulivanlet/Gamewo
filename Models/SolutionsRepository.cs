using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace Gamewo.Models;

public class SolutionsRepository {

	private readonly IDbConnection _db;

	public SolutionsRepository(string connectionString) {
		_db = new MySqlConnection(connectionString);
	}

	public async Task<int> Create(Solutions solution) {
		int var1 = await _db.ExecuteAsync("INSERT INTO solutions (taskid, userid, description, attachedfile) VALUES (@TaskId, @UserId, @Description, @AttachedFile)", solution);
		int var2 = await _db.ExecuteAsync("INSERT INTO tasksolutions (taskid, solutionid) VALUES (@TaskId, @SolutionId)", new { TaskId = solution.TaskId, SolutionId = GetLastSolutionId() });
		if (var1 == 1 && var2 == 1)
			return 1;
		else return 0;
	}

	public async Task<IEnumerable<Solutions>> GetNewest(int id) {
		return await _db.QueryAsync<Solutions>("SELECT * FROM solutions ORDER BY solutionid DESC LIMIT @Val", new { val = id });
	}

	public async Task<int> GetLastSolutionId() {
		Solutions sol = await _db.QueryFirstOrDefaultAsync<Solutions>("SELECT last_insert_id() FROM information_schema.TABLES WHERE TABLE_SCHEMA = \"gamewo\" AND TABLE_NAME = \"solutions\"");
		return sol.SolutionId;
	}

#if DEBUG
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

}
