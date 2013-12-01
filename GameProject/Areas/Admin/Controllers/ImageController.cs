using GameProject.Areas.Admin.ViewModels;
using GameProject.Enums;
using GameProject.Filters;
using GameProject.Helpers;
using GameProject.Models;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Areas.Admin.Controllers
{
    [AuthorizationFilter(UserRole.Admin)]
    public class ImageController : Controller
    {

        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var query = from a in db.Images
                        orderby a.FileName
                        select a;
            return View(query.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file,ImageCategory category)
        {
            Image image = new Image();

            try
            {
                if (file == null || file.ContentLength <= 0)
                {
                    throw new Exception("Plik pusty");
                }

                if (file.ContentLength > 204800)
                {
                    throw new Exception("Uwaga wielkość pliku przekracza 200kb.");
                }

                // Get file info
                var fileName = Path.GetFileName(file.FileName); //nazwa pliku
                var imageName = Path.GetFileNameWithoutExtension(fileName);
                var imageExtension = "";
                var contentLength = file.ContentLength; //wielkosc pliku
                var contentType = file.ContentType; //typ pliku
                
                if (contentType == "image/jpeg") { imageExtension = "jpg";}
                else if (contentType == "image/png") { imageExtension = "png";}

                if (imageExtension == "")
                {
                    throw new Exception("Uwaga niewłaściwy format zdjęcia. Akceptowane formaty: jpg, png.");
                }

                byte[] imageBytes = new byte[contentLength - 1];

                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    imageBytes = binaryReader.ReadBytes(file.ContentLength);
                }

                image.FileName = String.Format("{0:yyyyMMddHHmmss}.{1}", DateTime.Now, imageExtension);
                image.Data = imageBytes;
                image.Type = contentType;
                image.Category = category;
                db.Images.Add(image);
                db.SaveChanges();

                FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Dodanie zdjęcia przebiegło pomyślnie.");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z zapisem danych. " + e.Message);
            }
            return View(image);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = from i in db.Images
                        where i.ID == id
                        select i;
            var image = query.FirstOrDefault();

            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
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

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Images
                        where i.ID == id
                        select i;

            var image = query.FirstOrDefault();

            if (image == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.Images.Remove(image);
                db.SaveChanges();

                FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Usunięcie danych przebiegło pomyślnie.");
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z usuwaniem danych.");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Images
                        where i.ID == id
                        select i;

            var image = query.FirstOrDefault();

            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, HttpPostedFileBase file, ImageCategory category)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Images
                        where i.ID == id
                        select i;

            var image = query.FirstOrDefault();

            try
            {
                if (!Enum.IsDefined(typeof(ImageCategory), category))
                {
                    throw new Exception("Niewłaściwa Katygoria");
                }

                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName); //nazwa pliku
                    var imageName = Path.GetFileNameWithoutExtension(fileName);
                    var imageExtension = "";
                    var contentLength = file.ContentLength; //wielkosc pliku
                    var contentType = file.ContentType; //typ pliku

                    byte[] imageBytes = new byte[contentLength - 1];
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        imageBytes = binaryReader.ReadBytes(file.ContentLength);
                    }

                    image.FileName = String.Format("{0:yyyyMMddHHmmss}.{1}", DateTime.Now, imageExtension);
                    image.Data = imageBytes;
                    image.Type = contentType;
                }

                image.Category = category;

                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();

                FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Aktualizacja danych przebiegła pomyślnie.");
                return RedirectToAction("Index");
                
            }
            catch (DbUpdateConcurrencyException)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Warning, "Dane zostały zaktualizowane przez inną osobę. Należy odświeżyć stronę w celu wczytania nowych danych.");
            }
            catch (Exception e)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z aktualizowaniem danych. " + e.Message);
            }

            return View(image);
        }

        private ActionResult Set(ImageCategory category, int id)
        {
            var query = from i in db.Images
                        where i.Category == category
                        select i;

            var images = query.ToList();

            if (images.Count() == 0)
            {
                return HttpNotFound();
            }

            SetImageViewModel setItemViewModel = new SetImageViewModel()
            {
                Images = images,
                Id = (int)id,
                Category = category
            };

            return View("Set", setItemViewModel);
        }

        public ActionResult Set(ImageCategory? category, int? id, int? imageId)
        {
            if (category == null || !Enum.IsDefined(typeof(ImageCategory), category))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if(category == ImageCategory.Item)
            {
                var query = from i in db.Items
                            where i.Id == id
                            select i;

                var item = query.FirstOrDefault();

                if (item == null)
                {
                    return HttpNotFound();
                }

                if (imageId == null)
                {
                    return Set(ImageCategory.Item, item.Id);
                }

                var query2 = from i in db.Images
                             where i.ID == imageId
                             select i;

                var image = query2.FirstOrDefault();

                if (image == null)
                {
                    return HttpNotFound();
                }

                try
                {
                    item.ImageId = (int)imageId;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();

                    FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Obrazek został pomyślnie przypisany do przedmiotu.");

                    return RedirectToAction("Details", "Item", new { id = item.Id });
                }
                catch (Exception)
                {
                    FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd z przypisaniem obrazka do przedmiotu.");
                }

                return Set(ImageCategory.Item, item.Id);
            }
            else if (category == ImageCategory.Monster)
            {
                var query = from i in db.Monsters
                            where i.Id == id
                            select i;

                var monster = query.FirstOrDefault();

                if (monster == null)
                {
                    return HttpNotFound();
                }

                if (imageId == null)
                {
                    return Set(ImageCategory.Monster, monster.Id);
                }

                var query2 = from i in db.Images
                             where i.ID == imageId
                             select i;

                var image = query2.FirstOrDefault();

                if (image == null)
                {
                    return HttpNotFound();
                }

                try
                {
                    monster.ImageId = (int)imageId;
                    db.Entry(monster).State = EntityState.Modified;
                    db.SaveChanges();

                    FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Obrazek został pomyślnie przypisany do potwora.");

                    return RedirectToAction("Details", "Monster", new { id = monster.Id });
                }
                catch (Exception)
                {
                    FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd z przypisaniem obrazka do potwora.");
                }

                return Set(ImageCategory.Monster, monster.Id);
            }
            else if (category == ImageCategory.Location)
            {
                // do zrobienia
                return RedirectToAction("Index", "Location");
            }

            return HttpNotFound();
        }
    }
}
