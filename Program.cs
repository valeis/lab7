using lab7.DataAccess;
using lab7.Models;
using lab7.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IAdService, AdService>();
builder.Services.AddScoped<IAdRepository,AdRepository>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();

builder.Services.AddDbContext<TrackContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("Redis:Configuration"); 
    options.InstanceName = builder.Configuration.GetValue<string>("Redis:InstanceName") ?? "SampleInstance";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3001") 
                        .AllowAnyMethod()
                        .AllowAnyHeader() 
                        .AllowCredentials()); 
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting(); 
app.UseCors();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
