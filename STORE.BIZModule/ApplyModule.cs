using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using STORE.ODS;
using STORE.UTILITY;

namespace STORE.BIZModule
{
   public class ApplyModule
    {
        ApplyDB db = new ApplyDB();
        public Dictionary<string, object> fetchApplyRecordList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                if (d["userId"] == null ||d["userId"].ToString()=="") {
                    r["total"] = 0;
                    r["items"] = null;
                    r["noReadCount"] = 0;
                    r["code"] = -1;
                    r["message"] = "查询失败,用户id为空";
                    return r;
                }
                DataSet ds = db.getApplyRecord(d["userId"].ToString());
                if (ds!=null&&ds.Tables.Count>0) {
                    DataTable dtDetail = ds.Tables["detail"];
                    DataTable dtNoRead = ds.Tables["noReadCount"];
                    if (dtDetail!=null&&dtDetail.Rows.Count>0) {
                        r["total"] = dtDetail.Rows.Count;
                        r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dtDetail, page, limit));
                        r["noReadCount"] = 0;
                        if (dtNoRead != null&&dtNoRead.Rows.Count > 0)
                        {
                            r["noReadCount"] = dtNoRead.Rows[0][0] == null ? 0 : int.Parse(dtNoRead.Rows[0][0].ToString());
                        }
                        r["code"] = 2000;
                        r["message"] = "";
                        return r;
                    }
                    
