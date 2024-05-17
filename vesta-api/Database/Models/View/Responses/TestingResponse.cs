namespace vesta_api.Database.Models.View.Responses;

public class TestingResponse
{
    public int Id { get; set; }
    
    public DateTime Datetime { get; set; }
    
    public List<TestingCategoryQuestionsResponse> CategoryQuestions { get; set; } = [];
}