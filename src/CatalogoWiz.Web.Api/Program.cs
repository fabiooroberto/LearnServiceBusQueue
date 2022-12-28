using CatalogoWiz.Web.Api.Core;
using CatalogoWiz.Web.Api.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceBusExtension(builder.Configuration);
builder.Services.AddServices();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddBuilderServices();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
