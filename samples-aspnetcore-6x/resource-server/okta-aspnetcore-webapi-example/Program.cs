using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddCors(options =>
    {
        // The CORS policy is open for testing purposes. In a production application, you should restrict it to known origins.
        options.AddPolicy(
            "AllowAll",
            builder => builder.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader());
    }).AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
        options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
        options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
    })
    .AddOktaWebApi(new OktaWebApiOptions()
    {
        OktaDomain = builder.Configuration["Okta:OktaDomain"],
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.UseHttpsRedirection();

app.MapGet("/api/messages", [Authorize] (HttpContext HttpContext) =>
{
    var principal = HttpContext.User.Identity as ClaimsIdentity;

    var login = principal?.Claims
        .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
        ?.Value;

    return new
    {
        messages = new dynamic[]
        {
                    new { Date = DateTime.Now, Text = "I am a Robot." },
                    new { Date = DateTime.Now, Text = "Hello, world!" },
        },
    };
})
.WithName("Messages");

app.Run();
