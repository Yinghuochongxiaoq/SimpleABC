using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleABC.Api.DataAccess.LogDataAccess;
using SimpleABC.Api.Interface.ILogHistoryBll;
using SimpleABC.Api.Model.Enum;
using SimpleABC.Api.Model.ErrorInfoLog;
using FreshCommonUtility.Cache;
using FreshCommonUtility.Configure;
using Microsoft.AspNetCore.Http;

namespace SimpleABC.Api.Business.LogHistoryBll
{
    /// <summary>
    /// Deal error info
    /// </summary>
    public class ErrorLogBll : IErrorLogBll
    {
        #region [1、Private method]

        /// <summary>
        /// write log info to redis
        /// </summary>
        /// <param name="errorModel">log info.</param>
        /// <returns></returns>
        private Task<long> WriteAllLogToRedis(ErrorInfoLogModel errorModel)
        {
            var cachKey = AppConfigurationHelper.GetAppSettings<CacheLogModel>("AppSettings:ErrorLogCache");
            cachKey = string.IsNullOrEmpty(cachKey?.Cachekey) ? new CacheLogModel { Cachekey = "ErrorLogCache", DatabaseNumber = 1 } : cachKey;
            var addTask = RedisCacheHelper.AddListAsync(cachKey.Cachekey, errorModel, cachKey.DatabaseNumber);
            return addTask;
        }

        /// <summary>
        /// Stock Method
        /// </summary>
        private async void StockErrorLogInfo()
        {
            var logInfoDataAccess = new LogInfoDataAccess();
            var cacheKey = AppConfigurationHelper.GetAppSettings<CacheLogModel>("AppSettings:ErrorLogCache");
            cacheKey = string.IsNullOrEmpty(cacheKey?.Cachekey) ? new CacheLogModel { Cachekey = "ErrorLogCache", DatabaseNumber = 1 } : cacheKey;
            long cacheListLength = RedisCacheHelper.GetListLength(cacheKey.Cachekey, cacheKey.DatabaseNumber);
            while (cacheListLength > 0)
            {
                var cacheModel = RedisCacheHelper.GetLastOneList<ErrorInfoLogModel>(cacheKey.Cachekey,
                    cacheKey.DatabaseNumber);
                await logInfoDataAccess.AddLogInfoAsync(cacheModel);
                cacheListLength = RedisCacheHelper.GetListLength(cacheKey.Cachekey, cacheKey.DatabaseNumber);
            }
        }
        #endregion

        #region [2、Interface]

        /// <summary>
        /// write error info.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="context"></param>
        public Task<long> WriteErrorInfo(Exception exception, HttpContext context = null)
        {
            var errorModel = new ErrorInfoLogModel
            {
                ContentType = context == null ? "" : (string)context.Request?.Headers["Accept"],
                ErrorMessage = exception?.Message,
                ErrorTime = DateTime.Now,
                ErrorTypeFullName = exception?.GetType().FullName,
                InnerErrorMessage = exception?.InnerException?.Message,
                StackTrace = exception?.StackTrace,
                ErrorType = ErrorTypeEnum.Error.GetHashCode()
            };

            var resulte = WriteAllLogToRedis(errorModel);
            if (resulte.Result > 0)
            {
                StockErrorLogInfo();
            }
            return resulte;
        }

        /// <summary>
        /// write warning.
        /// </summary>
        /// <param name="warningString">warning string</param>
        /// <returns></returns>
        public Task<long> WriteWarningInfo(string warningString)
        {
            var errorModel = new ErrorInfoLogModel
            {
                ContentType = "程序警告",
                ErrorMessage = warningString,
                ErrorTime = DateTime.Now,
                ErrorTypeFullName = "警告",
                InnerErrorMessage = warningString,
                StackTrace = "",
                ErrorType = ErrorTypeEnum.Warning.GetHashCode()
            };
            var resulte = WriteAllLogToRedis(errorModel);
            if (resulte.Result > 0)
            {
                StockErrorLogInfo();
            }
            return resulte;
        }

        /// <summary>
        /// write info.
        /// </summary>
        /// <param name="infoString">warning string</param>
        /// <returns></returns>
        public Task<long> WriteInfo(string infoString)
        {
            var errorModel = new ErrorInfoLogModel
            {
                ContentType = "消息",
                ErrorMessage = infoString,
                ErrorTime = DateTime.Now,
                ErrorTypeFullName = "消息",
                InnerErrorMessage = infoString,
                StackTrace = "",
                ErrorType = ErrorTypeEnum.Info.GetHashCode()
            };
            var resulte = WriteAllLogToRedis(errorModel);
            if (resulte.Result > 0)
            {
                StockErrorLogInfo();
            }
            return resulte;
        }

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
        public List<ErrorInfoLogModel> SearchLogInfo(long id = 0, DateTime startTime = default(DateTime), DateTime endTime = default(DateTime), int errorType = -1, int pageIndex = 1, int pageSize = 10)
        {
            var logDataAccess = new LogInfoDataAccess();
            var logInfoListTask = logDataAccess.SearchLogInfo(id, startTime, endTime, errorType, pageIndex, pageSize);
            return logInfoListTask;
        }
        #endregion
    }
}
