using System.Text;
using blog.Context;
using blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<BlogServices>();

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

var secretKey = builder.Configuration["JWT:key"];
var signingCredentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

// We're adding Auth to your build to check the JWToken from our services

builder.Services.AddAuthentication(options => 
{
    // this line of code will set the Authentication behavior of our JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Sets the default behavior for when our AUTH Fails
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer( options =>
{
    // Configuring JWT Bearer Options (checking the params)
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Check if the Issuer is valid
        ValidateAudience = true, // Checks if the Token's audience is valid
        ValidateLifetime = true, // Ensure that our Token has not expired
        ValidateIssuerSigningKey = true, // Checking the Token's signature is valid

        ValidIssuer = "https://heckermanblog25-gqfxdzfacffuhwed.westus-01.azurewebsites.net/",
        ValidAudience = "https://heckermanblog25-gqfxdzfacffuhwed.westus-01.azurewebsites.net/",
        IssuerSigningKey = signingCredentials
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
