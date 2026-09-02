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