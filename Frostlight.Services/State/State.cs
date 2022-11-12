namespace Frostlight.Services.State;
public interface IState
{
    DateTime CreatedOnUtc { get; set; }
    DateTime UpdatedOnUtc { get; set; }
    IState Clone();
}

public abstract class BaseState : IState
{
    public DateTime CreatedOnUtc { get; set; }
    public DateTime UpdatedOnUtc { get; set; }
    public virtual IState Clone() => (IState)MemberwiseClone();
}

public class CounterState : BaseState
{
    public int Count { get; set; }
}