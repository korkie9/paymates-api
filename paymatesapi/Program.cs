using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using paymatesapi.Contexts;
using paymatesapi.Helpers;
using paymatesapi.Services;

var PaymatesAlloweOrigins = "PaymatesAlloweOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: PaymatesAlloweOrigins,
        policy =>
        {
            policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString");
Console.WriteLine("logging azure containter: ", builder.Configuration["Urls:AzureContainer"]);
builder.Services.AddDbContext<DataContext>(
    options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
builder.Services.AddControllers();
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
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<IUserService, UserService>();


builder
    .Services.AddAuthentication()
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(PaymatesAlloweOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
