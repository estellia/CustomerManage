using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xgx.WebAPI.Areas.Xgx.Controllers;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public class QRCodeResponseModel : BaseResponeModel
    {
        public string ImageUrl { get; set; }
        public string paraTmp { get; set; }
    }
}