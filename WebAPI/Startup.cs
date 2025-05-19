using AdhiveshanGrading.WebAPI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace AdhiveshanGrading.WebAPI;

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

        // Adhiveshan
        services.AddSingleton<IAdvGradingSettings>(sp => sp.GetRequiredService<IOptions<AdvGradingSettings>>().Value);
        services.Configure<AdvGradingSettings>(Configuration.GetSection(nameof(AdvGradingSettings)));
        services.AddSingleton<IUsersService, UsersService>();
        services.AddSingleton<IParticipantsService, ParticipantsService>();
        services.AddSingleton<ICompetitionEventsService, CompetitionEventsService>();
        services.AddSingleton<IGradingCriteriasService, GradingCriteriasService>();
        services.AddSingleton<IGradesService, GradesService>();
        services.AddSingleton<ConfigurationsService>();

        services.AddCors();
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdhiveshanGrading API", Version = "v1" });
            c.CustomSchemaIds(type => type.ToString());
        });

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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
