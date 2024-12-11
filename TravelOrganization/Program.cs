using ApexCharts;
using DeepL;
using TravelOrganization.Components;
using TravelOrganization.Controllers;
using TravelOrganization.Data;
using TravelOrganization.Data.Repositories.Reviews;
using TravelOrganization.Data.Services;
using TravelOrganization.Data.Models.Account;
using TravelOrganization.Data.Repositories.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddApexCharts();
builder.Services.AddSingleton<DataContext>();

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<ITranslationRepository, TranslationRepository>();

var deeplApiKey = builder.Configuration["Keys:Deepl"];
if (string.IsNullOrEmpty(deeplApiKey))
{
    throw new ArgumentException("API key for Deepl is missing in configuration.");
}

builder.Services.AddSingleton<ITranslator>(_ => new Translator(deeplApiKey));
builder.Services.AddScoped<TranslationService>();
builder.Services.AddScoped<ReviewController>();
builder.Services.AddScoped<PaymentController>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddSingleton<IBasicAuthCredentials, BasicAuthCredentials>();

builder.Services.AddTransient<BasicAuthMessageHandler>();

builder.Services.AddHttpClient("BasicAuthClient")
    .AddHttpMessageHandler<BasicAuthMessageHandler>();

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BasicPolicy", policy =>
        policy.RequireAuthenticatedUser());
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Init();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.Use(async (context, next) =>
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        context.Request.EnableBuffering();
        var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;
        if (bodyAsText.Length != 0)
            logger.LogInformation($"Request Body: {bodyAsText}");
        await next.Invoke();
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(builder =>
    {
        builder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var error = context.Features.Get<IExceptionHandlerFeature>();
            if (error != null)
            {
                var err = new { message = error.Error.Message };
                await context.Response.WriteAsJsonAsync(err);
            }
        });
    });
}

app.Run();
