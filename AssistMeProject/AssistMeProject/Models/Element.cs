using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Element : IComparable
    {


        public int Id { get; set; }

        [Required(ErrorMessage ="Agregue un texto como descripción"),Display(Name ="Descripción")]
        public string Description { get; set; }

        [DataType(DataType.Date),Display(Name ="Fecha")]
        public DateTime Date { get; set; }

        [DataType(DataType.Time),Display(Name ="Hora")]
        public DateTime Hour { get; set; }

        // Sin Implementar
        //public int UserId { get; set; }
        //public virtual User User { get; set; }




        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
