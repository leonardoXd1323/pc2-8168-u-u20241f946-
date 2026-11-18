Openpay Platform - Settlements API (PC2)
Descripción del proyecto

Openpay Platform es una API REST desarrollada con ASP.NET Core que permite registrar órdenes de desembolso para transferir fondos a tiendas afiliadas. Este proyecto fue desarrollado como parte de la Práctica Calificada 2 (PC2) del curso Aplicaciones Web.

La API cuenta con un endpoint principal que permite registrar órdenes de desembolso, validando los datos recibidos y aplicando las reglas de negocio correspondientes.

Datos del estudiante

Nombre: Sebastian Lopez
Código: U20241F946
Curso: Aplicaciones Web (1ASI0730)
NRC: 8168
Evaluación: Práctica Calificada 2 (PC2)

Tecnologías utilizadas

Para el desarrollo del proyecto se utilizó C# con .NET 10 y ASP.NET Core. También se implementó Entity Framework Core para trabajar con la base de datos MySQL.

Además, se utilizó OpenAPI y Scalar para documentar y probar los endpoints de la API. El proyecto también cuenta con soporte para varios idiomas, como inglés y español.

Funcionamiento del proyecto

El sistema permite registrar órdenes de desembolso mediante una solicitud POST al endpoint /api/v1/disbursement-orders.

Para registrar una orden se deben enviar datos como el código de seguimiento, el identificador de la tienda afiliada, las fechas de programación, el monto máximo de transferencia, la moneda y el usuario responsable.

El sistema valida que los datos sean correctos antes de guardar la información en la base de datos. También verifica que las fechas sean válidas, que el monto no sea negativo y que el código de seguimiento no esté registrado anteriormente.

Cada orden registrada recibe un identificador generado automáticamente. Por defecto, su estado inicial es Queued.

Estados de las órdenes

Las órdenes de desembolso pueden tener diferentes estados dependiendo de su situación. Estos son Queued, Processed, Cancelled y Failed, que representan una orden pendiente, procesada, cancelada o fallida.

Base de datos

El proyecto utiliza MySQL para almacenar la información. La base de datos se llama openpay_settlements y contiene la tabla disbursement_orders, donde se guardan las órdenes registradas.

También se implementó una validación para evitar que se registren dos órdenes con el mismo código de seguimiento.

Ejecución del proyecto

Para ejecutar el proyecto es necesario tener instalado .NET 10 SDK y contar con un servidor MySQL activo.

Primero se debe configurar la conexión a la base de datos en el archivo appsettings.Development.json. Luego se puede iniciar la API utilizando el siguiente comando:

bash
dotnet run --project PC28168.U20241F946.Openpay.Platform


Una vez iniciada la aplicación, se puede acceder a la documentación interactiva mediante la siguiente dirección:

http://localhost:5128/scalar

Desde allí es posible revisar y probar el funcionamiento del endpoint y sus diferentes respuestas.

Conclusión

Este proyecto permite aplicar los conocimientos adquiridos durante el curso mediante el desarrollo de una API REST utilizando .NET, C# y MySQL. Se implementaron reglas de negocio, validaciones, almacenamiento de información y documentación de los servicios, siguiendo una arquitectura organizada por capas.
