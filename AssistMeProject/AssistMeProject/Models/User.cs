using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class User
    {
        public User(int iD, string eMAIL, string pHOTO, int qUESTIONS_ANSWERED, int pOSITIVE_VOTES_RECEIVED, int qUESTIONS_ASKED, int iNTERESTING_VOTES_RECEIVED, string dESCRIPTION, string iNTERESTS_OR_KNOWLEDGE, string cOUNTRY, string cITY)
        {
            ID = iD;
            EMAIL = eMAIL;
            PHOTO = pHOTO;
            QUESTIONS_ANSWERED = qUESTIONS_ANSWERED;
            POSITIVE_VOTES_RECEIVED = pOSITIVE_VOTES_RECEIVED;
            QUESTIONS_ASKED = qUESTIONS_ASKED;
            INTERESTING_VOTES_RECEIVED = iNTERESTING_VOTES_RECEIVED;
            DESCRIPTION = dESCRIPTION;
            INTERESTS_OR_KNOWLEDGE = iNTERESTS_OR_KNOWLEDGE;
            COUNTRY = cOUNTRY;
            CITY = cITY;
        }

        public int ID { get; set; }
        public String USERNAME { get; set; }
        public String PASSWORD { get; set; }
        public String EMAIL { get; set; }
        public String PHOTO { get; set; }
        public int QUESTIONS_ANSWERED { get; set; }
        public int POSITIVE_VOTES_RECEIVED { get; set; }
        public int QUESTIONS_ASKED { get; set; }
        public int INTERESTING_VOTES_RECEIVED { get; set; }
        public String DESCRIPTION { get; set; }
        public String INTERESTS_OR_KNOWLEDGE { get; set; }
        public String COUNTRY { get; set; }
        public String CITY { get; set; }

        public String[] getStringData()
        {
            String[] data = { ID.ToString(), USERNAME,PASSWORD,EMAIL,PHOTO,QUESTIONS_ANSWERED.ToString(),POSITIVE_VOTES_RECEIVED.ToString(),
                QUESTIONS_ASKED.ToString(),INTERESTING_VOTES_RECEIVED.ToString(),DESCRIPTION,INTERESTS_OR_KNOWLEDGE,COUNTRY,CITY};
            return data;
        }

    }
}