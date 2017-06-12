using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using SimpleABC.Api.Interface.IUploadFileBll;
using SimpleABC.Api.Model.ImageInfo;

namespace SimpleABC.Api.Business.UploadFileBll
{
    /// <summary>
    /// upload file storage deal class.
    /// </summary>
    public class UploadFileStorageBll:IUploadFileStorageBll
    {
        /// <summary>
        /// Storage file
        /// </summary>
        /// <param name="newFileInfo"></param>
        /// <returns></returns>
        public int? StorageFile(ChooseImageInfo newFileInfo)
        {
            if (string.IsNullOrEmpty(newFileInfo.ImagePath) || string.IsNullOrEmpty(newFileInfo.ImageName)) return 0;
            int? resulte = 0;
            using (var connection = SqlConnectionHelper.GetOpenConnection())
            {
                resulte = connection.Insert(newFileInfo);
            }
            return resulte;
        }
    }
}
