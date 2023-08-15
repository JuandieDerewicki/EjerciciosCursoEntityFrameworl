using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoEntityFrameworkPractica;

public class Tarea
{
	//[Key]
	public Guid TareaId { get; set; }

	//[ForeignKey("CategoriaId")] // Ayuda a indicar al modelo de tarea que tiene una relacion con el otro modelo de Categoria a través de este campo
	public Guid CategoriaId { get; set; }

	//[Required]
	//[MaxLength(200)]	

	// Comentamos los que fueron reemplazados por FLUENT API
	public string Titulo { get; set; }

	public string Descripcion { get; set; }

	public Prioridad PrioridadTarea { get; set; }		

	public DateTime FechaCreacion { get; set; }	

	public virtual Categoria Categoria { get; set; }

	// Como no se tiene que mapear solo no la agregamos en el model creation
	//[NotMapped] // Cuando se haga el mapeo de nuestro contexto hacia la bd, omita este campo
	public string Resumen { get; set; }
}

public enum Prioridad
{
	Baja,
	Media, 
	Alta
}