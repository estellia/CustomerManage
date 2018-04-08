using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xgx.WebAPI.Areas.Xgx.Controllers;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public class ScanQRCodeResponseModel: BaseResponeModel
    {
        public string Status { get; set; }
        public string VipId { get; set; }
    }
}