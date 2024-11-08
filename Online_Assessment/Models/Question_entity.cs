using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Assessment.Models
{
    public class Question_entity
    {
        public int Question_Id { get; set; }
        public string Questions { get; set; }
        public string Subject_name { get; set; }
        public string Difficulty_level { get; set; }
    }

    public class User_credential
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Test_lists
    {
        public string Test_name { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public TimeSpan Duration { get; set; } 
    }
}