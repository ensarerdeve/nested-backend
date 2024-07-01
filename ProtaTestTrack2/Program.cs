using ProtaTestTrack2.Controllers;
using ProtaTestTrack2.Data;
using ProtaTestTrack2.Repository;
using ProtaTestTrack2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("ProtaTestDB"));
builder.Services.AddScoped<MongoDBModel>();
builder.Services.AddScoped<FeatureService>();
builder.Services.AddScoped<CaseService>();
builder.Services.AddControllers();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
app.UseAuthorization(); 
app.MapControllers();


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<ProtaTestTrack2.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();


app.Run();
