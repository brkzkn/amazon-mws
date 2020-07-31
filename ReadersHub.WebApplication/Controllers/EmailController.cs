using ReadersHub.Common.Constants;
using ReadersHub.WebApplication.Core;
using ReadersHub.WebApplication.Core.Attribute;
using ReadersHub.WebApplication.Models;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadersHub.WebApplication.Controllers
{
    public class EmailController : AuthorizedController
    {
        // GET: Email
        public ActionResult Index(bool isUpload = false)
        {
            var model = new EmailViewModel();
            //model.StoreId = storeId;
            //model.StoreList = _storeService.GetList().Select(x => new SelectListItem()
            //{
            //    Text = x.Text,
            //    Value = x.Value
            //}).ToList();

            if (isUpload)
            {
                SetNotificationInfo(NotificationHelper.MessageType.Success, "Yeni ürünler eklendi");
            }
            return View(model);
        }


        [Permission(Permissions.Email)]
        public ActionResult Upload(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    if (!fileName.Contains(".xlsx") && !fileName.Contains(".xls"))
                    {
                        SetNotificationInfo(NotificationHelper.MessageType.Error, "Yalnızca Excel formatı geçerlidir");
                        return View("Index");
                    }

                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    int excelColCount = 1;
                    List<string> orderIdList = new List<string>();

                    using (var doc = new SLDocument(file.InputStream))
                    {
                        var settings = doc.GetWorksheetStatistics();

                        var rowCount = GetRowCount(doc, settings, excelColCount);
                        for (var row = 1; row <= rowCount; row++)
                        {
                            orderIdList.Add(doc.GetCellValueAsString(row, 1));
                        }
                    }

                    for (int i = 0; i < orderIdList.Count; i++)
                    {
                        
                    }
                }
                else
                {
                    SetNotificationInfo(NotificationHelper.MessageType.Error, "Lütfen eklemek istediğiniz dosyayı seçiniz");
                    return View("Index");
                }
            }

            return RedirectToAction("Index", new { isUpload = true});
        }


        private int GetRowCount(SLDocument doc, SLWorksheetStatistics settings, int columnCount)
        {
            bool search = false;
            int rowCount = settings.EndRowIndex;
            //endrowindex may be greater than exact value. So clear empty rows and calculate true value.
            for (int row = settings.EndRowIndex; row >= settings.StartRowIndex; row--)
            {
                search = false;
                for (int col = settings.StartColumnIndex; col <= columnCount; col++)
                {
                    if (doc.HasCellValue(row, col))
                    {
                        search = true;
                        break;
                    }
                }
                if (search)
                    break;
                else
                    rowCount--;
            }
            return rowCount;
        }


    }
}