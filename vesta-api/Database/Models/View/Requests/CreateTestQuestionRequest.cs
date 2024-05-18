namespace vesta_api.Database.Models.View.Requests;

public class CreateTestQuestionRequest
{
    public string Text { get; set; } = null!;

    public bool IsMultipleChoice { get; set; }

    public int CategoryId { get; set; }
}