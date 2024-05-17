namespace vesta_api.Database.Models;

public partial class Testing
{
    public int Id { get; set; }

    public DateTime Datetime { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<TestAnswerOfClient> TestAnswersOfClient { get; set; } = new List<TestAnswerOfClient>();
}