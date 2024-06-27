using Quizzy_Main.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlTypes;

namespace Quizzy_Main.Controllers
{
    public class AdminController : Controller
    {
        public  OnlineExamPortalContext db=new OnlineExamPortalContext();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Admin/AddQuestion/{topicId}/")]
        public IActionResult AddQuestion(int topicId)
        {
            
            
            
            SelectList tps = new SelectList(db.Topics.OrderBy(t => t.TopicName).ToDictionary(t => t.TopicId, t => t.TopicName), "Key", "Value");
            SelectListItem deftop = new SelectListItem(Convert.ToString(topicId), db.Topics.Where(t => t.TopicId == topicId).FirstOrDefault().TopicName);
            ViewBag.Topic = tps.Prepend(deftop);

            
            return View();
        }

        public IActionResult AddTopic()
        {
            if(HttpContext.Session.GetString("UserRole") != null && HttpContext.Session.GetString("UserRole") == "ADMIN")
            {
                User? user = db.Users.Where(u => u.UserId == Convert.ToInt32(HttpContext.Session.GetString("UserID"))).FirstOrDefault();
                ViewBag.topics = new List<Topic>();
                foreach (Topic t in db.Topics.ToList())
                {
                    ViewBag.topics.Add(t);
                }

                ViewBag.users = new List<User>();
                foreach (User u in db.Users.ToList())
                {
                    ViewBag.users.Add(u);
                }
                ViewBag.score = new Dictionary<int, string>();
                foreach (User u in db.Users.ToList())
                {
                    int usrcnt = db.Exams.Where(e => e.UserId == u.UserId).Count();
                    if (usrcnt > 0)
                    {
                        var chck = db.Exams.Where(e => e.UserId == u.UserId).ToList().OrderByDescending(e => e.Finalscore).FirstOrDefault().Finalscore;

                        if (chck != null)
                        {
                            ViewBag.score[u.UserId] = chck.ToString();
                        }
                        else
                        {
                            ViewBag.score[u.UserId] = "0";
                        }
                    }
                    else
                    {
                        ViewBag.score[u.UserId] = "NA";
                    }
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            
        }

        [HttpPost]
        public IActionResult AddTopic(string categoryName) { 
            Topic topic = new Topic();
            topic.TopicName = categoryName;
            db.Topics.Add(topic);
            db.SaveChanges();
            return RedirectToAction("AddTopic");
                
                ; }

        public IActionResult ViewTopic() { return View(db.Topics.ToList()); }

        [HttpPost]
        public IActionResult AddQuestion(Question ques,int topicid,string diff)
        {
            ques.TopicId = topicid;
            ques.DifficultyLevel= diff;
            db.Questions.Add(ques);
            if (ques.CorrectOption==ques.Option1 || ques.CorrectOption == ques.Option2 || ques.CorrectOption == ques.Option3 || ques.CorrectOption == ques.Option4)
            {
                //var len = db.Questions.Count();
                //var ques1 = db.Questions.Where(x => x.QuestionId == 35);
                db.SaveChanges();
                ViewBag.Topic = new SelectList(db.Topics.OrderBy(t => t.TopicName).ToDictionary(t => t.TopicId, t => t.TopicName), "Key", "Value");
                TempData["success"] = "Qusetion Added Succesfully!";
                return RedirectToAction("AddQuestion");
            }
            else
            {
                ViewBag.Topic = new SelectList(db.Topics.OrderBy(t => t.TopicName).ToDictionary(t => t.TopicId, t => t.TopicName), "Key", "Value");
                TempData["error"] = "Correct ans does not match any of the given options!";
                return View(ques);
            }
            
    }
        public IActionResult ViewQuestion()
        {
            TempData["suc"] = "Question edited Successfully";

            return View(db.Questions.ToList());
            
            
        }
        public IActionResult EditQuestion(int? id)
        {
            Question? question = db.Questions.FirstOrDefault(item => item.QuestionId == id);
            return View(question);
        }

        [HttpPost]
        public IActionResult EditQuestion(Question ques)
        {

            db.Questions.Update(ques);
            
            
            db.SaveChanges();

            TempData["suc"]= "Question edited Successfully";

            return RedirectToAction("ViewQuestion",
            new
            {
                ques.TopicId
            });

        }

        
        public IActionResult DeleteQuestion(int? id)
        {

            Question? question = db.Questions.Find(id);

            if (question == null)
            {
                return NotFound();
            }

            var resultsToDelete = db.Results.Where(x => x.QuestionId == question.QuestionId);
            db.Results.RemoveRange(resultsToDelete);    

            db.Questions.Remove(question);

            
            db.SaveChanges();

            TempData["success"] = "Question deleted Successfully";

            return RedirectToAction("ViewQuestion");

        }

        public IActionResult ViewHistory(int id, string sortOrder) 
        {
            var user = db.Users.Where(u => u.UserId == id).FirstOrDefault() ;

            IEnumerable<Exam> ex = db.Exams.Where(e => e.UserId == id && e.ExamDuration != null);

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "date" : sortOrder;

            ViewBag.t = new Dictionary<int, string>();


            foreach (Exam e in ex.ToList())
            {
                if (e != null && e.TopicId!=null) { 
                string tpnm = db.Topics.Where(u => u.TopicId == e.TopicId).FirstOrDefault().TopicName.ToString();

                ViewBag.t[Convert.ToInt32(e.TopicId)] = tpnm;
                }


            }
            switch (ViewBag.NameSortParm)
            {
                case "date": return View(ex.ToList().OrderByDescending(t => t.ExamStartDateTime));
                case "score": return View(ex.ToList().OrderByDescending(t => t.Finalscore));
                default: return View(ex.ToList());
            }

        }
        public IActionResult UserDetails(int id)
        {
                ViewBag.user = db.Users.Where(u => u.UserId == id).FirstOrDefault();
                 int usrcnt= db.Exams.Where(u=>u.UserId == id).Count();
            
            if (usrcnt>0)
            {
                var chck = db.Exams.Where(u => u.UserId == id).ToList().OrderByDescending(e => e.Finalscore).FirstOrDefault().Finalscore;
                
                if ( chck != null)
                {
                    ViewBag.score = chck.ToString();
                    
                }
               else
                {
                    ViewBag.score = "0";
                        
                }
            }
            else 
            {
                ViewBag.score = "NA";
            }
                return View();

        }

        public IActionResult DeleteUser(int id) 
        {
            var user=db.Users.Where(u => u.UserId==id).FirstOrDefault();
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("AddTopic");
        }


        [HttpGet("/Admin/ViewQuestion/{topicId}")]
        public IActionResult ViewQuestion(string? topicId)
        {
            ViewBag.topicId = Convert.ToInt32(topicId);
            if (topicId == null || topicId=="")
            {
                return View(db.Questions.ToList());
            }
            var lst= db.Questions.Where(q => q.TopicId == Convert.ToInt32(topicId));
            if (lst== null)
                return View(db.Questions.ToList());
            List<Question> list = lst.ToList();
            if (list.Count() >0)
            {
                foreach(Question q in list) 
                {
                    if (q.CorrectOption == null) 
                    {
                        q.CorrectOption = "";
                    }
                }

                return View(list);
            }
            else
                return Content("Noquestions");
        }
    }
}
