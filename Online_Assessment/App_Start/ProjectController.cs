﻿using System;
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

        public ActionResult Test_create_page()
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

        public JsonResult Get_mapped_questionIds(int Test_Id)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();
            List<int> Mapped_questionIds = new List<int>();
            Mapped_questionIds = context.Database.SqlQuery<int>(
                "exec Get_mapped_questionIds @test_id",
                new SqlParameter("@test_id", Test_Id)).ToList();
            return Json(Mapped_questionIds, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Map_questions_totest(int Test_Id, string Question_Id)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();

            int Test_exist = context.Database.SqlQuery<int>(
                "exec Find_exist_testid @test_id",
                new SqlParameter("@test_id", Test_Id)).SingleOrDefault();

            if (Test_exist == 1)
            {
                int result = context.Database.ExecuteSqlCommand(
                    "exec Delete_existing_questionIds @test_id",
                    new SqlParameter("@test_id", Test_Id));

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
            else
            {
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
        }

        public ActionResult Invitation_page(int Id)
        {
            ViewBag.Test_Id = Id;
            return View();
        }

        public JsonResult Invite_for_test(int Id, string Invited_emails)
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

            //foreach (var user_email in Invited_emails)
            //{
            //    context.Test_invitation_table.Add(new Test_invitation_table { 
            //User_email = user_email,Invited_date = DateTime.Now,Test_Id =Id });}
            //context.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_invited_users_list(int Id)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();
            List<Test_invitation_table> Invited_list = context.Database.SqlQuery<Test_invitation_table>(
                "exec Get_invited_users_list @Test_id",
                new SqlParameter("@Test_id", Id)).ToList();
            return Json(Invited_list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult User_register_page()
        {
            return View();
        }

        public JsonResult Validate_email(string email)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();

            int Result = context.Database.SqlQuery<int>(
                "exec Find_email @Email",
                new SqlParameter("@Email", email)).SingleOrDefault();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Register_user(User_table formdata)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();
            context.User_table.Add(formdata);
            int result = context.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult User_login_page()
        {
            return View(new User_credential());
        }

        public ActionResult User_authentication(User_credential credential)
        {
            if (ModelState.IsValid)
            {
                Online_AssessmentEntities context = new Online_AssessmentEntities();
                User_table Find_user = null;

                foreach (var user in context.User_table)
                {
                    if (user.Email == credential.Email)
                    {
                        Find_user = user;
                        break;
                    }
                }



                if (Find_user != null)
                {
                    if (Find_user.Password == credential.Password)
                    {
                        Session["user_id"] = Find_user.User_Id;
                        Session["user_email"] = Find_user.Email;
                        return RedirectToAction("User_dashboard");

                    }
                    else
                    {
                        TempData["PasswordError"] = "Incorrect password";
                    }
                }
                else
                {
                    TempData["EmailError"] = "Email does not exist";
                }
            }
            else
            {
                TempData["EmailError"] = "Please enter a valid email address.";
                TempData["PasswordError"] = "Password is required.";
            }
            return View("User_login_page", credential);
        }


        public ActionResult User_dashboard()
        {
            return View();
        }

        public JsonResult Get_assigned_testlist()
        {
            var user_email = Session["user_email"];

            Online_AssessmentEntities context = new Online_AssessmentEntities();
            List<Test_lists> Test_list = context.Database.SqlQuery<Test_lists>(
                "exec Get_invited_testlist @email",
                new SqlParameter("@email", user_email)).ToList();

            return Json(Test_list, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Test_start_page(int Id)
        {
            var user_id = Session["user_id"];
            var Test_Id = Id;
            ViewData["Test_Id"] = Test_Id;
            ViewData["user_id"] = user_id;
            return View();
        }

        public ActionResult Live_test_page(int Test_id, int User_id)
        {
            ViewData["Test_Id"] = Test_id;
            ViewData["user_id"] = User_id;
            return View();
        }

        public JsonResult Get_data_for_livetest(int Test_id)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();

            var Test_data = context.Test_table.Find(1);
            var Test_duration = Test_data.Duration;
            return Json(Test_duration, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_questions_for_test(int Test_id)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();
            List<Question_entity_for_test> Question_list = new List<Question_entity_for_test>();
            Question_list = context.Database.SqlQuery<Question_entity_for_test>(
                "exec Questions_for_livetest @test_id",
                new SqlParameter("@test_id", Test_id)).ToList();

            List<Option_table> option = new List<Option_table>();
            Test_table test = new Test_table();
            test = context.Test_table.Find(Test_id);
            List<Question_table> question = new List<Question_table>();

            return Json(Question_list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_options_for_question(int Question_id)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();
            List<Options_enitty_for_test> Option_list = new List<Options_enitty_for_test>();
            Option_list = context.Database.SqlQuery<Options_enitty_for_test>(
                "exec Options_for_livetest @question_id",
                new SqlParameter("@question_id", Question_id)).ToList();
            return Json(Option_list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Collect_answers(int test_id, int user_id, string Question_answer)
        {
            Online_AssessmentEntities context = new Online_AssessmentEntities();
            Answer_table collection_question_answer = new Answer_table();
            context.Answer_table.Add(collection_question_answer);
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}