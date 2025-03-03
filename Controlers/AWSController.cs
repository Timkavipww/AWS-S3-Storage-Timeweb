namespace _dotnetaws;

[Route("api/clothings")]
public class AWSController : BaseApiController
{

    private readonly AwsOptions _awsOptions;
    private readonly IAmazonS3 _s3;
    public AWSController(IOptions<AwsOptions> options, IAmazonS3 s3)
    {
        _awsOptions = options.Value;
        _s3 = s3;
    }

    [HttpPost("{id}/image")]
    public async Task<ActionResult<PutObjectResponse>> Upload([FromRoute] int id,
    [FromForm(Name = "Data")] IFormFile file)
    {
        var request = new PutObjectRequest
        {
            BucketName = _awsOptions.BucketName,
            Key = $"clo_images/{id}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream(),
        };

        var response =  await _s3.PutObjectAsync(request);
        if(response.HttpStatusCode == HttpStatusCode.OK)
            return Ok(response);

        return BadRequest();

    }

    [HttpGet("{id}/image")]
    public async Task<IActionResult> GetImage([FromRoute] int id)
    {
        var objectRequest = new GetObjectRequest
        {
            BucketName = _awsOptions.BucketName,
            Key = $"clo_images/{id}"
        };

        var response = await _s3.GetObjectAsync(objectRequest);
        if(response.HttpStatusCode == HttpStatusCode.OK)
            return File(response.ResponseStream, response.Headers["Content-Type"]);
        
        return BadRequest();

    }

    [HttpDelete("{id}/image")]
    public async Task<IActionResult> DeleteImage([FromRoute] int id)
    {
        var objectRequest = new DeleteObjectRequest
        {
            BucketName = _awsOptions.BucketName,
            Key = $"clo_images/{id}"
        };
        var response = await _s3.DeleteObjectAsync(objectRequest);
        if(response.HttpStatusCode == HttpStatusCode.NoContent)
            return NoContent();

        return BadRequest();    

    }
}
