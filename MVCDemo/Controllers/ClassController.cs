using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCDemo.Models;
using MVCDemo.Repositorys;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MVCDemo.Models.DBDataModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCDemo.Controllers
{
    public class ClassController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClassData _ClassData;
        private readonly SelectClassData _SelectClassData;

        public ClassController(ILogger<HomeController> logger, ClassData ClassData, SelectClassData SelectClassData)
        {
            _logger = logger;
            _ClassData = ClassData;
            _SelectClassData = SelectClassData;
        }

        public IActionResult Index()
        {
            List<DBDataModel.Class> Result = new List<DBDataModel.Class>();
            var Query = _ClassData.Class.ToList();

            foreach (var item in Query)
            {
                Class Data = new Class();
                Data.UniqueID = item.UniqueID;
                Data.ClassNum = item.ClassNum;
                Data.Name = item.Name;
                Data.Credit = item.Credit;
                Data.Teacher = item.Teacher;
                Data.Place = item.Place;
                Result.Add(Data);
            }
            return View(Result);
        }

        [HttpPost]
        public IActionResult CheckDouble([FromForm] Class Class)
        {
            var ClassNum = Class.ClassNum;
            if (_ClassData.Class.Where(b => b.ClassNum == ClassNum).SingleOrDefault() == null)
            {
                return Json(1);
            }
            else return Json("課程代號欄位重複!");
        }

        [HttpPost]
        public IActionResult Create([FromForm] Class Class)
        {
            _ClassData.Class.Add(Class);
            _ClassData.SaveChanges();
            return RedirectToAction("Index", "Class");
        }

        [HttpPost]
        public IActionResult Delete([FromForm] string ClassNum)
        {
            try
            {
                var Class = _ClassData.Class.SingleOrDefault(c => c.ClassNum == ClassNum);
                if (Class != null)
                {
                    _ClassData.Class.Remove(Class);
                    _ClassData.SaveChanges();
                }
                var DataNum = _SelectClassData.SelectClass.Where(c => c.ClassNum == ClassNum).Count();
                if (DataNum != 0)
                {
                    var DeleteRange = _SelectClassData.SelectClass.Where(c => c.ClassNum == ClassNum).ExecuteDelete();
                    //_SelectClassData.SelectClass.RemoveRange(DeleteRange);
                    _SelectClassData.SaveChanges();
                }
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }

        [HttpGet]
        public IActionResult EditClass([FromQuery] string ClassNum)
        {
            var ClassData = _ClassData.Class.SingleOrDefault(p => p.ClassNum == ClassNum);
            return View(ClassData);
        }

        [HttpPost]
        public IActionResult Update([FromForm] Class Class)
        {
            if (ModelState.IsValid)
            {
                _ClassData.Entry(Class).CurrentValues.SetValues(Class);
                _ClassData.Entry(Class).State = EntityState.Modified;
                _ClassData.SaveChanges();
                return RedirectToAction("Index", "Class");
            }
            else
            {
                return View("EditClass", Class);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

