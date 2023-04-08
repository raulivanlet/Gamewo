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

	public async Task<string> AccountCreate(Users users) {
		MySqlDataAdapter dataAdapterUsername = new MySqlDataAdapter($"SELECT userid FROM users WHERE username = {users.Username}", _db.ConnectionString);
		MySqlDataAdapter dataAdapterEmail = new MySqlDataAdapter($"SELECT userid FROM users WHERE email = {users.Email}", _db.ConnectionString);
		DataTable dataTableUsername = new DataTable();
		dataAdapterUsername.Fill(dataTableUsername);

		DataTable dataTableEmail = new DataTable();
		dataAdapterEmail.Fill(dataTableEmail);

		if (dataTableUsername.Rows.Count > 0) {
			return "Username already in use";
		}
		else if (dataTableEmail.Rows.Count > 0) {
			return "Email already in use";
		}
		await _db.ExecuteAsync("INSERT INTO users (username, email, password, salt) VALUES (@Username, @Email, @Password, @Salt)", users);
		return "Account Created";
	}

	public async Task<string> AccountLogin(Users users) {
		MySqlDataAdapter dataAdapter = new MySqlDataAdapter($"SELECT userid FROM users WHERE email = {users.Email} AND password = {users.Password}", _db.ConnectionString);
		DataTable dataTable = new DataTable();
		dataAdapter.Fill(dataTable);

		if (dataTable.Rows.Count > 0) {
			return "User Valid";
		}
		else {
			return "User Invalid";
		}
	}


	public async Task<int> UpdateUserUsername(Users users) {
		return await _db.ExecuteAsync("UPDATE users SET username = @Username WHERE userid = @UserId", users);
	}

	public async Task<int> UpdateUserEmail(Users users) {
		return await _db.ExecuteAsync("UPDATE users SET email = @Email WHERE userid = @UserId", users);
	}

	public async Task<int> UpdateUserPassword(Users users) {
		return await _db.ExecuteAsync("UPDATE users SET password = @Password WHERE userid = @UserId", users);
	}

}
