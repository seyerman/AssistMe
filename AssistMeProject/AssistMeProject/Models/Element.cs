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
        public string Title { get; set; }
        public string Description { get; set; }
        public string IdUser { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
