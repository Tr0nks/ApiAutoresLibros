using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "El campo {0} es Requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe de tener mas de {1} caracteres")]
        public string Nombre { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }



    }
}
