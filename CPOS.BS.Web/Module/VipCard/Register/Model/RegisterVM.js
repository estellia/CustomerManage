﻿function InitVE() {
    Ext.define("VipCardViewEntity", {
        extend: "Ext.data.Model",
        fields: [
            { name: 'VipCardID', type: 'string' },
            { name: 'VipCardTypeID', type: 'string' },
            { name: 'VipCardTypeName', type: 'string' },
            { name: 'VipCardCode', type: 'string' },
            { name: 'VipCardName', type: 'string' },
            { name: 'VipCardStatusId', type: 'string' },
            { name: 'VipStatusName', type: 'string' },
            { name: 'MembershipTime', type: 'string' },
            { name: 'BeginDate', type: 'string' },
            { name: 'EndDate', type: 'string' },
            { name: 'TotalAmount', type: 'string' },
            { name: 'BalanceAmount', type: 'string' },
            { name: 'VipCardGradeID', type: 'string' },
            { name: 'VipCardGradeName', type: 'string' },
            { name: 'PurchaseTotalAmount', type: 'string' },
            { name: 'PurchaseTotalCount', type: 'string' },
            { name: 'LastSalesTime', type: 'string' },
            { name: 'CreateTime', type: 'string' },
            { name: 'CreateBy', type: 'string' },
            { name: 'LastUpdateTime', type: 'string' },
            { name: 'LastUpdateBy', type: 'string' },
            { name: 'IsDelete', type: 'string' },
            { name: 'CustomerID', type: 'string' },
            { name: 'UnitName', type: 'string' },
            { name: 'UnitID', type: 'string' }
            ]
    });

    Ext.define("VipExpandViewEntity", {
        extend: "Ext.data.Model",
        fields: [
            { name: 'VipExpandID', type: 'string' },
            { name: 'VipCardID', type: 'string' },
            { name: 'VipID', type: 'string' },
            { name: 'LicensePlateNo', type: 'string' },
            { name: 'CarBrandID', type: 'string' },
            { name: 'CarBrandName', type: 'string' },
            { name: 'CarModelsID', type: 'string' },
            { name: 'CarModelsName', type: 'string' },
            { name: 'ChassisNumber', type: 'string' },
            { name: 'CompartmentsForm', type: 'string' },
            { name: 'PurchaseTime', type: 'string' },
            { name: 'Remark', type: 'string' },
            { name: 'CreateTime', type: 'string' },
            { name: 'CreateBy', type: 'string' },
            { name: 'LastUpdateTime', type: 'string' },
            { name: 'LastUpdateBy', type: 'string' },
            { name: 'IsDelete', type: 'string' }
            ]
    });

    Ext.define("VipViewEntity", {
        extend: "Ext.data.Model",
        fields: [
            { name: "VIPID", type: 'string' },
            { name: "VipName", type: 'string' },
            { name: "VipLevel", type: 'string' },
            { name: "VipLevelDesc", type: 'string' },
            { name: "VipCode", type: 'string' },
            { name: "WeiXin", type: 'string' },
            { name: "WeiXinUserId", type: 'string' },
            { name: "Gender", type: 'string' },
            { name: "Age", type: 'string' },
            { name: "Phone", type: 'string' },
            { name: "SinaMBlog", type: 'string' },
            { name: "TencentMBlog", type: 'string' },
            { name: "Birthday", type: 'string' },
            { name: "Qq", type: 'string' },
            { name: "Email", type: 'string' },
            { name: "Status", type: 'string' },
            { name: "StatusDesc", type: 'string' },
            { name: "VipSourceId", type: 'string' },
            { name: "VipSourceName", type: 'string' },
            { name: "Integration", type: 'string' },
            { name: "ClientID", type: 'string' },
            { name: "RecentlySalesTime", type: 'string' },
            { name: "RegistrationTime", type: 'string' },
            { name: "CreateTime", type: 'string' },
            { name: "CreateBy", type: 'string' },
            { name: "LastUpdateTime", type: 'string' },
            { name: "LastUpdateBy", type: 'string' },
            { name: "IsDelete", type: 'string' },
            { name: "APPID", type: 'string' },
            { name: "HigherVipID", type: 'string' },
            { name: "QRVipCode", type: 'string' },
            { name: "City", type: 'string' },
            { name: "LastUnit", type: 'string' },
            { name: "NextLevelIntegralAmount", type: 'string' },
            { name: "IntegralAmount", type: 'string' },
            { name: 'IntegralForHightUser', type: 'string' },
            { name: 'PurchaseAmount', type: 'string' },
            { name: 'PurchaseCount', type: 'string' },
            { name: 'UserName', type: 'string' },
            { name: 'Enterprice', type: 'string' },
            { name: 'IsChainStores', type: 'string' },
            { name: 'IsWeiXinMarketing', type: 'string' },
            { name: 'GenderInfo', type: 'string' }
            ]
    });
}