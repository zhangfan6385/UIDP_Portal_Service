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
    [Route("Apply")]
    public class ApplyController : Controller
    {
        /// <summary>
        /// 查询所有消息和未读信息数量
        /// </summary>
        ApplyModule mm = new ApplyModule();
        [HttpGet("fetchApplyRecordList")]
        public IActionResult fetchApplyRecordList(string limit, string page, string userId)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["userId"] = userId;
            Dictionary<string, object> res = mm.fetchApplyRecordList(d);
            return Json(res);
        }
        /// <summary>
        /// 标记信息已读
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateApplyRecord")]
        public IActionResult updateApplyRecord([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = mm.updateApplyRecord(d);
            return Json(res);
        }
        [HttpPost("createApply")]
        public IActionResult createApply([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = mm.createApply(d);
            return Json(res);
        }
      
    }
}