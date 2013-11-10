using GameProject.Enums;
using GameProject.Helpers;
using GameProject.Models;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Areas.Admin.Controllers
{
    public class ImageController : Controller
    {

        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file,ImageCategory category)
        {
            if (file != null && file.ContentLength > 0)
            {
                // Get file info
                var fileName = Path.GetFileName(file.FileName); //nazwa pliku
                var contentLength = file.ContentLength; //wielkosc pliku
                var contentType = file.ContentType; //typ pliku

                byte[] imageBytes = new byte[contentLength - 1];
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    imageBytes = binaryReader.ReadBytes(file.ContentLength);
                }
                Image img = new Image();
                img.FileName = fileName;
                img.Data = imageBytes;
                img.Type = contentType;
                img.Category = category;
                db.Images.Add(img);
                db.SaveChanges();

                // redirect back to the index action to show the form once again
                FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Dodanie zdjęcia przebiegło pomyślnie.");
                return RedirectToAction("Index");
            }
            else
            {
                // redirect back to the index action to show the form once again
                return RedirectToAction("Create");
            }
        }

        public ActionResult Show()
        {
            var img = (from x in db.Images orderby x.ID descending select x).FirstOrDefault();

            return File(img.Data, img.Type);
        }
    }
}