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
            Key = $"clo_images/{id}.jpg",
            ContentType = "image/jpeg",
            InputStream = file.OpenReadStream(),
        };

        var response =  await _s3.PutObjectAsync(request);
      
        return response.HttpStatusCode == HttpStatusCode.OK ? Ok(response) : BadRequest();

    }

    [HttpGet("{id}/image")]
    public async Task<IActionResult> GetImage([FromRoute] int id)
    {
        var objectRequest = new GetObjectRequest
        {
            BucketName = _awsOptions.BucketName,
            Key = $"clo_images/{id}.jpg"
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
            Key = $"clo_images/{id}.jpg"
        };
        var response = await _s3.DeleteObjectAsync(objectRequest);
        if(response.HttpStatusCode == HttpStatusCode.NoContent)
            return NoContent();

        return BadRequest();    

    }

    [HttpPost("{id}/others")]
    public async Task<ActionResult<PutObjectResponse>> UploadOther([FromRoute] int id,
    [FromForm(Name = "Data")] IFormFile file)
    {
        
        
        var request = new PutObjectRequest
        {
            BucketName = _awsOptions.BucketName,
            Key = $"others/{id}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream(),
        };

        var response =  await _s3.PutObjectAsync(request);
      
        return response.HttpStatusCode == HttpStatusCode.OK ? Ok(response) : BadRequest();

    }

    [HttpGet("{id}/others")]
    public async Task<IActionResult> GetOthers([FromRoute] int id)
    {
        var objectRequest = new GetObjectRequest
        {
            BucketName = _awsOptions.BucketName,
            Key = $"others/{id}"
        };

        var response = await _s3.GetObjectAsync(objectRequest);
        if(response.HttpStatusCode == HttpStatusCode.OK)
            return File(response.ResponseStream, response.Headers["Content-Type"]);
        
        return BadRequest();

    }


}
