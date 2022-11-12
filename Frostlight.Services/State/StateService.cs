using Newtonsoft.Json;

namespace Frostlight.Services.State;
public class StateService<T> : IStateService<T> where T : IState, new()
{
    #region State
    private T State { get; set; }
    private IStateConfig<T> StateConfig { get; }
    #endregion

    public StateService(IStateConfig<T> stateConfig)
    {
        StateConfig = stateConfig;

        switch (StateConfig.SaveType)
        {
            case StateSaveType.JsonFile:
                var filePath = StateService<T>.StateFilePath(StateConfig.StateModel);
                if (File.Exists(filePath))
                {
                    var fileContents = File.ReadAllText(filePath) ?? "";
                    State = JsonConvert.DeserializeObject<T>(fileContents) ?? StateConfig.StateModel;
                }
                else
                {
                    if (!Directory.Exists(StateFolderPath))
                        Directory.CreateDirectory(StateFolderPath);

                    State = StateConfig.StateModel;
                    State.CreatedOnUtc = State.UpdatedOnUtc = DateTime.UtcNow;
                    File.WriteAllText(filePath, StateJson(State));
                }
                break;
            default: throw new NotImplementedException("StateSaveType Not Configured");
        }
    }

    public T ReadState() => (T)State.Clone();

    public async Task PersistState(T state, CancellationToken cancellationToken)
    {
        var currentState = ReadState();
        State = (T)state.Clone();
        State.UpdatedOnUtc = DateTime.UtcNow;

        switch (StateConfig.SaveType)
        {
            case StateSaveType.JsonFile:
                await File.WriteAllTextAsync(StateService<T>.StateFilePath(state), StateService<T>.StateJson(State), cancellationToken);
                break;
            default: throw new NotImplementedException("StateSaveType Not Configured");
        }

        if (cancellationToken.IsCancellationRequested) State = currentState;
    }

    #region Helper

    private static string StateFolderPath => FileSystem.AppDataDirectory + @"\States\";
    private static string StateFilePath(T state) => StateFolderPath + state.GetType().Name + ".json";
    private static string StateJson(T state) => JsonConvert.SerializeObject(state, Formatting.None);

    #endregion
}
