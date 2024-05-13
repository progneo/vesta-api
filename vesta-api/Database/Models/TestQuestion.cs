namespace vesta_api.Database.Models;

public partial class TestQuestion
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public bool IsMultipleChoice { get; set; }

    public bool IsActive { get; set; }

    public int CategoryId { get; set; }

    public virtual TestQuestionCategory Category { get; set; } = null!;

    public virtual ICollection<TestAnswerOfClient> TestAnswerOfClients { get; set; } = new List<TestAnswerOfClient>();

    public virtual ICollection<TestQuestionAnswer> TestQuestionAnswers { get; set; } = new List<TestQuestionAnswer>();
}