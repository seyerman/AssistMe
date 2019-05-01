using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Studio
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Studio()
        {

        }

        public Studio( int id, String name, String unit, String description)
        {
            Id = id;
            Name = name;
            Unit = unit;
            Description = description;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Digite el nombre del Studio"), MaxLength(50)]
        public String Name { get; set; }

        [Display(Name = "Unidad")]
        public String Unit { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

       
    }
}
