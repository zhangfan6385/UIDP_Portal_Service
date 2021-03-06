﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STORE.BIZModule;
using STORE.UTILITY;

namespace STORE.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("Home")]
    public class HomeController : Controller
    {
        HomeModule mm = new HomeModule();
        CommunityPostModule cpm = new CommunityPostModule();
        ApplyModule amm = new ApplyModule();
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
        public IActionResult fetchCountList(int limit)
        {
            Dictionary<string, object> res = mm.fetchCountList(limit);
            return Json(res);
        }
        [HttpGet("getTopPost")]
        public IActionResult getTopPost()
        {
            return Json(cpm.getTopPost());
        }
        /// <summary>
        /// 获取组件列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("fetchComponentList")]
        public IActionResult fetchComponentList(string limit, string page,string name)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["name"] = name;
            Dictionary<string, object> res = amm.fetchComponentList(d);
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
            Dictionary<string, object> res = amm.fetchComponentDetailList(userid, projectid, resourceid);
            return Json(res);
        }
        /// <summary>
        /// 获取服务列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("fetchServerList")]
        public IActionResult fetchServerList(string limit, string page,string name)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["name"] = name;
            Dictionary<string, object> res = amm.fetchServerList(d);
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
            Dictionary<string, object> res = amm.fetchServerDetailList(userid, projectid, resourceid);
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
            Dictionary<string, object> res = amm.fetchPlatformList(userid, projectid, resourceid, platType, isFirst);
            return Json(res);
        }

        [HttpGet("fetchPlatformDetail")]
        public IActionResult fetchPlatformDetail(string userid, string projectid,string resourceid)
        {
            Dictionary<string, object> res = amm.fetchPlatformDetail(userid,projectid,resourceid);
            return Json(res);
        }

        /// <summary>
        /// 获取帖子详情
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("fetchPostDetail")]
        public IActionResult fetchPostDetail([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> res = cpm.fetchPostDetail(d);
            //HttpResponseMessage result = new HttpResponseMessage{ Content=new StringContent(res.ToString(),Encoding.GetEncoding("UTF-8"),"application/json")};
            return Json(res);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("uploadCommunityPic")]
        public IActionResult PostPic([FromForm]IFormCollection formCollection)
        {
            string result = "";
            try
            {
                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                foreach (IFormFile file in fileCollection)
                {
                    StreamReader reader = new StreamReader(file.OpenReadStream());
                    String content = reader.ReadToEnd();
                    String name = file.FileName;
                    string suffix = name.Substring(name.LastIndexOf("."), (name.Length - name.LastIndexOf("."))); //扩展名
                    //double filesize = Math.Round(Convert.ToDouble(file.Length / 1024.00 / 1024.00), 2);
                    string filepath = @"\\UploadFiles\\community\\pic\\" + Guid.NewGuid().ToString() + suffix;
                    string filename = System.IO.Directory.GetCurrentDirectory() + filepath;
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        // 复制文件
                        file.CopyTo(fs);
                        // 清空缓冲区数据
                        fs.Flush();
                    }

                    result = filepath;
                    return Json(result);
                }
            }
            catch (Exception)
            {
                return Json(result);

            }

            return Json(result);
        }
    }
}