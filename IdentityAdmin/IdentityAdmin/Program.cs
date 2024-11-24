using IdentityAdmin.Database;
using IdentityAdmin.Exceptions;
using IdentityAdmin.TokenServices;
using Microsoft.AspNetCore.Diagnostics;
using ServiceStack.Redis;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IUserDbExecute, UserDbExecute>();
builder.Services.AddScoped<IClientDbExecute, ClientDbExecute>();
builder.Services.AddSingleton<IRedisClientAsync>(configure =>
{
    return new RedisClient("localhost", 1433);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseExceptionHandler(configure =>
{
    configure.Run(async (context) =>
    {

        IExceptionHandlerFeature exception = context.Features.Get<IExceptionHandlerFeature>()!;
        if (exception!.Error.InnerException is UnAuthorizedException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(exception.Error.InnerException.Message);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(exception.Error.InnerException!.Message);
        }
    });
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
