namespace ticomarkenet.Models
{
    public class Usuario
    {

        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Rol { get; set; }


        // Relación con Producto
        public List<Producto> Productos { get; set; }
    }
}
