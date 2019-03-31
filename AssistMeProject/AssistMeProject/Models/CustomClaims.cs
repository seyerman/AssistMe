using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public static class CustomClaims
    {
        public const string USERNAME = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/NAME";
        public const string PASSWORD = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/PASSWORD";
        public const string ID = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/ID";
        public const string EMAIL = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/EMAIL";
        public const string PHOTO = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/PHOTO";
        public const string LEVEL = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/LEVEL";
        public const string QUESTIONS_ANSWERED = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/QUESTIONS_ANSWERED";
        public const string POSITIVE_VOTES_RECEIVED = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/POSITIVE_VOTES_RECEIVED";
        public const string QUESTIONS_ASKED = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/QUESTIONS_ASKED";
        public const string INTERESTING_VOTES_RECEIVED = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/Name";
        public const string INTERESTS_OR_KNOWLEDGE = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/INTERESTING_VOTES_RECEIVED";
        public const string DESCRIPTION = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/DESCRIPTION";
        public const string LOCATION = "http:////schemas.xmlsoap.org/ws/2005/05/identity/claims/LOCATION";
    }
}
