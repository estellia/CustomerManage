using JIT.CPOS.BS.BLL.AP;
using JIT.CPOS.BS.Entity;
using JIT.Utility.Notification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace JIT.CPOS.BS.BLL {
	public class SendMailBLL {
		public void SendMail(LoggingSessionInfo loggingSessionInfo, string strTitle, string strBody, string strSupportType) {
			FromSetting fs = new FromSetting();
			fs.SMTPServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
			fs.SendFrom = ConfigurationManager.AppSettings["SendFrom"].ToString();
			fs.UserName = ConfigurationManager.AppSettings["SendFrom"].ToString();
			//获取加密密码
			string emailEncPwd = ConfigurationManager.AppSettings["EmailPassword"].Trim();
			//获取解密口令
			string decToken = ConfigurationManager.AppSettings["DecToken"].ToString();
			//获取解密密码
			string emailDecPwd = ZmindFx.ZmindEncryptTool.Decrypt(emailEncPwd, decToken);
			fs.Password = emailDecPwd;
			string mailTo = ConfigurationManager.AppSettings["MailTo"].ToString();// "business@zmind.cn";


			bool blResult = Mail.SendMail(fs, mailTo, strTitle, strBody, null);
			if (blResult) {
				ApplicationSupportLogBLL bllLog = new ApplicationSupportLogBLL(loggingSessionInfo);
				ApplicationSupportLogEntity entityLog = new ApplicationSupportLogEntity();
				entityLog.SurportType = strSupportType;
				entityLog.Title = strTitle;
				entityLog.Content = strBody;
				entityLog.SentUser = loggingSessionInfo.CurrentUser.User_Name;
				entityLog.SentUserPhone = loggingSessionInfo.CurrentUser.User_Telephone;
				entityLog.SentEMail = loggingSessionInfo.CurrentUser.User_Email;
				entityLog.SentUserSex = 1;
				entityLog.ReceiveEMail = mailTo;
				entityLog.IsSuccess = 1;
				entityLog.CustomerId = loggingSessionInfo.ClientID;

				bllLog.Create(entityLog);

			}
		}
	}
}
