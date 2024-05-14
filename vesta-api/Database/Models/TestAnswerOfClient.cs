namespace vesta_api.Database.Models;

public partial class TestAnswerOfClient
{
    public int Id { get; set; }

    public int TestingId { get; set; }

    public int AnswerId { get; set; }

    public virtual TestQuestionAnswer Answer { get; set; } = null!;

    public virtual Testing Testing { get; set; } = null!;
}