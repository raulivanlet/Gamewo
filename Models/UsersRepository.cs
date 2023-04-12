using System.Data;
using Dapper;
using Google.Protobuf.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
		return await _db.ExecuteAsync("INSERT INTO users (username, email, password, salt) VALUES (@Username, @Email, @Password, @Salt)", users);
	}

	public async Task<IEnumerable<Users>> GetAllUsers() {
		return await _db.QueryAsync<Users>("SELECT * FROM users");
	}

	public async Task<Users> GetUserById(int id) {
		return await _db.QueryFirstOrDefaultAsync<Users>("SELECT * FROM users WHERE userid = @UserId", new { UserId = id });
	}

	public async Task<int> UpdateUser(Users users) {
		return await _db.ExecuteAsync("UPDATE users SET username = @Username, email = @Email, password = @Password WHERE userid = @UserId", users);
	}

	public async Task<int> DeleteUser(int id) {
		return await _db.ExecuteAsync("DELETE FROM users WHERE userid = @UserId", new { UserId = id });
	}
#endif

	//Other Commands

	public async Task<IEnumerable<Users>> GetMostActiveUsers(int id) {
		return await _db.QueryAsync<Users>("SELECT * FROM users ORDER BY tokens DESC LIMIT @Val", new { Val = id } );
	}

	public async Task<int> AddTokensToUser(Users users) {
		Users tmp = new Users();
		tmp = await _db.QueryFirstOrDefaultAsync<Users>("SELECT * FROM users WHERE userid = @UserId", new { UserId = users.UserId });
		users.Tokens = users.Tokens + tmp.Tokens;
		return await _db.ExecuteAsync("UPDATE users SET tokens = @Tokens WHERE userid = @UserId", users);
	}

}
