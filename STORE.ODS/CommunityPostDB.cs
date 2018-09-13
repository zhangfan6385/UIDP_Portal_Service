using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json.Linq;
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
            string sql = "select a.* ,(select count(*) from ts_community_comment where POST_ID=a.POST_ID) COMMONT_COUNT ";
            sql += " from ts_community_post a ";
            sql += " where  IS_DELETE=0 ";
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
                sql += " ORDER BY a.CREATE_DATE desc ";
            }
            return db.GetDataTable(sql);
        }


        public string createCommunityPostArticle(Dictionary<string, object> d)
        {
            string col = "";
            string val = "";
            string score = "0";
            string type = "";
            List<string> sqllist = new List<string>();
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
            if (d["POST_TYPE"].ToString() == "1")
            {
                score = db.GetString("select SCORE from ts_community_score_conf WHERE OPER_TYPE='share'");
                type = "share";
            }
            else if (d["POST_TYPE"].ToString() == "2")
            {
                score = db.GetString("select SCORE from ts_community_score_conf WHERE OPER_TYPE='feedback'");
                type = "feedback";
            }
            else
            {
                score = db.GetString("select SCORE from ts_community_score_conf WHERE OPER_TYPE='help'");
                type = "help";
            }
            string sql = "INSERT INTO ts_community_post(" + col + ",SEND_DATE,CREATE_DATE,IS_DELETE) VALUES(" + val + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0)";
            string sql1 = "insert into ts_community_score_detail(SCORE_DETAIL_ID,USER_ID,SCORE,POST_DATE,USER_OPER,SOURCE_ID) values('" + Guid.NewGuid().ToString() + "','" + d["USER_ID"].ToString() + "'," + score + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + type + "','" + d["POST_ID"].ToString() + "')";//发帖积分明细
            string sql2 = "update ts_uidp_userinfo set SCORE=SCORE+" + Convert.ToDouble(score) + "-" + Convert.ToDouble(d["SCORE_POINT"].ToString()) + " where USER_ID='" + d["USER_ID"].ToString() + "'";
            if (Convert.ToDouble(d["SCORE_POINT"].ToString()) > 0&& d["POST_TYPE"].ToString() == "3")
            {
                string sql3 = "insert into ts_community_score_detail(SCORE_DETAIL_ID,USER_ID,SCORE,POST_DATE,USER_OPER,SOURCE_ID) values('" + Guid.NewGuid().ToString() + "','" + d["USER_ID"].ToString() + "',-" + d["SCORE_POINT"].ToString() + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','sendhelp','" + d["POST_ID"].ToString() + "')";//发帖积分明细
                sqllist.Add(sql3);
            }
            if (score != "0")
            {
                sqllist.Add(sql);
                sqllist.Add(sql1);
                sqllist.Add(sql2);
                return db.Executs(sqllist);
            }
            else
            {
                sqllist.Add(sql);
                return db.Executs(sqllist);

            }

            //string sql = "INSERT INTO ts_community_post(" + col + ",CREATE_DATE,IS_DELETE) VALUES(" + val + ",'"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0)";



            //return db.ExecutByStringResult(sql);
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
        /// <summary>
        /// 添加回复
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string addReply(Dictionary<string, object> d)
        {
            string sql = " insert into ts_community_reply (REPLY_ID,COMMENT_ID,REPLY_TARGET_ID," +
                "REPLY_TYPE,CONTENT,FROM_UID,TO_UID,CREATE_DATE) values(";
            sql += "'";
            sql += Guid.NewGuid().ToString() + "";
            sql += "'";
            sql += d["COMMENT_ID"] == null ? "" : d["COMMENT_ID"].ToString() + "',";
            sql += "'";
            sql += d["REPLY_TARGET_ID"] == null ? "" : d["REPLY_TARGET_ID"].ToString() + "',";
            sql += d["REPLY_TYPE"].ToString() + ",";
            sql += "'";
            sql += d["CONTENT"] == null ? "" : d["CONTENT"].ToString() + "',";
            sql += "'";
            sql += d["FROM_UID"] == null ? "" : d["FROM_UID"].ToString() + "',";
            sql += "'";
            sql += d["TO_UID"] == null ? "" : d["TO_UID"].ToString() + "',";
            sql += "'";
            sql += DateTime.Now.ToString("yyyy-MM-dd") + "')";
            return db.ExecutByStringResult(sql);

        }
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string addComment(Dictionary<string, object> d)
        {
            string score = "0";
            List<string> sqllist = new List<string>();
            var commentId = Guid.NewGuid().ToString();
            string sql = " insert into ts_community_comment (COMMENT_ID,POST_ID,CONTENT," +
                "FROM_UID,CREATE_DATE,IS_RIGHT_ANSWER,BONUS_POINTS) values(";
            sql += "'";
            sql += commentId + "',";
            sql += "'";
            sql += d["POST_ID"] == null ? "" : d["POST_ID"].ToString() + "',";
            sql += "'";
            sql += d["CONTENT"] == null ? "" : d["CONTENT"].ToString() + "',";
            sql += "'";
            sql += d["FROM_UID"] == null ? "" : d["FROM_UID"].ToString() + "',";
            sql += "'";
            //sql += d["TO_UID"] == null ? "" : d["TO_UID"].ToString() + "',";
            //sql += "'";
            sql += DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "',";
            sql += d["IS_RIGHT_ANSWER"] == null ? "" : d["IS_RIGHT_ANSWER"].ToString() + ",";
            sql += "";
            sql += d["BONUS_POINTS"] == null ? "0" : d["BONUS_POINTS"].ToString();
            sql += ")";
            score = db.GetString("select SCORE from ts_community_score_conf WHERE OPER_TYPE='comment'");
            //string sql1 = "insert into ts_community_score_detail(SCORE_DETAIL_ID,USER_ID,SCORE,POST_DATE,USER_OPER) values('" + Guid.NewGuid().ToString() + "','" + d["USER_ID"].ToString() + "'," + score + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + score + ")";//发帖积分明细
            //string sql1 = "insert into ts_community_score_detail(SCORE_DETAIL_ID,USER_ID,SCORE,POST_DATE,USER_OPER,SOURCE_ID) values('" + Guid.NewGuid().ToString() + "','" + d["FROM_UID"].ToString() + "'," + score + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + score + ",'" + commentId + "')";//发帖积分明细
            string sql1 = "insert into ts_community_score_detail(SCORE_DETAIL_ID,USER_ID,SCORE,POST_DATE,USER_OPER,SOURCE_ID) values('" + Guid.NewGuid().ToString() + "','" + d["FROM_UID"].ToString() + "'," + score + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','comment','" + d["POST_ID"].ToString() + "')";//发帖积分明细
            string sql2 = "update ts_uidp_userinfo set SCORE=SCORE+" + Convert.ToDouble(score) + " where USER_ID='" + d["FROM_UID"].ToString() + "'";
            if (score != "0")
            {
                sqllist.Add(sql);
                sqllist.Add(sql1);
                sqllist.Add(sql2);
                return db.Executs(sqllist);
            }
            else
            {
                sqllist.Add(sql);
                return db.Executs(sqllist);
                //return db.ExecutByStringResult(sql);
            }
        }
        /// <summary>
        /// 帖子浏览量+1
        /// </summary>
        /// <param name="POST_ID"></param>
        /// <returns></returns>
        public string updateComunityPostLookTimes(string POST_ID)
        {
            string sql = "update ts_community_post set BROWSE_NUM=(case when BROWSE_NUM is null then 0 else BROWSE_NUM end )+1 ";
            sql += "where POST_ID='" + POST_ID + "'";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 获取帖子排行
        /// </summary>
        /// <returns></returns>
        public DataTable getTopPost()
        {
            string sql = " select count(*) TOTAL , b.USER_NAME from ts_community_post a ";
            sql += " join ts_uidp_userinfo b on a.USER_ID=b.USER_ID ";
            sql += "  where a.IS_DELETE=0 group by a.USER_ID ,b.USER_NAME ORDER BY count(*) desc";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 获取帖子详情评论会回复
        /// </summary>
        /// <param name="POST_ID"></param>
        /// <returns></returns>
        public DataSet getPostByID(string POST_ID, string USER_ID)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            string sql = "select * from ts_community_post where POST_ID='" + POST_ID + "'";//获取帖子信息
            string sql2 = "select a.*,b.USER_NAME from ts_community_comment a join ts_uidp_userinfo b on b.USER_ID=a.FROM_UID" +
 " where a.POST_ID ='" + POST_ID + "' order BY a.CREATE_DATE  ";//获取评论
            string sql3 = "select COLLECTION_ID from ts_community_collection where POST_ID='" + POST_ID + "'";
            sql3 += "AND COLLECTION_PERSON_ID='" + USER_ID + "'";
            //string sql3 = "select * from ts_community_reply where POST_ID='"+ POST_ID + "'";//获取回复信息
            list.Add("dtP", sql);
            list.Add("dtC", sql2);
            list.Add("dtU", sql3);
            //list.Add("dtR",sql3);
            return db.GetDataSet(list);
        }

        public string delCommentByID(Dictionary<string, object> d)
        {
            string sql = "delete from ts_community_comment where COMMENT_ID='";
            if (d.Count > 0)
            {
                if (d["COMMENT_ID"] != null)
                {
                    sql += d["COMMENT_ID"].ToString() + "'";
                }
            }
            return db.ExecutByStringResult(sql);
        }

        public string delPostByID(Dictionary<string, object> d)
        {
            string sql1 = "delete from ts_community_comment where POST_ID='" + d["POST_ID"].ToString() + "'";
            string sql2 = "delete from ts_community_post where POST_ID='" + d["POST_ID"].ToString() + "'";
            List<string> sql = new List<string>();
            sql.Add(sql1);
            sql.Add(sql2);
            return db.Executs(sql);
        }

        public DataTable getSharePower(string POST_ID, string USER_ID)
        {
            string sql = @"select a.USER_ID,a.SCORE,a.POST_DATE,b.POST_ID from ts_community_score_detail a
                         left join ts_community_post b
                         on a.SOURCE_ID=b.POST_ID
                         where b.POST_TYPE=1 and a.USER_OPER='costshare' and b.POST_ID='" + POST_ID + "' and a.USER_ID='" + USER_ID + "'";
            return db.GetDataTable(sql);
        }

        public string payShare(string postId, string postUserId, string userId, string score)
        {
            string sql1 = "insert into ts_community_score_detail(SCORE_DETAIL_ID,USER_ID,SCORE,USER_OPER,POST_DATE,SOURCE_ID) values('" + Guid.NewGuid().ToString() + "','" + userId + "',-" + score + ",'costshare','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + postId + "')";
            string sql2 = "insert into ts_community_score_detail(SCORE_DETAIL_ID,USER_ID,SCORE,USER_OPER,POST_DATE,SOURCE_ID) values('" + Guid.NewGuid().ToString() + "','" + postUserId + "'," + score + ",'supply','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + postId + "')";
            string sql3 = "update ts_uidp_userinfo set SCORE=SCORE+" + Convert.ToDouble(score) + " where USER_ID='" + postUserId + "'";
            string sql4 = "update ts_uidp_userinfo set SCORE=SCORE-" + Convert.ToDouble(score) + " where USER_ID='" + userId + "'";
            //return db.ExecutByStringResult(sql);
            List<string> sql = new List<string>();
            sql.Add(sql1);
            sql.Add(sql2);
            sql.Add(sql3);
            sql.Add(sql4);
            return db.Executs(sql);
        }
        public string getString(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString().Replace("\\", "").Trim();
        }
        public string endPost(string postId,string scorePoint,string userId, List<Dictionary<string, object>> f)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder upsb = new StringBuilder();
            string sql3 = "";
            if (f.Count > 0)
            {
                string fengefu = "";
                sb.Append(" insert into ts_community_score_detail(SCORE_DETAIL_ID,USER_ID,SCORE,USER_OPER,POST_DATE,SOURCE_ID) values ");
                foreach (var item in f)
                {
                    sb.Append(fengefu + "('" + Guid.NewGuid().ToString() + "',");
                    sb.Append("'" + getString(item["FROM_UID"]) + "',");
                    sb.Append(getString(item["BONUS_POINTS"]) + ",");
                    sb.Append("'solveproblem',");
                    sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                    sb.Append("'" + getString(item["COMMENT_ID"]) + "')");
                    fengefu = ",";
                    upsb.Append(" update ts_community_comment set BONUS_POINTS=" + getString(item["BONUS_POINTS"]) + " where COMMENT_ID='" + getString(item["COMMENT_ID"]) + "';");
                }
            }
            else 
            {
                 sql3 = "update ts_uidp_userinfo set SCORE=SCORE+" + Convert.ToDouble(scorePoint) + " where USER_ID='" + userId + "'";
            }
            string sql0 = sb.ToString();
            string sql1 = "update ts_community_post set POST_STATUS=1 where POST_ID='" + postId + "'";
            string sql2 = upsb.ToString();
            List<string> sql = new List<string>();
            if (!string.IsNullOrEmpty(sql0))
            {
                sql.Add(sql0);
            }
            sql.Add(sql1);
            if (!string.IsNullOrEmpty(sql2))
            {
                sql.Add(sql2);
            }
            if (!string.IsNullOrEmpty(sql3))
            {
                sql.Add(sql3);
            }
            return db.Executs(sql);
        }
        public string getScore(string userId)
        {
            string score = "0";
            score = db.GetString("select SCORE from ts_uidp_userinfo WHERE USER_ID='" + userId + "'");
            return score;
        }

    }
}