using API.Services;
using API.Helpers.Utilities;
using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Set Aspose License
var licPath = Path.Combine(builder.Environment.WebRootPath, "assets", "Aspose", "Aspose.Total.lic");
AsposeUtility.SetLicense(licPath);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddDbContext<DBContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
builder.Services.AddAPIHelpersUtilities();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
