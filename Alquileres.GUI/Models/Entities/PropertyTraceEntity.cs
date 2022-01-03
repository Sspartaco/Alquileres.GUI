namespace Alquileres.GUI.Models.Entities
{
    public class PropertyTraceEntity
    {
        /// <summary>
        /// Id seguimiento propiedad
        /// </summary>
        public string IdPropertyTrace { get; set; }

        /// <summary>
        /// Fecha Venta
        /// </summary>
        public DateTime DateSale { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Impuesto
        /// </summary>
        public decimal Tax { get; set; }

    }
}
