# JUpload

[Español](#español) · [English](#english)

## Español

Sitio para intercambiar archivos y consultar su recepción y su revisión. Sirve cuando una persona debe entregar un archivo y otra necesita encontrarlo y conocer su estado.

### Qué puedes hacer

- Envía archivos desde el navegador con formularios de carga.
- Consulta los registros y el estado de recepción o revisión.
- Explora las variantes del sitio en la rama `respaldos`.

### Ejemplo

Trabaja con un archivo de texto ficticio de pocas líneas. Sigue desde la pantalla de carga hasta la consulta del archivo recibido.

### Cómo empezar

El sitio usa ASP.NET de su época. Revisa las páginas y su código en un entorno Windows compatible con .NET Framework. Para una ejecución local, identifica las conexiones que usa la aplicación y prepara una base vacía con datos de prueba. Las conexiones, recursos y servicios originales se gestionan por separado.

Puntos de entrada en el código. [`aprobacion.aspx`](aprobacion.aspx), [`archivo.aspx`](archivo.aspx), [`envio.aspx`](envio.aspx), [`renewSes.aspx`](renewSes.aspx).

Las fechas del historial se apoyan en Git o en la evidencia disponible de los respaldos. Las fechas reconstruidas a partir de archivos sin zona horaria usan las 12:00 UTC como referencia.

## English

A site for exchanging files and checking reception and review status. It helps when one person supplies a file and another needs to find it and check what happened.

### What you can do

- Submit files from a browser using upload forms.
- Look up records and check whether a file was received or reviewed.
- Explore older site variants in the `respaldos` branch.

### Example

Use a short fictional text file. Trace it from the upload screen to the received-file record.

### Getting started

The site uses historical ASP.NET. Review its pages and code in a Windows environment compatible with .NET Framework. To run locally, identify the connections the application uses and prepare an empty database with test data. Original connections, resources, and services are managed separately.

Entry points in the source. [`aprobacion.aspx`](aprobacion.aspx), [`archivo.aspx`](archivo.aspx), [`envio.aspx`](envio.aspx), [`renewSes.aspx`](renewSes.aspx).

History dates come from Git or available backup evidence. Reconstructed dates from files with no time zone use 12:00 UTC as a reference.
