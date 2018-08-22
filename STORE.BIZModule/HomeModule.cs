using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using STORE.ODS;
using STORE.UTILITY;

namespace STORE.BIZModule
{
  public  class HomeModule
    {
        HomeDB db = new HomeDB();
        /// <summary>
        /// 查询公告信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchNoticeList(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchNoticeList(d);
                if (dt != null && dt.Rows.Count > 0)
                {
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
                else
                {
                    r["total"] = 0;
                    r["items"] = null;
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        public void GetHierarchicalItem(DataTable _RptsDepartList, List<NoticeMode> noticeModes)
        {

            try
            {
                NoticeMode noticeMode;
                foreach (DataRow dr in _RptsDepartList.Select("MENU_ID_UPPER is null or MENU_ID_UPPER='' "))
                {
                    noticeMode = new NoticeMode();
                    noticeMode.NOTICE_ID = dr["NOTICE_ID"].ToString();
                    noticeMode.NOTICE_CODE = dr["NOTICE_CODE"] == null ? "" : dr["NOTICE_CODE"].ToString();
                    noticeMode.NOTICE_TITLE = dr["NOTICE_TITLE"] == null ? "" : dr["NOTICE_TITLE"].ToString();
                    noticeMode.NOTICE_CONTENT = dr["NOTICE_CONTENT"] == null ? "" : dr["NOTICE_CONTENT"].ToString();
                    noticeMode.NOTICE_DATETIME = dr["NOTICE_DATETIME"] == null ? DateTime.Now : DateTime.Parse(dr["NOTICE_DATETIME"].ToString());
                    noticeMode.NOTICE_ORGID = dr["NOTICE_ORGID"].ToString();
                    noticeMode.NOTICE_ORGNAME = dr["NOTICE_ORGNAME"].ToString();
                    noticeMode.CREATER = dr["CREATER"].ToString();
                    noticeMode.CREATE_DATE = dr["CREATE_DATE"] == null ? DateTime.Now : DateTime.Parse(dr["CREATE_DATE"].ToString());

                    noticeMode.NOTICE_DETAIL_ID = dr["NOTICE_DETAIL_ID"].ToString();
                    noticeMode.FILE_URL = dr["FILE_URL"].ToString();
                    noticeMode.FILE_NAME = dr["FILE_NAME"].ToString();
                    noticeMode.FILE_SIZE = dr["FILE_SIZE"].ToString();
                    noticeMode.children = new List<NoticeMode>();
                    GetHierarchicalChildItem(_RptsDepartList, noticeMode);
                    noticeModes.Add(noticeMode);
                }
            }
            catch
            {
            }
        }
        private void GetHierarchicalChildItem(DataTable _RptsDepartList, NoticeMode noticeModes)
        {

            NoticeMode noticeMode;
            foreach (DataRow dr in _RptsDepartList.Select("MENU_ID_UPPER ='" + noticeModes.NOTICE_CODE + "'"))
            {
                noticeMode = new NoticeMode();
                noticeMode.NOTICE_ID = dr["NOTICE_ID"].ToString();
                noticeMode.NOTICE_CODE = dr["NOTICE_CODE"]==null?"":dr["NOTICE_CODE"].ToString();
                noticeMode.NOTICE_TITLE = dr["NOTICE_TITLE"]==null?"":dr["NOTICE_TITLE"].ToString();
                noticeMode.NOTICE_CONTENT = dr["NOTICE_CONTENT"]==null?"":dr["NOTICE_CONTENT"].ToString();
                noticeMode.NOTICE_DATETIME = dr["NOTICE_DATETIME"] == null ? DateTime.Now : DateTime.Parse(dr["NOTICE_DATETIME"].ToString());
                noticeMode.NOTICE_ORGID = dr["NOTICE_ORGID"].ToString();
                noticeMode.NOTICE_ORGNAME = dr["NOTICE_ORGNAME"].ToString();
                noticeMode.CREATER = dr["CREATER"].ToString();
                noticeMode.CREATE_DATE = dr["CREATE_DATE"] == null ? DateTime.Now : DateTime.Parse(dr["CREATE_DATE"].ToString());

                noticeMode.NOTICE_DETAIL_ID = dr["NOTICE_DETAIL_ID"].ToString();
                noticeMode.FILE_URL = dr["FILE_URL"].ToString();
                noticeMode.FILE_NAME = dr["FILE_NAME"].ToString();
                noticeMode.FILE_SIZE = dr["FILE_SIZE"].ToString();
                noticeMode.children = new List<NoticeMode>();
                GetHierarchicalChildItem(_RptsDepartList, noticeMode);
                noticeModes.children.Add(noticeMode);
            }
        }
        
    }
}
