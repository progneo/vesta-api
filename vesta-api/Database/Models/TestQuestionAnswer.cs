namespace vesta_api.Database.Models;

public partial class TestQuestionAnswer
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int Score { get; set; }

    public bool IsActive { get; set; }

    public int QuestionId { get; set; }

    public virtual TestQuestion Question { get; set; } = null!;

    public virtual ICollection<TestAnswerOfClient> TestAnswerOfClients { get; set; } = new List<TestAnswerOfClient>();
}