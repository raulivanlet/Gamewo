namespace Gamewo.Models;

public class Solutions {
	public int SolutionId { get; set; }
	public int TaskId { get; set; }
	public int UserId { get; set; }
	public string? Description { get; set; }
	public DateTime? Date { get; set; }
	public string? AttachedFile { get; set; }

}
