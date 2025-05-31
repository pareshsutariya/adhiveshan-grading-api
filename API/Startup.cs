using System.Text;

using AdhiveshanGrading.Middlewares;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AdhiveshanGrading.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // requires using Microsoft.Extensions.Options
        services.AddAutoMapper(typeof(Startup));

        #region Auth
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidIssuer = Configuration.GetValue<string>("JwtConfig:issuer"),
                ValidAudience = Configuration.GetValue<string>("JwtConfig:audiance"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtConfig:key"))),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
        });
        services.AddAuthentication();
        #endregion

        #region Adhiveshan Services
        services.AddSingleton<IAdvGradingSettings>(sp => sp.GetRequiredService<IOptions<AdvGradingSettings>>().Value);
        services.Configure<AdvGradingSettings>(Configuration.GetSection(nameof(AdvGradingSettings)));
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IUsersService, UsersService>();
        services.AddSingleton<IParticipantsService, ParticipantsService>();
        services.AddSingleton<ICompetitionEventsService, CompetitionEventsService>();
        services.AddSingleton<IEventSchedulesService, EventSchedulesService>();
        services.AddSingleton<IGradingCriteriasService, GradingCriteriasService>();
        services.AddSingleton<IGradesService, GradesService>();
        services.AddSingleton<IAdhiveshanPortalService, AdhiveshanPortalService>();
        services.AddSingleton<ConfigurationsService>();
        #endregion

        services.AddCors();
        services.AddControllers();

        #region Swagger
        services.AddSwaggerGen(options =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Enter your JWT Access Token",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                }
            };
            options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Adhiveshan Grading API", Version = "v1" });
            c.CustomSchemaIds(type => type.ToString());
        });
        #endregion

        services.Configure<FormOptions>(o =>
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = int.MaxValue;
            o.MemoryBufferThreshold = int.MaxValue;
        });

        services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // app.Run(context =>
        // {
        //     return context.Response.WriteAsync("Hello from ASP.NET Core!");
        // });

        // if (env.IsDevelopment())
        // {
        //     app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdhiveshanGrading API v1"));

        // https://localhost:5001/swagger/index.html
        // }

        // app.UseHttpsRedirection();

        app.UseRouting();
        // global cors policy
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials()); // allow credentials

        // app.UseAuthorization();

        app.UseStaticFiles();
        // app.UseStaticFiles(new StaticFileOptions()
        // {
        //     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Images")),
        //     RequestPath = new PathString("/Images")
        // });

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
