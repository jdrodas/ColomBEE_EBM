-- Scripts de clase - 1 Octubre de 2026 
-- Curso de Tópicos Avanzados de base de datos - UPB 202620
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: ColomBEE_EBM
-- Motor de Base de datos: MongoDB - 8.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen oficial
docker pull mongo:latest

-- Crear el contenedor
docker run --name mongodb_ebm \
  -e MONGO_INITDB_ROOT_USERNAME=mongoadmin \
  -e MONGO_INITDB_ROOT_PASSWORD=unaClav3 \
  -e GLIBC_TUNABLES=glibc.pthread.rseq=1 \
  -p 27017:27017 \
  -d mongo:latest
