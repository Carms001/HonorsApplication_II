using HonorsApplication.Data;
using HonorsApplication.Pages;
using HonorsApplication.ViewModels;
using Microsoft.Extensions.Logging;

namespace HonorsApplication
{
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif


            builder.Services.AddTransient<ProjectsPage>();
            builder.Services.AddTransient<ProjectsPageViewModel>();

            builder.Services.AddTransient<UserSetupPage>();
            builder.Services.AddTransient<UserSetupViewModel>();

            builder.Services.AddSingleton<DataBaseService>();

            

            return builder.Build();
        }
    }
}