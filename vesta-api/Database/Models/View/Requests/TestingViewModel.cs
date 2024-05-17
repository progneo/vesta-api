namespace vesta_api.Database.Models.View.Requests;

public class TestingViewModel
{
    public int ClientId { get; set; }

    public List<int> AnswerIds { get; set; } = null!;
}