﻿function InitVE() {
    Ext.define("PrizesViewEntity", {
        extend: "Ext.data.Model",
        fields: [
            { name: 'PrizesID', type: 'string' },
            { name: 'PrizeName', type: 'string' },
            { name: 'PrizeShortDesc', type: 'string' },
            { name: 'PrizeDesc', type: 'string' },
            { name: 'LogoURL', type: 'string' },
            { name: 'ImageUrl', type: 'string' },
            { name: 'ContentText', type: 'string' },
            { name: 'ContentUrl', type: 'string' },
            { name: 'Price', type: 'string' },
            { name: 'DisplayIndex', type: 'string' },
            { name: 'CountTotal', type: 'string' },
            { name: 'CountLeft', type: 'string' },
            { name: 'EventId', type: 'string' },
            { name: 'CreateTime', type: 'string' },
            { name: 'CreateBy', type: 'string' },
            { name: 'LastUpdateBy', type: 'string' },
            { name: 'LastUpdateTime', type: 'string' },
            { name: 'IsDelete', type: 'string' },
            { name: 'PrizeTypeId', type: 'int' },
            { name: 'IsAutoPrizes', type: 'int' }
        ]
    });

    Ext.define("PrizesTypeEntity", {
        extend: "Ext.data.Model",
        fields: [
            { name: 'Id', type: 'string' },
            { name: 'Name', type: 'string' }
            ]
    });
    Ext.define("CouponTypeEntity", {
        extend: "Ext.data.Model",
        fields: [
            { name: 'CouponTypeID', type: 'string' },
            { name: 'CouponTypeName', type: 'string' }
            ]
    });
}