using Ext_Dynamic_Form.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CountryRepository>();
builder.Services.AddScoped<CityRepository>();
builder.Services.AddScoped<StateRepository>();
builder.Services.AddScoped<PageRepository>();
builder.Services.AddScoped<ActivityRepository>();
builder.Services.AddScoped<ActivityDetailRepository>();
builder.Services.AddScoped<DynamicFormRepository>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();



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
