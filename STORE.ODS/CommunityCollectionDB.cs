using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using STORE.UTILITY;
namespace STORE.ODS
{
    public class CommunityCollectionDB
    {
        DBTool db = new DBTool("");
        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchMyCommunityCollectionList(Dictionary<string, object> d)
        {
            string sql = "select a.COLLECTION_ID,b.* from ts_community_collection a INNER JOIN ts_community_post b on a.POST_ID=b.POST_ID ";
            sql += " where b.IS_DELETE=0 ";
            if (d.Count > 0)
            {
                if (d["userId"] != null && d["userId"].ToString() != "")
                {
                    sql += " and b.USER_ID = '" + d["userId"].ToString() + "'";
                }
            }
            return db.GetDataTable(sql);
        }


        public string createCommunityCollectionArticle(Dictionary<string, object> d)
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

            string sql = "INSERT INTO ts_community_collection(" + col + ",COLLECTION_DATE) VALUES(" + val + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

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


        public string deleteCommunityCollectionArticle(string id)
        {
            string sql = "delete from ts_community_collection where COLLECTION_ID ='" + id + "'";

            return db.ExecutByStringResult(sql);
        }



    }
}