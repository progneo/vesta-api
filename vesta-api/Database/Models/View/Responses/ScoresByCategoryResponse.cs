namespace vesta_api.Database.Models.View.Responses;

public class ScoresByCategoryResponse
{
    public string CategoryName { get; set; } = null!;

    public List<TestingScoreResponse> Scores { get; set; } = [];
}