using ReadersHub.Business.Service.Criterions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadersHub.Portal.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICriterionService _criterionService;
        public ProductController(ICriterionService criterionService)
        {
            _criterionService = criterionService;
        }
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
    }
}