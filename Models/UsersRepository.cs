using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace Gamewo.Models;

public class UsersRepository {

	private readonly IDbConnection _db;

	public UsersRepository(string connectionString) {
		_db = new MySqlConnection(connectionString);
	}

	//=============================
	//==Development Only CRUD API==
#if DEBUG
	public async Task<int> Create(Users users) {
		return await _db.ExecuteAsync("INSERT INTO users (userid, username, email, password, salt, tokens) VALUES (@UserId, @Username, @Email, @Password, @Salt, @Tokens)", users);
	}

	public async Task<IEnumerable<Users>> GetAll() {
		return await _db.QueryAsync<Users>("SELECT * FROM users");
	}

	public async Task<Users> GetById(int id) {
		return await _db.QueryFirstOrDefaultAsync<Users>("SELECT * FROM users WHERE userid = @UserId", new { UserId = id });
	}

	public async Task<int> Update(Users users) {
		return await _db.ExecuteAsync("UPDATE users SET username = @Username, email = @Email, password = @Password, tokens = @Tokens WHERE userid = @UserId", users);
	}

	public async Task<int> Delete(int id) {
		return await _db.ExecuteAsync("DELETE FROM users WHERE userid = @UserId", new { UserId = id });
	}
#endif

	//Other Commands



}
