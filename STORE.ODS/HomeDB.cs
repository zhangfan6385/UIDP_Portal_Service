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
        public DataTable fetchNoticeList(Dictionary<string, object> d)
        {
            string sql = "select a.*,b.FILE_NAME,b.FILE_SIZE,b.FILE_URL,b.NOTICE_DETAIL_ID from ts_store_notice a";
            sql += " left join  ts_store_notice_detail b on a.NOTICE_ID=b.NOTICE_ID ";
            return db.GetDataTable(sql);
        }
    }
}
