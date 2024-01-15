using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Vb.Business;
using Vb.Business.Mapper;
using Vb.Data;

namespace VbApi;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // string connection = Configuration.GetConnectionString("SqlConnection");
   // services.AddDbContext<VbDbContext>(options => options.UseSqlServer(connection, x =>

      //  {
      //      x.MigrationsAssembly(Assembly.GetAssembly(typeof(VbDbContext)).GetName().Name);
       // }));




    public void ConfigureServices(IServiceCollection services)
    {
        string connection = Configuration.GetConnectionString("SqlConnection");
        services.AddDbContext<VbDbContext>(options => options.UseSqlServer(connection));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(VbTransferCommand).GetTypeInfo().Assembly)); //bu classý eklememizin amacý sadece bu classýn referansýný verebilmek için


        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
        services.AddSingleton(mapperConfig.CreateMapper());

        services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    
    public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(x => { x.MapControllers(); });
    }
}
