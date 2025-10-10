
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenAI;
using Pinecone;
using RAGChatBot.API.Middlewares;
using RAGChatBot.Domain.Entities;
using RAGChatBot.Domain.Models;
using RAGChatBot.Infrastructure.DbContexts;
using RAGChatBot.Services.AuthenticationServices;
using RAGChatBot.Services.DocumentServices;
using RAGChatBot.Services.JwtTokenServices;
using RAGChatBot.Services.OpenAIServices;
using RAGChatBot.Services.PineconeServices;
using System.Text;

namespace RAGChatBot.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddControllers();
                builder.Services.AddDbContext<ChatDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));
                builder.Services.AddScoped<IAuthService, AuthService>();
                builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
                builder.Services.AddScoped<PasswordHasher<User>>();
                builder.Services.AddScoped<IPineconeService, PineconeService>();
                builder.Services.AddScoped<IDocumentService, DocumentService>();
                builder.Services.AddScoped<IOpenAIService, OpenAIService>();
                builder.Services.Configure<PineconeSettings>(builder.Configuration.GetSection("Pinecone"));
                builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAI"));
                builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
                builder.Services.AddSingleton<PineconeClient>(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<PineconeSettings>>().Value;
                    return new PineconeClient(settings.VectorApiKey);
                });
                builder.Services.AddSingleton<OpenAIClient>(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<OpenAISettings>>().Value;
                    return new OpenAIClient(settings.OpenAIApiKey);
                });
                builder.Services.AddHttpContextAccessor();

                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings!.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            
        }
    }
}
