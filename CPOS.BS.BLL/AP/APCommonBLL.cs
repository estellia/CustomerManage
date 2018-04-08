using JIT.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using JIT.CPOS.BS.DataAccess.AP;
using JIT.CPOS.BS.Entity;
using System.Data;

namespace JIT.CPOS.BS.BLL.AP {
	public class APCommonBLL {
		private BasicUserInfo CurrentUserInfo;
		private APCommonDAO _currentDAO;
		private string connectionString = ConfigurationManager.AppSettings["Conn_ap"];

		#region 构造函数
		/// <summary>
		/// 构造函数 
		/// </summary>
		public APCommonBLL(LoggingSessionInfo pUserInfo) {
			this._currentDAO = new APCommonDAO(pUserInfo, connectionString);
			this.CurrentUserInfo = pUserInfo;
		}
		#endregion
		/// <summary>
		/// 获取当前用户版本
		/// </summary>
		/// <param name="strCustomerId"></param>
		/// <returns></returns>
		public int GetCustomerVersion(string strCustomerId) {

			return this._currentDAO.GetCustomerVersion(strCustomerId);
		}
		public DataSet GetCustomerInfo(string strCustomerId) {
			return this._currentDAO.GetCustomerInfo(strCustomerId);
		}
		/// <summary>
		/// 获取单个门店售价和商户设置门店数量
		/// </summary>
		/// <param name="strCustomerId"></param>
		/// <returns></returns>
		public DataSet GetCustomerLimtUnitCount(string strCustomerId) {
			return this._currentDAO.GetCustomerLimtUnitCount(strCustomerId);
		}
		/// <summary>
		/// 获取门店数量限制配置表ID
		/// </summary>
		/// <returns></returns>
		public string GetSysUnitNumLimitConfigId() {
			return this._currentDAO.GetSysUnitNumLimitConfigId();

		}
		/// <summary>
		/// 更新为集客版本时 更新CustomerEx中的限制门店数
		/// </summary>
		/// <param name="strCode"></param>
		/// <param name="strCustomerId"></param>
		/// <param name="strUser"></param>
		/// <param name="intUnits"></param>
		public void UpdateCustomerEx(string strCustomerId, string strUser) {
			this._currentDAO.UpdateCustomerEx(strCustomerId, strUser);

		}
	}
}
