using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FarroRelay.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

namespace FarroRelay
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<farroRelayContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("farroRelayContext")));
			//			services.AddCors();
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
					builder.SetIsOriginAllowed(_ => true)
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials());
			});
			services.AddScoped<farroRelayContext>();
			services.AddControllers()
						.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
						; 
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseAuthentication();
			app.UseStatusCodePages();
			app.UseDefaultFiles();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();

			app.UseCors();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
