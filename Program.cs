using Microsoft.EntityFrameworkCore;
using Quizzy_Main.Models;
using Serilog.Events;
using Serilog;
using Serilog.Formatting.Compact;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .WriteTo.Console(new CompactJsonFormatter())
    .WriteTo.File(new CompactJsonFormatter(), "./Log/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


Log.Logger.Information("Logging is working fine");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OnlineExamPortalContext>(item => item.UseSqlServer(@"Server=enosislearning.database.windows.net;Database=OnlineExamPortal;User ID=enosis;Password=sisone@123;TrustServerCertificate=True"));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.  
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddAuthentication()
    .AddCookie("MyAppCookie", options =>
{
    options.Cookie.Name = "MyAppCookie";
    options.LoginPath = "/User/Login";
});

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services
    .AddMvc
    (options => options.EnableEndpointRouting = false);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

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

app.UseCookiePolicy();
app.UseAuthentication();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();