using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Pregunta : Elemento
    {
        //private  adj; 

        public Boolean IsArchived { get; set; }

        private List<Respuesta> ListRespuesta { get; set; }

    }
}
