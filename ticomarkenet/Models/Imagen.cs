using System.Text.Json.Serialization;

namespace ticomarkenet.Models
{
    public class Imagen
    {
        public int ImagenId { get; set; }
        public string Ruta { get; set; }

        public int ProductoId { get; set; }
        [JsonIgnore]
        public Producto? Producto { get; set; }
    }
}
