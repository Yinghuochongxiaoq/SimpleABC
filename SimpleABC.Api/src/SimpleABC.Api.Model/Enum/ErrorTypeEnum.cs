using System.ComponentModel;

namespace SimpleABC.Api.Model.Enum
{
    /// <summary>
    /// error type enum
    /// </summary>
    public enum ErrorTypeEnum
    {
        /// <summary>
        /// 严重错误
        /// </summary>
        [Description("严重错误")]
        Error=0,

        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        Warning=1,

        /// <summary>
        /// 消息通知
        /// </summary>
        [Description("消息")]
        Info=2
    }
}
