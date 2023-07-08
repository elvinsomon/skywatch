using SkyWatch.MetricsCollector.Core;
using SkyWatch.MetricsCollector.Core.Contracts;
using SkyWatch.MetricsCollector.Infrastructure.DataBase.Mock;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<IMetricRecordRepository, MetricRecordRepositoryMock>();
builder.Services.AddTransient<MetricCollectorService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();