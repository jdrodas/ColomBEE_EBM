// Scripts de clase - 1 Octubre de 2026 
// Curso de Tópicos Avanzados de base de datos - UPB 202620
// Juan Dario Rodas - juand.rodasm@upb.edu.co

// Proyecto: ColomBEE_EBM
// Motor de Base de datos: MongoDB - 8.x

// ****************************************
// Creación de base de datos y usuarios
// ****************************************

// Para conectarse al contenedor con el usuario admin 
mongodb://mongoadmin:{ponga_aqui_su_clave}@localhost:27017/

// crear la base de datos
use ebm_db;

// Crear el rol para el usuario de gestion de Documentos en las colecciones
db.createRole(
  {
    role: "GestorDocumentos",
    privileges: [
        {
            resource: { 
                db: "ebm_db", 
                collection: "" 
            }, 
            actions: [
                "find", 
                "insert", 
                "update", 
                "remove",
                "listCollections"
            ]
        }
    ],
    roles: []
  }
);

db.createRole(
  {
    role: "LectorDocumentos",
    privileges: [
        {
            resource: { 
                db: "ebm_db", 
                collection: "" 
            }, 
            actions: [
                "find", 
                "listCollections"
            ]
        }
    ],
    roles: []
  }
);


// Crear usuario para gestionar el modelo
db.createUser({
  user: "ebm_app",
  pwd: "unaClav3",  
  roles: [
    { role: "readWrite", db: "ebm_db" },
    { role: "dbAdmin", db: "ebm_db" }
  ],
    mechanisms: ["SCRAM-SHA-256"]
  }
);

db.createUser(
  {
    user: "ebm_usr",
    pwd: "unaClav3",
    roles: [ 
    { role: "GestorDocumentos", db: "ebm_db" }
    ],
    mechanisms: ["SCRAM-SHA-256"]
  }
);

db.createUser(
  {
    user: "ebm_qry",
    pwd: "unaClav3",
    roles: [ 
    { role: "LectorDocumentos", db: "ebm_db" }
    ],
    mechanisms: ["SCRAM-SHA-256"]
  }
);