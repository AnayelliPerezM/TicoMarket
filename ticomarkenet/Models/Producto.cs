using static System.Net.Mime.MediaTypeNames;

namespace ticomarkenet.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Precio { get; set; }
        public string Categoria { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Relación con Imagen
        public List<Imagen> Imagenes { get; set; } = new List<Imagen>();
    }
}
