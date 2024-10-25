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
}