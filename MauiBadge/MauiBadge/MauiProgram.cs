using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace MauiBadge;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureMauiHandlers((handlers) =>
            {
#if IOS
            handlers.AddHandler(typeof(Shell), typeof(Platforms.iOS.BadgeShellRenderer));
#endif
#if ANDROID
            handlers.AddHandler(typeof(Shell), typeof(Platforms.Android.BadgeShellRenderer));
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<BadgeModel>();

        return builder.Build();
    }
}
