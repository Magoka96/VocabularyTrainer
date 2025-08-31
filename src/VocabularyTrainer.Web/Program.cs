using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;
using VocabularyTrainer.Infrastructure;
using VocabularyTrainer.Infrastructure.Configuration;
using VocabularyTrainer.Web.Components;
using VocabularyTrainer.Web.Extensions;

namespace VocabularyTrainer.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddMudServices();

        builder.Services.AddDatabase(builder.Configuration)
            .AddTranslationService(builder.Configuration)
            .AddAuth0Authentication(builder.Configuration)
            .AddCurrentUserService();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapAuth0Endpoints();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
