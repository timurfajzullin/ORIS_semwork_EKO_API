using System.Text;
using Eko.Auth.Jwt;
using Eko.Common.Cqrs;
using Eko.Common.EmailService;
using Eko.Controllers;
using Eko.Database;
using Eko.Features;
using Eko.Features.Notification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EkoDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Db"));
});

builder.Services.AddAuthentication(options => 
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "CookieAndJwt";
    })
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/SignUp";
        options.AccessDeniedPath = "/Auth/SignUp"; 
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = JwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtOptions.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecurityKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt"];
                return Task.CompletedTask;
            },
            OnChallenge = async context =>
            {
                if (!context.Response.HasStarted)
                {
                    context.HandleResponse();
                    context.Response.Redirect("/Auth/SignUp");
                }
                await Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticated", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    });
});


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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=/}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllers();

app.Run();