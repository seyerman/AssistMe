using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Admin : User
    {
        private List<User> listUser;
        private List<Question> listElements;


        public Admin(int iD, string eMAIL, string pHOTO, int qUESTIONS_ANSWERED, int pOSITIVE_VOTES_RECEIVED, int qUESTIONS_ASKED, int iNTERESTING_VOTES_RECEIVED, string dESCRIPTION, string iNTERESTS_OR_KNOWLEDGE, string cOUNTRY, string cITY, bool administrador) : base(iD, eMAIL, pHOTO, qUESTIONS_ANSWERED, pOSITIVE_VOTES_RECEIVED, qUESTIONS_ASKED, iNTERESTING_VOTES_RECEIVED, dESCRIPTION, iNTERESTS_OR_KNOWLEDGE, cOUNTRY, cITY)
        {
    
        }

        public String addAdmin(int id)
        {
            String mensaje = "";
            List<User> buscado = listUser.Where(x => x.ID == id && x.LEVEL != User.LEVEL_ADMIN).ToList();
            if (buscado.First().Equals(null))
            {
                mensaje = "El usuario ya es un administrador";
            }
            else
            {
                buscado.First().LEVEL = User.LEVEL_ADMIN;
                mensaje = "Se agrego correctamente el administrador";
            }
            return mensaje;
        }

        public String removeQuestions(int idEle)
        {
            String mensaje = "";

            List<Question> preguntas = listElements.Where(x => x.Id == idEle).ToList();
            if (preguntas.First().Equals(null))
            {
                mensaje = "No se encontro la pregunta a eliminar";
            }
            else
            {
                listElements.Remove(preguntas.First());
                mensaje = "Se elimino correctamente la pregunta";
            }
            return mensaje;
        }

        public void generateReport()
        {


        }

        public int[] numQuestionsAndAnswers()
        {
            //info[0] = num question / info[1] = num answer
            int[] info = new int[2];
            int counter = 0;
            listElements.Where(x => x.GetType().ToString().Equals("AssistMeProject.Question")).ToList().ForEach(y => counter++);
            info[0] = counter;
            counter = 0;
            listElements.Where(x => x.GetType().ToString().Equals("AssistMeProject.Answer")).ToList().ForEach(y => counter++);
            info[1] = counter;

            return info;
        }

        //   public List<Question> filteringQuestions(String filter, String info)
        //   {
        //List<Question> listQuestion = new List<Question>();

        //  switch (filter)
        //{
        //  case "Etiqueta":
        //listQuestion = listElements.Where(x => x.GetType().ToString().Equals("AssistMeProject.Question")).Cast<Question>().Where(y => y.labels.Any(z => z.Tag.Equals(info))).ToList();
        //          break;
        //    case "Creador":
        // listQuestion = listUser.Where(x => x.ID == Int32.Parse(info)).First().elements.Where(y => y.GetType().ToString().Equals("AssistMeProject.Question")).Cast<Question>().ToList();
        //           break;
        //     default:
        //listQuestion = listElements.Where(x => x.GetType().ToString().Equals("AssistMeProject.Question")).Cast<Question>().Where(y => y.studios.Any(z => z.name.Equals(info))).ToList();
        //          break;
        //}

        //      return listQuestion;
        //}
    }

}
