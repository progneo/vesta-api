namespace vesta_api.Database.Models.View.Requests;

public class EditTestQuestionAnswerRequest
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int Score { get; set; }

    public bool IsActive { get; set; }
}