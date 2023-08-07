using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using DemoProject.ServiceManager;
using DemoProject.Middleware;
using DemoProject.Services;
using DemoProject.Utilities;
using DemoProject.Repository;
using DemoProject.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/*--------Cors policy-------------------------------*/
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        builder =>
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
            //.AllowCredentials()
            //.WithOrigins("http://localhost:4200/");
                .AllowAnyOrigin();
        });
});


/*--------Swagger authentication checking-----------*/
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                                  \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
    c.CustomSchemaIds(i => i.FullName);
});

/*-------------Database connection--------*/
builder.Services.AddDbContext<DPDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cn"));
});

//builder.Services.AddDbContext<LogDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("CnLog"));
//});


/*-------------HttpContext access permission----*/
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

/*-------------Service register area-------START-----*/

builder.Services.AddScoped<ISecurityServiceManager, SecurityServiceManager>();
builder.Services.AddScoped<IUserServiceManager, UserServiceManager>();

builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();
builder.Services.AddScoped<ILoginInfoService, LoginInfoService>();
builder.Services.AddScoped<ISystemUserService, SystemUserService>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IExceptionLogService, ExceptionLogService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();

builder.Services.AddScoped<ICommonServices, CommonServices>();
builder.Services.AddScoped<ISecurityServiceManager, SecurityServiceManager>();

builder.Services.AddScoped<IUserServiceManager, UserServiceManager>();

builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<ILoginInfoRepository, LoginInfoRepository>();
builder.Services.AddScoped<IPagePermissionRepository, PagePermissionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISystemUserRepository, SystemUserRepository>();
builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped< IMailService, MailService >();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddSingleton<ICustomDbContextFactory<DPDbContext>, CustomDbContextFactory<DPDbContext>>();

/*-------------Service register area-------END-----*/

//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (app.Environment.IsProduction())
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Versioned API v1.0");
    });
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseDeveloperExceptionPage();

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

//*** Authentication and Authorization Middleware
app.UseAuthenticationFilter();
app.UseAuthorizationFilter();

app.Run();  
