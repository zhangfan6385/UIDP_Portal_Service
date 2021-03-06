﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using STORE.UTILITY;
namespace STORE.ODS
{
    public class ApplyDB
    {
        DBTool db = new DBTool("");
        /// <summary>
        /// 1 查询个人邮件信息 2 查询未读信息数据流量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet getApplyRecord(string userId)
        {
            string sql = "select * from  ts_store_application_record where  USER_ID='" + userId + "'  order by  RECORD_ISREAD asc ,CREATE_DATE desc";
            string sql2 = " select count(*) from  ts_store_application_record where  RECORD_ISREAD=0 and USER_ID='" + userId + "' ";
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("detail", sql);
            list.Add("noReadCount", sql2);
            return db.GetDataSet(list);
        }
        /// <summary>
        /// 标记信息已读
        /// </summary>
        /// <param name="RECORD_ID"></param>
        /// <returns></returns>
        public string updateApplyRecord(string RECORD_ID)
        {
            string SQL = "UPDATE ts_store_application_record SET RECORD_ISREAD=1 WHERE RECORD_ID='" + RECORD_ID + "'";
            return db.ExecutByStringResult(SQL);
        }

        public string createApply(Dictionary<string, object> d)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" insert into ts_store_application (APPLY_ID,APPLY_ORG_ID,APPLY_ORG_NAME," +
                "PROJECT_ID,PROJECT_NAME,APPLY_TYPE,USE_CONTENT,USE_TYPE,APPLY_RESOURCE_ID" +
                ",APPLY_LINKMAN,APPLY_PHONE,APPLY_EMAIL,APPLY_USERID,APPLY_DATE,CHECK_STATE,IS_DELETE)  ");
            sb.Append("  values( '" + Guid.NewGuid().ToString() + "',");
            sb.Append("'");
            sb.Append(d["APPLY_ORG_ID"] == null ? "" : d["APPLY_ORG_ID"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["APPLY_ORG_NAME"] == null ? "" : d["APPLY_ORG_NAME"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["PROJECT_ID"] == null ? "" : d["PROJECT_ID"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["PROJECT_NAME"] == null ? "" : d["PROJECT_NAME"].ToString() + "',");

            sb.Append(d["APPLY_TYPE"] == null ? "" : d["APPLY_TYPE"].ToString() + ",");
            sb.Append("'");
            sb.Append(d["USE_CONTENT"] == null ? "" : d["USE_CONTENT"].ToString() + "',");
            sb.Append(d["USE_TYPE"] == null ? "" : d["USE_TYPE"].ToString() + ",");
            sb.Append("'");
            sb.Append(d["APPLY_RESOURCE_ID"] == null ? "" : d["APPLY_RESOURCE_ID"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["APPLY_LINKMAN"] == null ? "" : d["APPLY_LINKMAN"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["APPLY_PHONE"] == null ? "" : d["APPLY_PHONE"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["APPLY_EMAIL"] == null ? "" : d["APPLY_EMAIL"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["APPLY_USERID"] == null ? "" : d["APPLY_USERID"].ToString() + "',");
            sb.Append("'");
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,0");
            sb.Append("  ) ");
            return db.ExecutByStringResult(sb.ToString());
        }
        /// <summary>
        /// 查询是否已申请
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable getApply(Dictionary<string, object> d) {
            string sql1 = " select APPLY_ID from  ts_store_application where APPLY_USERID='" + d["APPLY_USERID"] + "' ";
            sql1 += " and PROJECT_ID='" + d["PROJECT_ID"] + "' and APPLY_RESOURCE_ID='" + d["APPLY_RESOURCE_ID"] + "'";
            sql1 += "and CHECK_STATE=2 ";
            DataTable app = new DataTable();
            app = db.GetDataTable(sql1);
            if (app.Rows.Count >=1)
            {
                string sql2= "update ts_store_application SET IS_DELETE=1 where APPLY_USERID='" + d["APPLY_USERID"] + "' ";
                sql2 += " and PROJECT_ID='" + d["PROJECT_ID"] + "' and APPLY_RESOURCE_ID='" + d["APPLY_RESOURCE_ID"] + "'";
                sql2 += "and CHECK_STATE=2 ";
                db.GetDataTable(sql2);
            }
            string sql = " select * from  ts_store_application where APPLY_USERID='"+ d["APPLY_USERID"]+"' ";
            sql+= " and PROJECT_ID='"+ d["PROJECT_ID"] + "' and APPLY_RESOURCE_ID='"+ d["APPLY_RESOURCE_ID"] + "'";
            sql += " and (CHECK_STATE=0 or (CHECK_STATE=1 and (APPLY_EXPIRET>'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' or APPLY_EXPIRET='"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'))) ";
            return db.GetDataTable(sql);
        }
        /// <summary>
        ///组件信息和top
        /// </summary>
        public DataTable getComponent(string name)
        {
            string sql = " select * from ts_store_component  where IS_DELETE=0  ";
            if (!string.IsNullOrEmpty(name))
            {
                sql += " and COMPONENT_NAME like '%" + name + "%' ";
            }
            sql += " ORDER BY  DOWNLOAD_TIMES DESC; ";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 获取组件详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <param name="resourceid"></param>
        /// <returns></returns>
        public DataSet getComponentDetail(string userid, string projectid, string resourceid)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            string sql = " select a.* ";

            if (userid != null && userid != "")
            {
                sql += " ,(select b.CHECK_STATE  from ts_store_application b   ";
                sql += " where a.COMPONENT_ID=b.APPLY_RESOURCE_ID  ";
                sql += " and (b.APPLY_EXPIRET>'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' or b.APPLY_EXPIRET is null or b.APPLY_EXPIRET='') ";
                sql += " AND b.IS_DELETE='0'";
                sql += "AND (b.CHECK_STATE!=2)";
                sql += " and case when b.PROJECT_ID is null or b.PROJECT_ID='' THEN '" + projectid + "' else b.PROJECT_ID end = '" + projectid + "' ";
                sql += " and case when b.APPLY_USERID is null or b.APPLY_USERID='' THEN '" + userid + "' else b.APPLY_USERID end = '" + userid + "' ";
                sql += " AND b.IS_DELETE='0'";
                sql += " ) CHECK_STATE ";//按照有效期判断审核状态 登录后调用
            }
            else
            {
                sql += " ,-1 CHECK_STATE";
            }
            sql += " from ts_store_component a where a.COMPONENT_ID='"+resourceid+"' ";
            string sql2 = " select * from  ts_store_component_detail where COMPONENT_ID='" + resourceid + "' ";
            sql2 +="AND IS_DELETE='0'";
            d.Add("dtCom", sql);
            d.Add("dtComDetail", sql2);
            return db.GetDataSet(d);
        }
        /// <summary>
        /// 获取服务信息top
        /// </summary>
        public DataTable getServer(string name)
        {
            string sql2 = " select* from ts_store_service  where IS_DELETE=0  ";
            if (!string.IsNullOrEmpty(name)) {
                sql2 += " and SERVICE_NAME like '%"+name+"%' ";
            }
            sql2 += " ORDER BY  SERVICE_TIMES DESC; ";
            return db.GetDataTable(sql2);
        }
        /// <summary>
        /// 获取服务详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <param name="resourceid"></param>
        /// <returns></returns>
        public DataSet getServerDetail(string userid, string projectid, string resourceid)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            string sql = " select a.*  ";

            if (userid != null && userid != "")
            {
                sql += " ,(select b.CHECK_STATE  from ts_store_application b   ";
                sql += " where a.SERVICE_ID=b.APPLY_RESOURCE_ID  ";
                sql += " and (b.APPLY_EXPIRET>'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' or b.APPLY_EXPIRET is null or b.APPLY_EXPIRET='') ";
                sql += " AND b.IS_DELETE='0'";
                sql += " and case when b.PROJECT_ID is null or b.PROJECT_ID='' THEN '" + projectid + "' else b.PROJECT_ID end = '" + projectid + "' ";
                sql += " and case when b.APPLY_USERID is null or b.APPLY_USERID='' THEN '" + userid + "' else b.APPLY_USERID end = '" + userid + "' ";
                sql += " ) CHECK_STATE ";//按照有效期判断审核状态 登录后调用
                sql += " ,(select b.SERVICE_CODE  from ts_store_application b   ";
                sql += " where a.SERVICE_ID=b.APPLY_RESOURCE_ID  ";
                sql += " and (b.APPLY_EXPIRET>'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' or b.APPLY_EXPIRET is null or b.APPLY_EXPIRET='') ";
                sql += " AND b.IS_DELETE='0'";
                sql += " and case when b.PROJECT_ID is null or b.PROJECT_ID='' THEN '" + projectid + "' else b.PROJECT_ID end = '" + projectid + "' ";
                sql += " and case when b.APPLY_USERID is null or b.APPLY_USERID='' THEN '" + userid + "' else b.APPLY_USERID end = '" + userid + "' ";
                sql += " ) SERVICE_CODE2 ";//按照有效期判断审核状态 登录后调用
            }
            else
            {
                sql += " ,-1 CHECK_STATE,'' SERVICE_CODE2";
            }

            sql += "  from ts_store_service a  where a.SERVICE_ID='" + resourceid + "' ";
            string sql2 = " select * from  ts_store_service_detail where SERVICE_ID='" + resourceid + "' ";
            sql2 += "AND IS_DELETE='0'";
            d.Add("dtServer", sql);
            d.Add("dtServerDetail", sql2);
            return db.GetDataSet(d);
        }
        /// <summary>
        /// 获取开发平台信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <param name="resourceid"></param>
        /// <param name="platType"></param>
        /// <returns></returns>
        public DataSet getPlatform(string userid, string projectid, string resourceid,int platType)
        {

            Dictionary<string, string> d = new Dictionary<string, string>();
            string sql = " select a.*  ";
            if (userid != null && userid != "")
            {
                sql += " ,(select b.CHECK_STATE  from ts_store_application b   ";
                sql += " where a.PLAT_ID=b.APPLY_RESOURCE_ID  ";
                //sql += " and (b.APPLY_EXPIRET>'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' or b.APPLY_EXPIRET is null or b.APPLY_EXPIRET='') ";
                sql += "AND (b.IS_DELETE=0)";
                sql += " and case when b.PROJECT_ID is null or b.PROJECT_ID='' THEN '" + projectid + "' else b.PROJECT_ID end = '" + projectid + "' ";
                sql += " and case when b.APPLY_USERID is null or b.APPLY_USERID='' THEN '" + userid + "' else b.APPLY_USERID end = '" + userid + "' ";
                sql += " ) CHECK_STATE ";//按照有效期判断审核状态 登录后调用
            }
            else {
                sql += " ,-1 CHECK_STATE";
            }
            sql += "  from ts_store_platform a  ";
            sql += " where a.IS_DELETE=0 and a.PLAT_TYPE=" + platType;
            sql += " order by a.CREATE_DATE desc ";
            string sql2 = " select * from  ts_store_platform_detail where IS_DELETE=0";
            d.Add("dtPlat", sql);
            d.Add("dtPlatDetail", sql2);
            return db.GetDataSet(d);
        }

        public DataSet getPlatDetail (string userid,string projectid,string resourceid)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            string sql = " select a.*  ";
            if (userid != null && userid != "")
            {
                sql += " ,(select b.CHECK_STATE  from ts_store_application b   ";
                sql += " where a.PLAT_ID=b.APPLY_RESOURCE_ID  ";
                //sql += " and (b.APPLY_EXPIRET>'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' or b.APPLY_EXPIRET is null or b.APPLY_EXPIRET='') ";
                sql += "AND (b.IS_DELETE=0)";
                sql += " and case when b.PROJECT_ID is null or b.PROJECT_ID='' THEN '" + projectid + "' else b.PROJECT_ID end = '" + projectid + "' ";
                sql += " and case when b.APPLY_USERID is null or b.APPLY_USERID='' THEN '" + userid + "' else b.APPLY_USERID end = '" + userid + "' ";
                sql += " ) CHECK_STATE ";//按照有效期判断审核状态 登录后调用
            }
            else
            {
                sql += " ,-1 CHECK_STATE";
            }
            sql += "  from ts_store_platform a  ";
            sql += "where a.PLAT_ID='" + resourceid + "'";
            string sql2 = "select * from ts_store_platform_detail where PLAT_ID='" + resourceid + "'";
            sql2 += "and IS_DELETE='0'";
            d.Add("dtPlat", sql);
            d.Add("dtPlatDetail", sql2);
            return db.GetDataSet(d);
        }
    }
}
