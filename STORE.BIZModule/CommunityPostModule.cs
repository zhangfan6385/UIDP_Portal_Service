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
        /// <summary>
        /// 获取帖子top
        /// </summary>
        /// <returns></returns>

        public Dictionary<string, object> getTopPost() {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getTopPost();
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 10)
                    {
                        r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, 1, 10));
                        r["code"] = 2000;
                        r["message"] = "查询成功";
                    }
                    else
                    {
                        r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dt));
                        r["code"] = 2000;
                        r["message"] = "查询成功";
                    }
                    return r;
                }
                r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new DataTable()));
                r["code"] = 2000;
                r["message"] = "查询成功";
            }

            catch (Exception e)
            {
                r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new DataTable()));
                r["code"] = -1;
                r["message"] =  e.Message;
            }
            return r;
        }


        /// <summary>
        /// 查询帖子详情
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchPostDetail(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                //int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                //int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());
                if (d["USER_ID"] == null)
                {
                    d["USER_ID"] = " ";
                };
                DataSet ds = db.getPostByID(d["POST_ID"].ToString(),d["USER_ID"].ToString());
                PostModel postModel = new PostModel();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataTable dtDetail = new DataTable();
                    DataTable dtCollection = new DataTable();
                    if (ds.Tables.Count > 1)
                    {
                        dtDetail = ds.Tables[1];
                        dtCollection = ds.Tables[2];
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            postModel.POST_ID = dr["POST_ID"].ToString();
                            postModel.USER_ID = dr["USER_ID"] == null ? "" : dr["USER_ID"].ToString();
                            postModel.USER_NAME = dr["USER_NAME"] == null ? "" : dr["USER_NAME"].ToString();
                            postModel.TITLE_NAME = dr["TITLE_NAME"] == null ? "" : dr["TITLE_NAME"].ToString();
                            postModel.POST_TYPE = Convert.ToInt32(dr["POST_TYPE"].ToString());
                            postModel.POST_CONTENT = dr["POST_CONTENT"].ToString();
                            postModel.SCORE_POINT =Convert.ToDouble(dr["SCORE_POINT"].ToString());
                            postModel.BROWSE_NUM = Convert.ToInt32(dr["BROWSE_NUM"].ToString()); ;
                            //postModel.COLLECTION_ID = dr["COLLECTION_ID"] == null ? " ": dr["COLLECTION_ID"].ToString();
                            postModel.SEND_DATE = dr["SEND_DATE"] == null ? DateTime.Now : DateTime.Parse(dr["SEND_DATE"].ToString());
                            List<PostComment> listdetail = new List<PostComment>();
                            if (dtDetail != null && dtDetail.Rows.Count > 0)
                            {
                                //DataRow[] arry = dtDetail.Select("NOTICE_ID='" + dr["NOTICE_ID"].ToString() + "'");
                                listdetail.Clear();

                                    foreach (DataRow item in dtDetail.Rows)
                                    {
                                        PostComment postComment = new PostComment();
                                    postComment.COMMENT_ID = item["COMMENT_ID"].ToString();
                                    postComment.POST_ID = item["POST_ID"].ToString();
                                    postComment.CONTENT = item["CONTENT"].ToString();
                                    postComment.FROM_UID = item["FROM_UID"].ToString();
                                    postComment.USER_NAME = item["USER_NAME"].ToString();
                                    //postComment.IS_RIGHT_ANSWER= Convert.ToInt32(dr["IS_RIGHT_ANSWER"].ToString());
                                    //postComment.BONUS_POINTS= Convert.ToDouble(dr["BONUS_POINTS"].ToString());
                                    postComment.CREATE_DATE = item["CREATE_DATE"] == null ? DateTime.Now : DateTime.Parse(item["CREATE_DATE"].ToString());
                                        listdetail.Add(postComment);
                                    }
                            }
                            if (dtCollection.Rows.Count==0)
                            {
                                postModel.COLLECTION_STATE ="0";
                            }
                            else
                            {
                                postModel.COLLECTION_STATE = "1";
                            }
                            postModel.children = listdetail;
                        }

                        r["items"] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(postModel));// KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                        r["code"] = 2000;
                        r["message"] = "查询成功";
                    }
                }
                else
                {
                    r["items"] = null;
                    r["code"] = 2000;
                    r["message"] = "查询成功";
                }
            }
            catch (Exception e)
            {
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public string deleteCommit(Dictionary<string, object> d)
        {
            return db.delCommentByID(d);
        }
        
        public string deletePost(Dictionary<string,object> d)
        {
            return db.delPostByID(d);
        }
    }
}
