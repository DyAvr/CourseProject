using CourseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Xceed.Words.NET;

namespace CourseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Encrypt()
        {
            return View();
        }

        public IActionResult Decrypt()
        {
            return View();
        }

        public IActionResult Help()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DownloadFile(string encodedText, string downlType)
        {
            try
            {
                var stream = new MemoryStream();
                if (downlType == "Download as docx")
                {
                    using (var t = DocX.Create(stream))
                    {
                        t.InsertParagraph(encodedText);
                        t.Save();
                    }
                    stream.Position = 0;
                    return File(stream, "application/vnd.ms-word", "result.docx");
                }
                else if (downlType == "Download as txt")
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    using (var t = new StreamWriter(stream,Encoding.GetEncoding(1251)))
                    {
                        t.Write(encodedText.ToCharArray());
                        t.Flush();
                        stream.Position = 0;
                        return File(new MemoryStream(stream.ToArray()), "text/plain", "result.txt");
                    }
                }
                else throw new Exception("Неизвестная ошибка");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }

        }

        [HttpPost]
        public IActionResult DecryptFile(IFormFile UsersFile, string Key, string InputText, bool Rus)
        {
            if (UsersFile != null)
            {
                try
                {
                    if (Rus)
                    {
                        ViewBag.Result = Encryption.Decoder(Encryption.ParseTextFromFile(UsersFile), Key, "Rus");
                    }
                    else
                    {
                        ViewBag.Result = Encryption.Decoder(Encryption.ParseTextFromFile(UsersFile), Key, "Eng");
                    }
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            else
            {
                try
                {
                    if (Rus)
                    {
                        ViewBag.Result = Encryption.Decoder(InputText, Key, "Rus");
                    }
                    else
                    {
                        ViewBag.Result = Encryption.Decoder(InputText, Key, "Eng");
                    }
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            ViewBag.BaseValue = InputText;
            ViewBag.Key = Key;
            return View("Decrypt");
        }

        [HttpPost]
        public IActionResult EncryptFile(IFormFile UsersFile, string InputText, string Key, bool Rus)
        {
            if (UsersFile != null)
            {
                try
                {
                    if (Rus)
                    {
                        ViewBag.Result = Encryption.Encoder(Encryption.ParseTextFromFile(UsersFile), Key, "Rus");
                    }
                    else
                    {
                        ViewBag.Result = Encryption.Encoder(Encryption.ParseTextFromFile(UsersFile), Key, "Eng");
                    }
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            else
            {
                try
                {
                    if (Rus)
                    {
                        ViewBag.Result = Encryption.Encoder(InputText, Key, "Rus");
                    }
                    else
                    {
                        ViewBag.Result = Encryption.Encoder(InputText, Key, "Eng");
                    }
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            ViewBag.BaseValue = InputText;
            ViewBag.Key = Key;
            return View("Decrypt");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
