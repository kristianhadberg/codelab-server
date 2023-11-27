using codelab_exam_server.Data;
using codelab_exam_server.ErrorHandling;
using codelab_exam_server.Helpers;
using codelab_exam_server.Services;
using codelab_exam_server.Services.ExerciseService;
using codelab_exam_server.Services.SubmissionService;
using codelab_exam_server.Services.TopicService;
using codelab_exam_server.Services.UserService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Access to the configuration.
var configuration = builder.Configuration;

builder.Services.AddControllers();

// Database connection
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection"); 
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
    options.UseMySql(connectionString, serverVersion);
});

// Services
builder.Services.AddTransient<ITopicService, TopicService>();
builder.Services.AddTransient<IExerciseService, ExerciseService>();
builder.Services.AddTransient<ISubmissionService, SubmissionService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddHttpClient<JudgeZeroSubmissionHandler>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


// only for development
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
