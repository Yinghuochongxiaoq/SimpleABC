using System;
using System.IO;
using System.Linq;
using FreshCommonUtility.Configure;
using FreshCommonUtility.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SimpleABC.Api.Interface.IUploadFileBll;
using SimpleABC.Api.Model.BaseModel;
using SimpleABC.Api.Model.ImageInfo;

namespace SimpleABC.Api.Program.Controllers
{
    /// <summary>
    /// upload file controller
    /// </summary>
    public class UploadFileController : BaseController
    {
        /// <summary>
        /// Environment
        /// </summary>
        private IHostingEnvironment hostingEnv;

        /// <summary>
        /// upload file storage bll
        /// </summary>
        private IUploadFileStorageBll UploadFileServer { get; set; }

        /// <summary>
        /// Constructed function.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="uploadFileServer"></param>
        public UploadFileController(IHostingEnvironment env, IUploadFileStorageBll uploadFileServer)
        {
            hostingEnv = env;
            UploadFileServer = uploadFileServer;
        }

        /// <summary>
        /// post file
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddNewFile")]
        public ApiResultModel<DataBaseModel> AddNewFile()
        {
            long size = 0;
            var files = Request.Form.Files;
            var newName = HttpContext.GetStringFromParameters("fileName");
            var fileRootPath = AppConfigurationHelper.GetAppSettings("AppSettings:FileSavePath");
            //one file or multifile
            var file = files.FirstOrDefault();
            {
                var filesuffix = Path.GetExtension(file.FileName);
                var fileName = string.IsNullOrEmpty(newName) ? ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"') : newName + filesuffix;
                var dirfilePath = hostingEnv.WebRootPath + $@"\{fileRootPath}";
                var dirfileSavePullName = dirfilePath + $@"\{fileName}";
                size += file.Length;
                if (!Directory.Exists(dirfilePath))
                {
                    Directory.CreateDirectory(dirfilePath);
                }
                using (FileStream fs = System.IO.File.Create(dirfileSavePullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                var newFile = new ChooseImageInfo();
                newFile.ImageCreateTime = DateTime.Now;
                newFile.ImageDriverFlag = Guid.NewGuid().ToString();
                newFile.ImageName = fileName;
                newFile.ImagePath = $@"\{fileRootPath}\{fileName}";
                newFile.ImageSize = file.Length;
                UploadFileServer.StorageFile(newFile);
                var resutleInfo = new DataBaseModel();
                resutleInfo.StateCode = "200";
                resutleInfo.StateDesc = $"/{fileRootPath}/{fileName}";
                return ResponseDataApi(resutleInfo);
            }
        }
    }
}
