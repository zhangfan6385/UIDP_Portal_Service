using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using STORE.UTILITY;
namespace STORE.ODS
{
    public class CommunityPostDB
    {
        DBTool db = new DBTool("");
        /// <summary>
        /// 查询社区信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchCommunityPostList(Dictionary<string, object> d)
        {
            string sql = "select * from ts_community_post a ";
            sql += " where 1=1 and IS_DELETE=0 ";
            if (d.Count > 0)
            {
                if (d["POST_TYPE"] != null && d["POST_TYPE"].ToString() != "")
                {
                    sql += " and a.POST_TYPE =" + d["POST_TYPE"].ToString();
                }
                
                if (d["USER_ID"] != null && d["USER_ID"].ToString() != "")
                {
                    sql += " and a.USER_ID like '" + d["USER_ID"].ToString() + "'";
                }
                if (d["TITLE_NAME"] != null && d["TITLE_NAME"].ToString() != "")
                {
                    sql += " and a.TITLE_NAME like '%" + d["TITLE_NAME"].ToString() + "%'";
                }
                if (d["BEGIN_SEND_DATE"] != null && d["BEGIN_SEND_DATE"].ToString() != "" && (d["END_SEND_DATE"] == null || d["END_SEND_DATE"].ToString() == ""))
                {
                    DateTime date = Convert.ToDateTime(d["BEGIN_SEND_DATE"].ToString());
                    sql += " and SEND_DATE > '" + date.Year + "-" + date.Month + "-" + date.Day + " 00:00:00'";
                    //sql += " and NOTICE_DATETIME between '" + date.Year + "-" + date.Month + "-" + date.Day + " 00:00:00' and '" + date.Year + "-" + date.Month + "-" + date.Day + " 23:59:59'";
                }
                else if (d["END_SEND_DATE"] != null && d["END_SEND_DATE"].ToString() != "" && (d["BEGIN_SEND_DATE"] == null || d["BEGIN_SEND_DATE"].ToString() == ""))
                {
                    DateTime date = Convert.ToDateTime(d["END_SEND_DATE"].ToString());
                    sql += " and SEND_DATE < '" + date.Year + "-" + date.Month + "-" + date.Day + " 23:59:59'";

                }
                else if (d["BEGIN_SEND_DATE"] != null && d["BEGIN_SEND_DATE"].ToString() != "" && d["END_SEND_DATE"] != null && d["END_SEND_DATE"].ToString() != "")
                {
                    DateTime bdate = Convert.ToDateTime(d["BEGIN_SEND_DATE"].ToString());
                    DateTime edate = Convert.ToDateTime(d["END_SEND_DATE"].ToString());
                    sql += " and SEND_DATE between '" + bdate.Year + "-" + bdate.Month + "-" + bdate.Day + " 00:00:00' and '" + edate.Year + "-" + edate.Month + "-" + edate.Day + " 23:59:59'";
                }
            }
            return db.GetDataTable(sql);
        }


        public string createCommunityPostArticle(Dictionary<string, object> d)
        {
            string col = "";
            string val = "";
            foreach (var v in d)
            {
                if (v.Value != null)
                {
                    col += "," + v.Key;
                    val += ",'" + v.Value + "'";
                }
                else
                {
                    col += "," + v.Key;
                    val += ",''";
                }
            }
            if (col != "")
            {
                col = col.Substring(1);
            }
            if (val != "")
            {
                val = val.Substring(1);
            }

            string sql = "INSERT INTO ts_community_post(" + col + ",CREATE_DATE,IS_DELETE) VALUES(" + val + ",'"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0)";

            return db.ExecutByStringResult(sql);
        }
        public string GetIsNullStr(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }

        public string updateCommunityPostData(Dictionary<string, object> d)
        {
            string col = "";

            foreach (var v in d)
            {
                if (v.Value == null)
                {
                    col += "," + v.Key + "=''";
                }
                else
                {
                    col += "," + v.Key + "='" + v.Value.ToString() + "'";
                }


            }
            if (col != "")
            {
                col = col.Substring(1);
            }

            string sql = "update ts_community_post set " + col + " where POST_ID='" + d["POST_ID"].ToString() + "'";

            return db.ExecutByStringResult(sql);
        }

        public string updateCommunityPostArticle(string id)
        {
            string sql = "update ts_community_post set IS_DELETE=1 where POST_ID ='" + id + "'";

            return db.ExecutByStringResult(sql);
        }

      

    }
}