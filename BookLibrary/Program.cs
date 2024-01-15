using Microsoft.EntityFrameworkCore;
using BookLibrary.Model;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().
    AddJsonOptions(opt => 
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); //ReferenceHandler.Preserve

//builder.Services.AddDbContext<LibraryDbContext>(opt => 
//{
//    opt.UseSqlServer(builder.Configuration.GetConnectionString("Db"));
//    opt.LogTo(m => Debug.WriteLine(m)).EnableSensitiveDataLogging();
//});
builder.Services.AddDbContext<LibraryDbContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("AzureDb");
    var connectionBuilder = new SqlConnectionStringBuilder(connectionString)
    {
        Password = builder.Configuration["DbPassword"]
    };
    connectionString = connectionBuilder.ConnectionString;
    opt.UseSqlServer(connectionString);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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