                    r["noReadCount"] = 0;
                    r["total"] = 0;
                    r["items"] = null;
                    r["code"] = 2000;
                    r["message"] = "";
                }

            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = new Dictionary<string, object>();
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;

        }
        /// <summary>
        /// 更新已读
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>

        public Dictionary<string, object> updateApplyRecord(Dictionary<string, object> d) {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (d["RECORD_ID"]==null||d["RECORD_ID"].ToString()=="") {
                    r["code"] = -1;
                    r["message"] = "请选择一条信息";
                    return r;
                }
                string res = db.updateApplyRecord(d["RECORD_ID"].ToString());
                if (res == "")
                {
                    r["code"] = 2000;
                    r["message"] = "";
                    return r;
                }
                else {
                    r["code"] = -1;
                    r["message"] = res;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;

        }
        /// <summary>
        /// 创建申请信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> createApply(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string res = db.createApply(d);
                if (res == "")
                {
                    r["code"] = 2000;
                    r["message"] = "";
                    return r;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = res;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;

        }
        /// <summary>
        ///组件信息列表
        /// </summary>
        public Dictionary<string, object> fetchComponentList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
            int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
            try
            {
                DataTable dt = db.getComponent();
                if (dt!=null&&dt.Rows.Count>0) {
                    r["code"] = 2000;
                    r["message"] = "";
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    return r;
                }
                r["code"] = 2000;
                r["message"] = "";
                r["total"] = 0;
                r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new DataTable()));
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = new Dictionary<string, object>();
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        /// <summary>
        /// 获取组件详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <param name="resourceid"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchComponentDetailList(string userid,string projectid,string resourceid) {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.getComponentDetail(userid,projectid,resourceid);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dtCom = ds.Tables["dtCom"];
                    DataTable dtComDetail = ds.Tables["dtComDetail"];
                    ComponentModule mod = new ComponentModule();
                    if (dtCom!=null&&dtCom.Rows.Count>0) {
                        mod.COMPONENT_ID = dtCom.Rows[0]["COMPONENT_ID"] ==null?"": dtCom.Rows[0]["COMPONENT_ID"].ToString();
                        mod.COMPONENT_CODE = dtCom.Rows[0]["COMPONENT_CODE"] == null ? "" : dtCom.Rows[0]["COMPONENT_CODE"].ToString();
                        mod.COMPONENT_NAME = dtCom.Rows[0]["COMPONENT_NAME"] == null ? "" : dtCom.Rows[0]["COMPONENT_NAME"].ToString();
                        mod.COMPONENT_CONTENT = dtCom.Rows[0]["COMPONENT_CONTENT"] == null ? "" : dtCom.Rows[0]["COMPONENT_CONTENT"].ToString();
                        mod.DOWNLOAD_TIMES = dtCom.Rows[0]["DOWNLOAD_TIMES"] == null ?0 : int.Parse(dtCom.Rows[0]["DOWNLOAD_TIMES"].ToString());
                        mod.MANAGE_ORG_ID = dtCom.Rows[0]["MANAGE_ORG_ID"] == null ? "" : dtCom.Rows[0]["MANAGE_ORG_ID"].ToString();
                        mod.MANAGE_ORG_NAME = dtCom.Rows[0]["MANAGE_ORG_NAME"] == null ? "" : dtCom.Rows[0]["MANAGE_ORG_NAME"].ToString();
                        mod.MANAGE_TEL = dtCom.Rows[0]["MANAGE_TEL"] == null ? "" : dtCom.Rows[0]["MANAGE_TEL"].ToString();
                        mod.MANAGE_ROLE_ID = dtCom.Rows[0]["MANAGE_ROLE_ID"] == null ? "" : dtCom.Rows[0]["MANAGE_ROLE_ID"].ToString();
                        if (dtCom.Rows[0]["COMPONENT_SIZE"] == null || dtCom.Rows[0]["COMPONENT_SIZE"].ToString() == "")
                        {
                            mod.COMPONENT_SIZE = 0.0;
                        }
                        else {
                            mod.COMPONENT_SIZE =  double.Parse(dtCom.Rows[0]["COMPONENT_SIZE"].ToString());
                        }
                        mod.SOFTWARE_LANGUAGE = dtCom.Rows[0]["SOFTWARE_LANGUAGE"] == null ? "" : dtCom.Rows[0]["SOFTWARE_LANGUAGE"].ToString();
                        mod.IS_DELETE = dtCom.Rows[0]["IS_DELETE"] == null ? 0 : int.Parse(dtCom.Rows[0]["IS_DELETE"].ToString());
                        mod.CREATER = dtCom.Rows[0]["CREATER"] == null ? "" : dtCom.Rows[0]["CREATER"].ToString();
                        mod.CREATE_DATE = dtCom.Rows[0]["CREATE_DATE"] == null ? DateTime.Now : Convert.ToDateTime(dtCom.Rows[0]["CREATE_DATE"].ToString());
                        mod.CHECK_STATE = dtCom.Rows[0]["CHECK_STATE"] == null ? -1 : int.Parse(dtCom.Rows[0]["CHECK_STATE"].ToString());
                        List<ComponentDetail> list = new List<ComponentDetail>();
                        if (dtComDetail!=null&&dtComDetail.Rows.Count>0) {
                            foreach  (DataRow row in dtComDetail.Rows)
                            {
                                ComponentDetail detail = new ComponentDetail();
                                detail.COMPONENT_DETAIL_ID = dtComDetail.Rows[0]["COMPONENT_DETAIL_ID"] == null ? "" : dtComDetail.Rows[0]["COMPONENT_DETAIL_ID"].ToString(); 
                                detail.COMPONENT_ID = dtComDetail.Rows[0]["COMPONENT_ID"] == null ? "" : dtComDetail.Rows[0]["COMPONENT_ID"].ToString(); 
                                detail.CREATER = dtComDetail.Rows[0]["CREATER"] == null ? "" : dtComDetail.Rows[0]["CREATER"].ToString(); 
                                detail.CREATE_DATE = dtComDetail.Rows[0]["CREATE_DATE"] == null ? DateTime.Now : Convert.ToDateTime(dtComDetail.Rows[0]["CREATE_DATE"].ToString());
                                detail.FILE_NAME = dtComDetail.Rows[0]["FILE_NAME"] == null ? "" : dtComDetail.Rows[0]["FILE_NAME"].ToString();
                                if (dtComDetail.Rows[0]["FILE_SIZE"] == null|| dtComDetail.Rows[0]["FILE_SIZE"].ToString()=="") {
                                    detail.FILE_SIZE =  0.0 ;
                                }
                                else {
                                    detail.FILE_SIZE = double.Parse(dtComDetail.Rows[0]["FILE_SIZE"].ToString());
                                }
                                detail.FILE_TYPE = dtComDetail.Rows[0]["FILE_TYPE"] == null ?1 : int.Parse(dtComDetail.Rows[0]["FILE_TYPE"].ToString()); 
                                detail.FILE_URL = dtComDetail.Rows[0]["FILE_URL"] == null ? "" : dtComDetail.Rows[0]["FILE_URL"].ToString();
                                list.Add(detail);
                            }
                        }
                        mod.children = list;
                    }
                    r["code"] = 2000;
                    r["message"] = "";
                    r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(mod));
                    return r;
                }
                r["code"] = 2000;
                r["message"] = "";
                r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new DataTable()));
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = new Dictionary<string, object>();
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        /// <summary>
        /// 获取服务信息
        /// </summary>
        public Dictionary<string, object> fetchServerList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
            int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
            try
            {
                DataTable dt = db.getServer();
                if (dt != null && dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "";
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    return r;
                }
                r["code"] = 2000;
                r["message"] = "";
                r["total"] = 0;
                r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new DataTable()));
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = new Dictionary<string, object>();
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        /// <summary>
        /// 获取服务详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <param name="resourceid"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchServerDetailList(string userid, string projectid, string resourceid)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.getServerDetail(userid, projectid, resourceid);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dtSer = ds.Tables["dtServer"];
                    DataTable dtSerDetail = ds.Tables["dtServerDetail"];
                    ServerMoudle mod = new ServerMoudle();
                    if (dtSer != null && dtSer.Rows.Count > 0)
                    {
                        mod.SERVICE_ID = dtSer.Rows[0]["SERVICE_ID"] == null ? "" : dtSer.Rows[0]["SERVICE_ID"].ToString();
                        mod.SERVICE_CODE = dtSer.Rows[0]["SERVICE_CODE"] == null ? "" : dtSer.Rows[0]["SERVICE_CODE"].ToString();
                        mod.SERVICE_NAME = dtSer.Rows[0]["SERVICE_NAME"] == null ? "" : dtSer.Rows[0]["SERVICE_NAME"].ToString();
                        mod.SERVICE_CONTENT = dtSer.Rows[0]["SERVICE_CONTENT"] == null ? "" : dtSer.Rows[0]["SERVICE_CONTENT"].ToString();
                        if (dtSer.Rows[0]["SERVICE_TIMES"] == null || dtSer.Rows[0]["SERVICE_TIMES"].ToString() == "")
                        {
                            mod.SERVICE_TIMES = 0;
                        }
                        else {
                            mod.SERVICE_TIMES = int.Parse(dtSer.Rows[0]["SERVICE_TIMES"].ToString());
                        }
                        mod.MANAGE_ORG_ID = dtSer.Rows[0]["MANAGE_ORG_ID"] == null ? "" : dtSer.Rows[0]["MANAGE_ORG_ID"].ToString();
                        mod.MANAGE_TEL = dtSer.Rows[0]["MANAGE_TEL"] == null ? "" : dtSer.Rows[0]["MANAGE_TEL"].ToString();
                        mod.MANAGE_ROLE_ID = dtSer.Rows[0]["MANAGE_ROLE_ID"] == null ? "" : dtSer.Rows[0]["MANAGE_ROLE_ID"].ToString();
                        mod.SERVICE_URL= dtSer.Rows[0]["SERVICE_URL"] == null ? "" : dtSer.Rows[0]["SERVICE_URL"].ToString();
                        mod.SUIT_PLAT = dtSer.Rows[0]["SUIT_PLAT"] == null ? "" : dtSer.Rows[0]["SUIT_PLAT"].ToString();
                        mod.IS_DELETE = dtSer.Rows[0]["IS_DELETE"] == null ? 0 : int.Parse(dtSer.Rows[0]["IS_DELETE"].ToString());
                        mod.CREATER = dtSer.Rows[0]["CREATER"] == null ? "" : dtSer.Rows[0]["CREATER"].ToString();
                        if (dtSer.Rows[0]["CREATE_DATE"] == null || dtSer.Rows[0]["CREATE_DATE"].ToString() == "")
                        {
                            mod.CREATE_DATE=DateTime.Now;
                        }
                        else {
                            mod.CREATE_DATE = Convert.ToDateTime(dtSer.Rows[0]["CREATE_DATE"].ToString());
                        }
                        if (dtSer.Rows[0]["CHECK_STATE"] == null || dtSer.Rows[0]["CHECK_STATE"].ToString() == "")
                        {
                            mod.CHECK_STATE = -1;
                        }
                        else
                        {
                            mod.CHECK_STATE = Convert.ToInt32(dtSer.Rows[0]["CHECK_STATE"].ToString());
                        }
                        List<ServerDetail> list = new List<ServerDetail>();
                        if (dtSerDetail != null && dtSerDetail.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtSerDetail.Rows)
                            {
                                ServerDetail detail = new ServerDetail();
                                detail.SERVICE_DETAIL_ID = dtSerDetail.Rows[0]["SERVICE_DETAIL_ID"] == null ? "" : dtSerDetail.Rows[0]["SERVICE_DETAIL_ID"].ToString();
                                detail.SERVICE_ID = dtSerDetail.Rows[0]["SERVICE_ID"] == null ? "" : dtSerDetail.Rows[0]["SERVICE_ID"].ToString();
                                detail.CREATER = dtSerDetail.Rows[0]["CREATER"] == null ? "" : dtSerDetail.Rows[0]["CREATER"].ToString();
                               
                                if (dtSerDetail.Rows[0]["CREATE_DATE"] == null || dtSerDetail.Rows[0]["CREATE_DATE"].ToString() == "")
                                {
                                    detail.CREATE_DATE = DateTime.Now;
                                }
                                else
                                {
                                    detail.CREATE_DATE = Convert.ToDateTime(dtSerDetail.Rows[0]["CREATE_DATE"].ToString());
                                }
                                detail.FILE_NAME = dtSerDetail.Rows[0]["FILE_NAME"] == null ? "" : dtSerDetail.Rows[0]["FILE_NAME"].ToString();
                                if (dtSerDetail.Rows[0]["FILE_SIZE"] == null || dtSerDetail.Rows[0]["FILE_SIZE"].ToString() == "")
                                {
                                    detail.FILE_SIZE = 0.0;
                                }
                                else
                                {
                                    detail.FILE_SIZE = double.Parse(dtSerDetail.Rows[0]["FILE_SIZE"].ToString());
                                }
                                detail.FILE_TYPE = dtSerDetail.Rows[0]["FILE_TYPE"] == null ? 1 : int.Parse(dtSerDetail.Rows[0]["FILE_TYPE"].ToString());
                                detail.FILE_URL = dtSerDetail.Rows[0]["FILE_URL"] == null ? "" : dtSerDetail.Rows[0]["FILE_URL"].ToString();
                                list.Add(detail);
                            }
                        }
                        mod.children = list;
                       
                    }
                    r["code"] = 2000;
                    r["message"] = "";
                    r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(mod));
                    return r;
                }
                r["code"] = 2000;
                r["message"] = "";
                r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new DataTable()));
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = new Dictionary<string, object>();
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
    }
}
