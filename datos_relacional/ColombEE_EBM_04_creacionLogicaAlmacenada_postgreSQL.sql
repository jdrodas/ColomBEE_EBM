-- Scripts de clase - 26 Agosto de 2026 
-- Curso de Tópicos Avanzados de base de datos - UPB 202620
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: ColomBEE_EBM
-- Motor de Base de datos: PostgreSQL 18.x

-- Con el usuario ebm_app

-- ****************************************
-- Creación de procedimientos almacenados
-- ****************************************

-- ### Apiarios ####

-- p_inserta_apiario
create or replace procedure core.p_inserta_apiario(
                            in p_nombre          varchar,
                            in p_latitud         double precision,
                            in p_longitud        double precision)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if  p_nombre is null or
            length(p_nombre) = 0 then        
               raise exception 'El nombre del apiario es nulo o inválido.';
        end if;

        if  p_latitud is null or
            p_longitud is null or
            p_latitud not between -90 and 90 or
            p_longitud not between -180 and 180 then        
               'Los valores de latitud o longitud no estan en los rangos válidos o son nulos.';
        end if;        

        select count(id) into l_total_registros
        from core.apiarios
        where upper(p_nombre) = upper(nombre)
        and p_latitud = latitud
        and p_longitud = longitud;

        if l_total_registros != 0  then
            raise exception 'ya existe ese apiario registrado con ese nombre y esas coordenadas geográficas';
        end if;

        insert into core.apiarios (nombre,latitud, longitud)
        values (initcap(p_nombre), p_latitud, p_longitud);
    end;
$$;

-- p_actualiza_apiario
create or replace procedure core.p_actualiza_apiario(
                            in p_id              uuid,
                            in p_nombre          varchar,
                            in p_latitud         double precision,
                            in p_longitud        double precision)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if  p_nombre is null or
            length(p_nombre) = 0 then        
               raise exception 'El nombre del apiario es nulo o inválido.';
        end if;

        if  p_latitud is null or
            p_longitud is null or
            p_latitud not between -90 and 90 or
            p_longitud not between -180 and 180 then        
               raise exception 'Los valores de latitud o longitud no estan en los rangos válidos o son nulos.';
        end if;  

        select count(id) into l_total_registros
        from core.apiarios
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un apiario con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.apiarios
        where upper(p_nombre) = upper(nombre)
        and p_latitud = latitud
        and p_longitud = longitud;

        if l_total_registros != 0  then
            raise exception 'ya existe ese apiario registrado con ese nombre y esas coordenadas geográficas';
        end if;

        update core.apiarios
        set
            nombre = initcap(p_nombre),
            latitud = p_latitud,
            longitud = p_longitud
        where id = p_id;
    end;
$$;


