namespace _dotnetaws;

[Route("api/aws")]
public class AWSBucketController : BaseApiController
{
    private readonly IAmazonS3 _s3;
    private readonly AwsOptions _options;
    public AWSBucketController(IAmazonS3 s3, IOptions<AwsOptions> options)
    {
        _options = options.Value;
        _s3 = s3;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBucket(string bucketname)
    {
        //Only if you have your own AWS S3
        var response = await _s3.PutBucketAsync(
            new PutBucketRequest 
            { 
                BucketName = bucketname 
            }
        );
        if(response.HttpStatusCode == HttpStatusCode.OK)
            return Ok();

        return BadRequest();
    }

    [HttpGet("metadata")]
    public async Task<IActionResult> GetMetadataBucket(string bucketname)
    {
        var response = await _s3.GetBucketLocationAsync(
            new GetBucketLocationRequest 
            { 
                BucketName = bucketname 
            }
        );

         if(response.HttpStatusCode == HttpStatusCode.OK)
            return Ok();

        return BadRequest();
        
    }

    [HttpGet]
    public async Task<ActionResult<ListBucketsResponse>> GetBuckets()
    {
        var buckets = await _s3.ListBucketsAsync();

        if(buckets != null)
            return Ok(buckets.Buckets.ToList());

        return BadRequest();
    }

    [HttpGet("objects")]
    public async Task<IActionResult> GetBucketsObjects(string bucketname)
    {
            
    var listObjects = await _s3.ListObjectsAsync(bucketname);

    var objects = listObjects.S3Objects.Select(obj => new
        {
            Key = obj.Key,
            Size = obj.Size
        }).ToList();

    if(!objects.Any())
        return Ok(objects);
    
    return BadRequest();

    }

    [HttpPost("copy")]
    public async Task<IActionResult> CopyObject(
        string bucketname, 
        string FromobjectKey, 
        string toObjectKey)
    {
    var response = await _s3.CopyObjectAsync(new CopyObjectRequest
    {
        SourceBucket = bucketname,
        SourceKey = FromobjectKey,
        DestinationBucket = bucketname,
        DestinationKey = toObjectKey
    }); 

    if(response.HttpStatusCode == HttpStatusCode.OK)
        return Ok($"successfully copy \nfrom {FromobjectKey} to \n{toObjectKey} \nbucketname {bucketname}");

    return BadRequest();

    }


}
