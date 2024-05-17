namespace vesta_api.Database.Models.View.Responses;

public class TestingQuestionAnswersResponse
{
    public string Question { get; set; } = null!;

    public List<string> AnswerList { get; set; } = null!;
}