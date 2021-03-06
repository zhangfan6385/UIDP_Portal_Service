﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace STORE.WebAPI.Controllers
{
    //    [Produces("application/json")]
    [Route("api/WebApiBase")]
    public class WebApiBaseController : Controller
    {
        public string ClientIp = "";
        public string UserId = "";
        public string UserName = "";
        public string accessToken = "";
        public string actionName = "";
       
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            try
            {
                    Microsoft.Extensions.Primitives.StringValues AccessToken;//获取header中某一项的值
                    context.HttpContext.Request.Headers.TryGetValue("X-Token", out AccessToken);
                    ClientIp = Extension.GetClientUserIp(Request.HttpContext);
                    actionName = (context.RouteData.Values["action"]).ToString().ToLower();//获取当前方法
                    if (actionName== "loginconfig") { base.OnActionExecuting(context); return; }
                    //根据实际需求进行具体实现
                    accessToken = AccessToken;
                    if (accessToken == "")
                    {
                        context.Result = new ObjectResult(new { code = 50008, msg = "没有找到X-Token" });
                    }
                    string userId = STORE.UTILITY.AccessTokenTool.GetUserId(AccessToken);
                    UserId = userId;
                    if (actionName == "login")
                    {
                        STORE.UTILITY.Message mes = STORE.UTILITY.AccessTokenTool.IsInValidUser(userId, AccessToken, "user");
                        if (mes.code != 2000)
                        {
                            context.Result = new ObjectResult(mes);
                        }
                    }
                    else
                    {
                        BIZModule.UserModule mm = new BIZModule.UserModule();
                        string admin = mm.getAdminCode();
                        if (userId == admin)
                        {
                            UserName = "系统超级管理员";
                        }
                        else
                        {
                            UserName = mm.getUserInfoByUserId(userId).USER_NAME;
                        }
                        STORE.UTILITY.Message mes = STORE.UTILITY.AccessTokenTool.IsInValidUser(userId, AccessToken, "user");
                        mes = STORE.UTILITY.AccessTokenTool.IsInValidUser(userId, AccessToken, admin);
                        if (mes.code != 2000)
                        {
                            context.Result = new ObjectResult(mes);
                        }
                    }
                STORE.LOG.SysLog log = new LOG.SysLog();
                log.Info(DateTime.Now, userId, UserName, ClientIp, 0, actionName, "",1);
            }
            catch (Exception ex)
            {
                STORE.LOG.SysLog log = new LOG.SysLog();
                log.Info(DateTime.Now, UserId, UserName, ClientIp, 1, actionName, ex.Message.Length > 120 ? ex.Message.Substring(0, 100) : ex.Message,1);
                context.Result = new ObjectResult(new { code = 5008, msg = "验证token时程序出错", result = ex.Message });
            }
            #region 备份原来的逻辑
            /*
              public override void OnActionExecuting(ActionExecutingContext context)
            {
                try
                {
                    Microsoft.Extensions.Primitives.StringValues AccessToken;//获取header中某一项的值
                    context.HttpContext.Request.Headers.TryGetValue("X-Token", out AccessToken);

                    actionName = (context.RouteData.Values["action"]).ToString().ToLower();//获取当前方法
                    //根据实际需求进行具体实现
                    accessToken = AccessToken;
                    if (accessToken == "")
                    {
                        context.Result = new ObjectResult(new { code = 50008, msg = "没有找到X-Token" });
                    }
                    string userId = STORE.UTILITY.AccessTokenTool.GetUserId(AccessToken);
                    BIZModule.UserModule mm = new BIZModule.UserModule();
                    string admin = mm.getAdminCode();
                    if (userId == admin)
                    {
                        UserName = "系统超级管理员";
                    }
                    else
                    {
                        UserName = mm.getUserInfoByUserId(userId).USER_NAME;
                    }
                    STORE.UTILITY.Message mes = STORE.UTILITY.AccessTokenTool.IsInValidUser(userId, AccessToken,admin);
                    if (mes.code != 2000)
                    {
                        context.Result = new ObjectResult(mes);
                    }
                    UserId = userId;
                    ClientIp = Extension.GetClientUserIp(Request.HttpContext);
                    STORE.LOG.SysLog log = new LOG.SysLog();
                    log.Info(DateTime.Now, userId, UserName, ClientIp, 0, actionName, "");
                }
                catch (Exception ex)
                {
                    STORE.LOG.SysLog log = new LOG.SysLog();
                    log.Info(DateTime.Now, UserId, UserName, ClientIp, 1, actionName, ex.Message.Length > 120 ? ex.Message.Substring(0, 100) : ex.Message);
                    context.Result = new ObjectResult(new { code = -1, msg = "验证token时程序出错", result = ex.Message });
                }

            }
             */
            #endregion
        }
    }


}