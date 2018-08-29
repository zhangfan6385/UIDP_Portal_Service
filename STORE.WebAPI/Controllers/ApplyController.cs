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
        /// <summary>
        /// 获取组件列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("fetchComponentList")]
        public IActionResult fetchComponentList(string limit, string page)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            Dictionary<string, object> res = mm.fetchComponentList(d);
            return Json(res);
        }
        /// <summary>
        /// 获取组件列表详情
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
           [HttpGet("fetchComponentDetailList")]
        public IActionResult fetchComponentDetailList(string userid, string projectid, string resourceid)
        {
            Dictionary<string, object> res = mm.fetchComponentDetailList( userid,  projectid,  resourceid);
            return Json(res);
        }
        /// <summary>
        /// 获取服务列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("fetchServerList")]
        public IActionResult fetchServerList(string limit, string page)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            Dictionary<string, object> res = mm.fetchServerList(d);
            return Json(res);
        }
        /// <summary>
        /// 获取服务列表详情
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("fetchServerDetailList")]
        public IActionResult fetchServerDetailList(string userid, string projectid, string resourceid)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            Dictionary<string, object> res = mm.fetchServerDetailList(userid, projectid, resourceid);
            return Json(res);
        }
        /// <summary>
        /// 开发平台查询
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <param name="resourceid"></param>
        /// <returns></returns>
        [HttpGet("fetchPlatformList")]
        public IActionResult fetchPlatformList(string userid, string projectid, string resourceid, int platType, bool isFirst)
        {
            Dictionary<string, object> res = mm.fetchPlatformList( userid,  projectid,  resourceid,  platType,  isFirst);
            return Json(res);
        }
    }
}