using Alten.API;
using Alten.API.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Alten.API.Models;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

builder.Services
    .AddDbContext<ApiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("db")))
    .AddScoped<IReservationService, ReservationService>()
    .AddControllers()
    .AddFluentValidation(fv => {});

builder.Services.AddTransient<IValidator<Reservation>, ReservationModelValidator>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Alten API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alten API v1"));
app.UseHealthChecks("/status");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
