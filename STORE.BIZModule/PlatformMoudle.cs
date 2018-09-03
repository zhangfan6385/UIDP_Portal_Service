using System;
using System.Collections.Generic;
using System.Text;

namespace STORE.BIZModule
{
   public class PlatformMoudle
    {
        public string PLAT_ID { get; set; }
        public string PLAT_CODE { get; set; }
        public string PLAT_NAME { get; set; }
        public string PLAT_VERSION { get; set; }
        public DateTime PLAT_PUBLISHDATE { get; set; }
        public string PLAT_SIZE { get; set; }
        public string SOFTWARE_LANGUAGE { get; set; }
        public string SUIT_PLAT { get; set; }
        public string APPLICATION_BROWSER { get; set; }
        public string  PLAT_RUNREQUIRE { get; set; }
        public int PLAT_TYPE { get; set; }
        public string  MANAGE_ROLE_ID { get; set; }
        public string MANAGE_ORG_ID { get; set; }
        public string MANAGE_ORG_NAME { get; set; }
        public string MANAGE_TEL { get; set; }
        public int IS_DELETE { get; set; }
        public string CREATER { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public int CHECK_STATE { get; set; }
        public string URL { get; set; }
        public List<PlatformDetail> children { get; set; }
    }
    public class PlatformDetail
    {
        public string PLAT_DETAIL_ID { get; set; }
        public string PLAT_ID { get; set; }
        public string FILE_NAME { get; set; }
        public int FILE_TYPE { get; set; }
        public string FILE_URL { get; set; }
        public string FILE_SIZE { get; set; }
        public string CREATER { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}
