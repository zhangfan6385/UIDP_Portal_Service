using System;
using System.Collections.Generic;
using System.Text;

namespace STORE.BIZModule
{
   public class ComponentModule
    {
        public string COMPONENT_ID { get; set; }
        public string COMPONENT_CODE { get; set; }
        public string COMPONENT_NAME { get; set; }
        public string COMPONENT_CONTENT { get; set; }
        public int DOWNLOAD_TIMES { get; set; }
        public string MANAGE_ORG_ID { get; set; }
        public string MANAGE_ORG_NAME { get; set; }
        public string MANAGE_TEL { get; set; }
        public string MANAGE_ROLE_ID { get; set; }
        public string COMPONENT_SIZE { get; set; }
        public string SOFTWARE_LANGUAGE { get; set; }
        public string SUIT_PLAT { get; set; }
        public string APPLICATION_BROWSER { get; set; }
        public int IS_DELETE { get; set; }
        public string CREATER { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public int CHECK_STATE { get; set; }
        public string URL { get; set; }
        public DateTime COMPONENT_PUBLISHDATE { get; set; }
        public string COMPONENT_VERSION { get; set; }
        public List<ComponentDetail> children { get; set; }
    }
    public class ComponentDetail {
        public string COMPONENT_DETAIL_ID { get; set; }
        public string COMPONENT_ID { get; set; }
        public string FILE_NAME { get; set; }
        public int FILE_TYPE { get; set; }
        public string FILE_URL { get; set; }
        public string FILE_SIZE { get; set; }
        public string CREATER { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}
