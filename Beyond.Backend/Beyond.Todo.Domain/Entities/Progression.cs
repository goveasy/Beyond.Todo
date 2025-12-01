namespace Beyond.Todo.Domain.Entities;

public sealed class Progression
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public decimal Percent { get; init; }

    private Progression() { }
    public Progression(DateTime date, decimal percent)
    {
        if (percent <= 0 || percent > 100)
        {
            throw new ArgumentException("Progress percent must be between 0 and 100.", nameof(percent));
        }

        Date = date;
        Percent = percent;
    }
}
