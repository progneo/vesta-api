namespace vesta_api.Database.Models.View.Responses;

public class TestingCategoryQuestionsResponse
{
    public string Category { get; set; } = null!;

    public List<TestingQuestionAnswersResponse> QuestionList { get; set; } = null!;
}