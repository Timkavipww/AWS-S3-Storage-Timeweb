# Руководство по использованию API

## Инструкции по настройке

### 1. Создание файла `.env`
Перед запуском приложения создайте файл `.env` и добавьте в него следующие переменные окружения:

```plaintext
# ПЕРЕИМЕНОВАТЬ В .ENV
AWS_ACCESS_KEY=
AWS_SECRET_ACCESS_KEY=
AWS_SERVICE_URL=https://s3.timeweb.cloud
AWS_BUCKET_NAME=
ASPNETCORE_URLS=http://+:80
```

Эти переменные настраивают доступ к AWS S3 (или совместимому сервису) и определяют базовый URL API.
https://timeweb.cloud/my/storage

---

### 2. Установка необходимых пакетов
Убедитесь, что ваш проект содержит все необходимые зависимости. Если они ещё не добавлены, используйте следующую команду для их установки:

```bash
# Добавление необходимых пакетов NuGet
dotnet add package AWSSDK.Extensions.NETCore.Setup --version 4.0.0-preview

dotnet add package AWSSDK.S3 --version 4.0.0-preview

dotnet add package DotNetEnv --version 3.1.1

dotnet add package Microsoft.AspNetCore.OpenApi --version 9.0.2

dotnet add package Scalar.AspNetCore --version 2.0.21
```

Альтернативно, убедитесь, что в вашем файле `.csproj` включены следующие зависимости:

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

## Конечные точки API

### Управление хранилищем S3 (AWS)

#### Создание нового S3-бакета
```
POST /api/aws
```
**Параметры запроса:**
- `bucketname` (строка, обязательный): Имя создаваемого бакета.

#### Получение метаданных бакета
```
GET /api/aws/metadata
```
**Параметры запроса:**
- `bucketname` (строка, обязательный): Имя бакета.

#### Список всех доступных бакетов
```
GET /api/aws
```
Возвращает список всех доступных бакетов.

#### Список объектов в бакете
```
GET /api/aws/objects
```
**Параметры запроса:**
- `bucketname` (строка, обязательный): Имя бакета, из которого нужно получить список объектов.

#### Копирование объекта внутри бакета
```
POST /api/aws/copy
```
**Параметры запроса:**
- `bucketname` (строка, обязательный): Бакет, в котором находится объект.
- `FromobjectKey` (строка, обязательный): Ключ исходного объекта.
- `toObjectKey` (строка, обязательный): Ключ нового объекта (копии).

---

### Управление файлами (изображения одежды)

#### Загрузка изображения
```
POST /api/clothings/{id}/image
```
**Параметры пути:**
- `id` (целое число, обязательный): Уникальный идентификатор элемента одежды.

**Данные формы:**
- `Data` (файл, обязательный): Файл изображения для загрузки.

#### Получение загруженного изображения
```
GET /api/clothings/{id}/image
```
**Параметры пути:**
- `id` (целое число, обязательный): Идентификатор элемента одежды.

Возвращает файл изображения, если он найден.

#### Удаление изображения
```
DELETE /api/clothings/{id}/image
```
**Параметры пути:**
- `id` (целое число, обязательный): Идентификатор элемента одежды.

Удаляет указанное изображение из хранилища.

---

## Запуск API

### Запуск через .NET
Для запуска API используйте команду:
```bash
dotnet run
```

### Запуск через Docker
Если API работает в контейнере Docker, убедитесь, что файл `.env` загружен корректно, затем выполните:
```bash
docker build -t my-dotnet-api .
docker run --env-file .env -p 80:80 my-dotnet-api
```

### Запуск через Docker Compose
Для упрощённого запуска API используйте `docker-compose`:

#### 1. Создайте файл `docker-compose.yml`:
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

#### 2. Запустите сервис:
```bash
docker-compose up -d
```
Это создаст и запустит API в контейнере.

Теперь API доступно по следующим адресам:
```
http://localhost/api/aws
http://localhost/api/clothings
```

---

## Примеры URL для Postman

Используйте эти URL для тестирования API в **Postman** или другом HTTP-клиенте:

| Конечная точка         | Метод  | Пример URL |
|------------------------|--------|--------------------------------|
| Загрузка изображения   | POST   | `http://localhost/api/clothings/2/image` |
| Получение изображения  | GET    | `http://localhost/api/clothings/2/image` |
| Удаление изображения   | DELETE | `http://localhost/api/clothings/2/image` |
| Создание бакета        | POST   | `http://localhost/api/aws?bucketname=mybucket` |
| Метаданные бакета      | GET    | `http://localhost/api/aws/metadata?bucketname=mybucket` |
| Список бакетов         | GET    | `http://localhost/api/aws` |
| Список объектов        | GET    | `http://localhost/api/aws/objects?bucketname=mybucket` |
| Копирование объекта    | POST   | `http://localhost/api/aws/copy?bucketname=mybucket&FromobjectKey=old.jpg&toObjectKey=new.jpg` |

---

### 🔹 Примечания:
- Этот API предназначен для работы с **AWS S3** или другим совместимым хранилищем.
- Перед запуском убедитесь, что у вас есть действительные AWS-учётные данные в `.env`.
- Документация OpenAPI/Scalar будет доступна по адресу `http://localhost/scalar/v1`, если настроена.

Приятного кодинга! 🚀co