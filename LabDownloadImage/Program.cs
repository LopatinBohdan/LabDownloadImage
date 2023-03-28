using LabDownloadImage.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connStr = "AccountEndpoint=https://lopatin-cvision.documents.azure.com:443/;AccountKey=nLyeFxTAEwqzNCt3RWb3B4rmyzUjs0N5jgCpgkSdd8rRtokAgCdGS3K0DrUKzho7zsMRBQfLwtPwACDbv3YXQw==;";
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyContext>(options => options.UseCosmos(connStr,"MyDB"));

//string connStr = builder.Configuration.GetConnectionString(Environment.GetEnvironmentVariable("AZURE_VALUE"));
//builder.Services.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
