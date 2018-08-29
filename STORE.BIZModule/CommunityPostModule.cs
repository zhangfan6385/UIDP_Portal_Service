using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using STORE.ODS;
using STORE.UTILITY;
using Newtonsoft.Json;

namespace STORE.BIZModule
{
   public class CommunityPostModule
    {
        CommunityPostDB db = new CommunityPostDB();
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> fetchCommunityPostList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                DataTable dt = db.fetchCommunityPostList(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception ex)
            {
                r["items"] = null;
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createCommunityPostArticle(Dictionary<string, object> d)
        {
            d["POST_ID"] = Guid.NewGuid().ToString();
            return db.createCommunityPostArticle(d);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateCommunityPostData(Dictionary<string, object> d)
        {
            return db.updateCommunityPostData(d);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateCommunityPostArticle(string id)
        {
            return db.updateCommunityPostArticle(id);
        }
        /// <summary>
        /// 添加回复
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string addReply(Dictionary<string, object> d)
        {
            return db.addReply(d);
        }
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string addComment(Dictionary<string, object> d)
        {
            return db.addComment(d);
        }
        /// <summary>
        /// 帖子浏览量+1
        /// </summary>
        /// <param name="POST_ID"></param>
        /// <returns></returns>
        public string updateComunityPostLookTimes(string POST_ID) => db.updateComunityPostLookTimes(POST_ID);
    }
}
