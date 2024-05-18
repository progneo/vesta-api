namespace vesta_api.Database.Models.View.Requests;

public class CreateTestQuestionAnswerRequest
{
    public string Text { get; set; } = null!;

    public int Score { get; set; }

    public int QuestionId { get; set; }
}