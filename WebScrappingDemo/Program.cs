using Microsoft.Extensions.DependencyInjection.Extensions;
using WebScrappingDemo.Background;
using WebScrappingDemo.Services;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.TryAddSingleton<BrowserSessionStorage>();
builder.Services.AddScoped<PuppeteerService>();
builder.Services.AddScoped<OutageScheduleService>();

builder.Services.AddHostedService<BrowserSessionCleanerJob>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();