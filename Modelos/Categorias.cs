using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CursoEntityFrameworkPractica;


public class Categoria
{

	//[Key] // Le estamos diciendo que use lo de abajo como clave
	public Guid CategoriaId { get; set; }

	//[Required] // Poniendo este dataAnotation estamos haciendo que esta propiedad sea requerida al momento de insertar un nuevo registro dentro de la tabala categorias
	//[MaxLength(150)] // Solo ocupara esta cantidad de caracteres
	public string Nombre { get; set; }	

	public string Descripcion { get; set; }	

	public int Peso { get; set; }

	[JsonIgnore]
	public virtual ICollection<Tarea> Tareas { get; set; }	
}

// Cada tarea va a tener asignada una categoria. Si yo quisiera dentro de la consulta que haga,
// traer la categoria que está asociada a ese ID, puedo utilizar la propiedad virtual categoria
// e incluir toda la informacion que tiene esa categoria
// Tambien puedo usar en categoria, la propiedad virtual de tareas para traer todas las tareas que
// estan asociadas a la categoria que estoy buscando 