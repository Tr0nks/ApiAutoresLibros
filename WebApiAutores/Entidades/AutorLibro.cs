namespace WebApiAutores.Entidades
{
    public class AutorLibro
    {

        // para establecer la relacion de muchos a muchos 
        public int LibroId { get; set; }
        public int AutorId { get; set; }
        public int Orden { get; set; }
        public Libro Libro { get; set; }
        public Autor Autor { get; set; }

    }
}
