﻿function InitVE() {
    Ext.define("NewsViewEntity", {
        extend: "Ext.data.Model",
        fields: [{
            name: 'NewsId',
            type: 'string'
        },
        {
            name: 'NewsType',
            type: 'string'
        },
        {
            name: 'NewsTitle',
            type: 'string'
        },
        {
            name: 'NewsSubTitle',
            type: 'string'
        },
        {
            name: 'PublishTime',
            type: 'string'
        },
        {
            name: 'Content',
            type: 'string'
        },
        {
            name: 'ContentUrl',
            type: 'string'
        },
        {
            name: 'ImageUrl',
            type: 'string'
        },
        {
            name: 'ThumbnailImageUrl',
            type: 'string'
        },
        {
            name: 'APPId',
            type: 'string'
        },
        {
            name: 'CreateTime',
            type: 'string'
        },
        {
            name: 'CreateBy',
            type: 'string'
        },
        {
            name: 'LastUpdateTime',
            type: 'string'
        },
        {
            name: 'LastUpdateBy',
            type: 'string'
        },
        {
            name: 'IsDelete',
            type: 'string'
        },
        {
            name: 'NewsTypeName',
            type: 'string'
        }, 
        {
            name: 'StrPublishTime',
            type: 'string'
        }]
    });
}