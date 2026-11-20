# ETL Client Opinion Analysis (Extracción)

Este proyecto es la **E (Extract)** del ETL del Sistema de Análisis de Opiniones de Clientes. Un Worker Service en .NET 8 lee opiniones de tres fuentes y las guarda sin modificar en archivos JSON de staging:

- **CSV:** encuestas internas
- **Base de datos SQL Server:** reseñas web, clientes y productos (ADO.NET)
- **API REST (`SocialCommentsApi`):** comentarios de redes sociales

## Requisitos

- .NET 8 SDK
- SQL Server con la db `ClientOpinionAnalysis` cargada

## Cómo correrlo

### 1. Configurar los secretos

Desde la carpeta `SocialCommentsApi`:

```
dotnet user-secrets set "ApiKey" "<clave>"
```

Desde la carpeta `Worker`:

```
dotnet user-secrets set "Api:ApiKey" "<clave>"
```

La `<clave>` debe ser el mismo texto en los dos proyectos.

### 2. Ajustar `Worker/appsettings.json`

- `Csv:SurveysPath`: ruta de `surveys_part1.csv`
- `Api:BaseUrl`: dirección de la API, con el puerto que muestre al arrancar

### 3. Levantar la API

```
cd SocialCommentsApi
dotnet run --launch-profile http
```

Déjala corriendo en esa terminal.

### 4. Ejecutar el ETL

En otra terminal:

```
cd Worker
dotnet run
```

## Resultado

El Worker extrae las cinco fuentes en paralelo, muestra en consola cuántos registros guardó y cuánto tardó cada una, y se apaga solo. En la carpeta `staging/` quedan: `surveys.json`, `web_reviews.json`, `clients.json`, `products.json` y `social_comments.json`.
