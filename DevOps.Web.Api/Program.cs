using DevOps.Web.Api.Core;
using DevOps.Web.Api.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddServiceBusExtension(builder.Configuration);
builder.Services.AddServices();
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

app.AddBuildServices();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
