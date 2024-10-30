using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Online_Assessment.Models;

namespace Online_Assessment.Controllers
{
    public class ProjectController : Controller
    {
        //
        // GET: /Project/

        public ActionResult Test_page()
        {
            return View();
        }

        public JsonResult Create_test(string test)
        {
            Test_table Test_objects = new Test_table();
            Test_objects = JsonConvert.DeserializeObject<Test_table>(test);
            Online_AssessmentEntities context = new Online_AssessmentEntities();
            Test_objects.Created_date = DateTime.Now;
            context.Test_table.Add(Test_objects);
            context.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult Question_create_page()
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();

            List<Subject_table> Subject_list = new List<Subject_table>();
            List<Difficulty_table> Difficulty_list = new List<Difficulty_table>();
            Subject_list = context.Subject_table.ToList();
            Difficulty_list = context.Difficulty_table.ToList();
            ViewBag.Subject_list = new SelectList(Subject_list, "Subject_Id", "Subject_name");
            ViewBag.Difficulty_list = new SelectList(Difficulty_list, "Difficulty_Id", "Difficulty_level");
            return View();
        }

        public JsonResult Add_Question(string Questions, string Options)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();

            Question_table New_question = new Question_table();
            New_question = JsonConvert.DeserializeObject<Question_table>(Questions);
            context.Question_table.Add(New_question);
            context.SaveChanges();


            List<Option_table> New_options = new List<Option_table>();
            New_options = JsonConvert.DeserializeObject<List<Option_table>>(Options);

            foreach (var option in New_options)
            {
                option.Question_Id = New_question.Question_Id;
                context.Option_table.Add(option);
                context.SaveChanges();
            }


            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_test_lists()
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();

            List<Test_table> Test_list = new List<Test_table>();
            Test_list = context.Test_table.ToList();
            return Json(Test_list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Question_map_page(int Id)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();

            List<Subject_table> Subject_list = new List<Subject_table>();
            List<Difficulty_table> Difficulty_list = new List<Difficulty_table>();
            Subject_list = context.Subject_table.ToList();
            Difficulty_list = context.Difficulty_table.ToList();
            ViewBag.Subject_list = new SelectList(Subject_list, "Subject_Id", "Subject_name");
            ViewBag.Difficulty_list = new SelectList(Difficulty_list, "Difficulty_Id", "Difficulty_level");
            ViewBag.Id = Id;
            return View();
        }

        public JsonResult Get_questions(int Subject_Id, int Difficulty_Id)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();

            List<Question_entity> Question_list = context.Database.SqlQuery<Question_entity>(
                "exec Get_questions @Subject_id,@Difficulty_Id",
                new SqlParameter("@Subject_id", Subject_Id),
                new SqlParameter("@Difficulty_Id", Difficulty_Id)).ToList();

            return Json(Question_list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Map_questions_totest(int Test_Id,string Question_Id)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();

            List<int> Question_id_list = new List<int>();
            Question_id_list = JsonConvert.DeserializeObject<List<string>>(Question_Id).Select(int.Parse).ToList();

            Question_mapping_table Mapping = new Question_mapping_table();

            foreach (var row in Question_id_list)
            {
                Mapping.Test_Id = Test_Id;
                Mapping.Question_Id = row;
                context.Question_mapping_table.Add(Mapping);
                context.SaveChanges();
            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult Invitation_page(int Id)
        {
            ViewBag.Test_Id = Id;
            return View();
        }

        public JsonResult Invite_for_test(int Id,string Invited_emails)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();
            Test_invitation_table Invitation = new Test_invitation_table();
            List<string> Email_list = new List<string>();
            Email_list = JsonConvert.DeserializeObject<List<string>>(Invited_emails).ToList();

            foreach (var row in Email_list)
            {
                Invitation.Test_Id = Id;
                Invitation.User_email = row;
                Invitation.Invited_date = DateTime.Now;
                context.Test_invitation_table.Add(Invitation);
                context.SaveChanges();
            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_invited_users_list(int Id)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();
            List<Test_invitation_table> Invited_list = context.Database.SqlQuery<Test_invitation_table>(
                "exec Get_invited_users_list @Test_id",
                new SqlParameter("@Test_id",Id)).ToList();
            return Json(Invited_list, JsonRequestBehavior.AllowGet);
        }
    }
}
