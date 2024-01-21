using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;
using ChatHubChat;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using UserContext;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .SetIsOriginAllowed(_ => true) 
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddScoped<ChatHubServices.ChatService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNetWebSocketChat", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetWebSocketChat");
        c.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

app.UseCors("CorsPolicy");

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");



await app.RunAsync();