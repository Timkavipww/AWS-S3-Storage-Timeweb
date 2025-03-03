namespace _dotnetaws;

public static class InfrastructureDI
{
    public static WebApplicationBuilder AddAwsSupport(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AWSOptions>(builder.Configuration);

        builder.Services.AddSingleton<IAmazonS3>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AwsOptions>>().Value;

            return new AmazonS3Client(
                options.AccessKey, 
                options.SecretKey, 
                new AmazonS3Config
                {
                    ServiceURL = options.ServiceUrl,
                    ForcePathStyle = true
                });
        });
        
        return builder;
    }


    public static WebApplicationBuilder AddEnvVariableSupport(this WebApplicationBuilder builder)
    {
        Env.Load();
        builder.Configuration.AddEnvironmentVariables();
        
        return builder;
    }



}
