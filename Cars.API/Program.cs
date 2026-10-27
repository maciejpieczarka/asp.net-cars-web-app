using Cars.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using Cars.Application.Common.Behaviors;
using Cars.Application.Cars;
using Cars.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Zarejestrowanie DataContext 
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Dodanie polityki CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.WithOrigins("http://localhost:5173");
    });
});

//Dodanie walidacji
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetCarsList.Handler).Assembly);
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateCar.CommandValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseMiddleware<ApiExceptionMiddleware>();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration.");
}


app.MapControllers();

app.Run();