using FluentValidation.AspNetCore;
using InventAutoApi.FileServer;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Application;
using ProductCatalogAPI.Application.Interfaces;
using ProductCatalogAPI.Domain.Interfaces;
using ProductCatalogAPI.Domain.Services;
using ProductCatalogAPI.Infrastructure.DAO;

var builder = WebApplication.CreateBuilder(args);

var allOrigins = "MyAllowSpecificOrigins";

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(allOrigins,
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration["DbSetting:ConnectionString"]));

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProductDomain, ProductDomain>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton<UploadImage>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    return new UploadImage(Path.Combine(env.WebRootPath, "images"));
});

builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
