﻿/*
 * Author		:Alex.Tian
 * EMail		:changjian.tian@jitmarketing.cn
 * Company		:JIT
 * Create On	:2014/5/21 15:40
 * Description	:获取套餐列表
 * 1st Modified On	:
 * 1st Modified By	:
 * 1st Modified Desc:
 * 1st Modifier EMail	:
 * 2st Modified On	:
 * 2st Modified By	:
 * 2st Modifier EMail	:
 * 2st Modified Desc:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.DTO.Module.WeiXin.SysPage.Request;
using JIT.CPOS.DTO.Module.WeiXin.SysPage.Response;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.Utility.ExtensionMethod;
using JIT.CPOS.DTO.Base;
using System.Data;

namespace JIT.CPOS.Web.ApplicationInterface.Module.WX.SysPage
{
    public class SetModulePageMappingAH : BaseActionHandler<SetModulePageMappingRP, EmptyResponseData>
    {

        protected override EmptyResponseData ProcessRequest(APIRequest<SetModulePageMappingRP> pRequest)
        {
            EmptyResponseData eData = new EmptyResponseData();
            var para = pRequest.Parameters;
            var userInfo = this.CurrentUserInfo;
            userInfo.CurrentLoggingManager.Connection_String = System.Configuration.ConfigurationManager.AppSettings["Conn_ap"];
            SysModulePageMappingBLL bll = new SysModulePageMappingBLL(userInfo);
            SysModulePageMappingEntity entity = new SysModulePageMappingEntity();
//解除页面和行业版本的绑定关系
            string propIds = "";
            foreach (var itemInfo in para.VocaVerMappingID)//数组，更新数据
            {
                if (!string.IsNullOrEmpty(itemInfo))
                {
                    if (propIds != "")
                    {
                        propIds += ",";
                    }
                    propIds += "'" + itemInfo + "'";
                }
            }
            //删除不在这个里面的
            bll.DeleteMappingByIds(propIds, para.PageId);//牵扯到地方比较多，所以直接删除了

            for (int i = 0; i < para.VocaVerMappingID.Length; i++)//所有新传过来的行业版本参数
            {
                DataSet ds = bll.GetExistsVocaVerMappingIDandPageId(para.VocaVerMappingID[i], para.PageId);
                if (ds!=null && ds.Tables[0].Rows.Count > 0)
                {
                    entity =new SysModulePageMappingEntity ();
                    entity.MappingID = Guid.Parse(ds.Tables[0].Rows[0]["MappingID"].ToString());
                    entity.VocaVerMappingID = Guid.Parse(para.VocaVerMappingID[i]);
                    entity.PageID = Guid.Parse(para.PageId);
                    entity.Sequence =Convert.ToInt32(ds.Tables[0].Rows[0]["Sequence"].ToString());
                    entity.IsDelete = 0;
                    bll.Update(entity);
                }
                else
                {
                    object Seq=bll.GetModulePageMappingBySequence(para.VocaVerMappingID[i]);
                    int Sequence=0;
                    if (!string.IsNullOrWhiteSpace(Seq.ToString()))
	                {
		               Sequence =Convert.ToInt32(Seq.ToString());
	                }
                    entity = new SysModulePageMappingEntity(); 
                    entity.MappingID = Guid.NewGuid();
                    entity.VocaVerMappingID = Guid.Parse(para.VocaVerMappingID[i]);
                    entity.PageID = Guid.Parse(para.PageId);
                    entity.Sequence =Sequence+1;
                    entity.IsDelete = 0;
                    bll.Create(entity);
                }
            }
            return eData;
        }
    }
}