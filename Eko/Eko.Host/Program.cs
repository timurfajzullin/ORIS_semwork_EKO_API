using Eko.Common.Cqrs;
using Eko.Controllers;
using Eko.Database;
using Eko.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EkoDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Db"));
});

builder.Services.AddScoped<IEkoDbContext, EkoDbContext>();

builder.Services.AddScoped<ICommandHandler<PersonCommand>, PersonCommandHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=/}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllers();

app.Run();