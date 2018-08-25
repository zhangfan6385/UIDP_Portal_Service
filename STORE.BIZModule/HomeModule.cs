using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using STORE.ODS;
using STORE.UTILITY;

namespace STORE.BIZModule
{
    public class HomeModule
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

                DataSet ds = db.fetchNoticeList(d);
                List<NoticeMode> list = new List<NoticeMode>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataTable dtDetail = new DataTable();
                    if (ds.Tables.Count > 1)
                    {
                        dtDetail = ds.Tables[1];
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            NoticeMode noticeMode = new NoticeMode();
                            noticeMode.NOTICE_ID = dr["NOTICE_ID"].ToString();
                            noticeMode.NOTICE_CODE = dr["NOTICE_CODE"] == null ? "" : dr["NOTICE_CODE"].ToString();
                            noticeMode.NOTICE_TITLE = dr["NOTICE_TITLE"] == null ? "" : dr["NOTICE_TITLE"].ToString();
                            noticeMode.NOTICE_CONTENT = dr["NOTICE_CONTENT"] == null ? "" : dr["NOTICE_CONTENT"].ToString();
                            noticeMode.NOTICE_DATETIME = dr["NOTICE_DATETIME"] == null ? DateTime.Now : DateTime.Parse(dr["NOTICE_DATETIME"].ToString());
                            noticeMode.NOTICE_ORGID = dr["NOTICE_ORGID"].ToString();
                            noticeMode.NOTICE_ORGNAME = dr["NOTICE_ORGNAME"].ToString();
                            noticeMode.CREATER = dr["CREATER"].ToString();
                            noticeMode.CREATE_DATE = dr["CREATE_DATE"] == null ? DateTime.Now : DateTime.Parse(dr["CREATE_DATE"].ToString());
                            List<NoticeMode> listdetail = new List<NoticeMode>();
                            if (dtDetail != null && dtDetail.Rows.Count > 0)
                            {
                                DataRow[] arry = dtDetail.Select("NOTICE_ID='" + dr["NOTICE_ID"].ToString() + "'");
                                listdetail.Clear();
                                if (arry.Length > 0)
                                {
                                    foreach (var item in arry)
                                    {
                                        NoticeMode noticeModeDetail = new NoticeMode();
                                        noticeModeDetail.NOTICE_DETAIL_ID = item["NOTICE_DETAIL_ID"].ToString();
                                        noticeModeDetail.FILE_URL = item["FILE_URL"].ToString();
                                        noticeModeDetail.FILE_NAME = item["FILE_NAME"].ToString();
                                        noticeModeDetail.FILE_SIZE = item["FILE_SIZE"].ToString();
                                        noticeModeDetail.CREATER = item["CREATER"].ToString();
                                        noticeModeDetail.CREATE_DATE = item["CREATE_DATE"] == null ? DateTime.Now : DateTime.Parse(item["CREATE_DATE"].ToString());
                                        listdetail.Add(noticeModeDetail);
                                    }
                                }
                            }
                            noticeMode.children = listdetail;
                            list.Add(noticeMode);
                        }
                        int totals = 0;
                        list = (List<NoticeMode>)KVTool.PaginationDataSource<NoticeMode>(list, page, limit, out totals);
                        r["total"] = dt.Rows.Count;
                        r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(list));// KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                        r["code"] = 2000;
                        r["message"] = "查询成功";
                    }
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
        public Dictionary<string, object> fetchCountList()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dtComponentMonth = new DataTable();
                DataTable dtServerMonth = new DataTable();
                DataTable dtComponentTop = new DataTable();
                DataTable dtServerCountTop = new DataTable();

                DataTable dtCom = db.getCountByMonth("1");//组件
                DataTable dtServer = db.getCountByMonth("2");//服务
                DataSet ds = db.getCountTop();
                DataTable dtcomT = new DataTable();
                DataTable dtserT = new DataTable();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dtcomT = ds.Tables["comp"];
                    dtserT = ds.Tables["server"];
                    if (dtcomT != null && dtcomT.Rows.Count > 0)
                    {
                        dtComponentTop = dtcomT.Clone();
                    }
                    if (dtserT != null && dtserT.Rows.Count > 0)
                    {
                        dtServerCountTop = dtserT.Clone();
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        if (dtcomT != null && dtcomT.Rows.Count > i)
                        {
                            dtComponentTop.ImportRow(dtcomT.Rows[i]);
                        }
                        if (dtserT != null && dtserT.Rows.Count > i)
                        {
                            dtServerCountTop.ImportRow(dtserT.Rows[i]);
                        }
                    }

                }
                if (dtCom != null && dtCom.Rows.Count > 0)
                {
                    dtComponentMonth = dtCom.Clone();
                }
                if (dtServer != null && dtServer.Rows.Count > 0)
                {
                    dtServerMonth = dtServer.Clone();
                }
                for (int i = 0; i < 6; i++)
                {
                    if (dtCom != null && dtCom.Rows.Count > i)
                    {
                        dtComponentMonth.ImportRow(dtCom.Rows[i]);
                    }
                    if (dtServer != null && dtServer.Rows.Count > i)
                    {
                        dtServerMonth.ImportRow(dtServer.Rows[i]);
                    }
                }
                r["dtComponentMonth"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dtComponentMonth));
                r["dtServerMonth"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dtServerMonth));
                r["dtComponentTop"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dtComponentTop));
                r["dtServerCountTop"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dtServerCountTop));
                r["code"] = 2000;
                r["message"] = "查询成功";
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

    }
}
