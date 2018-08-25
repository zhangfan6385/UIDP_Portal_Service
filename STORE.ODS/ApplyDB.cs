using System;
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
            string sql = "select * from  ts_store_application_record where  USER_ID='" + userId + "'  order by  RECORD_ISREAD asc ,RECORD_CREATEDATE desc";
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
                ",APPLY_LINKMAN,APPLY_PHONE,APPLY_EMAIL,APPLY_USERID)  ");
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
            sb.Append("'");
            sb.Append(d["USE_TYPE"] == null ? "" : d["USE_TYPE"].ToString() + ",");
            sb.Append(d["APPLY_RESOURCE_ID"] == null ? "" : d["APPLY_RESOURCE_ID"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["APPLY_LINKMAN"] == null ? "" : d["APPLY_LINKMAN"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["APPLY_PHONE"] == null ? "" : d["APPLY_PHONE"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["APPLY_EMAIL"] == null ? "" : d["APPLY_EMAIL"].ToString() + "',");
            sb.Append("'");
            sb.Append(d["APPLY_USERID"] == null ? "" : d["APPLY_USERID"].ToString() + "'");
            sb.Append("  ) ");
            return db.ExecutByStringResult(sb.ToString());
        }
        /// <summary>
        ///组件信息和top
        /// </summary>
        public DataTable getComponent()
        {
            string sql = " select * from ts_store_component  where IS_DELETE=0 ORDER BY  DOWNLOAD_TIMES DESC; ";
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
            string sql = " select a.*,b.CHECK_STATE from ts_store_component a  ";
            sql += " left join ts_store_application b on a.COMPONENT_ID=b.APPLY_RESOURCE_ID ";

            sql += " where case when b.PROJECT_ID is null or b.PROJECT_ID='' THEN '" + projectid + "' else b.PROJECT_ID end = '" + projectid + "' ";
            sql += " and case when b.APPLY_USERID is null or b.APPLY_USERID='' THEN '" + userid + "' else b.APPLY_USERID end = '" + userid + "' ";
            sql += " and case when b.APPLY_RESOURCE_ID is null or b.APPLY_RESOURCE_ID='' THEN '" + resourceid + "' else b.APPLY_RESOURCE_ID end = '" + resourceid + "' ";
           
            sql += " order by CHECK_DATE,APPLY_DATE desc ";
            string sql2 = " select * from  ts_store_component_detail where COMPONENT_ID='" + resourceid + "' ";
            d.Add("dtCom", sql);
            d.Add("dtComDetail", sql2);
            return db.GetDataSet(d);
        }
        /// <summary>
        /// 获取服务信息top
        /// </summary>
        public DataTable getServer()
        {
            string sql2 = " select* from ts_store_service  where IS_DELETE=0 ORDER BY  SERVICE_TIMES DESC; ";
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
            string sql = " select a.*,b.CHECK_STATE from ts_store_service a  ";
            sql += " left join ts_store_application b on a.SERVICE_ID=b.APPLY_RESOURCE_ID ";
            sql += " where case when b.PROJECT_ID is null or b.PROJECT_ID='' THEN '" + projectid + "' else b.PROJECT_ID end = '" + projectid + "' ";
            sql += " and case when b.APPLY_USERID is null or b.APPLY_USERID='' THEN '" + userid + "' else b.APPLY_USERID end = '" + userid + "' ";
            sql += " and case when b.APPLY_RESOURCE_ID is null or b.APPLY_RESOURCE_ID='' THEN '" + resourceid + "' else b.APPLY_RESOURCE_ID end = '" + resourceid + "' ";

            sql += " order by CHECK_DATE,APPLY_DATE desc ";
            string sql2 = " select * from  ts_store_service_detail where SERVICE_ID='" + resourceid + "' ";
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
            string sql = " select a.*,b.CHECK_STATE from ts_store_platform a  ";
            sql += " left join ts_store_application b on a.PLAT_ID=b.APPLY_RESOURCE_ID ";
            sql += " where a.IS_DELETE=0 and a.PLAT_TYPE=" + platType;
            sql += " and case when b.PROJECT_ID is null or b.PROJECT_ID='' THEN '"+projectid+"' else b.PROJECT_ID end = '"+projectid+"' ";
            sql += " and case when b.APPLY_USERID is null or b.APPLY_USERID='' THEN '" + userid + "' else b.APPLY_USERID end = '" + userid + "' ";
            sql += " and case when b.APPLY_RESOURCE_ID is null or b.APPLY_RESOURCE_ID='' THEN '" + resourceid + "' else b.APPLY_RESOURCE_ID end = '" + resourceid + "' ";
            sql += " order by CHECK_DATE,APPLY_DATE,a.CREATE_DATE desc ";
            string sql2 = " select * from  ts_store_platform_detail where PLAT_ID='" + resourceid + "' ";
            d.Add("dtPlat", sql);
            d.Add("dtPlatDetail", sql2);
            return db.GetDataSet(d);
        }
    }
}
