using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCDemo.Models;
using MVCDemo.Repositorys;
using static MVCDemo.Models.DBDataModel;

namespace MVCDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly StudentData _StudentData;
    private readonly SelectClassData _SelectClassData;

    public HomeController(ILogger<HomeController> logger, StudentData StudentData, SelectClassData SelectClassData)
    {
        _logger = logger;
        _StudentData = StudentData;
        _SelectClassData = SelectClassData;
    }

    public IActionResult Index()
    {
        List<DBDataModel.Student> Result = new List<DBDataModel.Student>();
        var Query = _StudentData.Student.ToList();

        foreach (var item in Query)
        {
            Student Data = new Student();
            Data.UniqueID = item.UniqueID;
            Data.StudentNum = item.StudentNum;
            Data.Name = item.Name;
            Data.Birthday = item.Birthday;
            Data.Email = item.Email;
            Result.Add(Data);
        }
        return View(Result);
    }

    [HttpPost]
    public IActionResult CheckDouble([FromForm] Student student)
    {
        var StudentNum = student.StudentNum;
        if (ModelState.IsValid)
        {
            if (_StudentData.Student.Where(b => b.StudentNum == StudentNum).SingleOrDefault() == null)
            {
                return Json(1);
            }
            else return Json("學號欄位重複!");
        }
        return Json("Email格式不符!");
    }

    [HttpPost]
    public IActionResult Create([FromForm] Student student)
    {
        _StudentData.Student.Add(student);
        _StudentData.SaveChanges();
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Delete([FromForm] string StudentNum)
    {
        try
        {
            var Student = _StudentData.Student.SingleOrDefault(c => c.StudentNum == StudentNum);
            if (Student != null)
            {
                _StudentData.Student.Remove(Student);
                _StudentData.SaveChanges();
            }
            var DataNum = _SelectClassData.SelectClass.Where(c => c.StudentNum == StudentNum).Count();
            if (DataNum != 0)
            {
                var DeleteRange = _SelectClassData.SelectClass.Where(c => c.StudentNum == StudentNum).ExecuteDelete();
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
    public IActionResult EditStudent([FromQuery] string StudentNum)
    {
        var StudentData = _StudentData.Student.SingleOrDefault(p => p.StudentNum == StudentNum);
        return View(StudentData);
    }

    [HttpPost]
    public IActionResult Update([FromForm] Student student)
    {
        if (ModelState.IsValid)
        {
            _StudentData.Entry(student).CurrentValues.SetValues(student);
            _StudentData.Entry(student).State = EntityState.Modified;
            _StudentData.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return View("EditStudent", student);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

