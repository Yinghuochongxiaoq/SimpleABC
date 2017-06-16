using SimpleABC.Api.Model.ImageInfo;

namespace SimpleABC.Api.Interface.IUploadFileBll
{
    /// <summary>
    /// interface UploadFile storage bll
    /// </summary>
    public interface IUploadFileStorageBll
    {
        /// <summary>
        /// Storage file
        /// </summary>
        /// <param name="newFileInfo"></param>
        /// <returns></returns>
        int? StorageFile(ChooseImageInfo newFileInfo);
    }
}
