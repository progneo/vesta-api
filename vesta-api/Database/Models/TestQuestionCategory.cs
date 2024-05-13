namespace vesta_api.Database.Models;

public partial class TestQuestionCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();
}