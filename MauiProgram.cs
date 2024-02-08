using HonorsApplication_II.Data;
using HonorsApplication_II.Pages;
using HonorsApplication_II.ViewModels;
using Microsoft.Extensions.Logging;

namespace HonorsApplication_II
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

            builder.Services.AddSingleton<DatabaseContext>();

            

            return builder.Build();
        }
    }
}