using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using UEditorNetCore;
using UEditor.Core;

namespace STORE.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/UEditor")]
    public class UEditorController : Controller
    {
        private readonly UEditorService _ueditorService;
        public UEditorController(UEditorService ueditorService)
        {
            this._ueditorService = ueditorService;
        }

        [HttpGet, HttpPost]
        public ContentResult Upload()
        {
            var response = _ueditorService.UploadAndGetResponse(HttpContext);
            return Content(response.Result, response.ContentType);
        }
        //private UEditorService ue;
        //public UEditorController(UEditorService ue)
        //{
        //    this.ue = ue;
        //}
        //[HttpGet, HttpPost]
        //public void Do()
        //{
        //    ue.DoAction(HttpContext);
        //}
    }
}