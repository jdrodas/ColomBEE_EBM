-- Scripts de clase - 26 Agosto de 2026 
-- Curso de Tópicos Avanzados de base de datos - UPB 202620
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: ColomBEE_EBM
-- Motor de Base de datos: PostgreSQL 18.x

-- ***********************************
-- Creación del modelo de datos
-- ***********************************

-- Con el usuario ebm_app

-- ****************************************
-- Creación de Tablas
-- ****************************************

-- Tabla: apiarios
create table core.apiarios (
    id                      uuid default uuidv7() constraint apiarios_pk primary key,
    nombre                  varchar(100) not null,
    latitud                 double precision not null,
    longitud                double precision not null,

    constraint apiarios_latitud_ck check (latitud between -90 and 90),
    constraint apiarios_longitud_ck check (longitud between -180 and 180)
);

comment on table core.apiarios is 'ubicación física que agrupa una o más colmenas.';
comment on column core.apiarios.id is 'ID del apiario.';
comment on column core.apiarios.nombre is 'nombre descriptivo del apiario.';
comment on column core.apiarios.latitud is 'Coordenada de latitud del apiario, en grados decimales.';
comment on column core.apiarios.longitud is 'Coordenada de longitud del apiario, en grados decimales.';

-- Tabla: colmenas
create table core.colmenas (
    id                      uuid default uuidv7() constraint colmenas_pk primary key,
    apiario_id              uuid not null constraint colmenas_apiarios_fk references core.apiarios,
    codigo                  varchar(20) not null,
    fecha_instalacion       date not null,

    constraint colmena_apiario_codigo_uk unique (apiario_id, codigo),
    constraint colmena_fecha_instalacion_ck check (fecha_instalacion <= current_date)
);

comment on table core.colmenas is 'colmena física perteneciente a un apiario.';
comment on column core.colmenas.id is 'Id de la colmena.';
comment on column core.colmenas.apiario_id is 'Id del apiario al que pertenece la colmena.';
comment on column core.colmenas.codigo is 'código o etiqueta física de la colmena (ej. "a3-07").';
comment on column core.colmenas.fecha_instalacion is 'fecha en que la colmena entró en operación.';


-- Tabla: tipos_sensores
create table core.tipos_sensores (
    id                      uuid default uuidv7() constraint tipos_sensores_pk primary key,
    nombre                  varchar(20) not null,
    unidad_medida           varchar(20) not null,

    constraint tipos_sensores_uk unique (nombre, unidad_medida)
);

comment on table core.tipos_sensores is 'Tipos de sensores que se instalan en las colmenas';
comment on column core.tipos_sensores.id is 'Id del tipo de sensor';
comment on column core.tipos_sensores.nombre is 'Nombre del tipo de sensor';
comment on column core.tipos_sensores.nombre is 'Unidad de medida usada por el tipo de sensor';


-- Tabla: sensores
create table core.sensores (
    id                      uuid default uuidv7() constraint sensores_pk primary key,
    colmena_id              uuid not null constraint sensores_colmenas_fk references core.colmenas,
    tipo_id                 uuid not null constraint sensores_tipos_fk references core.tipos_sensores,
    frecuencia_muestreo     integer not null,
    fecha_instalacion       date not null,

    constraint sensores_frecuencia_muestreo_ck check (frecuencia_muestreo > 0),
    constraint sensores_fecha_instalacion_ck check (fecha_instalacion <= current_date),
    constraint sensores_uk unique (colmena_id, tipo_id, fecha_instalacion)
);

comment on table core.sensores is 'dispositivo físico instalado en una colmena que captura una métrica específica a una frecuencia propia.';
comment on column core.sensores.id is 'Id del sensor instalado en la colmena';
comment on column core.sensores.colmena_id is 'Id de la colmena donde está instalado el sensor.';
comment on column core.sensores.tipo_id is 'Id del tipo sensor.';
comment on column core.sensores.frecuencia_muestreo is 'Intervalo de muestreo configurado para este sensor, en minutos.';
comment on column core.sensores.fecha_instalacion is 'Fecha en que el sensor entró en operación.';

-- Tabla: lecturas
create table core.lecturas (
    id                      uuid default uuidv7() constraint lecturas_pk primary key,
    sensor_id               uuid not null constraint lecturas_sensores_fk references core.sensores,
    fecha_registro          timestamptz not null,
    valor                   double precision not null,

    constraint lecturas_sensor_fecha_uk unique (sensor_id, fecha_registro)
);

comment on table core.lecturas is 'medición puntual capturada por un sensor en un instante determinado.';
comment on column core.lecturas.id is 'Id de la lectura';
comment on column core.lecturas.sensor_id is 'Id del sensor que generó la medición.';
comment on column core.lecturas.fecha_registro is 'instante real de captura de la lectura.';
comment on column core.lecturas.valor is 'valor numérico medido.';

-- ***************************************************************
-- Creación de Tablas para monitoreo de la operación del sistema
-- ***************************************************************

-- Tabla: estados_sistema
create table monitoreo.estados_sistema (
    id                      uuid default uuidv7() constraint estados_sistema_pk primary key,
    fecha_verificacion      timestamptz not null default current_timestamp,
    estado                  varchar(20) not null,
    mensaje                 text not null default '',
    tiempo_respuesta_ms     bigint not null default 0,
    db_conectada            boolean not null default false,
    tipo_verificacion       varchar(50) default 'health',
    fecha_creacion          timestamptz not null default current_timestamp    
);


comment on table monitoreo.estados_sistema is 'Registros de health checks del sistema';
comment on column monitoreo.estados_sistema.id is 'Id único del registro';
comment on column monitoreo.estados_sistema.fecha_verificacion is 'Fecha y hora de la verificación';
comment on column monitoreo.estados_sistema.estado is 'estado: ok, warning, error';
comment on column monitoreo.estados_sistema.mensaje is 'mensaje descriptivo del estado';
comment on column monitoreo.estados_sistema.tiempo_respuesta_ms is 'tiempo de respuesta en ms';
comment on column monitoreo.estados_sistema.db_conectada is 'indica si la bd estaba conectada';
comment on column monitoreo.estados_sistema.tipo_verificacion is 'tipo de verificación realizada';
comment on column monitoreo.estados_sistema.fecha_creacion is 'fecha de creación del registro';

create index estados_sistema_fecha_verificacion_ix on monitoreo.estados_sistema(fecha_verificacion desc);
create index estados_sistema_estado_ix on monitoreo.estados_sistema(estado);
create index estados_sistema_tipo_verificacion_ix on monitoreo.estados_sistema(tipo_verificacion);
create index estados_sistema_fecha_creacion_ix on monitoreo.estados_sistema(fecha_creacion desc);

