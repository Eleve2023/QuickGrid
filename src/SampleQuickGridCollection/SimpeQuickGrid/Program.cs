using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using SimpeQuickGrid.Data;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddControllers().AddOData(o => o.EnableQueryFeatures().AddRouteComponents("odata", GetEdmModel()));

var serviceProvider = builder.Services.BuildServiceProvider();
var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
builder.Services.AddHttpClient("SimpleQuickGridServerAPI", client => client.BaseAddress = new Uri(navigationManager.BaseUri));

builder.Services.AddDbContext<PeopleDbContext>();

builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<PersonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

await PersonService.InitDataBase(app.Services);


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.Run();

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Person>("Person").EntityType.HasKey(e => e.Id);
    return builder.GetEdmModel();
}