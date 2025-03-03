Вот перевод на **французский**:  

---

# Guide d'utilisation de l'API  

## Instructions d'installation  

### 1. Créer un fichier `.env`  
Avant d'exécuter l'application, créez un fichier `.env` et ajoutez les variables d'environnement suivantes :  

```plaintext
# RENOMMER EN .ENV
AWS_ACCESS_KEY=
AWS_SECRET_ACCESS_KEY=
AWS_SERVICE_URL=https://s3.timeweb.cloud
AWS_BUCKET_NAME=
ASPNETCORE_URLS=http://+:80
```  

Ces variables configurent l'accès à AWS S3 (ou un service compatible) et définissent l'URL de base de l'API.  
[https://timeweb.cloud/my/storage](https://timeweb.cloud/my/storage)  

---  

### 2. Installer les paquets requis  
Assurez-vous que votre projet inclut les dépendances nécessaires. Si elles ne sont pas encore ajoutées, utilisez les commandes suivantes pour les installer :  

```bash
# Ajouter les paquets NuGet requis
dotnet add package AWSSDK.Extensions.NETCore.Setup --version 4.0.0-preview

dotnet add package AWSSDK.S3 --version 4.0.0-preview

dotnet add package DotNetEnv --version 3.1.1

dotnet add package Microsoft.AspNetCore.OpenApi --version 9.0.2

dotnet add package Scalar.AspNetCore --version 2.0.21
```  

Ou assurez-vous que votre fichier `.csproj` contient les dépendances suivantes :  

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

## Points de terminaison de l'API  

### Gestion des buckets (AWS S3)  

#### Créer un nouveau bucket S3  
```
POST /api/aws
```
**Paramètres de requête :**  
- `bucketname` (string, requis) : Nom du bucket à créer.  

#### Obtenir les métadonnées d'un bucket  
```
GET /api/aws/metadata
```
**Paramètres de requête :**  
- `bucketname` (string, requis) : Nom du bucket.  

#### Lister tous les buckets S3 disponibles  
```
GET /api/aws
```
Retourne une liste de tous les buckets accessibles.  

#### Lister les objets d'un bucket spécifique  
```
GET /api/aws/objects
```
**Paramètres de requête :**  
- `bucketname` (string, requis) : Nom du bucket dont on veut lister les objets.  

#### Copier un objet dans un bucket  
```
POST /api/aws/copy
```
**Paramètres de requête :**  
- `bucketname` (string, requis) : Bucket où se trouve l'objet.  
- `FromobjectKey` (string, requis) : Clé de l'objet source.  
- `toObjectKey` (string, requis) : Clé de l'objet copié.  

---  

### Gestion des fichiers (images de vêtements)  

#### Télécharger une image  
```
POST /api/clothings/{id}/image
```
**Paramètres de chemin :**  
- `id` (int, requis) : Identifiant unique de l'article de vêtement.  

**Données du formulaire :**  
- `Data` (fichier, requis) : Image à télécharger.  

#### Obtenir une image téléchargée  
```
GET /api/clothings/{id}/image
```
**Paramètres de chemin :**  
- `id` (int, requis) : Identifiant de l'article de vêtement.  

Retourne le fichier image s'il est trouvé.  

#### Supprimer une image  
```
DELETE /api/clothings/{id}/image
```
**Paramètres de chemin :**  
- `id` (int, requis) : Identifiant de l'article de vêtement.  

Supprime l'image spécifiée du stockage.  

---  

## Exécution de l'API  

### Exécuter avec .NET  
Pour démarrer l'API, utilisez la commande suivante :  
```bash
dotnet run
```  

### Exécuter avec Docker  
Si vous utilisez un conteneur Docker, assurez-vous que le fichier `.env` est bien chargé, puis construisez et exécutez le conteneur :  
```bash
docker build -t my-dotnet-api .
docker run --env-file .env -p 80:80 my-dotnet-api
```  

### Exécuter avec Docker Compose  
Pour simplifier l'exécution de l'API, utilisez `docker-compose` :  

#### 1. Créez un fichier `docker-compose.yml` :  
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

#### 2. Démarrer le service :  
```bash
docker-compose up -d
```  
Cela va construire et exécuter l'API dans un conteneur.  

Vous pouvez maintenant accéder à l'API à l'adresse suivante :  
```
http://localhost/api/aws
http://localhost/api/clothings
```  

---  

## Exemples d'URL pour Postman  

Utilisez ces URL pour tester l'API avec **Postman** ou un autre client HTTP :  

| Endpoint                 | Méthode | Exemple d'URL |
|--------------------------|--------|--------------------------------|
| Télécharger une image    | POST   | `http://localhost/api/clothings/2/image` |
| Obtenir une image       | GET    | `http://localhost/api/clothings/2/image` |
| Supprimer une image     | DELETE | `http://localhost/api/clothings/2/image` |
| Créer un bucket         | POST   | `http://localhost/api/aws?bucketname=mybucket` |
| Obtenir les métadonnées d'un bucket | GET | `http://localhost/api/aws/metadata?bucketname=mybucket` |
| Lister les buckets      | GET    | `http://localhost/api/aws` |
| Lister les objets d'un bucket | GET | `http://localhost/api/aws/objects?bucketname=mybucket` |
| Copier un objet dans un bucket | POST | `http://localhost/api/aws/copy?bucketname=mybucket&FromobjectKey=old.jpg&toObjectKey=new.jpg` |  

---  

### 🔹 Remarques :  
- Cette API est conçue pour fonctionner avec **AWS S3** ou tout autre fournisseur de stockage compatible S3.  
- Assurez-vous d'avoir des **identifiants AWS valides** dans le fichier `.env` avant de lancer l'application.  
- La documentation OpenAPI/Scalar sera disponible à `http://localhost/scalar/v1` si elle est configurée.  

Bon développement ! 🚀