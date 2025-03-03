Вот перевод на **немецкий**:  

---

# API-Nutzungsanleitung  

## Installationsanweisungen  

### 1. Erstellen einer `.env`-Datei  
Bevor Sie die Anwendung ausführen, erstellen Sie eine `.env`-Datei und fügen Sie die folgenden Umgebungsvariablen hinzu:  

```plaintext
# UMBENENNEN IN .ENV
AWS_ACCESS_KEY=
AWS_SECRET_ACCESS_KEY=
AWS_SERVICE_URL=https://s3.timeweb.cloud
AWS_BUCKET_NAME=
ASPNETCORE_URLS=http://+:80
```  

Diese Variablen konfigurieren den Zugriff auf AWS S3 (oder einen kompatiblen Dienst) und definieren die Basis-URL der API.  
[https://timeweb.cloud/my/storage](https://timeweb.cloud/my/storage)  

---  

### 2. Erforderliche Pakete installieren  
Stellen Sie sicher, dass Ihr Projekt die erforderlichen Abhängigkeiten enthält. Falls nicht, fügen Sie sie mit den folgenden Befehlen hinzu:  

```bash
# Erforderliche NuGet-Pakete hinzufügen
dotnet add package AWSSDK.Extensions.NETCore.Setup --version 4.0.0-preview

dotnet add package AWSSDK.S3 --version 4.0.0-preview

dotnet add package DotNetEnv --version 3.1.1

dotnet add package Microsoft.AspNetCore.OpenApi --version 9.0.2

dotnet add package Scalar.AspNetCore --version 2.0.21
```  

Alternativ stellen Sie sicher, dass Ihre `.csproj`-Datei die folgenden Abhängigkeiten enthält:  

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

## API-Endpunkte  

### Bucket-Verwaltung (AWS S3)  

#### Einen neuen S3-Bucket erstellen  
```
POST /api/aws
```
**Query-Parameter:**  
- `bucketname` (string, erforderlich): Der Name des zu erstellenden Buckets.  

#### Metadaten eines Buckets abrufen  
```
GET /api/aws/metadata
```
**Query-Parameter:**  
- `bucketname` (string, erforderlich): Der Name des Buckets.  

#### Alle verfügbaren S3-Buckets auflisten  
```
GET /api/aws
```
Gibt eine Liste aller zugänglichen Buckets zurück.  

#### Objekte in einem bestimmten Bucket auflisten  
```
GET /api/aws/objects
```
**Query-Parameter:**  
- `bucketname` (string, erforderlich): Der Name des Buckets, aus dem die Objekte aufgelistet werden sollen.  

#### Ein Objekt innerhalb eines Buckets kopieren  
```
POST /api/aws/copy
```
**Query-Parameter:**  
- `bucketname` (string, erforderlich): Der Bucket, in dem sich das Objekt befindet.  
- `FromobjectKey` (string, erforderlich): Der Schlüssel des Quellobjekts.  
- `toObjectKey` (string, erforderlich): Der Schlüssel des kopierten Objekts.  

---  

### Dateiverwaltung (Kleidungsbilder)  

#### Ein Bild hochladen  
```
POST /api/clothings/{id}/image
```
**Pfad-Parameter:**  
- `id` (int, erforderlich): Die eindeutige ID des Kleidungsstücks.  

**Formulardaten:**  
- `Data` (Datei, erforderlich): Die hochzuladende Bilddatei.  

#### Ein hochgeladenes Bild abrufen  
```
GET /api/clothings/{id}/image
```
**Pfad-Parameter:**  
- `id` (int, erforderlich): Die Kleidungsstück-ID.  

Gibt die Bilddatei zurück, falls vorhanden.  

#### Ein Bild löschen  
```
DELETE /api/clothings/{id}/image
```
**Pfad-Parameter:**  
- `id` (int, erforderlich): Die Kleidungsstück-ID.  

Löscht das angegebene Bild aus dem Speicher.  

---  

## API ausführen  

### Ausführen mit .NET  
Um die API zu starten, verwenden Sie den folgenden Befehl:  
```bash
dotnet run
```  

### Ausführen mit Docker  
Falls Sie einen Docker-Container verwenden, stellen Sie sicher, dass die `.env`-Datei korrekt geladen ist, dann erstellen und starten Sie den Container:  
```bash
docker build -t my-dotnet-api .
docker run --env-file .env -p 80:80 my-dotnet-api
```  

### Ausführen mit Docker Compose  
Um die API einfacher auszuführen, verwenden Sie `docker-compose`:  

#### 1. Erstellen Sie eine `docker-compose.yml`-Datei:  
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

#### 2. Starten Sie den Dienst:  
```bash
docker-compose up -d
```  
Dies wird die API innerhalb eines Containers erstellen und ausführen.  

Nun können Sie die API unter den folgenden Adressen aufrufen:  
```
http://localhost/api/aws
http://localhost/api/clothings
```  

---  

## Beispiel-URLs für Postman  

Verwenden Sie diese URLs, um die API mit **Postman** oder einem anderen HTTP-Client zu testen:  

| Endpunkt                | Methode | URL-Beispiel |
|--------------------------|--------|--------------------------------|
| Bild hochladen          | POST   | `http://localhost/api/clothings/2/image` |
| Bild abrufen            | GET    | `http://localhost/api/clothings/2/image` |
| Bild löschen            | DELETE | `http://localhost/api/clothings/2/image` |
| Bucket erstellen        | POST   | `http://localhost/api/aws?bucketname=mybucket` |
| Bucket-Metadaten abrufen | GET  | `http://localhost/api/aws/metadata?bucketname=mybucket` |
| Alle Buckets auflisten  | GET    | `http://localhost/api/aws` |
| Objekte in einem Bucket auflisten | GET | `http://localhost/api/aws/objects?bucketname=mybucket` |
| Objekt in einem Bucket kopieren | POST | `http://localhost/api/aws/copy?bucketname=mybucket&FromobjectKey=old.jpg&toObjectKey=new.jpg` |  

---  

### 🔹 Hinweise:  
- Diese API ist für die Verwendung mit **AWS S3** oder einem kompatiblen S3-Speicheranbieter konzipiert.  
- Stellen Sie sicher, dass Sie **gültige AWS-Anmeldedaten** in der `.env`-Datei haben, bevor Sie die Anwendung starten.  
- Falls konfiguriert, ist die OpenAPI/Scalar-Dokumentation unter `http://localhost/sc