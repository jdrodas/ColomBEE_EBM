-- Scripts de clase - 25 Agosto de 2026 
-- Curso de Tópicos Avanzados de base de datos - UPB 202620
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: ColomBEE_EBM
-- Motor de Base de datos: PostgreSQL 18.x

-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Con usuario Postgres:

-- crear el esquema la base de datos
create database ebm_db;

-- Conectarse a la base de datos
\c ebm_db;

-- Creamos un esquema para almacenar todo el modelo de datos del dominio
create schema core;

-- Creamos un esquema para almacenar los datos de monitoreo del funcionamiento de la app
create schema monitoreo;

-- crear el usuario con el que se implementará la creación del modelo
create user ebm_app with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database ebm_db to ebm_app;
grant create on database ebm_db to ebm_app;

grant create, usage on schema core to ebm_app;
grant create, usage on schema monitoreo to ebm_app;
alter user ebm_app set search_path to core;

-- crear el usuario con el que se conectará la aplicación
create user ebm_usr with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database ebm_db to ebm_usr;
grant usage on schema core to ebm_usr;
grant usage on schema monitoreo to ebm_usr;

-- Privilegios sobre tablas existentes
grant select, insert, update, delete, trigger on all tables in schema core to ebm_usr;
grant select, insert, update, delete, trigger on all tables in schema monitoreo to ebm_usr;

-- privilegios sobre secuencias existentes
grant usage, select on all sequences in schema core to ebm_usr;
grant usage, select on all sequences in schema monitoreo to ebm_usr;

-- privilegios sobre funciones existentes
grant execute on all functions in schema core to ebm_usr;
grant execute on all functions in schema monitoreo to ebm_usr;
-- privilegios sobre procedimientos existentes
grant execute on all procedures in schema core to ebm_usr;
grant execute on all procedures in schema monitoreo to ebm_usr;

-- privilegios sobre objetos futuros
alter default privileges in schema core grant select, insert, update, delete on tables TO ebm_usr;
alter default privileges in schema core grant execute on routines to ebm_usr;

alter default privileges in schema monitoreo grant select, insert, update, delete on tables TO ebm_usr;
alter default privileges in schema monitoreo grant execute on routines to ebm_usr;

alter user ebm_usr set search_path to core;

-- crear el usuario con el que se conectará la aplicación para los dashboards
create user ebm_qry with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database ebm_db to ebm_qry;
grant usage on schema core to ebm_qry;
grant usage on schema monitoreo to ebm_qry;


-- Privilegios sobre tablas existentes
grant select on all tables in schema core to ebm_qry;
grant select on all tables in schema monitoreo to ebm_qry;

-- privilegios sobre secuencias existentes
grant usage, select on all sequences in schema core to ebm_qry;
grant usage, select on all sequences in schema monitoreo to ebm_qry;

-- privilegios sobre funciones existentes
grant execute on all functions in schema core to ebm_qry;
grant execute on all functions in schema monitoreo to ebm_qry;

-- privilegios sobre objetos futuros
alter default privileges in schema core grant select on tables TO ebm_qry;
alter default privileges in schema core grant execute on functions to ebm_qry;

alter default privileges in schema monitoreo grant select on tables TO ebm_qry;
alter default privileges in schema monitoreo grant execute on functions to ebm_qry;

alter user ebm_qry set search_path to core;