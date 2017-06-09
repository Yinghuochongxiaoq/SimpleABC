using Microsoft.AspNetCore.Mvc;

namespace SimpleABC.Api.Program.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    [Route("[controller]")]
    public class BaseController: Controller
    {
        /// <summary>
        /// Response data method.
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="data">response date</param>
        /// <returns></returns>
        public ApiResultModel<T> ResponseDataApi<T>(T data)
        {
            var resultData = new ApiResultModel<T> {Data = data};
            return resultData;
        }
    }

    /// <summary>
    /// Response data model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResultModel<T>
    {
        /// <summary>
        /// Construct
        /// </summary>
        public ApiResultModel()
        {
            RepCode = "200";
            RepMessage = "请求成功";
        }

        /// <summary>
        /// response code 
        /// </summary>
        public string RepCode { get; set; }

        /// <summary>
        /// response message
        /// </summary>
        public string RepMessage { get; set; }

        /// <summary>
        /// response data.
        /// </summary>
        public T Data { get; set; }
    }
}
