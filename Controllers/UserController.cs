using Quizzy_Main.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using System.Security.Cryptography.X509Certificates;

namespace Quizzy_Main.Controllers
{
    public class UserController : Controller
    {
        private static List<Question> filteredQues= new List<Question>();
        private static int quesId;
        private static int exId;
        private readonly IEmailSender emailSender;

        public UserController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        OnlineExamPortalContext db = new OnlineExamPortalContext();

        public string getRandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            return finalString;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(HttpContext.Session.Get("Email") != null)
            {
                return RedirectToAction("Homepage","User");
            } 
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login");
            }
            var user = db.Users.Where(u => u.Email == email).FirstOrDefault();
            if (user != null && user.Password != null && user.Password.Equals(password))
            {

                HttpContext.Session.SetString("Email", email);
                HttpContext.Session.SetString("UserID", user.UserId.ToString());
                HttpContext.Session.SetString("UserRole", user.UserRole.ToString().ToUpper());
                if(user.UserName != null)
                    HttpContext.Session.SetString("Username", user.UserName.ToString());

                return RedirectToAction("Homepage", "User");
            }
            return Content("InCorrect!");
        }

        /*
        public IActionResult UserHomepage()
        {
            var u = db.Users.Where(x => x.UserId == 1).FirstOrDefault();
            return View(u);
        }
        */
        public IActionResult ViewHistory(string sortOrder)
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                int id = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                var user = db.Users.Where(u => u.UserId == id).FirstOrDefault();

                IEnumerable<Exam> ex = db.Exams.Where(e => e.UserId == id && e.ExamDuration != null);

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "date" : sortOrder;

                
                ViewBag.t = new Dictionary<int, string>();
                foreach (Exam e in ex.ToList())
                {
                    if (e != null && e.TopicId != null)
                    {
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
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public IActionResult UserDetail() 
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                int id = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                ViewBag.user = db.Users.Where(u => u.UserId == id).FirstOrDefault();
                int usrcnt = db.Exams.Where(u => u.UserId == id).Count();
                if (usrcnt > 0)
                {
                    var chck = db.Exams.Where(u => u.UserId == id).ToList().OrderByDescending(e => e.Finalscore).FirstOrDefault().Finalscore;

                    if (chck != null)
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
            else
            {
                return RedirectToAction("Login","User");
            }
        }

        [HttpGet("/User/ShowAnswer/{exid}")]
        public IActionResult ShowAnswer(int exid) 
        {
            int xid = exid;
           List<Result> res=db.Results.Where(r => r.ExamId == xid).ToList();
            ViewBag.ques= new Dictionary<int,string>();

            foreach(var item in res) 
            {
                ViewBag.ques[Convert.ToInt32(item.QuestionId)]=db.Questions.Where(q=>q.QuestionId==item.QuestionId).FirstOrDefault().Question1.ToString();
            }
            List<string> resop=new List<string>() ;
            List<string> selop = new List<string>();
            foreach(Result item in res) 
            {
              
                resop.Add((item.CorrectOption != null ? ((item.CorrectOption.ToString() == null) ? "" : item.CorrectOption.ToString()) : ""));
                // (item.SelectedOption.ToString() != null) ? ((item.) ? : ) : ();
                selop.Add((item.SelectedOption != null ? ((item.SelectedOption.ToString() == null) ? "" : item.SelectedOption.ToString()) : ""));
            }
            /*if (db.Results.Where(r => r.ExamId == xid).FirstOrDefault() != null)
            {
                selop = db.Results.Where(r => r.ExamId == xid).SelectedOption.ToString();
            }
            if (db.Results.Where(r => r.ExamId == xid).FirstOrDefault()!= null)
            {
                resop = db.Results.Where(r => r.ExamId == xid).CorrectOption.ToString();
            }*/
            ViewBag.htmlcls = new Dictionary<int,string>();
            int i = 0;
            foreach(Result item in res) 
            {
                if (selop[i]==null)
                {
                    selop[i] = "";
                }
                if (selop[i]!="" && resop[i] == selop[i])
                {
                    ViewBag.htmlcls[Convert.ToInt32(item.QuestionId)] = "selected-option-right";
                }
                else
                {
                    ViewBag.htmlcls[Convert.ToInt32(item.QuestionId)] = "selected-option-wrong";
                }
                i++;
            }

            return View(res);
        }

        public IActionResult SelectQuiz()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                return View(db.Topics.ToList());
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            
        }

        [HttpPost]
        public IActionResult SelectQuiz(string category, string difficulty)
        {

            var examCheck= db.Exams.Where(x => x.Topic.TopicName==category && x.DifficultyLevel==difficulty && x.UserId == Convert.ToInt32(HttpContext.Session.GetString("UserID").ToString())).FirstOrDefault();
            int c = 1;

            if(examCheck != null)
            {
                c =  Convert.ToInt32( examCheck.NoOfAttempts) + 1;
            }
            quesId = 0;
            Exam exam = new Exam();
            exam.ExamStartDateTime = DateTime.Now;
            exam.DifficultyLevel = difficulty;
            exam.NoOfAttempts = c;

            db.Exams.Add(exam);
            db.SaveChanges();
            exId = exam.ExamId;
            var questions = db.Questions.Where(q => q.Topic.TopicName == category && q.DifficultyLevel == difficulty).ToList();
            filteredQues = questions;
            if (filteredQues.Count() == 0)
            {
                ViewBag.disp = "Not Added yet...";
                return RedirectToAction("SelectQuiz");
            }
            int id = questions.ElementAt(quesId).QuestionId;
            quesId++;
            return RedirectToAction("Question",
            new
            {
                id
            });
        }

        public IActionResult Question(int id )
        {
            ViewBag.quesNavs =new  List<List<Question>>();
            for(int i=0; i < filteredQues.Count(); i=i+4)
            {
                var list = new List<Question>();
                list.Add(filteredQues.ElementAt(i));
                if (i + 1 < filteredQues.Count())
                {
                    list.Add(filteredQues.ElementAt(i + 1));
                }
                if(i+2 < filteredQues.Count())
                {
                    list.Add(filteredQues.ElementAt(i + 2));
                }
                if(i+3 < filteredQues.Count())
                {
                    list.Add(filteredQues.ElementAt(i + 3));
                }
               ViewBag.quesNavs.Add(list);
            }
            Question? q=db.Questions.Where(x=>x.QuestionId == id).FirstOrDefault();
            if (q == null)return NotFound();
            return View(q);
        }

        [HttpPost]
        public IActionResult Question(string selectedOption, string correctOption,int questionId)
        {
            Question? q1=db.Questions.Where(x=>x.QuestionId==questionId).FirstOrDefault();
            string? op = q1.Option1;
            Result res=new Result();

            res.SelectedOption = selectedOption;    
            res.CorrectOption = correctOption;
            res.ExamId = exId;
            res.QuestionId = questionId;
            if (selectedOption == correctOption)
            {
                res.Result1 = 1;
            }
            else
            {
                res.Result1 = 0;
            }

            db.Results.Add(res);    
            db.SaveChanges();

            if (quesId < filteredQues.Count())
            {
                int id = filteredQues.ElementAt(quesId++).QuestionId;
                return RedirectToAction("Question", new
                {
                    id = id
                });
            }

            return RedirectToAction("Result", new
            {
                exId = exId
            });

        }

        public IActionResult Submit()
        {
            int score = db.Results.Where(r => r.ExamId == exId && r.Result1 == 1).Count();
            ViewBag.Score = score;
            return  RedirectToAction("Result");
        }

        [HttpGet("/User/Result/{exId}")]
        public IActionResult Result(int exId)
        {
            ViewBag.ExamId = exId;
            int score=db.Results.Where(r=>r.ExamId==exId && r.Result1==1 ).Count();
            int total= db.Results.Where(r => r.ExamId == exId).Count();

            Exam exam = db.Exams.Where(e => e.ExamId == exId).FirstOrDefault();
            int userId;
            userId = Convert.ToInt32(HttpContext.Session.GetString("UserID").ToString());
            Topic topic1 = db.Topics.Where(t => t.TopicId == exam.TopicId).FirstOrDefault();
            int topicId;
            if(topic1 != null) {
                var questions = db.Questions.Where(q => q.Topic.TopicName == topic1.TopicName && q.DifficultyLevel == exam.DifficultyLevel).ToList();
                topicId = Convert.ToInt32(questions[0].TopicId);
            }
            else{
                topicId = Convert.ToInt32(filteredQues[0].TopicId);
            }
            var topic = db.Topics.Where(t => t.TopicId == topicId).FirstOrDefault();
            

            var user = db.Users.Where(u => u.UserId == userId).FirstOrDefault();
            int s = score * 100;
            try
            {
                s = s / total;
            }
            catch (DivideByZeroException)
            {
                s = 0;
            }
            ViewBag.Score = s;
            if (exam != null)
            {
                exam.ExamEndDateTime = DateTime.Now;
                exam.User = user;
                exam.Topic = topic;

                exam.Finalscore = score;
                exam.ExamDuration = (int)(Convert.ToDateTime(exam.ExamEndDateTime).Subtract(Convert.ToDateTime(exam.ExamStartDateTime)).TotalMinutes); ;
                db.Exams.Update(exam);
                db.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("SelectQuiz", "User");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(String username, String email, String password, String phone)
        {
            User user = new User();
            user.Email = email;
            user.Password = password;
            user.UserName = username;
            user.UserRole = "USER";
            user.Mobile = phone;
            user.token = getRandomString();
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Homepage() {
            if(HttpContext.Session.GetString("Email") != null)
            {
                var email = HttpContext.Session.GetString("Email").ToString();
                User user = db.Users.Where(u => u.Email == email).FirstOrDefault();
                return View(user);
            }
            else
            {
                return View();
            }
        }
      
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = db.Users.Where(u => u.Email == email).FirstOrDefault();
            if(user != null)
            {
                string body = "<a href=https://quizzyy.azurewebsites.net/User/ChangePassword/" + user.token + ">CLICK HERE!</a>";
                await emailSender.SendEmailAsync(email, "RESET PASSWORD LINK", body);
                return View();
            }
            else
            {
                return RedirectToAction("Register", "User");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendMail()
        {
            await emailSender.SendEmailAsync("<enter mail here>", "FIRST MAIL FROM ASP.NET", "HELLO WORLD!");
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string subscribeMail)
        {
            var user = db.Users.Where(u => u.Email == subscribeMail).FirstOrDefault();
            if (user != null)
            {
                user.newsletter = true;
                db.Update(user);
                db.SaveChanges();
                string subject = "Quizzy App Newsletter";
                string cong_template = "<b> Congratulations!! You have been subscribed to the Quizzy Newsletter! </b>";
                await emailSender.SendEmailAsync(subscribeMail, subject, cong_template);
                return RedirectToAction("Homepage", "User");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet("/User/ChangePassword/{token}")]
        public IActionResult ChangePassword(string token)
        {
            ViewBag.token = token;
            return View();
        }
        
        [HttpPost]
        public IActionResult ChangePassword(string email, string password, string token)
        {
            var user = db.Users.Where(u => u.token == token).FirstOrDefault();
            if (user != null && user.Email == email) {
                user.Password = password;
                user.token = getRandomString();
                db.Update(user);
                db.SaveChanges();
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "User");
            }
            else {
                return Content("Invalid Credentials!");
            }
        }
    }
}
