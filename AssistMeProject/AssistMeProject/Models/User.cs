using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class User : ICloneable 
    {

        public const int LEVEL_ROOT = 1;
        public const int LEVEL_ADMIN = 2;
        public const int LEVEL_NORMAL = 3;
       

        public User(int iD, string gOOGLE_KEY, int lEVEL, string uSERNAME, string pASSWORD, string eMAIL, string pHOTO, int qUESTIONS_ANSWERED, int pOSITIVE_VOTES_RECEIVED, int qUESTIONS_ASKED, int iNTERESTING_VOTES_RECEIVED, string dESCRIPTION, string iNTERESTS_OR_KNOWLEDGE, string cOUNTRY, string cITY, int studioID)
        {
            ID = iD;
            GOOGLE_KEY = gOOGLE_KEY ?? throw new ArgumentNullException(nameof(gOOGLE_KEY));
            LEVEL = lEVEL;
            USERNAME = uSERNAME ?? throw new ArgumentNullException(nameof(uSERNAME));
            PASSWORD = pASSWORD ?? throw new ArgumentNullException(nameof(pASSWORD));
            EMAIL = eMAIL ?? throw new ArgumentNullException(nameof(eMAIL));
            PHOTO = pHOTO ?? throw new ArgumentNullException(nameof(pHOTO));
            QUESTIONS_ANSWERED = qUESTIONS_ANSWERED;
            POSITIVE_VOTES_RECEIVED = pOSITIVE_VOTES_RECEIVED;
            QUESTIONS_ASKED = qUESTIONS_ASKED;
            INTERESTING_VOTES_RECEIVED = iNTERESTING_VOTES_RECEIVED;
            DESCRIPTION = dESCRIPTION ?? throw new ArgumentNullException(nameof(dESCRIPTION));
            INTERESTS_OR_KNOWLEDGE = iNTERESTS_OR_KNOWLEDGE ?? throw new ArgumentNullException(nameof(iNTERESTS_OR_KNOWLEDGE));
            COUNTRY = cOUNTRY ?? throw new ArgumentNullException(nameof(cOUNTRY));
            CITY = cITY ?? throw new ArgumentNullException(nameof(cITY));
            StudioId = studioID;

            this.Comments = new HashSet<Comment>();
            this.Answers = new HashSet<Answer>();
            this.Questions = new HashSet<Question>();
        }

        public User()
        {
            this.Comments = new HashSet<Comment>();
            this.Answers = new HashSet<Answer>();
            this.Questions = new HashSet<Question>();
        }

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

            this.Comments = new HashSet<Comment>();
            this.Answers = new HashSet<Answer>();
            this.Questions = new HashSet<Question>();
            this.Notifications = new HashSet<Notification>();
        }

        public int ID { get; set; }
        public string GOOGLE_KEY { get; set; }
        public int LEVEL { get; set; }
        [Display(Name = "USERNAME")]
        public String USERNAME { get; set; }
        public String PASSWORD { get; set; }
        [Display(Name = "EMAIL")]
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


        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Comment> Comments { get; set;}
        public virtual ICollection<Notification> Notifications { get; set; }

        public int StudioId { get; set; }
        public Studio Studio { get; set; }


        public String[] getStringData()
        {
            String[] data = { ID.ToString(), GOOGLE_KEY, USERNAME,PASSWORD,EMAIL,PHOTO,QUESTIONS_ANSWERED.ToString(),POSITIVE_VOTES_RECEIVED.ToString(),
                QUESTIONS_ASKED.ToString(),INTERESTING_VOTES_RECEIVED.ToString(),DESCRIPTION,INTERESTS_OR_KNOWLEDGE,COUNTRY,CITY};
            return data;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

       
    }
}