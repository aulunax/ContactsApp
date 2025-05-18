using ContactsApp.Server.Data;
using ContactsApp.Server.Models;
using ContactsApp.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


// Dependency Injections
builder.Services.AddDbContext<ContactsDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubcategoryService, SubcategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Controllers Configuration
builder.Services.AddControllers()
                .AddJsonOptions(opts => {
    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
}); ;

builder.Services.AddEndpointsApiExplorer();


// Swagger Setup
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Contacts List API", Version = "v1" });

    // Allow Swagger to use the JWT Bearer token
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Description",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme, new string[] { }
        }
    });
});


// Authorization and Authentication Setup

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.User.RequireUniqueEmail = true;  
    })
    .AddEntityFrameworkStores<ContactsDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("accessToken"))
            {
                context.Token = context.Request.Cookies["accessToken"];
            }

            return Task.CompletedTask;
        }
    };
});


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
