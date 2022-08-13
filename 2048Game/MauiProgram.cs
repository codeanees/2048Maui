using CommunityToolkit.Maui;
using Microsoft.Maui.LifecycleEvents;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace _2048Game;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");

            })
		   .ConfigureLifecycleEvents(events =>
            {
#if ANDROID
              events.AddAndroid(android => android
                  .OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

              static void MakeStatusBarTranslucent(Android.App.Activity activity)
              {
                  activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);

                  activity.Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

                  activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
              }
#endif
           });

        return builder.Build();
	}
}
