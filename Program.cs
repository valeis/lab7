using lab7.DataAccess;
using lab7.Models;
using lab7.Services;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000") 
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
