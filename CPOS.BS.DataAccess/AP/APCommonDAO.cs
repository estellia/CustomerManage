using JIT.CPOS.BS.Entity;
using JIT.Utility.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace JIT.CPOS.BS.DataAccess.AP {
	public class APCommonDAO : Base.BaseCPOSDAO{
		public string StaticConnectionString { get; set; }
		private ISQLHelper staticSqlHelper;

		public APCommonDAO(LoggingSessionInfo pUserInfo, string connectionString)
            : base(pUserInfo)
        {
			this.StaticConnectionString = connectionString;
			this.SQLHelper = StaticSqlHelper;
		}
		protected ISQLHelper StaticSqlHelper
		{
			get
			{
				if (null == staticSqlHelper)
					staticSqlHelper = new DefaultSQLHelper(StaticConnectionString);
				return staticSqlHelper;
			}
		}
		/// <summary>
		/// 获取当前用户版本
		/// </summary>
		/// <param name="strCustomerId"></param>
		/// <returns></returns>
		public int GetCustomerVersion(string strCustomerId) {

			string strSql = string.Format(@"SELECT isnull(b.VersionID,0) VersionID
											  FROM [cpos_ap].[dbo].[CustomerModuleMapping] a
											  INNER JOIN SysVocationVersionMapping b  ON a.VocaVerMappingID=b.VocaVerMappingID
											  WHERE a.CustomerID='{0}'", strCustomerId);
			return Convert.ToInt16(this.SQLHelper.ExecuteScalar(strSql));
		}
		/// <summary>
		/// ap库商户信息
		/// </summary>
		/// <param name="strCustomerId"></param>
		/// <returns></returns>
		public DataSet GetCustomerInfo(string strCustomerId) {

			string strSql = string.Format(@"SELECT  customer_id,customer_code,customer_name FROM dbo.t_customer
											WHERE customer_id='{0}'", strCustomerId);
			return this.SQLHelper.ExecuteDataset(strSql);
		}
		/// <summary>
		/// 获取单个门店售价和商户设置门店数量
		/// </summary>
		/// <param name="strCustomerId"></param>
		/// <returns></returns>
		public DataSet GetCustomerLimtUnitCount(string strCustomerId) {
			string strSql = string.Format(@"SELECT units FROM T_CustomerEx
											WHERE customerid='{0}'
											SELECT SingleUnitPrice FROM SysUnitNumLimitConfig
											", strCustomerId);
			return this.SQLHelper.ExecuteDataset(strSql);
		}
		/// <summary>
		/// 获取门店数量限制配置表ID
		/// </summary>
		/// <returns></returns>
		public string GetSysUnitNumLimitConfigId() {
			string strSql = "SELECT UnitNumLimitConfigId FROM SysUnitNumLimitConfig WHERE IsDelete=0";
			return this.SQLHelper.ExecuteScalar(strSql).ToString(); 

		}
		public void UpdateCustomerEx(string strCustomerId,string strUser) {
			string strSql = string.Format(@"
										DECLARE @VersionID INT 
										SELECT @VersionID=VersionID
										FROM [CustomerModuleMapping] a	
													INNER JOIN SysVocationVersionMapping b ON a.VocaVerMappingID=b.VocaVerMappingID 
										WHERE a.CustomerID='{0}'
										
											DECLARE @C_Code NVARCHAR(50)
											SELECT @C_Code=customer_code FROM dbo.t_customer 
											WHERE customer_id='{0}'

											DECLARE @UnitLimitNum INT
										IF (@VersionID=3)
										BEGIN
											SELECT  @UnitLimitNum=UnitLimitNum
											FROM    SysUnitNumLimitConfig
											WHERE   IsDelete = 0
										END
										ELSE
										BEGIN
											SET @UnitLimitNum=-1
										END
											IF NOT EXISTS(SELECT * FROM T_CustomerEx WHERE CustomerId='{0}')
											BEGIN
												INSERT INTO [dbo].[T_CustomerEx]
													   ([customer_id]
													   ,[customer_code]
													   ,[Units]
													   ,[CustomerId]
													   ,[CreateBy]
													   ,[CreateTime]
													   ,[LastUpdateBy]
													   ,[LastUpdateTime]
													   ,[IsDelete])
												 VALUES
													   (newid(),
														@C_Code,
														@UnitLimitNum,
														'{0}',
														'{1}',
														getdate(),
														'{1}',
														getdate(),
														0
														)
											END
											ELSE
											BEGIN
												UPDATE [dbo].[T_CustomerEx]
												SET Units=@UnitLimitNum
													,[LastUpdateTime]=getdate()
												WHERE CustomerId='{0}'

											END
										", strCustomerId,strUser);
			this.SQLHelper.ExecuteNonQuery(strSql);
		}
	}
}
