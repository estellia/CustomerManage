﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.WeiXin.SysPage.Request;
using JIT.CPOS.DTO.Module.WeiXin.SysPage.Response;
using JIT.CPOS.BS.BLL;
using System.Data;
using JIT.CPOS.BS.Entity;

using System.Text.RegularExpressions;
using System.Configuration;


namespace JIT.CPOS.Web.ApplicationInterface.Module.WX.SysPage
{
    public class CreateCustomerConfigAH : BaseActionHandler<CreateCustomerConfigRP, CreateCustomerConfigRD>
    {
        protected override CreateCustomerConfigRD ProcessRequest(APIRequest<CreateCustomerConfigRP> pRequest)
        {
            CreateCustomerConfigRD rdRes = new CreateCustomerConfigRD();

            string pCustomerID = "";
            if (string.IsNullOrEmpty(pRequest.CustomerID))
            {
                pCustomerID = pRequest.CustomerID;
            }
            else if (CurrentUserInfo != null)
            {
                pCustomerID = CurrentUserInfo.ClientID.ToString();
            }

            #region 1.写入文件。单个客户
            if (!string.IsNullOrEmpty(pCustomerID))//如果CustomerID不为空
            {
                var currentInfo = Default.GetBSLoggingSession(pCustomerID, "");
                try
                {
                    SysPageBLL bll = new SysPageBLL(currentInfo);
                    string strConfig = bll.GetCreateCustomerConfig(currentInfo.ClientID); //获取要生成的Config内容
                    strConfig = Regex.Replace(strConfig, @"\s", "");
                    //写入Config文件
                    string FileName = currentInfo.ClientID.ToString() + ".js";
                    string strpath = ConfigurationManager.AppSettings["CposWebConfigPath"];
                    if (!Directory.Exists(strpath))
                    {
                        Directory.CreateDirectory(strpath);
                    }
                    strpath = strpath + FileName; //存储文件路径
                    FileStream fs = new FileStream(strpath, FileMode.Create); //新建一个文件
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(strConfig);  //开始写入
                    sw.Flush();//清除缓冲区
                    sw.Close();//关闭流
                    // 写入varsion文件
                    string strVersion = bll.GetCreateCustomerVersion(currentInfo.ClientID,currentInfo.CurrentLoggingManager.Customer_Name); //获取要生成的Version内容
                    strVersion = Regex.Replace(strVersion, @"\s", "");
                    string FileNameVersion = currentInfo.ClientID.ToString() + ".js";
                    // string strpathVersion  = HttpContext.Current.Server.MapPath(@"../" +  "/HtmlApps/version/Test/").Replace("CPOS.BS.Web", "CPOS.Web");
                    string strpathVersion = ConfigurationManager.AppSettings["CposWebVersionPath"];
                    if (!Directory.Exists(strpathVersion))
                    {
                        Directory.CreateDirectory(strpathVersion);
                    }
                    strpathVersion = strpathVersion + FileNameVersion;//存储文件路径
                    FileStream fsVersion = new FileStream(strpathVersion, FileMode.Create); //新建一个文件
                    StreamWriter swVersion = new StreamWriter(fsVersion);
                    swVersion.Write(strVersion);  //开始写入
                    swVersion.Flush();//清除缓冲区
                    swVersion.Close();//关闭流
                }
                catch (Exception ex)
                {
                    JIT.Utility.Log.Loggers.Exception(new Utility.Log.ExceptionLogInfo(ex));
                    throw ex;
                }
            }
            #endregion
            #region 2.写入文件。全部客户
            else
            {
                //访问ap数据库
                var userInfo = new LoggingSessionInfo() { CurrentLoggingManager = new LoggingManager() };
                userInfo.CurrentLoggingManager.Connection_String = System.Configuration.ConfigurationManager.AppSettings["Conn_ap"];
                SysPageBLL BLL1 = new SysPageBLL(userInfo);
                DataTable dt = BLL1.GetCustomerInfo();
                var tempCustoemrID = "";
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        try
                        {
                            string CustomerId = dt.Rows[i]["customer_id"].ToString();
                            tempCustoemrID = CustomerId;
                            var currentInfo = Default.GetBSLoggingSession(CustomerId, "");//根据商户的数据的标识来取数据库连接
                            SysPageBLL bll = new SysPageBLL(currentInfo);
                            string AllConfig = bll.GetCreateCustomerConfig(CustomerId); //获取要生成的Config内容
                            AllConfig = Regex.Replace(AllConfig, @"\s", "");

                            //写入Config文件
                            string FileName = CustomerId + ".js";

                            string strpath = ConfigurationManager.AppSettings["CposWebConfigPath"];
                            if (!Directory.Exists(strpath))
                            {
                                Directory.CreateDirectory(strpath);
                            }
                            strpath = strpath + FileName; //存储文件路径
                            // string outpath = HttpContext.Current.Server.MapPath(@"../" + strpath).Replace("CPOS.BS.Web", "CPOS.Web");
                            FileStream fs = new FileStream(strpath, FileMode.Create); //新建一个文件（这样每一次发布都新建****）
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write(AllConfig);  //开始写入
                            sw.Flush();//清除缓冲区
                            sw.Close();//关闭流
                            // 写入varsion文件
                            string strVersion = bll.GetCreateCustomerVersion(CustomerId,currentInfo.CurrentLoggingManager.Customer_Name); //获取要生成的Version内容
                            strVersion = Regex.Replace(strVersion, @"\s", "");
                            string FileNameVersion = CustomerId + ".js";

                            //string strpathVersion = HttpContext.Current.Server.MapPath(@"../" + "/HtmlApps/version/Test/").Replace("CPOS.BS.Web", "CPOS.Web"); 
                            string strpathVersion = ConfigurationManager.AppSettings["CposWebVersionPath"];
                            if (!Directory.Exists(strpathVersion))
                            {
                                Directory.CreateDirectory(strpathVersion);
                            }
                            strpathVersion = strpathVersion + FileNameVersion;//存储文件路径（存储的信息是从customerbasicSetting里取的）

                            FileStream fsVersion = new FileStream(strpathVersion, FileMode.Create); //新建一个文件
                            StreamWriter swVersion = new StreamWriter(fsVersion);
                            swVersion.Write(strVersion);  //开始写入
                            swVersion.Flush();//清除缓冲区
                            swVersion.Close();//关闭流
                        }
                        catch (Exception ex)
                        {
                            string s = tempCustoemrID;//查看出问题的商户****
                            JIT.Utility.Log.Loggers.Exception(new Utility.Log.ExceptionLogInfo(ex));
                            throw ex;
                        }
                    }
                }
            }
            #endregion
            return rdRes;
        }
    }
}