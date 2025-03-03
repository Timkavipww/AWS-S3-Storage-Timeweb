# API Usage Guide

## Setup Instructions

### 1. Create a `.env` File
Before running the application, create a `.env` file and add the following environment variables:

```plaintext
# RENAME TO .ENV
AWS_ACCESS_KEY=
AWS_SECRET_ACCESS_KEY=
AWS_SERVICE_URL=https://s3.timeweb.cloud
AWS_BUCKET_NAME=
ASPNETCORE_URLS=http://+:80
```

These variables configure access to AWS S3 (or a compatible service) and define the API's base URL.
https://timeweb.cloud/my/storage
---

### 2. Install Required Packages
Ensure your project includes the necessary dependencies. If you haven't added them yet, use the following alias command to install all required packages:

```bash
# Add required NuGet packages
dotnet add package AWSSDK.Extensions.NETCore.Setup --version 4.0.0-preview

dotnet add package AWSSDK.S3 --version 4.0.0-preview

dotnet add package DotNetEnv --version 3.1.1

dotnet add package Microsoft.AspNetCore.OpenApi --version 9.0.2

dotnet add package Scalar.AspNetCore --version 2.0.21
```

Alternatively, ensure your `.csproj` file includes the following dependencies:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <RootNamespace>_dotnetaws</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="AWSSDK.Extensions.NETCore.Setup" Version="4.0.0-preview" />
    <PackageReference Include="AWSSDK.S3" Version="4.0.0-preview" />
    <PackageReference Include="DotNetEnv" Version="3.1.1" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.2" />
    <PackageReference Include="Scalar.AspNetCore" Version="2.0.21" />
  </ItemGroup>
</Project>
```

---

## API Endpoints

### Bucket Management (AWS S3)

#### Create a new S3 bucket
```
POST /api/aws
```
**Query Parameters:**
- `bucketname` (string, required): The name of the bucket to create.

#### Get metadata for a bucket
```
GET /api/aws/metadata
```
**Query Parameters:**
- `bucketname` (string, required): The bucket name.

#### List all available S3 buckets
```
GET /api/aws
```
Returns a list of all accessible buckets.

#### List objects in a specific bucket
```
GET /api/aws/objects
```
**Query Parameters:**
- `bucketname` (string, required): The name of the bucket to list objects from.

#### Copy an object within a bucket
```
POST /api/aws/copy
```
**Query Parameters:**
- `bucketname` (string, required): The bucket where the object resides.
- `FromobjectKey` (string, required): The key of the source object.
- `toObjectKey` (string, required): The key for the copied object.

---

### File Management (Clothing Images)

#### Upload an image
```
POST /api/clothings/{id}/image
```
**Path Parameters:**
- `id` (int, required): The unique ID of the clothing item.

**Form Data:**
- `Data` (file, required): The image file to upload.

#### Get an uploaded image
```
GET /api/clothings/{id}/image
```
**Path Parameters:**
- `id` (int, required): The clothing item ID.

Returns the image file if found.

#### Delete an image
```
DELETE /api/clothings/{id}/image
```
**Path Parameters:**
- `id` (int, required): The clothing item ID.

Deletes the specified image from storage.

---

## Running the API

### Running with .NET
To start the API, use the following command:
```bash
dotnet run
```

### Running with Docker
If running in a Docker container, ensure the `.env` file is correctly loaded, then build and run the container:
```bash
docker build -t my-dotnet-api .
docker run --env-file .env -p 80:80 my-dotnet-api
```

### Running with Docker Compose
To simplify running the API, use `docker-compose`:

#### 1. Create a `docker-compose.yml` file:
```yaml
services:
  backend:
    container_name: aws
    build:
      context: .
      dockerfile: Dockerfile
    env_file:
      - .env
    ports:
      - 80:80
    networks:
      - app-network

networks:
  app-network:
    driver: bridge
```

#### 2. Start the service:
```bash
docker-compose up -d
```
This will build and run the API inside a container.

Now you can access the API at:
```
http://localhost/api/aws
http://localhost/api/clothings
```

---

## Example URLs for Postman

Use these URLs to test the API using **Postman** or any other HTTP client:

| Endpoint                 | Method | URL Example |
|--------------------------|--------|--------------------------------|
| Upload Image             | POST   | `http://localhost/api/clothings/2/image` |
| Get Image                | GET    | `http://localhost/api/clothings/2/image` |
| Delete Image             | DELETE | `http://localhost/api/clothings/2/image` |
| Create Bucket            | POST   | `http://localhost/api/aws?bucketname=mybucket` |
| Get Bucket Metadata      | GET    | `http://localhost/api/aws/metadata?bucketname=mybucket` |
| List Buckets             | GET    | `http://localhost/api/aws` |
| List Objects in Bucket   | GET    | `http://localhost/api/aws/objects?bucketname=mybucket` |
| Copy Object in Bucket    | POST   | `http://localhost/api/aws/copy?bucketname=mybucket&FromobjectKey=old.jpg&toObjectKey=new.jpg` |

---

### 🔹 Notes:
- This API is designed to work with **AWS S3** or any compatible S3 storage provider.
- Ensure you have valid AWS credentials in the `.env` file before starting the application.
- The OpenAPI/Scalar documentation will be available at `http://localhost/scalar/v1` if configured.

Happy coding! 🚀

