﻿function InitStore() {

    Ext.create('Ext.data.Store', {
        storeId: "WKeywordReplyStore",
        model: "WKeywordReplyViewEntity",
        proxy: {
            type: 'ajax',
            reader: {
                type: 'json',
                root: "topics",
                totalProperty: "totalCount"
            },
            extraParams: {
                form: ""
            },
            actionMethods: { read: 'POST' }
        }
    });
    

}