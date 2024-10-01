using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebScrappingDemo.Background;
using WebScrappingDemo.Configurations;
using WebScrappingDemo.Services;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddFastEndpoints()
//    .SwaggerDocument(o => o.TagCase = TagCase.None); //define a swagger document
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = GetConfig(builder.Configuration);
builder.Services.TryAddSingleton(config);

builder.Services.TryAddSingleton<BrowserSessionStorage>();
builder.Services.AddScoped<PuppeteerService>();
builder.Services.AddScoped<OutageScheduleService>();

builder.Services.AddHostedService<BrowserSessionCleanerJob>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

//app.UseFastEndpoints(c =>
//{
//    c.Endpoints.RoutePrefix = "api/endpoints";
//})
//    .UseSwaggerGen();

app.Run();


static YasnoScrappingConfig GetConfig(IConfiguration configuration)
{
    var config = new YasnoScrappingConfig();
    configuration.GetSection(YasnoScrappingConfig.ConfigName).Bind(config);

    return config;
}