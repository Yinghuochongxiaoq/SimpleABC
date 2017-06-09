using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleABC.Api.Model.ErrorInfoLog;
using Microsoft.AspNetCore.Http;

namespace SimpleABC.Api.Interface.ILogHistoryBll
{
    /// <summary>
    /// Error log bll interface.
    /// </summary>
    public interface IErrorLogBll
    {
        /// <summary>
        /// write error info.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="context"></param>
        Task<long> WriteErrorInfo(Exception exception, HttpContext context = null);

        /// <summary>
        /// write warning.
        /// </summary>
        /// <param name="warningString">warning string</param>
        /// <returns></returns>
        Task<long> WriteWarningInfo(string warningString);

        /// <summary>
        /// write info.
        /// </summary>
        /// <param name="infoString">warning string</param>
        /// <returns></returns>
        Task<long> WriteInfo(string infoString);

        /// <summary>
        /// search log info.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="startTime">start time</param>
        /// <param name="endTime">end time</param>
        /// <param name="errorType">error type</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">pagesize</param>
        /// <returns></returns>
        List<ErrorInfoLogModel> SearchLogInfo(long id = 0, DateTime startTime = default(DateTime), DateTime endTime = default(DateTime), int errorType = -1, int pageIndex = 1, int pageSize = 10);
    }
}
