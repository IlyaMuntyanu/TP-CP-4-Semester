using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TP_CP_5_Semester.Configuration;
using TP_CP_5_Semester.Data;
using TP_CP_5_Semester.PaymentApiClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("Default");

ArgumentException.ThrowIfNullOrEmpty(connectionString);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddSingleton<Client>();
builder.Services.AddOptions<PaymentConfiguration>()
    .BindConfiguration("PaymentConfiguration")
    .ValidateOnStart();

builder.Services.AddSingleton(
    resolver =>
        resolver.GetRequiredService<IOptions<PaymentConfiguration>>().Value
);

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

await QuartzConfiguration.Configure(connectionString);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();