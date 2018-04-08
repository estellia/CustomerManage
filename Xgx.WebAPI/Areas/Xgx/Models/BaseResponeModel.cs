using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xgx.WebAPI.Areas.Xgx
{
    public class BaseResponeModel
    {
        public bool IsSucess;
        public string ErrorMessage { get; set; }
    }
}