using Alquileres.GUI.Models.Entities;

namespace Alquileres.GUI.Models
{
    public class FullPropertyVM
    {
        public PropertyImageEntity PropertyImage
        { get; set; }

        public PropertyTraceEntity PropertyTrace
        { get; set; }

        public PropertyEntity PropertyEntity
        { get; set; }

        public string Response { get; set; }
    }
}
