# ColomBEE_EBM

Repositorio del proyecto de gestión de información para el monitoreo electrónico de colmenas de abejas (Electronic Beehive Monitoring).

APIs REST desarrolladas como ejercicio demostrativo para el curso de **Tópicos Avanzados de Bases de Datos**, enfocadas en la implementación del **patrón repositorio** y la separación por capas. El dominio de problema aborda el registro de la información de apiarios, colmenas, sensores IoT y las lecturas capturadas por estos.

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/jdrodas/ColomBEE_EBM)
![License](https://img.shields.io/badge/license-Academic-orange)
![.NET](https://img.shields.io/badge/.NET-10.0-purple)
![PostgreSQL](https://img.shields.io/badge/database-postgreSQL-blue)
![MongoDB](https://img.shields.io/badge/database-mongoDB-green)

## Objetivos Académicos

- Demostrar la implementación del **patrón repositorio.**
- Evidenciar la separación clara entre capas de la aplicación.
- Mostrar el desacople de la capa de persistencia para intercambio de bases de datos.
- Implementar versionamiento de APIs siguiendo estándares de la industria.
- Implementar paginación en las respuestas de las peticiones con gran cantidad de resultados.
- Aplicar mejores prácticas de seguridad usando GUIDs en lugar de IDs secuenciales.

## Modelo de Datos

### Entidades Principales

#### Apiarios

- `Id` (UUID): Identificador único del apiario.
- `nombre` (TEXT): Nombre descriptivo del apiario.
- `latitud` (FLOAT): Coordenada de latitud del apiario, en grados decimales.
- `longitud` (FLOAT): Coordenada de longitud del apiario, en grados decimales.

#### Colmenas

- `Id` (UUID): Identificador único de la colmena.
- `apiario_id` (UUID): Identificador único del apiario al que pertenece la colmena.
- `codigo` (TEXT): Código o etiqueta física de la colmena (ej. "A3-07").
- `fecha_instalacion` (DATE): Fecha en que la colmena entró en operación.

#### Tipos de Sensores

- `Id` (UUID): Identificador único del tipo de sensor.
- `nombre` (TEXT): Nombre del tipo de sensor.
- `unidad_medida` (TEXT): Unidad de medida usada por el tipo de sensor.

#### Sensores

- `Id` (UUID): Identificador único del sensor.
- `colmena_id` (UUID): Identificador único de la colmena donde está instalado el sensor.
- `tipo_id` (UUID): Identificador único del tipo de sensor.
- `frecuencia_muestreo` (INT): Intervalo de muestreo configurado para el sensor, en minutos.
- `fecha_instalacion` (DATE): Fecha en que el sensor entró en operación.

#### Lecturas

- `Id` (UUID): Identificador único de la lectura.
- `sensor_id` (UUID): Identificador único del sensor que generó la medición.
- `fecha_registro` (TIMESTAMP): Instante real de captura de la lectura.
- `valor` (FLOAT): Valor numérico medido.

### Origen de los datos

Los datos utilizados para este proyecto son **datos sintéticos** generados para simular lecturas de sensores IoT, que no corresponden a ninguna representación real de apiarios, colmenas o mediciones, por lo tanto no se está incurriendo en mal manejo de datos reservados, personales o sensibles.

## Stack Tecnológico

- **Framework base**: C# en .NET 10.x
- **Base de Datos**: PostgreSQL 18.x / MongoDB 8.x
- **ORM**: Dapper (micro-ORM) 2.1.66
- **Documentación**: Swagger/OpenAPI usando Swashbuckle 10.0.1
- **Driver DB Relacional**: Npgsql 10.0.0

## Arquitectura - API REST

### Estructura de Capas

```
Controllers → Services → Repositories (via Interfaces) → DB Context
                  ↓
              IRepositories (Interfaces)
```

### Componentes

- **Controllers**: Capa de presentación y manejo de HTTP.
- **Services**: Lógica de negocio y reglas de dominio.
- **Interfaces**: Contratos para desacoplamiento.
- **Repositories**: Implementaciones de acceso a datos.
- **Models**: Modelos de dominio.
- **DBContext**: Contextos y configuraciones de base de datos.

## Endpoints - API REST

El versionamiento de los endpoints utilizará parámetro en el encabezado o parámetro de consulta, en lugar de incluirlo en el URL

### Apiarios

```http
GET    /api/apiarios                      # Listar todos los apiarios
GET    /api/apiarios/{id}                 # Obtener un apiario por ID
POST   /api/apiarios                      # Crear nuevo apiario
PUT    /api/apiarios                      # Actualizar apiario
DELETE /api/apiarios/{id}                 # Eliminar apiario
```

### Colmenas

```http
GET    /api/colmenas                      # Listar todas las colmenas
GET    /api/colmenas/{id}                 # Obtener una colmena por ID
GET    /api/colmenas/{apiarioId}          # Obtener colmenas de un apiario
POST   /api/colmenas                      # Crear nueva colmena
PUT    /api/colmenas                      # Actualizar colmena
DELETE /api/colmenas/{id}                 # Eliminar colmena
```

### Tipos de Sensores

```http
GET    /api/tipos-sensores                # Listar todos los tipos de sensores
GET    /api/tipos-sensores/{id}           # Obtener un tipo de sensor por ID
POST   /api/tipos-sensores                # Crear nuevo tipo de sensor
PUT    /api/tipos-sensores                # Actualizar tipo de sensor
DELETE /api/tipos-sensores/{id}           # Eliminar tipo de sensor
```

### Sensores

```http
GET    /api/sensores                      # Listar todos los sensores
GET    /api/sensores/{id}                 # Obtener un sensor por ID
GET    /api/sensores/{colmenaId}          # Obtener sensores instalados en una colmena
POST   /api/sensores                      # Crear nuevo sensor
PUT    /api/sensores                      # Actualizar sensor
DELETE /api/sensores/{id}                 # Eliminar sensor
```

### Lecturas

```http
GET    /api/lecturas                      # Listar todas las lecturas
GET    /api/lecturas/{id}                 # Obtener una lectura por ID
GET    /api/lecturas/{sensorId}           # Obtener lecturas registradas por un sensor
GET    /api/lecturas/{fechaId}            # Obtener lecturas registradas por fecha
POST   /api/lecturas                      # Crear nueva lectura
PUT    /api/lecturas                      # Actualizar lectura
DELETE /api/lecturas/{id}                 # Eliminar lectura
```
