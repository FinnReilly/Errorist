using Errorist.Extensions;
using Errorist.Implementations;
using Errorist.Models;
using TestApplication.Services;
using TestApplication.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddErrorConfiguration<ApiExceptionDto>();
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IServiceWithTryCatch,  ServiceWithTryCatch>();
builder.Services.AddSingleton<ISingletonService, SingletonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorConfigurationMiddleware<ApiExceptionDto>();
app.UseGlobalDefaultErrorConfigurationWithBuilder<ApiExceptionDtoConfigurationBuilder<Exception>, ApiExceptionDto>()
    .WithStatusCode(500)
    .WithMessage("Something went wrong")
    .WithUserAdvice("Hang tight and we'll be right with you");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
