namespace Alquileres.GUI.Models.Entities
{
    public class PropertyImageEntity
    {
        /// <summary>
        /// Id propiedad imagen
        /// </summary>
        public string IdPropertyImage { get; set; }

        /// <summary>
        /// Id propiedad (FK)
        /// </summary>
        public string IdProperty { get; set; }

        /// <summary>
        /// Archivo
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// ¿Habilitado?
        /// </summary>
        public bool Enable { get; set; }

    }
}
