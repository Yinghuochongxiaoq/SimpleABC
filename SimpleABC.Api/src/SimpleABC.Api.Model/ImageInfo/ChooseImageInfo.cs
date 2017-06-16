using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleABC.Api.Model.ImageInfo
{
    /// <summary>
    /// Choose image model
    /// </summary>
    public class ChooseImageInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Image name
        /// </summary>
        [Required]
        public string ImageName { get; set; }

        /// <summary>
        /// Image size
        /// </summary>
        [Required]
        public long ImageSize { get; set; }

        /// <summary>
        /// Image save path
        /// </summary>
        [Required]
        public string ImagePath { get; set; }

        /// <summary>
        /// Image create time
        /// </summary>
        [Required]
        public DateTime ImageCreateTime { get; set; }

        /// <summary>
        /// image driver flag
        /// </summary>
        public string ImageDriverFlag { get; set; }

        /// <summary>
        /// user id
        /// </summary>
        public long UserId { get; set; }

    }
}
