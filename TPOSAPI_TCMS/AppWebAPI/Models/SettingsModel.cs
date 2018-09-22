using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace AppWebAPI.Models
{
    /// <summary>
    /// 設定畫面
    /// </summary>
    public class SettingModel
    {
        /// <summary>
        /// 一般字串
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Normal")]
        public string normal { get; set; }

        /// <summary>
        /// 敏感字串
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Sensitive")]
        public string sensitive { get; set; }

        /// <summary>
        /// 網頁訊息
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string message { get; set; }

        /// <summary>
        /// APP語系
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "lang")]
        public string lang { get; set; }
    }
}