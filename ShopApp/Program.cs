using MySql.Data.MySqlClient;  // 
using System.Data;
using Dapper_Example.DAL.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllers();

// Configure Swagger/OpenAPI 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure repositories
/*
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
*/

// Configure Unit of Work 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure MySQL connection and transaction 
builder.Services.AddScoped((s) => new MySqlConnection(builder.Configuration.GetConnectionString("MySqlConnection")));
builder.Services.AddScoped<IDbTransaction>(s =>
{
    var conn = s.GetRequiredService<MySqlConnection>();
    conn.Open();
    return conn.BeginTransaction();
});

var app = builder.Build();

// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
