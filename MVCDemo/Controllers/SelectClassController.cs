using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCDemo.Models;
using MVCDemo.Repositorys;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static MVCDemo.Models.DBDataModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCDemo.Controllers
{
    public class SelectClassController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SelectClassData _SelectClassData;
        private readonly StudentData _StudentData;
        private readonly ClassData _ClassData;

        public SelectClassController(ILogger<HomeController> logger, SelectClassData SelectClassData, StudentData StudentData, ClassData ClassData)
        {
            _logger = logger;
            _SelectClassData = SelectClassData;
            _StudentData = StudentData;
            _ClassData = ClassData;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<DBDataModel.Student> Result = new List<DBDataModel.Student>();
            List<DBDataModel.SelectClassDetail> ClassDetail = new List<DBDataModel.SelectClassDetail>();
            var StudentNum = _SelectClassData.SelectClass.Select(x => x.StudentNum).Distinct().ToArray();
            var Class = _ClassData.Class.ToList();
            ViewBag.Class = Class;
            var Query = _StudentData.Student.Where( c => !StudentNum.Contains(c.StudentNum));
            string ClassName, ClassNum;

            foreach (var stu in StudentNum)
            {
                ClassName = "";
                ClassNum = "";
                var StudentData = _StudentData.Student.SingleOrDefault(c => c.StudentNum == stu);
                if (StudentData != null)
                {
                    var ClassNumQuery = _SelectClassData.SelectClass.Where(x => x.StudentNum == stu).Select(x=>x.ClassNum).ToArray();
                    for (int i = 0; i < ClassNumQuery.Length; i++)
                    {
                        var ClassData = _ClassData.Class.SingleOrDefault(p => p.ClassNum == ClassNumQuery[i]);
                        if (ClassData != null)
                        {
                            if (i == 0)
                            {
                                ClassName += ClassData.Name;
                                ClassNum += ClassNumQuery[i];
                            }
                            else
                            {
                                ClassName += "、" + ClassData.Name;
                                ClassNum += "、" + ClassNumQuery[i];
                            }
                        }
                    }
                    /*
                    foreach (var cla in ClassNumQuery)
                    {
                        var ClassData = _ClassData.Class.SingleOrDefault(p => p.ClassNum == cla.ClassNum);
                        if (ClassData != null)
                        {
                            ClassName += ClassData.Name;
                            ClassNum += cla.ClassNum;
                        }
                    }
                    */
                    SelectClassDetail DetailData = new SelectClassDetail();
                    DetailData.StudentNum = stu;
                    DetailData.Name = StudentData.Name;
                    DetailData.ClassNum = ClassNum;
                    DetailData.ClassName = ClassName;
                    ClassDetail.Add(DetailData);
                }
            }

            ViewBag.DetailData = ClassDetail;

            foreach (var item in Query)
            {
                Student Data = new Student();
                Data.StudentNum = item.StudentNum;
                Data.Name = item.Name;
                Result.Add(Data);
            }
            return View(Result);
        }

        [HttpPost]
        public IActionResult Create([FromForm]string StudentNum, List<string> ClassNum)
        {
            try
            {
                SelectClass Data = new SelectClass();
                foreach (var d in ClassNum)
                {
                    Data.StudentNum = StudentNum;
                    Data.ClassNum = d;
                    _SelectClassData.SelectClass.Add(Data);
                    _SelectClassData.SaveChanges();
                }
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }

        [HttpPost]
        public IActionResult Delete([FromForm] string StudentNum)
        {
            var DataNum = _SelectClassData.SelectClass.Where(c => c.StudentNum == StudentNum).Count();
            if (DataNum != 0)
            {
                _SelectClassData.SelectClass.RemoveRange(_SelectClassData.SelectClass.Where(c => c.StudentNum == StudentNum));
                _SelectClassData.SaveChanges();
                return Json(1);
            }
            return Json(0);
        }

        [HttpGet]
        public IActionResult EditSelectClass([FromQuery] string StudentNum)
        {
            var StudentData = _StudentData.Student.SingleOrDefault(p => p.StudentNum == StudentNum);
            var Class = _ClassData.Class.ToList();
            ViewBag.StudentData = StudentData;
            ViewBag.Class = Class;
            var ClassNumQuery = _SelectClassData.SelectClass.Where(x => x.StudentNum == StudentNum).Select(x => x.ClassNum).ToList();
            ViewBag.SelectClassNum = ClassNumQuery;
            return View();
        }

        [HttpPost]
        public IActionResult Update([FromForm] string StudentNum, List<string> ClassNum)
        {
            try
            {
                var DataNum = _SelectClassData.SelectClass.Where(c => c.StudentNum == StudentNum).Count();
                if (DataNum != 0)
                {
                    _SelectClassData.SelectClass.RemoveRange(_SelectClassData.SelectClass.Where(c => c.StudentNum == StudentNum));
                    _SelectClassData.SaveChanges();
                }

                SelectClass Data = new SelectClass();
                foreach (var d in ClassNum)
                {
                    Data.StudentNum = StudentNum;
                    Data.ClassNum = d;
                    _SelectClassData.SelectClass.Add(Data);
                    _SelectClassData.SaveChanges();
                }
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }
    }
}

