using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using TasksManager.DataAccess.DbImplementation.Projects;
using TasksManager.DataAccess.DbImplementation.Tags;
using TasksManager.DataAccess.DbImplementation.Tasks;
using TasksManager.DataAccess.Projects;
using TasksManager.DataAccess.Tags;
using TasksManager.DataAccess.Tasks;

namespace TasksManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Db.TasksContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TasksContext")));
            RegisterQueriesAndCommands(services);

            services.AddMvc();
            services.AddAutoMapper();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ASP.NET Core API", Version = "v1.1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, Db.TasksContext context)
        {
            context.Database.Migrate();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET Core API v1.1");
            });
        }

        private void RegisterQueriesAndCommands(IServiceCollection services)
        {
            services
                .AddScoped<IProjectQuery, ProjectQuery>()
                .AddScoped<IProjectsListQuery, ProjectsListQuery>()
                .AddScoped<ICreateProjectCommand, CreateProjectCommand>()
                .AddScoped<IDeleteProjectCommand, DeleteProjectCommand>()
                .AddScoped<IUpdateProjectCommand, UpdateProjectCommand>()
                .AddScoped<ICreateTaskCommand, CreateTaskCommand>()
                .AddScoped<ITaskQuery, TaskQuery>()
                .AddScoped<IDeleteTaskCommand, DeleteTaskCommand>()
                .AddScoped<IUpdateTaskCommand, UpdateTaskCommand>()
                .AddScoped<ITasksListQuery, TasksListQuery>()
                .AddScoped<IDeleteTagCommand, DeleteTagCommand>()
                .AddScoped<IDeleteTagInTaskCommand, DeleteTagInTaskCommand>()
                .AddScoped<ITagsListQuery, TagsListQuery>()
                ;
        }
    }
}
