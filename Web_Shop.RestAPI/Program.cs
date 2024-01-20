using Web_Shop.Persistence.Extensions;
using Web_Shop.Application.Extensions;
using Web_Shop.Application.Utils;
using FluentValidation.AspNetCore;
using Web_Shop.Persistence.UOW.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddApplicationLayer(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        await CrmContextSeed.SeedAsync(scope.ServiceProvider.GetRequiredService<IUnitOfWork>(),
                                            scope.ServiceProvider.GetRequiredService<ILoggerFactory>());
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();
