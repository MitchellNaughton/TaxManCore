using TaxManCoreAPI.Services;
using TaxManCoreDL.model;
using TaxManCoreDL.bo;
using TaxManCoreDL.da;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISmtpService, InternalSmtp>();
builder.Services.AddTransient<ISalaryReturnModel, SalaryReturnModel>();
builder.Services.AddTransient<ITaxValuesModel, TaxValuesModel>();
builder.Services.AddTransient<ISqlTaxModel, SqlTaxModel>();

var app = builder.Build();

//As TaxManCoreDL.bo is a static class we want to use method injection and so will call it here
var Injection1 = app.Services.GetService<ISalaryReturnModel>();
var Injection2 = app.Services.GetService<ITaxValuesModel>();
var Injection3 = app.Services.GetService<ISqlTaxModel>();
TaxManCoreBo.BoDependencies(Injection1, Injection2);
DataAccess.DaDependencies(Injection2, Injection3);

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
