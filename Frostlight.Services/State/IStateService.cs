namespace Frostlight.Services.State;

public interface IStateService<T> where T : IState, new()
{
    Task PersistState(T state, CancellationToken cancellationToken);
    T ReadState();
}