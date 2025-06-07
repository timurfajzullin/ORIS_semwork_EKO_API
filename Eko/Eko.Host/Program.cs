using System.Net.Http.Headers;
using System.Text;
using Eko.Auth;
using Eko.Auth.Jwt;
using Eko.Common.Cqrs;
using Eko.Common.EmailService;
using Eko.Controllers;
using Eko.Database;
using Eko.Database.Entities;
using Eko.Features;
using Eko.Features.AdminPanel;
using Eko.Features.AskDeepSeek;
using Eko.Features.Notification;
using Eko.Features.Person;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EkoDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Db"));
});

builder.Services.AddAuth();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/SignUp";
    options.AccessDeniedPath = "/Auth/SignUp";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(JwtOptions.ExpirationTime);
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<IEkoDbContext, EkoDbContext>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICommandHandler<NotificationCommand>, NotificationCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreatePersonCommand, string>, CreatePersonCommandHandler>();
builder.Services.AddScoped<ICommandHandler<VerifyPersonCommand, string>, VerifyPersonCommandHandler>();
builder.Services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
builder.Services.AddScoped<IQueryHandler<GetPersonInfoQuery, Profile>, GetPersonInfoQueryHandler>();
builder.Services.AddScoped<ICommandHandler<ChangePersonInfoCommand>, ChangePersonInfoCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetRequestAiQuery, string>, GetRequestAiQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetPersonsQuery, List<Person>>, GetPersonsQueryHandler>();
builder.Services.AddScoped<ICommandHandler<DeletePersonCommand>, DeletePersonCommandHandler>();
builder.Services.AddScoped<ICommandHandler<RegisterPersonCommand>, RegisterPersonCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetProfileQuery, Profile>, GetProfileQueryHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateProfileCommand>, UpdateProfileCommandHandler>();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("DeepSeek", client =>
{
    client.BaseAddress = new Uri("https://api.deepseek.com/v1/");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Information/HandleError", "?statusCode={0}");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=/}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllers();

app.Run();