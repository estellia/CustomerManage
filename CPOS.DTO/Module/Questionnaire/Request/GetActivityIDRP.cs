﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JIT.CPOS.DTO.Base;

namespace JIT.CPOS.DTO.Module.Questionnaire.Request
{
    public class GetActivityIDRP : IAPIRequestParameter
    {

        #region 属性
        /// <summary>
        /// 问卷标识
        /// </summary>
        public string ActivityID { get; set; }



        #endregion





        public void Validate()
        {

        } 
    }
}
