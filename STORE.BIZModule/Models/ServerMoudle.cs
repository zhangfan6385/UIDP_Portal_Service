using System;
using System.Collections.Generic;
using System.Text;

namespace STORE.BIZModule
{
    public class ServerMoudle
    {
        public string SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string REQUEST_METHOD { get; set; }
        public int SERVICE_TIMES { get; set; }
        public string SERVICE_CONTENT { get; set; }
        public string MANAGE_ORG_ID { get; set; }
        public string MANAGE_TEL { get; set; }
        public string MANAGE_ROLE_ID { get; set; }
        public string ORIGINAL_URL { get; set; }
        public string SERVICE_URL { get; set; }
        public string DATA_FORMAT { get; set; }
        public int IS_DELETE { get; set; }
        public string CREATER { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime SERVICE_PUBLISHDATE { get; set; }
        public int CHECK_STATE { get; set; }
        public List<ServerDetail> children { get; set; }
    }
    public class ServerDetail {
        public string SERVICE_DETAIL_ID { get; set; }
        public string SERVICE_ID { get; set; }
        public string FILE_NAME { get; set; }
        public int FILE_TYPE { get; set; }
        public string FILE_URL { get; set; }
        public Double FILE_SIZE { get; set; }
        public string CREATER { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}
