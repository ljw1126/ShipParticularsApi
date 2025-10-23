using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // Async 접미사 제거 비활성화
    options.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddDbContext<ShipParticularsContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
      .UseLazyLoadingProxies()
);

builder.Services.AddApplicationServices();

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

app.UseGlobalExceptioHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }