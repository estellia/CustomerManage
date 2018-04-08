using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public class ScanQRCodeRequestModel
    {
        public string ParaTmp { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string UserId { get; set; }
    }
}