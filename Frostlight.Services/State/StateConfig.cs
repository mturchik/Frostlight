namespace Frostlight.Services.State;
public interface IStateConfig<T> where T : IState
{
    StateSaveType SaveType { get; }
    T StateModel { get; }
}

public class JsonStateConfig<T> : IStateConfig<T> where T : IState, new()
{
    public StateSaveType SaveType => StateSaveType.JsonFile;
    public T StateModel => new();
}

public enum StateSaveType
{
    JsonFile
}