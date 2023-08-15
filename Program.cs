using CursoEntityFrameworkPractica;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Configuracion general de ef para conectarse a la bd en memoria
//builder.Services.AddDbContext<TareasContext>(p => p.UseInMemoryDatabase("TareasDB"));
//Configuracion de ef para conectarse a la bd de sql server donde hacemos el mismo mapeo con tareas context
//builder.Services.AddSqlServer<TareasContext>("Data Source=localhost\\SQLEXPRESS;Initial Catalog=TareasDb:user id=sa;password=44276706jdd");
builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("cnTareas")); // Hace lo mismo pero ahora tenemos mas seguridad al hacerlo pq no lo hacemos desde el codigo sino desde appsettings.json

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", async ([FromServices] TareasContext dbContext) =>
{
    dbContext.Database.EnsureCreated(); // Nos asegura si la bd esta creada y sino la crea
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());
    // Para probar si los modelos que creamos estan funcionando
});

// Emtpoint para poder consumir tareas y obtener datos de la tabla tarea
// Mapeamos la ruta con la que vamos a invocar el metodo, luego ponemos el metodo asincrono y luego de la configuracion de los servicios que esta mas arriba, nos traemos TareasContext que tiene toda la conf de EF
app.MapGet("/api/tareas", async ([FromServices] TareasContext dbContext) =>
{
    // Se traen todos los datos que existan en la tabla tareas, no es una filtracion
    //return Results.Ok(dbContext.Tareas);
    // Filtro para traer por prioridad de tarea baja
    //return Results.Ok(dbContext.Tareas.Where(p => p.PrioridadTarea == Prioridad.Baja));
    // Filtro para traer datos relacionados compartidos que los pones como null
    return Results.Ok(dbContext.Tareas.Include(p => p.Categoria));
    // Esto da error porque el modelo de categorias tiene la lista de tareas, entonces cuando intentamos encontrar la categoria dentro de tareas tambien va a incluir las tareas que tiene la categoria. Entonces le agregamos un jsonIgnore en la coleccion de tareas
});

// Para guardar datos con EF
// Cambiamos el mapeo para que sea tipo post que se utiliza para agregar un dato
// A lo ultimo especificamos el modelo o los datos que vamos a recibir para poder crear el nuevo registro en la bd
app.MapPost("/api/tareas", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea) =>
{
    tarea.TareaId = Guid.NewGuid(); // Garantizamos que se genera un nuevo id
    tarea.FechaCreacion = DateTime.Now;
    await dbContext.AddAsync(tarea); // Usamos el await porque tenemos un metodo asincrono
    //await dbContext.Tareas.AddAsync(tarea); Esta es otra manera yendo directamente a la coleccion de tareas y agregamos una tarea con un metodo asincrono

    await dbContext.SaveChangesAsync(); // Para confirmar que los cambios se guarden en la bd

    return Results.Ok(); // Se devuelve esto pq si hay un error no se alcanzaria a ejecutar  
    //return Results.Ok(dbContext.Tareas.Include(p => p.Categoria).Where(p => p.PrioridadTarea == Prioridad.Baja));
});


// Para actualizar datos con EF
// Cambiamos el mapeo para que sea tipo put que se utiliza para actualizar un dato
// A lo ultimo especificamos el modelo o los datos que vamos a recibir para poder crear el nuevo registro en la bd
// Obtener desde la ruta con FromRoute el id que se está utilizando. Entonces vamos a usar ese atributo para obtener desde la URL el id y con ese id vamos a buscar el elemento que tenemos que actualizar
app.MapPut("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea, [FromRoute] Guid id) =>
{
    // Buscar tarea actual con find donde buscamos por id, busca la que esta marcada por key
    var tareaActual = dbContext.Tareas.Find(id);
    // cuando encontramos la tarea ya estamos listos para hacer el mapeo y asignar los datos actualizados
    if (tareaActual != null)
    {
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo = tarea.Titulo;  
        tareaActual.PrioridadTarea = tarea.PrioridadTarea;
        tareaActual.Descripcion = tarea.Descripcion;

        await dbContext.SaveChangesAsync(); // Confirmar

        return Results.Ok();
    }

    return Results.NotFound(); 
});


app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id) =>
{
    var tareaActual = dbContext.Tareas.Find(id);
    if (tareaActual != null)
    {
        dbContext.Remove(tareaActual); // Siempre que hacemos un  cambio sobre un contexto debemos confirmar el cambio con por ejemplo lo siguiente:

        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
});


app.Run();

// Configurar modelos con funciones que nos permiten gestionar restricciones o validaciones para nuestros modelos usando FLUENT API
