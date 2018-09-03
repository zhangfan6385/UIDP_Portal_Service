using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using STORE.BIZModule;
namespace STORE.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("Home")]
    public class HomeController : Controller
    {
        HomeModule mm = new HomeModule();
        CommunityPostModule cpm = new CommunityPostModule();

        /// <summary>
        /// 查询社区信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchCommunityPostList")]
        public IActionResult fetchCommunityPostList(string limit, string page, string POST_TYPE, string USER_ID, string TITLE_NAME, string BEGIN_SEND_DATE, string END_SEND_DATE)
        {
            //UserModule user = new UserModule();
            //string Admin = user.getAdminCode();
            //bool isAdmin = UserId.Equals(Admin);
            //Dictionary<string, object> res = mm.GetPagedTable(isAdmin);
            //return Json(res);
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["USER_ID"] = USER_ID;
            d["POST_TYPE"] = POST_TYPE;
            d["TITLE_NAME"] = TITLE_NAME;
            d["BEGIN_SEND_DATE"] = BEGIN_SEND_DATE;
            d["END_SEND_DATE"] = END_SEND_DATE;
            Dictionary<string, object> res = cpm.fetchCommunityPostList(d);
            return Json(res);
        }
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
        [HttpGet("getTopPost")]
        public IActionResult getTopPost()
        {
            return Json(cpm.getTopPost());
        }

        [HttpPost("fetchPostDetail")]
        public IActionResult fetchPostDetail([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = cpm.fetchPostDetail(d);
            return Json(res);
        }

    }
}