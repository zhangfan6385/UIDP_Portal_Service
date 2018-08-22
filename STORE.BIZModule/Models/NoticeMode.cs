using System;
using System.Collections.Generic;
using System.Text;

namespace STORE.BIZModule
{
   public class NoticeMode
    {
        public string NOTICE_ID { get; set; }
        public string NOTICE_CODE { get; set; }
        public string NOTICE_TITLE { get; set; }
        public string NOTICE_CONTENT { get; set; }
        public DateTime NOTICE_DATETIME { get; set; }
        public string NOTICE_ORGID { get; set; }
        public string NOTICE_ORGNAME { get; set; }
        public int? IS_DELETE { get; set; }
        public string CREATER { get; set; }
        public DateTime CREATE_DATE { get; set; }
        //明细
        public string NOTICE_DETAIL_ID { get; set; }
        public string FILE_URL { get; set; }
        public string FILE_NAME { get; set; }
        public string FILE_SIZE { get; set; }

        public List<NoticeMode> children { get; set; }
    }
}
