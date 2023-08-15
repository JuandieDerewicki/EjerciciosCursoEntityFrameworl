using Microsoft.EntityFrameworkCore;

namespace CursoEntityFrameworkPractica;

public class TareasContext : DbContext
{
	public DbSet<Categoria> Categorias { get; set; } 
	// Se representan de manera plural estas dos propiedades pq representan toda la coleccion de todos los datos que se encuentran en ese modelo (la tabla de la bd)
	public DbSet<Tarea> Tareas { get; set; }	

	public TareasContext(DbContextOptions<TareasContext> options) : base(options) { } // metodo base del constructor de ef
																					  // No le hacemos implementacion pq solo va a contener los valores base para poder hacer la configuracion de las opciones que podamos crear a futuro para ef

	// Esto hacemos para poder usar FLUENT
	// Los metodos override son protected siempre 
	// Metodo interno de dbcontext que invoca para el diseño de la BD
	// Aca diseñamos el modelo de categoria y las restricciones dependiendo lo que declaramos en la clase
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//Coleccion para crear data inicial que queremos, creando coleccion de categoria vamos agregar datos inciiales a la tabla categoria
		List<Categoria> categoriasInit = new List<Categoria>();
		// CategoriaId es un tipo Guid.NewGuid para asignar el id pero va a estar cambiando cada vez que se hagan las comparaciones con el modelo actual y los cambios y no es recomendable pq esta cambiando todo el tiempo
		categoriasInit.Add(new Categoria { CategoriaId = Guid.Parse("10bf5d9f-710f-41a6-9d8c-97aef3ad718f"), Nombre = "Actividades pendientes", Peso = 20 });
		categoriasInit.Add(new Categoria { CategoriaId = Guid.Parse("10bf5d9f-710f-41a6-9d8c-97aef3ad7102"), Nombre = "Actividades personales", Peso = 50 });
		modelBuilder.Entity<Categoria>(categoria =>
		{
			categoria.ToTable("Categoria"); // Indicamos que se convierta en una tabla
			categoria.HasKey(p => p.CategoriaId); // Ya no necesitamos la otra key en el modelo de categoria
			categoria.Property(p => p.Nombre).IsRequired().HasMaxLength(150); // Dos validaciones: que es requerido y max de caracteres
			categoria.Property(p => p.Descripcion).IsRequired(false);
			categoria.Property(p => p.Peso);
			categoria.HasData(categoriasInit); // Recibe un vector de categorias y podemos pasar esa lista de datos

			// Es importante especificar para la configuracion general del modelo aunque por debajo lo cree adicionalmente
			// Configuracion del modelo de categoria
		});
		// Fluent va a predominar sobre los atributos porque EF lee los atributos o data-annotations, crea los temas de ejecucion o scripts que necesita y luego ejecuta este metodo. Entonces cuando crea se encuentra con fluent api y sobreescribe usando esa
		// Por eso no es buena practica combinar con los data annotations y borrarlos.

		List<Tarea> tareasInit = new List<Tarea>();
		// Primer registro para la tabla tareas
		tareasInit.Add(new Tarea { TareaId = Guid.Parse("10bf5d9f-710f-41a6-9d8c-97aef3ad7110"), CategoriaId = Guid.Parse("10bf5d9f-710f-41a6-9d8c-97aef3ad718f"), PrioridadTarea = Prioridad.Media, Titulo = "Pago de servicios publicos", FechaCreacion = DateTime.Now, Resumen = "Resumen tarea 1"}); // ID para tareas y para cada tareas una categoria, para asignar esto tenemos que tener los mismos ID creados en categorias asi queda signado con lo mismo 
        tareasInit.Add(new Tarea { TareaId = Guid.Parse("10bf5d9f-710f-41a6-9d8c-97aef3ad7111"), CategoriaId = Guid.Parse("10bf5d9f-710f-41a6-9d8c-97aef3ad7102"), PrioridadTarea = Prioridad.Baja, Titulo = "Terminar de ver pelicula en Netflix", FechaCreacion = DateTime.Now, Resumen = "Resumen tarea 2" }); // El ID del primero cambia a 11 para que tenga diferente, y el otro a 02 para que tenga el mismo que el de categorias


        modelBuilder.Entity<Tarea>(tarea =>
		{
            tarea.ToTable("Tarea"); 
            tarea.HasKey(p => p.TareaId); // Indicamos cual es la clave
			tarea.HasOne(p => p.Categoria).WithMany(p => p.Tareas).HasForeignKey(p => p.CategoriaId); // tarea tiene categorias que pueden contener multiples tareas con with many y a su vez decimos que tiene una clave foranea
            tarea.Property(p => p.Titulo).IsRequired().HasMaxLength(200); 
            tarea.Property(p => p.Descripcion).IsRequired(false);
            tarea.Property(p => p.PrioridadTarea);
            tarea.Property(p => p.FechaCreacion);
			tarea.HasData(tareasInit);
			tarea.Ignore(p => p.Resumen); // esto es solo si no lo ignora previamente
        });
	}
}

// Todo esto es lo unico que tenemos que hacer para que ef tome nuestros modelos como si fueran tablas
// en la base de datos