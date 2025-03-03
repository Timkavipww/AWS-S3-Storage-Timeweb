namespace _dotnetaws;

public class AwsOptions
{
    public string AccessKey { get; set; } = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY") ?? "";
    public string SecretKey { get; set; } = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? "";
    public string ServiceUrl { get; set; } = Environment.GetEnvironmentVariable("AWS_SERVICE_URL") ?? "";
    public string BucketName { get; set; } = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME") ?? "";
}
