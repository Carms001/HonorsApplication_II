using HonorsApplication_II.Data;
using HonorsApplication_II.Functions;
using HonorsApplication_II.Pages;

using HonorsApplication_II.ViewModels;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;

namespace HonorsApplication_II
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            //Transient: Each time you request an instance, you get a new independent instance.
            //Singleton: Only one instance of the class exists throughout the lifetime of the application, and all requests for that class return the same instance.

            builder.Services.AddTransient<ProjectsPage>();
            builder.Services.AddTransient<ProjectsPageViewModel>();

            builder.Services.AddTransient<UserSetupPage>();
            builder.Services.AddTransient<UserSetupViewModel>();

            builder.Services.AddTransient<Tasks>();
            builder.Services.AddTransient<TasksViewModel>();

            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddTransient<ProjectFunctions>();

            

            return builder.Build();
        }
    }
}