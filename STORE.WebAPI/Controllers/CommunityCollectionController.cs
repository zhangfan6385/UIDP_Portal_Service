using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STORE.BIZModule;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http.Internal;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace STORE.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("collection")]
    public class CommunityCollectionController : WebApiBaseController
    {
        CommunityCollectionModule mm = new CommunityCollectionModule();
        /// <summary>
        /// 查询社区信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchMyCommunityCollectionList")]
        public IActionResult fetchMyCommunityCollectionList(string limit, string page,string USER_ID)
        {
            //UserModule user = new UserModule();
            //string Admin = user.getAdminCode();
            //bool isAdmin = UserId.Equals(Admin);
            //Dictionary<string, object> res = mm.GetPagedTable(isAdmin);
            //return Json(res);
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["userId"] = USER_ID;
          
            Dictionary<string, object> res = mm.fetchMyCommunityCollectionList(d);
            return Json(res);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createCommunityCollectionArticle")]
        public IActionResult createCommunityCollectionArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.createCommunityCollectionArticle(d);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }
       
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("deleteCommunityCollectionArticle")]
        public IActionResult deleteCommunityCollectionArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.deleteCommunityCollectionArticle(d);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }

        //public IActionResult deleteCommunityCollectionArticle([FromBody]JObject value)
        //{
        //    Dictionary<string, object> r = new Dictionary<string, object>();
        //    Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
        //    try
        //    {
        //        string b = mm.deleteCommunityCollectionArticle(d["COLLECTION_ID"].ToString());
        //        if (b == "")
        //        {
        //            r["message"] = "成功";

        //            r["code"] = 2000;
        //        }
        //        else
        //        {
        //            r["code"] = -1;
        //            r["message"] = b;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        r["code"] = -1;
        //        r["message"] = e.Message;
        //    }
        //    return Json(r);
        //}

    }
}