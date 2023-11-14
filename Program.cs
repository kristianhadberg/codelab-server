using codelab_exam_server.Data;
using codelab_exam_server.ErrorHandling;
using codelab_exam_server.Services.TopicService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Access to the configuration.
var configuration = builder.Configuration;

builder.Services.AddControllers();
//builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseInMemoryDatabase("CodeLabAPI"));
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection"); 
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
    options.UseMySql(connectionString, serverVersion);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<ITopicService, TopicService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
