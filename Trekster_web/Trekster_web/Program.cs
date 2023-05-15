using BusinessLogic.Implementations;
using BusinessLogic.Interfaces;
using BusinessLogic.Mapping.Profiles;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceImplementation;
using BusinessLogic.Services.ServiceInterfaces;
using EmailService;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Configuration;
using Trekster_web.ControllerServices.Implementation;
using Trekster_web.ControllerServices.Interfaces;
using Trekster_web.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICurrency, CurrencyRepository>();
builder.Services.AddScoped<IAccount, AccountRepository>();
builder.Services.AddScoped<ICategory, CategoryRepository>();
builder.Services.AddScoped<IStartBalance, StartBalanceRepository>();
builder.Services.AddScoped<ITransaction, TransactionRepository>();

builder.Services.AddAutoMapper(typeof(AccountProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CurrencyProfile).Assembly);
builder.Services.AddAutoMapper(typeof(StartBalanceProfile).Assembly);
builder.Services.AddAutoMapper(typeof(TransactionProfile).Assembly);
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStartBalanceService, StartBalanceService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddAutoMapper(typeof(CategoryVMProfile).Assembly);
builder.Services.AddAutoMapper(typeof(AccountVMProfile).Assembly);
builder.Services.AddAutoMapper(typeof(AccountsVMProfile).Assembly);
builder.Services.AddAutoMapper(typeof(TransactionVMProfile).Assembly);

builder.Services.AddScoped<IAccountControllerService, AccountControllerService>();
builder.Services.AddScoped<IExpensesControllerService, ExpensesControllerService>();
builder.Services.AddScoped<IHistoryControllerService, HistoryControllerService>();
builder.Services.AddScoped<IHomeControllerService, HomeControllerService>();
builder.Services.AddScoped<IProfitsControllerService, ProfitsControllerService>();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
});

var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
  .Get<EmailConfiguration>();

builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();