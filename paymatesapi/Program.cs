using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using paymatesapi.Contexts;
using paymatesapi.Entities;
using paymatesapi.Helpers;
using paymatesapi.Services;


        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString");
        // Add services to the container.
        builder.Services.AddDbContext<DataContext>(
            options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAuthorization();

        //DI app services
        builder.Services.AddScoped<IJwtUtils, JwtUtils>();
        builder.Services.AddScoped<IUserAuthService, UserAuthService>();
        builder.Services.AddScoped<IFriendService, FriendService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder
            .Services.AddAuthentication(
            // x =>
            // {
            //     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            // }

            )
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Token").Value!)
                    )
                };
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
