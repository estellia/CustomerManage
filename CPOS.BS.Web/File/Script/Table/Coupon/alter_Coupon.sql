/*
   2014年9月22日9:19:46
   用户: dev
   服务器: 112.124.68.147
   数据库: cpos_bs_hotels
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
ALTER TABLE dbo.Coupon ADD
	CustomerID varchar(50) NULL
GO

alter table Coupon alter column CouponDesc nvarchar(max)

ALTER TABLE dbo.Coupon ADD CONSTRAINT
	DF_Coupon_CouponID DEFAULT newid() FOR CouponID
GO

ALTER TABLE dbo.Coupon SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col1] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col2] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col3] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col4] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col5] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col6] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col7] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col8] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col9] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col10] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col11] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col12] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col13] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col14] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col15] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col16] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col17] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col18] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col19] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col20] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col21] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col22] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col23] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col24] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col25] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col26] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col27] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col28] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col29] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col30] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col31] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col32] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col33] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col34] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col35] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col36] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col37] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col38] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col39] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col40] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col41] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col42] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col43] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col44] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col45] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col46] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col47] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col48] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col49] nvarchar(max) ;
alter table [cpos_bs_hotel].[dbo].[Coupon] add [Col50] nvarchar(max) ;