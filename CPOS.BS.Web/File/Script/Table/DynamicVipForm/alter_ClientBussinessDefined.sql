/*
   2014年8月7日14:53:28
   用户: dev
   服务器: 112.124.68.147
   数据库: cpos_bs_lj
   应用程序: 
*/

/* 为了防止任何可能出现的数据丢失问题，您应该先仔细检查此脚本，然后再在数据库设计器的上下文之外运行此脚本。*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ClientBussinessDefined
	DROP CONSTRAINT DF__ClientBus__IsDef__3652C63E
GO
CREATE TABLE dbo.Tmp_ClientBussinessDefined
	(
	ClientBussinessDefinedID nvarchar(50) NOT NULL,
	TableName nvarchar(50) NULL,
	ColumnName nvarchar(50) NULL,
	ColumnType int NULL,
	ControlType int NULL,
	MinLength int NULL,
	MaxLength int NULL,
	ColumnDesc nvarchar(50) NULL,
	ColumnDescEn nvarchar(50) NULL,
	HierarchyID nvarchar(50) NULL,
	CorrelationValue nvarchar(50) NULL,
	IsRead int NULL,
	IsMustDo int NULL,
	IsUse int NULL,
	IsRepeat int NULL,
	EditOrder int NULL,
	ListOrder int NULL,
	ConditionOrder int NULL,
	GridWidth decimal(18, 3) NULL,
	SqlDesc nvarchar(2000) NULL,
	Remark nvarchar(200) NULL,
	ClientID nvarchar(50) NULL,
	CreateBy varchar(50) NULL,
	CreateTime datetime NULL,
	LastUpdateBy varchar(50) NULL,
	LastUpdateTime datetime NULL,
	IsDelete int NULL,
	AttributeTypeID int NULL,
	IsTemplate int NULL,
	DisplayType int NULL,
	IsDefaultProp int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ClientBussinessDefined SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_ClientBussinessDefined ADD CONSTRAINT
	DF__ClientBus__IsDef__3652C63E DEFAULT ((1)) FOR IsDefaultProp
GO
IF EXISTS(SELECT * FROM dbo.ClientBussinessDefined)
	 EXEC('INSERT INTO dbo.Tmp_ClientBussinessDefined (ClientBussinessDefinedID, TableName, ColumnName, ColumnType, ControlType, MinLength, MaxLength, ColumnDesc, ColumnDescEn, HierarchyID, CorrelationValue, IsRead, IsMustDo, IsUse, IsRepeat, EditOrder, ListOrder, ConditionOrder, GridWidth, SqlDesc, Remark, ClientID, CreateBy, CreateTime, LastUpdateBy, LastUpdateTime, IsDelete, AttributeTypeID, IsTemplate, DisplayType, IsDefaultProp)
		SELECT ClientBussinessDefinedID, TableName, ColumnName, ColumnType, ControlType, MinLength, MaxLength, ColumnDesc, ColumnDescEn, HierarchyID, CorrelationValue, IsRead, IsMustDo, IsUse, IsRepeat, EditOrder, ListOrder, ConditionOrder, GridWidth, SqlDesc, Remark, ClientID, CONVERT(varchar(50), CreateBy), CreateTime, CONVERT(varchar(50), LastUpdateBy), LastUpdateTime, IsDelete, AttributeTypeID, IsTemplate, DisplayType, IsDefaultProp FROM dbo.ClientBussinessDefined WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.ClientBussinessDefined
GO
EXECUTE sp_rename N'dbo.Tmp_ClientBussinessDefined', N'ClientBussinessDefined', 'OBJECT' 
GO
ALTER TABLE dbo.ClientBussinessDefined ADD CONSTRAINT
	PK_ClientBussinessDefined PRIMARY KEY CLUSTERED 
	(
	ClientBussinessDefinedID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
