using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;

namespace ProductLoader.Web.Configuration
{
    public static class ElseWorkflowContainer
    {
        public static IServiceCollection AddElseWorkflow(this IServiceCollection services)
        {
            services.AddElsa(elsa => {
                // Configure Management layer to use EF Core.
                elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore());

                // Configure Runtime layer to use EF Core.
                elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore());

                // Default Identity features for authentication/authorization.
                elsa.UseIdentity(identity => {
                    identity.TokenOptions = options => options.SigningKey = "sufficiently-large-secret-signing-key"; // This key needs to be at least 256 bits long.
                    identity.UseAdminUserProvider();
                });

                // Configure ASP.NET authentication/authorization.
                elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

                // Expose Elsa API endpoints.
                elsa.UseWorkflowsApi();

                // Setup a SignalR hub for real-time updates from the server.
                elsa.UseRealTimeWorkflows();

                // Enable C# workflow expressions
                elsa.UseCSharp();

                // Enable HTTP activities.
                elsa.UseHttp();

                // Use timer activities.
                elsa.UseScheduling();

                // Register custom activities from the application, if any.
                elsa.AddActivitiesFrom<Program>();

                // Register custom workflows from the application, if any.
                elsa.AddWorkflowsFrom<Program>();
            });

            // Configure CORS to allow designer app hosted on a different origin to invoke the APIs.
            services.AddCors(cors => cors
                .AddDefaultPolicy(policy => policy
                    .AllowAnyOrigin() // For demo purposes only. Use a specific origin instead.
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("x-elsa-workflow-instance-id"))); //


            // Add Health Checks.
            services.AddHealthChecks();

            return services;
        }

        public static WebApplication AddLoaderWebDependencies(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");

            // Configure web application's middleware pipeline.
            app.UseCors();
            app.UseWorkflowsApi(); // Use Elsa API endpoints.
            app.UseWorkflows(); // Use Elsa middleware to handle HTTP requests mapped to HTTP Endpoint activities.
            app.UseWorkflowsSignalRHubs(); // Optional SignalR integration. Elsa Studio uses SignalR to receive real-time updates from the server.

            return app;
        }


    }
}
