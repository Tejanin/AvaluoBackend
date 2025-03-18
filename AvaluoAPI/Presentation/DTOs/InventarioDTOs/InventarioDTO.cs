using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Presentation.DTOs.InventarioDTOs
{
    public class InventarioDTO
    {
        [Required(ErrorMessage = "La descripción del inventario es requerida.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El estado del inventario es requerido.")]
        public int IdEstado { get; set; }
    }
}
