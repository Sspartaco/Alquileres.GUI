namespace Alquileres.GUI.Models
{
    public class PropertyVM
    {

        public PropertyEntity PropertyEntity
        { get; set; }

        public OwnerEntity OwnerEntity
        { get; set; }

        public OwnerEntity[]? Owners
        { get; set; }

        public PropertyEntity[]? Property
        { get; set; }

        public string Response
        { get; set; }
    }
}
