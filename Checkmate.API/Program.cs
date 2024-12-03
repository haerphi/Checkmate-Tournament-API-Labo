using Checkmate.API.Services;
using Checkmate.BLL.Services;
using Checkmate.BLL.Services.Interfaces;
using Checkmate.DAL.Interfaces;
using Checkmate.DAL.Repositories;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependancy injection
builder.Services.AddTransient<SqlConnection>(c =>
	new SqlConnection(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddTransient<MailHelperService>();

// DAL injections
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();

// BLL injections
builder.Services.AddScoped<IPlayerService, PlayerService>();

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
