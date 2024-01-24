using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//config UI swagger authen
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "LM Online Information System", Version = "v.10.24" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add Identity
builder.Services
    .AddIdentity<Account, IdentityRole>()
    .AddEntityFrameworkStores<LMOnlineSystemDbContext>()
    .AddDefaultTokenProviders();

//Congig local db
builder.Services.AddDbContext<LMOnlineSystemDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LMOnlineSystemDB"));
});



//--------------------PLEASE MUST DON'T OPEN THIS COMMENT BELOW :)-------

//config database to deploy on azure
//var connection = String.Empty;
//if (builder.Environment.IsDevelopment())
//{
//    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
//    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
//}
//else
//{
//    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
//}
//// Config SQLAzure
//builder.Services.AddDbContext<LMOnlineSystemDbContext>(options =>
//    options.UseSqlServer(connection));

// ------------------- OPEN COMMENT CHAT KOO ---------------------------

//add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("app-cors",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination")
            .AllowAnyMethod();
        });
});

// Add Authentication and JwtBearer
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
        };
    });

// add automapper
builder.Services.AddAutoMapper(typeof(AutomapperProfile).Assembly);

//Add Dependenci Injection, Life cycle DI: AddSingleton(), AddTransisent(), AddScoped()
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IWishListRepository, WishListRepository>();
builder.Services.AddScoped<IWishListService, WishListService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LM Online Information System");
});


//use cors
app.UseCors("app-cors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
