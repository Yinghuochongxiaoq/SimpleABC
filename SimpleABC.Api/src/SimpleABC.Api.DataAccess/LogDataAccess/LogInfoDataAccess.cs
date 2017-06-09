using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using SimpleABC.Api.DataAccess.Common;
using FreshCommonUtility.SqlHelper;
using SimpleABC.Api.Model.ErrorInfoLog;

namespace SimpleABC.Api.DataAccess.LogDataAccess
{
    /// <summary>
    /// Log info data access
    /// </summary>
    public class LogInfoDataAccess
    {
        /// <summary>
        /// Add log
        /// </summary>
        /// <param name="newLogInfo"></param>
        /// <returns></returns>
        public int AddLogInfo(ErrorInfoLogModel newLogInfo)
        {
            if (newLogInfo == null) return 0;
            var searchSql = @"INSERT INTO piferrorlog(
    ContentType,
    ErrorMessage,
    InnerErrorMessage,
    ErrorTypeFullName,
    StackTrace,
    ErrorTime,
    ErrorType
)
VALUES
    (
        @ContentType,
        @ErrorMessage,
        @InnerErrorMessage,
        @ErrorTypeFullName,
        @StackTrace,
        @ErrorTime,
        @ErrorType
    )";
            var param = new DynamicParameters(newLogInfo);
            var userId = SqlHelper.ExcuteNonQuery(searchSql, param);
            return userId;
        }

        /// <summary>
        /// Async add log.
        /// </summary>
        /// <param name="newLogInfo"></param>
        /// <returns></returns>
        public Task<int> AddLogInfoAsync(ErrorInfoLogModel newLogInfo)
        {
            var searchSql = @"INSERT INTO piferrorlog (
	ContentType,
	ErrorMessage,
	InnerErrorMessage,
	ErrorTypeFullName,
	StackTrace,
	ErrorTime,
	ErrorType
)
VALUES
	(
		@ContentType,
		@ErrorMessage,
		@InnerErrorMessage,
		@ErrorTypeFullName,
		@StackTrace,
		@ErrorTime,
		@ErrorType
	)";
            var param = new DynamicParameters(newLogInfo);
            var userId = SqlHelper.ExcuteNonQueryAsync(searchSql, param);
            return userId;
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
            var strWhere = new StringBuilder();
            if (id > 1) strWhere.Append(" and Id=@id ");
            if (startTime != default(DateTime)) strWhere.Append(" and ErrorTime>@startTime ");
            if (endTime != default(DateTime)) strWhere.Append(" and ErrorTime<@endTime ");
            if (errorType != -1) strWhere.Append(" and ErrorType=@errorType ");
            var orderBy = " ErrorTime desc ";
            var fieldList = " * ";
            long countNumber;
            var param = new DynamicParameters();
            param.Add("id", id);
            param.Add("startTime", startTime, DbType.DateTime);
            param.Add("endTime", endTime, DbType.DateTime);
            param.Add("errorType", errorType);
            var errorLogList = SqlHelper.SearchPageList<ErrorInfoLogModel>(DataTableGlobal.PiFErrorLog, strWhere.ToString(), orderBy,
                fieldList, pageIndex, pageSize, param, out countNumber);
            return errorLogList.ToList();
        }
    }
}
