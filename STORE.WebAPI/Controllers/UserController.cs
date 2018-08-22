using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using STORE.BIZModule;
using STORE.LOG;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;

namespace STORE.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("User")]
    public class UserController : WebApiBaseController
    {
        UserModule mm = new UserModule();
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updatePasswordData")]
        public IActionResult updatePasswordData([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updatePasswordData(d);
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
        /// 获取用户信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("Info")]
        public IActionResult Info([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                
                string tokenUserId = STORE.UTILITY.AccessTokenTool.GetUserId(d["token"].ToString());
                string userID= tokenUserId;
                if (d.Keys.Contains("userId")&&d["userId"] != null && d["userId"].ToString()!= "")
                {
                    userID = d["userId"].ToString();
                }
                if (userID == mm.getAdminCode()) {
                    //if (tokenUserId == mm.getAdminCode()&&(d["userId"]==null|| d["userId"].ToString()=="")){
                    STORE.LOG.SysLog log = new LOG.SysLog();
                    log.Info(DateTime.Now, tokenUserId, "系统超级管理员", ClientIp, 0, "info", "", 1);
                    return Json(new
                    {
                        code = 2000,
                        message = "",
                        roles = JsonConvert.DeserializeObject("['admin']"),
                        name = "系统超级管理员",
                        userCode = tokenUserId,
                        token = d["token"].ToString(),
                        introduction = "",
                        avatar = "",
                        sysCode = "1",
                        sysName = mm.getSysName(),
                        userId = tokenUserId,
                        userSex = 0,
                        departCode = "",
                        departName = ""
                    });
                }
                //string token = STORE.UTILITY.AccessTokenTool.GetAccessToken(d["userId"].ToString());
                string token = STORE.UTILITY.AccessTokenTool.GetAccessToken(userID);
                //DataTable dt = mm.GetUserAndOrgByUserId(d["userId"].ToString());
                DataTable dt = mm.GetUserAndOrgByUserId(userID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string _name = dt.Rows[0]["USER_NAME"] == null ? "" : dt.Rows[0]["USER_NAME"].ToString();
                    string _userCode = dt.Rows[0]["USER_DOMAIN"] == null ? "" : dt.Rows[0]["USER_DOMAIN"].ToString();
                    string _userId = dt.Rows[0]["USER_ID"] == null ? "" : dt.Rows[0]["USER_ID"].ToString();
                    int _userSex = Convert.ToInt32(dt.Rows[0]["USER_SEX"].ToString());
                    string _deptCode = dt.Rows[0]["ORG_CODE"] == null ? "" : dt.Rows[0]["ORG_CODE"].ToString();
                    string _deptName = dt.Rows[0]["ORG_NAME"] == null ? "" : dt.Rows[0]["ORG_NAME"].ToString();
                    STORE.LOG.SysLog log = new LOG.SysLog();
                    //log.Info(DateTime.Now, d["userId"].ToString(), _name, ClientIp, 0, "info", "",1);
                    log.Info(DateTime.Now, userID, _name, ClientIp, 0, "info", "", 1);
                    return Json(new
                    {
                        code = 2000,
                        message = "",
                        roles = new Dictionary<string, object>(),
                        token = token,
                        introduction = "",
                        avatar = "",
                        name = _name,
                        userCode = _userCode,
                        sysCode = "1",
                        sysName = mm.getSysName(),
                        userId = _userId,
                        userSex = _userSex,
                        departCode = _deptCode,
                        departName = _deptName
                    });
                }
                return Json(new
                {
                    code = 2000,
                    message = "",
                    roles = "",
                    name = "",
                    userCode = "",
                    token = token,
                    introduction = "",
                    avatar = "",
                    sysCode = "1",
                    sysName = mm.getSysName(),
                    userId = "",
                    userSex = 0,
                    departCode = "",
                    departName = ""
                });
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            return Json(r);
        }
        
        /// <summary>
        /// 查询用户信息(包括角色信息)
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        // GET api/values
        [HttpGet("fetchUserRoleList")]
        public IActionResult fetchUserRoleList(string limit, string page, string USER_NAME, int? FLAG, string sort, string roleId)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["USER_NAME"] = USER_NAME;
            d["FLAG"] = FLAG;
            d["sort"] = sort;
            d["roleId"] = roleId;
            Dictionary<string, object> res = mm.fetchUserRoleList(d);
            return Json(res);
        }

    }
}