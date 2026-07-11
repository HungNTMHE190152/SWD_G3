using EduNexus.Data;
using EduNexus.ServiceRegistrations;
using EduNexus.Services;
using EduNexus.Services.Implementations;
using EduNexus.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EduNexusContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();

builder.Services.AddCoreServices();
builder.Services.AddLessonServices();
builder.Services.AddAssignmentServices();
builder.Services.AddFlashcardServices();
builder.Services.AddQuestionQuizServices();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddAssignmentServices();
builder.Services.AddScoped<IAssignmentQuestionService, AssignmentQuestionService>();
builder.Services.AddScoped<IRubricService, RubricService>();
builder.Services.AddScoped<IRubricCriterionService, RubricCriterionService>();
builder.Services.AddScoped<IGradingService, GradingService>();
builder.Services.AddScoped<IStudentAssignmentService, StudentAssignmentService>();
builder.Services.AddScoped<IAIGradingService, AIGradingService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();