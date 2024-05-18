namespace vesta_api.Database.Models.View.Requests;

public class EditTestQuestionRequest
{
    public int Id { get; set; }
    
    public string Text { get; set; } = null!;

    public bool IsMultipleChoice { get; set; }

    public bool IsActive { get; set; }
}