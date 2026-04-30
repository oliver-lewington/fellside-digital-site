using FellsideDigital.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFellsideDigitalPlatform(builder.Configuration);

var app = builder.Build();

await app.ApplyStartupTasksAsync();

app.UseFellsideDigitalPlatform();

app.Run();
