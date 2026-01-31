
namespace Insurtix_Server.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                DotNetEnv.Env.Load();
                var builder = WebApplication.CreateBuilder(args);

                string? clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL");
                if (clientUrl == null)
                {
                    throw new Exception("client url not found");
                }

                string? enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (enviroment == null)
                {
                    throw new Exception("enviroment not found");
                }

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("allowOrigins", builder =>
                    {
                        builder.WithOrigins(clientUrl)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
                });

                builder.Services.AddControllers();
                builder.Services.AddSwaggerGen();


                var app = builder.Build();
                app.UseCors("allowOrigins");
                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseAuthorization();
                app.MapControllers();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
