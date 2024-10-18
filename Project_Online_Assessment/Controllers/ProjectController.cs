using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
namespace Project_Online_Assessment.Controllers
{
    public class ProjectController : Controller
    {
        //
        // GET: /Project/

        public ActionResult Question_page()
        {
            Project_Online_AssessmentEntities context = new Project_Online_AssessmentEntities();

            List<Subject_table> Subject_list = new List<Subject_table>();
            List<Difficulty_table> Difficulty_list = new List<Difficulty_table>();
            Subject_list = context.Subject_table.ToList();
            Difficulty_list = context.Difficulty_table.ToList();
            ViewBag.Subject_list = new SelectList(Subject_list, "Subject_Id", "Subject_name");
            ViewBag.Difficulty_list = new SelectList(Difficulty_list, "Difficulty_Id", "Difficulty_level");
            return View();
        }

        public JsonResult Add_Question(string Question,string Option)
        {
            Project_Online_AssessmentEntities Context = new Project_Online_AssessmentEntities();

            Question_table Get_question = new Question_table();
            Get_question = JsonConvert.DeserializeObject<Question_table>(Question);
            Context.Question_table.Add(Get_question);
            Context.SaveChanges();


            List<Option_table> Get_option = new List<Option_table>();
            Get_option = JsonConvert.DeserializeObject<List<Option_table>>(Option);

            foreach (var option in Get_option)
            {
                option.Question_Id = Get_question.Question_Id;
                Context.Option_table.Add(option);
                Context.SaveChanges();
            }
           
            
            return Json(JsonRequestBehavior.AllowGet);
            }
            
        }

    }

