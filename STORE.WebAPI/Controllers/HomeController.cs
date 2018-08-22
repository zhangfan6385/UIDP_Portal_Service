using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STORE.BIZModule;
namespace STORE.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : Controller
    {
        HomeModule mm = new HomeModule();
        [HttpGet("fetchUserRoleList")]
        public IActionResult fetchUserRoleList(string limit, string page)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            Dictionary<string, object> res = mm.fetchNoticeList(d);
            return Json(res);
        }

    }
}