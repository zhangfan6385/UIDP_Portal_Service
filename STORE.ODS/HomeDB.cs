using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using STORE.UTILITY;

namespace STORE.ODS
{
   public  class HomeDB
    {
        DBTool db = new DBTool("");
        public DataSet fetchNoticeList(Dictionary<string, object> d)
        {
            Dictionary<string, string> sqld = new Dictionary<string, string>();
            if (d["id"]==null||d["id"].ToString() == "")
            {
                sqld.Add("store", "select * from ts_store_notice where IS_DELETE=0 order by CREATE_DATE desc ;");
                sqld.Add("storeDetail", "select * from  ts_store_notice_detail where IS_DELETE=0 order by CREATE_DATE desc ");
            }
            else {
                sqld.Add("store", "select * from ts_store_notice where  IS_DELETE=0 and NOTICE_ID='" + d["id"].ToString()+ "' order by CREATE_DATE desc ;");
                sqld.Add("storeDetail", "select * from  ts_store_notice_detail where IS_DELETE=0 and NOTICE_ID='" + d["id"].ToString() + "' order by CREATE_DATE desc ");
            }
            
            return db.GetDataSet(sqld);
        }
        /// <summary>
        /// 根据申请类型按月分组查询下载或者调用次数
        /// </summary>
        /// <param name="applytype"></param>
        /// <returns></returns>
        public DataTable getCountByMonth(string applytype) {
            string sql = "select  count(*) TOTAL,MONTH(CHECK_DATE) CHECK_MONTH from ts_store_application where APPLY_TYPE ="+ applytype + " ";
            sql += "   and CHECK_STATE=1 AND YEAR(CHECK_DATE)=YEAR('" + DateTime.Now.ToString("yyyy-MM-dd")+"') ";
            sql += " group by month(CHECK_DATE) ORDER BY  MONTH(CHECK_DATE) DESC ";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 获取服务和组件top
        /// </summary>
        public DataSet getCountTop() {
            Dictionary<string, string> sqld = new Dictionary<string, string>();
            string sql = " select DOWNLOAD_TIMES,COMPONENT_NAME from ts_store_component  where IS_DELETE=0 ORDER BY  DOWNLOAD_TIMES DESC; ";
            string sql2 = " select SERVICE_TIMES,SERVICE_NAME from ts_store_service  where IS_DELETE=0 ORDER BY  SERVICE_TIMES DESC; ";
            sqld.Add("comp",sql);
            sqld.Add("server",sql2);
            return db.GetDataSet(sqld);
        }
    }
}
