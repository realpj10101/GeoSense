using api.Interfaces;
using api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDataProtection();

builder.Services.AddScoped<IShapeAnalysisService, ShapeAnalysisService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
