using Frostlight.Data;
using Frostlight.Services.State;

namespace Frostlight;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton<WeatherForecastService>();

        builder.Services.AddSingleton<IStateService<CounterState>>(s => new StateService<CounterState>(new JsonStateConfig<CounterState>()));

        return builder.Build();
    }
}
