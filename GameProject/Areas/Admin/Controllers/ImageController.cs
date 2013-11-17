using GameProject.Areas.Admin.ViewModels;
using GameProject.Enums;
using GameProject.Helpers;
using GameProject.Models;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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
            //jeśli nie jest pusty i mniejszy niż 200kb
            if (file != null && file.ContentLength > 0 && file.ContentLength < 204800)
            {
                // Get file info
                var fileName = Path.GetFileName(file.FileName); //nazwa pliku
                var imageName = Path.GetFileNameWithoutExtension(fileName);
                var imageExtension = "";
                var contentLength = file.ContentLength; //wielkosc pliku
                var contentType = file.ContentType; //typ pliku
                
                if (contentType == "image/jpeg") { imageExtension = "jpg";}
                else if (contentType == "image/png") { imageExtension = "png";}

                if (imageExtension != "")
                {
                    byte[] imageBytes = new byte[contentLength - 1];
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        imageBytes = binaryReader.ReadBytes(file.ContentLength);
                    }
                    Image img = new Image();
                    img.FileName = String.Format("{0:yyyyMMddHHmmss}.{1}", DateTime.Now, imageExtension);
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
                    FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Uwaga niewłaściwy format zdjęcia. Akceptowane formaty: jpg, png."); 
                    return RedirectToAction("Create");
                }
            }
            else
            {
                // redirect back to the index action to show the form once again
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Uwaga nie wybrano pliku lub jego wielkość przekracza 200kb.");
                return RedirectToAction("Create");
            }
        }

        public ActionResult Show(string imageName = "")
        {
            if (String.IsNullOrEmpty(imageName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from a in db.Images
                        where a.FileName == imageName
                        select a;

            var img = query.FirstOrDefault();

            if (img == null)
            {
                return HttpNotFound();
            }

            return File(img.Data, img.Type);
        }

        public ActionResult SetToItem(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Items
                        where i.Id == id
                        select i;

            var item = query.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            var query2 = from i in db.Images
                         where i.Category == ImageCategory.Item
                         select i;

            var images = query2.ToList();

            if (images.Count() == 0)
            {
                return HttpNotFound();
            }

            SetImageViewModel setItemViewModel = new SetImageViewModel()
            {
                Images = images,
                Id = (int)id
            };

            return View(setItemViewModel);
        }

        [HttpPost]
        public ActionResult SetToItem(int? id, int? imageId)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if(imageId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Items
                        where i.Id == id
                        select i;

            var item = query.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            var query2 = from i in db.Images
                         where i.ID == imageId
                         select i;

            var image = query2.FirstOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }

            try
            {
                item.ImageId = (int)imageId;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Item");
            }
            catch (Exception)
            {

            }

            var query3 = from i in db.Images
                         where i.Category == ImageCategory.Item
                         select i;

            var images = query2.ToList();

            if (item == null)
            {
                return HttpNotFound();
            }

            SetImageViewModel setItemViewModel = new SetImageViewModel()
            {
                Images = images,
                Id = (int)id
            };

            return View(setItemViewModel);
        }
    }
}
