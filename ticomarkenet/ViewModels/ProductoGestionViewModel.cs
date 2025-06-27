using System.Collections.Generic;
using ticomarkenet.Models;

namespace ticomarkenet.ViewModels
{
    public class ProductoGestionViewModel
    {
        public List<Producto> Productos { get; set; }
        public Producto Producto { get; set; } = new Producto();
        public List<Usuario> Usuarios { get; set; }
    }
}
