using System.Text;
using Eko.Auth.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Eko.Auth;

public static class Entry
{
    public static void AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(options => 
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
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Authenticated", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });
    
            options.AddPolicy(Policy.Admin, policy => 
            {
                policy.RequireRole(Policy.Admin);
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });
    
            options.AddPolicy("AdminOrUser", policy => 
            {
                policy.RequireRole(Policy.Admin, Policy.AdminOrUser);
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });
        });
    }
}