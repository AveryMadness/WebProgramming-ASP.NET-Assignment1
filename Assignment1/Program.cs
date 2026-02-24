var builder = WebApplication.CreateBuilder(args);

// Add services for controllers (Assignment 2 – Controller-based API)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

// Map attribute-routed controllers
app.MapControllers();

app.Run();