-- p_elimina_apiario
create or replace procedure core.p_elimina_apiario(
                            in p_id                     uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_id is null then
               raise exception 'El Id no puede ser nulo.';
        end if;

        select count(id) into l_total_registros
        from core.apiarios
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un apiario con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.colmenas
        where apiario_id = p_id;

        if l_total_registros != 0  then
            raise exception 'No se puede eliminar, hay colmenas registradas que dependen de este apiario.';
        end if;

        delete from core.apiarios
        where id = p_id;
    end;
$$;

-- ### Colmenas ####

-- p_inserta_colmena
create or replace procedure core.p_inserta_colmena(
                            in p_codigo              varchar,
                            in p_apiario_id          uuid,
                            in p_fecha_instalacion   date)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if  p_codigo is null or
            p_apiario_id is null or
            p_fecha_instalacion is null or
            length(p_codigo) = 0 then        
               raise exception 'El codigo del apiario, o de la colmena o la fecha de instalacion son nulos o inválidos.';
        end if;

        select count(id) into l_total_registros
        from core.colmenas
        where upper(p_codigo) = upper(codigo)
        and p_apiario_id = apiario_id
        and p_fecha_instalacion = fecha_instalacion;

        if l_total_registros != 0  then
            raise exception 'ya existe esa colmena registrada con ese codigo, apiario y fecha de instalación.';
        end if;

        insert into core.colmenas (codigo, apiario_id,fecha_instalacion)
        values (initcap(p_codigo),p_apiario_id,p_fecha_instalacion);
    end;
$$;


-- p_actualiza_colmena
create or replace procedure core.p_actualiza_colmena(
                            in p_id                  uuid,
                            in p_codigo              varchar,
                            in p_apiario_id          uuid,
                            in p_fecha_instalacion   date)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if  p_codigo is null or
            p_apiario_id is null or
            p_fecha_instalacion is null or
            length(p_codigo) = 0 then        
               raise exception 'El codigo del apiario, o de la colmena o la fecha de instalacion son nulos o inválidos.';
        end if;

        select count(id) into l_total_registros
        from core.colmenas
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe una colmena con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.colmenas
        where upper(p_codigo) = upper(codigo)
        and p_apiario_id = apiario_id
        and p_fecha_instalacion = fecha_instalacion;

        if l_total_registros != 0  then
            raise exception 'ya existe esa colmena registrada con ese codigo, apiario y fecha de instalación.';
        end if;

        update core.colmenas
        set
            codigo = initcap(p_codigo),
            apiario_id = p_apiario_id,
            fecha_instalacion = p_fecha_instalacion
        where id = p_id;
    end;
$$;


-- p_elimina_colmena
create or replace procedure core.p_elimina_colmena(
                            in p_id                     uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_id is null then
               raise exception 'El Id no puede ser nulo.';
        end if;

        select count(id) into l_total_registros
        from core.colmenas
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe una colmena con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.sensores
        where colmena_id = p_id;

        if l_total_registros != 0  then
            raise exception 'No se puede eliminar, hay sensores registrados que dependen de esta colmena.';
        end if;

        delete from core.colmenas
        where id = p_id;
    end;
$$;

-- ### Tipos_sensores ####

-- p_inserta_tipo_sensor
create or replace procedure core.p_inserta_tipo_sensor(
                            in p_nombre                 varchar,
                            in p_unidad_medida          varchar)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if  p_nombre is null or
            p_unidad_medida is null or
            length(p_nombre) = 0 or
            length(p_unidad_medida) = 0 then        
               raise exception 'El nombre o la unidad de medida del tipo de sensor son nulos o inválidos.';
        end if;

        select count(id) into l_total_registros
        from core.tipos_sensores
        where upper(p_nombre) = upper(nombre)
        and upper(p_unidad_medida) = upper(unidad_medida);

        if l_total_registros != 0  then
            raise exception 'ya existe ese tipo de sensor registrado con ese nombre y unidad de medida';
        end if;

        insert into core.tipos_sensores (nombre,unidad_medida)
        values (initcap(p_nombre), initcap(p_unidad_medida));
    end;
$$;


-- p_actualiza_tipo_sensor
create or replace procedure core.p_actualiza_tipo_sensor(
                            in p_id                     uuid,
                            in p_nombre                 varchar,
                            in p_unidad_medida          varchar)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if  p_nombre is null or
            p_unidad_medida is null or
            length(p_nombre) = 0 or
            length(p_unidad_medida) = 0 then        
               raise exception 'El nombre o la unidad de medida del tipo de sensor son nulos o inválidos.';
        end if;

        select count(id) into l_total_registros
        from core.tipos_sensores
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un tipo de sensor con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.tipos_sensores
        where upper(p_nombre) = upper(nombre)
        and upper(p_unidad_medida) = upper(unidad_medida);

        if l_total_registros > 0  then
            raise exception 'ya existe ese tipo de sensor registrado con ese nombre y unidad de medida';
        end if;

        update core.tipos_sensores
        set
            nombre = initcap(p_nombre),
            unidad_medida = initcap(p_unidad_medida)
        where id = p_id;
    end;
$$;

-- p_elimina_tipo_sensor
create or replace procedure core.p_elimina_tipo_sensor(
                            in p_id                     uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_id is null then
               raise exception 'El Id no puede ser nulo.';
        end if;

        select count(id) into l_total_registros
        from core.tipos_sensores
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un tipo de sensor con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.sensores
        where tipo_id = p_id;

        if l_total_registros != 0  then
            raise exception 'No se puede eliminar, hay sensores registrados que dependen de este tipo sensor.';
        end if;

        delete from core.tipos_sensores
        where id = p_id;
    end;
$$;