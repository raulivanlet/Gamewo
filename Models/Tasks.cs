namespace Gamewo.Models;

public class Tasks {
	public int TaskId { get; set; }
	public int UserId { get; set; }
	public int SolutionID { get; set; }
	public string? Title { get; set; }
	public string? Description { get; set; }
	public DateTime? Date { get; set; }
	public string? AttachedFile { get; set; }
	public bool Finished { get; set; }
	public int Tokens { get; set; }

}