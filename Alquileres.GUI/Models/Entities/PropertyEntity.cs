namespace Alquileres.GUI.Models
{
    /// <summary>
    /// Entidad correspondiente a una propiedad
    /// </summary>
    public class PropertyEntity
    {

        /// <summary>
        /// Id propiedad
        /// </summary>
        public string IdProperty
        { get; set; }

        /// <summary>
        /// Nombre propiedad
        /// </summary>
        public string Name
        { get; set; }

        /// <summary>
        /// Dirección propiedad
        /// </summary>
        public string Address
        { get; set; }

        /// <summary>
        /// Precio propiedad
        /// </summary>
        public string Price
        { get; set; }

        /// <summary>
        /// Codigo interno propiedad
        /// </summary>
        public string CodeInternal
        { get; set; }

        /// <summary>
        /// Año de la propiedad
        /// </summary>
        public int Year
        { get; set; }

        /// <summary>
        /// Nombre del propietario
        /// </summary>
        public string OwnerName
        { get; set; }

        /// <summary>
        /// Id Owner
        /// </summary>
        public string idOwner
        { get; set; }
    }
}
