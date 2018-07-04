using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.Design.OperationExecutor;

namespace FileUploadDownload.WebApi.Controllers
{
    

    [Produces("application/json")]
    [Route("api/File")]
    public class FileController : Controller
    {
        

        [HttpPost("new")]
        public async Task<JsonResult> UploadFile(IFormFile file)
        {

            if (file!=null)   
            {
                 var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/file",file.FileName);
                using (var stream =new FileStream(path,FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                }
                return Json(1);

            }else{
                return Json(-1);
            }

            
        }

        [HttpGet("Get/{name}")]
        public async Task<FileResult> DownloadFile(string name)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/file", name);
            var memory = new MemoryStream();
            using (var stream=new FileStream(path,FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory,"application/octet-stream");
        }

        

        //DownloadFile

        [HttpGet]
        public JsonResult Get()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/file");
            var fileList = Directory.GetFiles(path);
            List<string> fileNames = new List<string>();

            foreach (var item in fileList)
            {
                fileNames.Add(Path.GetFileName(item));
            }
            
            return Json(fileNames);

        }

    }
    
}