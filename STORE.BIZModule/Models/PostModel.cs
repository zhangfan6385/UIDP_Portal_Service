using System;
using System.Collections.Generic;
using System.Text;

namespace STORE.BIZModule
{
    public class PostComment
    {
        public string COMMENT_ID { get; set; }
        public string POST_ID { get; set; }
        public string CONTENT { get; set; }
        public string FROM_UID { get; set; }
        public string USER_NAME { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public int IS_RIGHT_ANSWER { get; set; }
        public double BONUS_POINTS { get; set; }

    }

    public class PostModel
    {

        public string POST_ID { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string TITLE_NAME { get; set; }
        public int POST_TYPE { get; set; }
        public string POST_CONTENT { get; set; }
        public DateTime SEND_DATE { get; set; }
        public int BROWSE_NUM { get; set; }
        public double SCORE_POINT { get; set; }
        public string COLLECTION_STATE { get; set; }
        public List<PostComment> children { get; set; }
    }
}
