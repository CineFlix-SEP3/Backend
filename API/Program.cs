using GrpcClient;

var builder = WebApplication.CreateBuilder(args);

// Register gRPC client and UserService for DI
builder.Services.AddScoped<UserClient>(_ => new UserClient("http://localhost:9090"));
builder.Services.AddScoped<API.Services.UserService>();

// Register gRPC client and MovieService for DI
builder.Services.AddScoped<MovieClient>(_ => new MovieClient("http://localhost:9090"));
builder.Services.AddScoped<API.Services.MovieService>();

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