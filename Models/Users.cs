﻿namespace Gamewo.Models;

public class Users {
	public int UserId { get; set; }
	public string? Username { get; set; }
	public string? Email { get; set; }
	public string? Password { get; set; }
	public string? Salt { get; set; }
	public int Tokens { get; set; }

}
