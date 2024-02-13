using ChatHubServices;
using UserContext;
using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;
using ChatHubChat;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
//to add jwt
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using AuthServiceJwt;

var builder = WebApplication.CreateBuilder(args);
//loggin all http req end respo send 
builder.Services.AddHttpLogging(o => { });

var configuration = new ConfigurationBuilder()

    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

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
    
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["jwt:issuer"],
            ValidAudience = configuration["jwt:audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:secreteKey"])),
        };
    });

builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Bearer", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        });
builder.Services.AddScoped<AuthService>(); 
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddScoped<ChatService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Chat with SinalR and .Net7", Version = "v2" , Description = "AppChat made with signalR, allow create chat-rooms, auth users using JWTBearer token, join chats, and in next features send medias at each chat that current user is in."});
});

var app = builder.Build();
app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "SignalR Chat ");
        c.RoutePrefix = string.Empty;
    });
}

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

await app.RunAsync();