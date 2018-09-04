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
    [Route("communitypost")]
    public class CommunityPostController : WebApiBaseController
    {
        CommunityPostModule mm = new CommunityPostModule();
        /// <summary>
        /// 查询社区信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchCommunityPostList")]
        public IActionResult fetchCommunityPostList(string limit, string page,string POST_TYPE,string USER_ID, string TITLE_NAME, string BEGIN_SEND_DATE, string END_SEND_DATE)
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
            Dictionary<string, object> res = mm.fetchCommunityPostList(d);
            return Json(res);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createCommunityPostArticle")]
        public IActionResult createCommunityPostArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                d["POST_IP"] = ClientIp;
                string b = mm.createCommunityPostArticle(d);
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
        /// 修改
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateCommunityPostData")]
        public IActionResult updateCommunityPostData([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                d["POST_IP"] = ClientIp;
                string b = mm.updateCommunityPostData(d);
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
        [HttpPost("updatePlatformArticle")]
        public IActionResult updateCommunityPostArticle([FromBody]JObject value)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            try
            {
                string b = mm.updateCommunityPostArticle(d["POST_ID"].ToString());
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
        /// 添加评论
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("addComment")]
        public IActionResult addComment([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.addComment(d);
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
        /// 添加回复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("addReply")]
        public IActionResult addReply([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.addReply(d);
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
        /// 贴子浏览量加1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateComunityPostLookTimes")]
        public IActionResult updateComunityPostLookTimes([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateComunityPostLookTimes(d["POST_ID"].ToString());
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
        /// 查询帖子top
        /// </summary>
        /// <returns></returns>
        [HttpGet("getTopPost")]
        public IActionResult getTopPost()
        {
            return Json(mm.getTopPost());
        }
        /// <summary>
        /// 查询帖子详情
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("fetchPostDetail")]
        public IActionResult fetchPostDetail([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = mm.fetchPostDetail(d);
            return Json(res);
        }
        /// <summary>
        /// 删除帖子内的评论
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("deleteComment")]
        public IActionResult deleteComment([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.deleteCommit(d);
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
        /// 删除帖子以及帖子下面的回复
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("deletePost")]
        public IActionResult deletePost([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.deletePost(d);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["message"] = "b";
                    r["code"] = -1;
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }
    }
}