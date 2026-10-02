// Scripts de clase - 1 Octubre de 2026 
// Curso de Tópicos Avanzados de base de datos - UPB 202620
// Juan Dario Rodas - juand.rodasm@upb.edu.co

// Proyecto: ColomBEE_EBM
// Motor de Base de datos: MongoDB - 8.x

// ****************************************
// Creación del modelo de datos
// ****************************************

-- Con el usuario pesca_app

-- String de conexión:
mongodb://ebm_app:{ponga_su_clave}@{ponga_aqui_host}:{ponga_aqui_puerto}/?authMechanism={ponga_aqui_mecanismo}&authSource={ponga_aqui_authSource}

-- *******************************************************
-- Creación de Colecciones -- Sin JSON Schema Validator
-- *******************************************************

use ebm_db;

db.createCollection("colmenas");
db.createCollection("tipos_sensores");
db.createCollection("apiarios");
db.createCollection("sensores");
db.createCollection("lecturas");

-- ********************************************************
-- Sentencias de migración desde modelo relacional
-- ********************************************************

-- Crear colección básica e importar json inicial para luego 
-- relacionar los ObjectId de apiarios, colmenas, tipos_sensores, sensores

-- Actualizar el ObjectId de apiario en colección de colmenas
db.colmenas.find().forEach(function(unaColmena){
    let unApiario = db.apiarios.findOne({"apiario_id":unaColmena.apiario_id});

    if(unApiario){
        db.colmenas.updateOne(
            {_id:unaColmena._id},
            {$set:{"apiario_objectId": unApiario._id}}
        );
    }
});

-- Actualizar el ObjectId de tipo de sensor en colección de sensores
db.sensores.find().forEach(function(unSensor){
    let unTipoSensor = db.tipos_sensores.findOne({"tipo_sensor_id":unSensor.tipo_sensor_id});

    if(unTipoSensor){
        db.sensores.updateOne(
            {_id:unSensor._id},
            {$set:{"tipo_sensor_objectId": unTipoSensor._id}}
        );
    }
});

-- Actualizar el ObjectId de la colmena en colección de sensores
db.sensores.find().forEach(function(unSensor){
    let unaColmena = db.colmenas.findOne({"colmena_id":unSensor.colmena_id});

    if(unaColmena){
        db.sensores.updateOne(
            {_id:unSensor._id},
            {$set:{"colmena_objectId": unaColmena._id}}
        );
    }
});


-- Actualizar el ObjectId del sensor en colección de lecturas
db.lecturas.find().forEach(function(unLectura){
    let unSensor = db.sensores.findOne({"sensor_id":unLectura.sensor_id});

    if(unSensor){
        db.lecturas.updateOne(
            {_id:unLectura._id},
            {$set:{"sensor_objectId": unSensor._id}}
        );
    }
});

-- Renombrar el campo "colmena_codigo" a "nombre" en la coleccion colmenas
db.colmenas.updateMany(
    {},
    { $rename: { "colmena_codigo": "nombre" } }
);


-- Para quitar campos que ya no necesitamos en las colecciones
-- Colección apiarios
db.apiarios.updateMany({},{$unset:{apiario_id:""}});

-- Colección colmenas
db.colmenas.updateMany(
  {},
  {$unset:{
    apiario_id:"",
    colmena_id:""
    }
  }
);

-- Colección tipos_sensores
db.tipos_sensores.updateMany({},{$unset:{tipo_sensor_id:""}});

-- Colección sensores
db.sensores.updateMany(
  {},
  {$unset:{
    colmena_id:"",  
    sensor_id:"",
    tipo_sensor_id:""
    }
  }
);

-- Colección lecturas
db.lecturas.updateMany(
  {},
  {$unset:{
    lectura_id:"",
    sensor_id:""
    }
  }
);

-- En la colección lecturas, cambiar el campo "fecha_registro" de tipo string a tipo date
db.lecturas.find().forEach(function(unRegistro){
    let fechaDate = new Date(unRegistro.fecha_registro);
    db.lecturas.updateOne(
        {_id:unRegistro._id},
        {$set:{"fecha_registro": fechaDate}}
    );
}); 

-- ********************************************************
-- Agregar json validator a las colecciones existentes
-- ********************************************************
-- Agregar json validator a la colección apiarios
db.runCommand({
  collMod: "apiarios",
  validator: {
  "$jsonSchema": {
    "bsonType": "object",
    "required": [
      "_id",
      "latitud",
      "longitud",
      "nombre"
    ],
    "properties": {
      "_id": {
        "bsonType": "objectId"
      },
      "latitud": {
        "bsonType": "double"
      },
      "longitud": {
        "bsonType": "double"
      },
      "nombre": {
        "bsonType": "string"
      }
    },
    additionalProperties: false
  }
  }
}
);     

-- Agregar json validator a la colección colmenas
db.runCommand({
  collMod: "colmenas",
  validator: {
  "$jsonSchema": {
    "bsonType": "object",
    "required": [
      "_id",
      "apiario_objectId",
      "fecha_instalacion",
      "nombre"
    ],
    "properties": {
      "_id": {
        "bsonType": "objectId"
      },
      "apiario_objectId": {
        "bsonType": "objectId"
      },
      "fecha_instalacion": {
        "bsonType": "string"
      },
      "nombre": {
        "bsonType": "string"
      }
    },
    additionalProperties: false
  }
}
}
);

-- Agregar json validator a la colección tipos_sensores
db.runCommand({
  collMod: "tipos_sensores",
  validator: {
  "$jsonSchema": {
    "bsonType": "object",
    "required": [
      "_id",
      "unidad_medida",
      "nombre"
    ],
    "properties": {
      "_id": {
        "bsonType": "objectId"
      },
      "unidad_medida": {
        "bsonType": "string"
      },
      "nombre": {
        "bsonType": "string"
      }
    },
    additionalProperties: false
  }
  }
}
);

-- Agregar json validator a la colección sensores
db.runCommand({
  collMod: "sensores",
  validator: {
  "$jsonSchema": {
    "bsonType": "object",
    "required": [
      "_id",
      "colmena_objectId",
      "tipo_sensor_objectId",
      "fecha_instalacion",
      "frecuencia_muestreo"
    ],
    "properties": {
      "_id": {
        "bsonType": "objectId"
      },
      "colmena_objectId": {
        "bsonType": "objectId"
      },
      "tipo_sensor_objectId": {
        "bsonType": "objectId"
      },
      "fecha_instalacion": {
        "bsonType": "string"
      },
      "frecuencia_muestreo": {
        "bsonType": "int"
      }
    },
    additionalProperties: false
  }
  }
}
);

-- Agregar json validator a la colección lecturas
db.runCommand({
  collMod: "lecturas",
  validator: {
  "$jsonSchema": {
    "bsonType": "object",
    "required": [
      "_id",
      "sensor_objectId",
      "fecha_registro",
      "valor"
    ],
    "properties": {
      "_id": {
        "bsonType": "objectId"
      },
      "sensor_objectId": {
        "bsonType": "objectId"
      },
      "fecha_registro": {
        "bsonType": "date"
      },
      "valor": {
        "bsonType": 
            [
            "double",
            "int"
            ]        
      }
    },
    additionalProperties: false
  }
  }
}
);