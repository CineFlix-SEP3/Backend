using API.Services;
using GrpcClient;
using GrpcToolkit.Proto;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register gRPC client and UserService for DI
builder.Services.AddScoped<UserClient>(_ => new UserClient("http://localhost:9090"));
builder.Services.AddScoped<API.Services.UserService>();


// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();