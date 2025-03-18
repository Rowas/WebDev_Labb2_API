using Microsoft.AspNetCore.Mvc.Formatters;
using WebDev_Labb2_API.Model;

var AllowedOrigins = "_allowedOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedOrigins,
        policy =>
        {
            policy.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
});

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

app.UseCors(AllowedOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var db = new DBContext())
{
    db.Database.EnsureCreated();
    app.Run();
}
