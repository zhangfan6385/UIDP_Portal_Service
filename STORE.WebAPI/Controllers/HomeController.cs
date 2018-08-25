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
    [Route("Home")]
    public class HomeController : Controller
    {
        HomeModule mm = new HomeModule();
        /// <summary>
        /// 查询公告
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("fetchNoticeList")]
        public IActionResult fetchNoticeList(string limit, string page,string id)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["id"] = id;
            Dictionary<string, object> res = mm.fetchNoticeList(d);
            return Json(res);
        }
        /// <summary>
        /// 查询首页统计表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
         [HttpGet("fetchCountList")]
        public IActionResult fetchCountList()
        {
            Dictionary<string, object> res = mm.fetchCountList();
            return Json(res);
        }

    }
}