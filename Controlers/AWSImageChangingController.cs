using Microsoft.AspNetCore.Mvc;

namespace _dotnetaws;

[Route("api/clothings")]
public class AWSImageChangingController : ControllerBase
{
    private readonly AwsOptions _awsOptions;
    private readonly IAmazonS3 _s3;
    public AWSImageChangingController(IOptions<AwsOptions> options, IAmazonS3 s3)
    {
        _awsOptions = options.Value;
        _s3 = s3;
    }

    [HttpPost("{id}/image/resized")]
    public async Task<ActionResult<PutObjectResponse>> Upload([FromRoute] int id,
    [FromForm(Name = "Data")] IFormFile file)
    {
        using var image = await Image.LoadAsync(file.OpenReadStream());
        image.Mutate(x => x.Resize(400, 400));

        using var ms = new MemoryStream();
        await image.SaveAsJpegAsync(ms);
        ms.Position = 0;

        var request = new PutObjectRequest
        {
            BucketName = _awsOptions.BucketName,
            Key = $"clo_images_resized/{id}.jpg",
            ContentType = "image/jpeg",
            InputStream = ms,
        };

        var response =  await _s3.PutObjectAsync(request);
      
        return response.HttpStatusCode == HttpStatusCode.OK ? Ok(response) : BadRequest();

    }


     [HttpGet("{id}/image/resized")]
    public async Task<IActionResult> GetImage([FromRoute] int id)
    {
        var objectRequest = new GetObjectRequest
        {
            BucketName = _awsOptions.BucketName,
            Key = $"clo_images_resized/{id}.jpg"
        };

        var response = await _s3.GetObjectAsync(objectRequest);
        if(response.HttpStatusCode == HttpStatusCode.OK)
            return File(response.ResponseStream, response.Headers["Content-Type"]);
        
        return BadRequest();

    }


    [HttpPost("{id}/image/blured")]
    public async Task<ActionResult<PutObjectResponse>> UploadBlured([FromRoute] int id,
    [FromForm(Name = "Data")] IFormFile file)
    {
        using var image = await Image.LoadAsync(file.OpenReadStream());
        image.Mutate(x => x.BokehBlur().BackgroundColor(color:Color.Transparent));

        using var ms = new MemoryStream();
        await image.SaveAsJpegAsync(ms);
        ms.Position = 0;

        var request = new PutObjectRequest
        {
            BucketName = _awsOptions.BucketName,
            Key = $"clo_images_blured/{id}.jpg",
            ContentType = "image/jpeg",
            InputStream = ms,
        };

        var response =  await _s3.PutObjectAsync(request);
      
        return response.HttpStatusCode == HttpStatusCode.OK ? Ok(response) : BadRequest();

    }

    [HttpGet("{id}/image/blured")]
    public async Task<IActionResult> GetImageBlured([FromRoute] int id)
    {
        var objectRequest = new GetObjectRequest
        {
            BucketName = _awsOptions.BucketName,
            Key = $"clo_images_blured/{id}.jpg"
        };

        var response = await _s3.GetObjectAsync(objectRequest);
        if(response.HttpStatusCode == HttpStatusCode.OK)
            return File(response.ResponseStream, response.Headers["Content-Type"]);
        
        return BadRequest();

    }



}
