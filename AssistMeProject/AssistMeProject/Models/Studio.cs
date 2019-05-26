using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Studio : IComparable<Studio>
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Digite el nombre del Studio"), MaxLength(50)]
        public String Name { get; set; }

        [Display(Name = "Unidad")]
        public String Unit { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        [Display(Name = "Email")]
        public String Email { get; set; }


        public virtual ICollection<User> Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Studio()
        {
            Users = new HashSet<User>();
        }

        public Studio( int id, String name, String unit, String description, String email)
        {
            Id = id;
            Name = name;
            Unit = unit;
            Description = description;
            Email = email;
        }

      

       public virtual ICollection<QuestionStudio> QuestionStudios { get; set; }

        public int CompareTo(Studio other)
        {
            int comp1 = -(QuestionStudios.Count - other.QuestionStudios.Count);
            if (comp1 != 0) return comp1;
            return Name.CompareTo(other.Name);
        }
    }
}
