using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using RecipeHelperApp.Data;
using RecipeHelperApp.Interfaces;
using RecipeHelperApp.Services;
using RecipeHelperApp.Settings;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

/* builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); */

// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// AZURE KEYVAULT: 
if (builder.Environment.IsProduction())
{
    var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");

    var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
    
    builder.Configuration.AddAzureKeyVault(keyVaultURL.Value!.ToString(), new DefaultKeyVaultSecretManager());
    
    var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), new DefaultAzureCredential());


    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
   //   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
     options.UseSqlServer(client.GetSecret("ProductionConnection").Value.Value.ToString());
}); 
}

if (builder.Environment.IsDevelopment())
{
var connectionString = builder.Configuration.GetConnectionString("ProdConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProdConnection")));
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ---- BUILD SECTIONS FROM --- /// 

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();


// Protect against brute force attacks. 
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
});




builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<ISmsSender, SmsSender>();

builder.Services.Configure<AuthMessageSenderOptions>(options =>
{
    options.SendGridKey = builder.Configuration["SendGridKey"];
});

builder.Services.Configure<SMSoptions>(options =>
{
    options.SMSAccountIdentification = builder.Configuration["SMSAccountIdentification"];
    options.SMSAccountPassword = builder.Configuration["SMSAccountPassword"];
    options.SMSAccountFrom = builder.Configuration["SMSAccountFrom"];
});




builder.Services.AddScoped<IPhotoService, PhotoService>();

builder.Services.Configure<CloudinarySettings>(options =>
{
    options.CloudName = builder.Configuration["CloudName"];
    options.CloudinaryApiKey = builder.Configuration["CloudinaryApiKey"];
    options.CloudinaryApiSecret = builder.Configuration["CloudinaryApiSecret"]; 
});

builder.Services.AddScoped<IRecipeGenerator, RecipeGenerator>();

builder.Services.Configure<OpenAISettings>(options =>
{
    options.OpenAIKey = builder.Configuration["OpenAIKey"];
});

builder.Services.AddScoped<INutritionService, NutritionService>();

builder.Services.Configure<NutritionServiceSettings>(options =>
{
    options.NutritionAPIKey = builder.Configuration["NutritionAPIKey"];
});

// -----------------------------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Added
app.MapControllers();
// ^^^^^^^^^


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
