using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STORE.BIZModule;
using STORE.UTILITY;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.DirectoryServices;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Internal;

namespace STORE.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("LogIn")]
    public class LogInController : Controller
    {
        STORE.LOG.SysLog log = new LOG.SysLog();
        public static IConfiguration Configuration { get; set; }
        [HttpPost("login")]
        public IActionResult loginByUsernames([FromBody]JObject value)
        {
            string userId = "";
            string userName = "";
            try
            {
                Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
                string username = d["username"] == null ? "" : d["username"].ToString();
                string password = d["password"] == null ? "" : d["password"].ToString();
                UserLoginModule um = new UserLoginModule();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return Json(new { code = -1, message = "用户名或密码不能为空！" });
                }
                DataTable du = um.getUserType(username);
                if (du != null&&du.Rows.Count>0)
                {
                    DataTable dr = um.getAdminInfoByName(username, password);//获取用户是否存在
                    if (dr != null && dr.Rows.Count > 0)
                    {
                        userId = dr.Rows[0]["CONF_CODE"].ToString();
                        string accessToken = AccessTokenTool.GetAccessToken(userId);
                        STORE.UTILITY.AccessTokenTool.DeleteToken(userId);
                        STORE.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
                        DataTable dtProject = um.getProject(userId);
                        int level = 1;
                        //if (Extension.GetClientUserIp(Request.HttpContext).ToString() != dt.Rows[0]["USER_IP"].ToString())
                        //{
                        //    level = 2;
                        //}
                        log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", "", level);
                        return Json(new
                        {
                            code = 2000,
                            message = "超级管理员登录成功！",
                            token = accessToken,
                            //userInfo = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dr)),
                            roleLevel = 0
                        });
                    }
                    else{
                        return Json(new { code = -1, message = "账号或者密码错误！" });
                    }
                }
                else
                {
					 password = Security.SecurityHelper.StringToMD5Hash(password);
                    DataTable dt = um.getUserInfoByName(username);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return Json(new { code = -1, message = "此用户不存在！" });
                    }
                    else if (password != dt.Rows[0]["USER_PASS"].ToString())
                    {
                        return Json(new { code = -1, message = "密码错误！" });
                    }
                    else
                    {
                        DataTable dc = um.getAdminTokenByName(dt.Rows[0]["USER_ID"].ToString());//获取用户Token是否存在
                        if (dc == null || dc.Rows.Count < 1)
                        {
                            userId = dt.Rows[0]["USER_ID"].ToString();
                            string accessToken = AccessTokenTool.GetAccessToken(userId);
                            STORE.UTILITY.AccessTokenTool.DeleteToken(userId);
                            STORE.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
                            DataTable dtProject = um.getProject(userId);
                            int level = 1;
                            //if (Extension.GetClientUserIp(Request.HttpContext).ToString() != dt.Rows[0]["USER_IP"].ToString())
                            //{
                            //    level = 2;
                            //}
                            log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", "", level);
                            return Json(new
                            {
                                code = 2000,
                                message = "",
                                token = accessToken,
                                projectInfo = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dtProject)),
                                userInfo = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dt)),
                                roleLevel = dt.Rows[0]["USER_TYPE"].ToString()
                            });
                        }
                        else
                        {
                            userId = dt.Rows[0]["USER_ID"].ToString();
                            string accessToken = dc.Rows[0]["ACCESS_TOKEN"].ToString();
                            DataTable dtProject = um.getProject(userId);
                            int level = 1;
                            //if (Extension.GetClientUserIp(Request.HttpContext).ToString() != dt.Rows[0]["USER_IP"].ToString())
                            //{
                            //    level = 2;
                            //}
                            log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", "", level);
                            return Json(new
                            {
                                code = 2000,
                                message = "",
                                token = accessToken,
                                projectInfo = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dtProject)),
                                userInfo = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dt)),
                                roleLevel = dt.Rows[0]["USER_TYPE"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", ex.Message.Length > 120 ? ex.Message.Substring(0, 100) : ex.Message, 1);
                return Json(new { code = -1, message = "登录时程序发生错误" + ex.Message });

            }

        }

        #region 备用


        //[HttpPost("login")]
        //public IActionResult LogIn([FromBody]JObject value)
        //{
        //    string userId = "";
        //    string userName = "";
        //    try
        //    {
        //        Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
        //        string username = d["username"]==null?"" : d["username"].ToString();
        //        string password = d["password"]==null?"": d["password"].ToString();
        //        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //        {
        //            return Json(new { code = -1, message = "用户名或密码不能为空！" });
        //        }
        //        UserModule mm = new UserModule();
        //         userId = mm.getAdminCode();
        //        string pass = mm.getAdminPass();
        //        if ((username== userId) &&(password==pass)) {
        //            userName = "系统超级管理员";
        //            string accessToken = AccessTokenTool.GetAccessToken(userId);
        //            STORE.UTILITY.AccessTokenTool.DeleteToken(userId);
        //            STORE.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));

        //            log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 2, "LogIn", "");
        //            return Json(new { code = 2000, message = "", token = accessToken,roleLevel = "admin" });
        //        }
        //        else {
        //            STORE.BIZModule.Models.ts_uidp_userinfo mode = mm.getUserInfoByLogin(username, d["userDomain"].ToString());
        //            if (mode == null)
        //            {
        //                return Json(new { code = -1, message = "此用户不存在！" });
        //            }
        //            if (password != mode.USER_PASS)
        //            {
        //                return Json(new { code = -1, message = "密码错误！" });
        //            }
        //            userId = mode.USER_ID;
        //            userName = mode.USER_NAME;
        //            string accessToken = AccessTokenTool.GetAccessToken(userId);
        //            STORE.UTILITY.AccessTokenTool.DeleteToken(userId);
        //            STORE.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
        //            DataTable dtUserOrg = mm.GetUserOrg(mode.USER_ID);
        //            log.Info(DateTime.Now, userId, mode.USER_NAME, Extension.GetClientUserIp(Request.HttpContext), 2, "LogIn", "");
        //            return Json(new { code = 2000, message = "", token = accessToken, orgList = dtUserOrg, roleLevel = "" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", ex.Message.Length>120?ex.Message.Substring(0,100):ex.Message);
        //        return Json(new { code = -1, message = "登录时程序发生错误"+ex.Message});

        //    }

        //}
        #endregion

        [HttpPost("apiLogin")]
        public IActionResult apiLogin([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            string userCode = d["userCode"] == null ? "" : d["userCode"].ToString();
            string password = d["password"] == null ? "" : Security.SecurityHelper.StringToMD5Hash(d["password"].ToString());
            string userId = "";
            string userName = "云主机推送服务";
            string accessToken = "";
            try
            {
                if (string.IsNullOrEmpty(userCode) || string.IsNullOrEmpty(password))
                {
                    //return Json(new { code = -1, message = "推送接口用户名或密码不能为空！" });
                    return Content("");
                }
                UserLoginModule um = new UserLoginModule();
                DataTable dt = um.getUserInfoByName(userCode);
                if (dt == null || dt.Rows.Count == 0)
                {
                    //return Json(new { code = -1, message = "云同步用户不存在！" });
                    return Content("");
                }
                if (password != dt.Rows[0]["USER_PASS"].ToString())
                {
                    //return Json(new { code = -1, message = "云同步用户密码错误！" });
                    return Content("");
                }
                userId = dt.Rows[0]["USER_ID"].ToString();
                userName = dt.Rows[0]["USER_NAME"].ToString();
                accessToken = AccessTokenTool.GetAccessToken(userId);
                STORE.UTILITY.AccessTokenTool.DeleteToken(userId);
                STORE.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
                log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 2, "云组织数据同步", "", 1);
                return Content(accessToken);

            }
            catch (Exception ex)
            {
                log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "云组织数据同步", ex.Message.Length > 120 ? ex.Message.Substring(0, 100) : ex.Message, 1);
                return Content("");

            }

        }
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

        //public IActionResult loginByUsernames([FromBody]JObject value)
        //{
        //    string userId = "";
        //    string userName = "";
        //    try
        //    {
        //        Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
        //        string username = d["username"] == null ? "" : d["username"].ToString();
        //        string password = d["password"] == null ? "" : d["password"].ToString();
        //        UserLoginModule um = new UserLoginModule();
        //        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //        {
        //            return Json(new { code = -1, message = "用户名或密码不能为空！" });
        //        }
        //        else if (username == "admin")
        //        {
        //            DataTable dt = um.getAdminInfoByName(username, password);
        //            if (dt == null || dt.Rows.Count == 0)
        //            {
        //                return Json(new { code = -1, message = "账号或者密码错误！" });
        //            }
        //            else
        //            {
        //                userId = dt.Rows[0]["CONF_CODE"].ToString();
        //                string accessToken = AccessTokenTool.GetAccessToken(userId);
        //                STORE.UTILITY.AccessTokenTool.DeleteToken(userId);
        //                STORE.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
        //                DataTable dtProject = um.getProject(userId);
        //                int level = 1;
        //                //if (Extension.GetClientUserIp(Request.HttpContext).ToString() != dt.Rows[0]["USER_IP"].ToString())
        //                //{
        //                //    level = 2;
        //                //}
        //                log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", "", level);
        //                return Json(new
        //                {
        //                    code = 2000,
        //                    message = "",
        //                    token = accessToken,

        //                    userInfo = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dt)),
        //                    roleLevel = "1"
        //                });
        //            }
        //        }
        //        else
        //        {
        //            DataTable dt = um.getUserInfoByName(username);
        //            if (dt == null || dt.Rows.Count == 0)
        //            {
        //                return Json(new { code = -1, message = "此用户不存在！" });
        //            }
        //            if (password != dt.Rows[0]["USER_PASS"].ToString())
        //            {
        //                return Json(new { code = -1, message = "密码错误！" });
        //            }
        //            userId = dt.Rows[0]["USER_ID"].ToString();
        //            string accessToken = AccessTokenTool.GetAccessToken(userId);
        //            STORE.UTILITY.AccessTokenTool.DeleteToken(userId);
        //            STORE.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
        //            DataTable dtProject = um.getProject(userId);
        //            int level = 1;
        //            //if (Extension.GetClientUserIp(Request.HttpContext).ToString() != dt.Rows[0]["USER_IP"].ToString())
        //            //{
        //            //    level = 2;
        //            //}
        //            log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", "", level);
        //            return Json(new
        //            {
        //                code = 2000,
        //                message = "",
        //                token = accessToken,
        //                projectInfo = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dtProject)),
        //                userInfo = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dt)),
        //                roleLevel = "0"
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", ex.Message.Length > 120 ? ex.Message.Substring(0, 100) : ex.Message, 1);
        //        return Json(new { code = -1, message = "登录时程序发生错误" + ex.Message });

        //    }

        //}
    }

}