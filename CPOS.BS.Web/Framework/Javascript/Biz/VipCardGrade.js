﻿Ext.define("ContorlVipCardGradeEntity", {
    extend: "Ext.data.Model",
    fields: [{
        name: "id",
        mapping: "VipCardGradeID",
        type: "string"
    }, {
        name: "value",
        mapping: "VipCardGradeID",
        type: "string"
    }, {
        name: "text",
        mapping: "VipCardGradeName",
        type: "string"
    }]
});

// VipCardGrade 业务控件
Ext.define('jit.biz.VipCardGrade', {
    alias: 'widget.jitbizvipcardgrade',
    constructor: function (args) {
        var store = new Ext.data.Store({
            storeId: args.storeId,
            model: "ContorlVipCardGradeEntity",
            proxy: {
                type: 'ajax',
                reader: {
                    type: 'json',
                    root: 'data'
                }
            },
            listeners: {
                load: function (store) {
                    store.insert(0, {
                        "name": args.Name,
                        "value": '',
                        "text": "--请选择--"
                    });
                }
            }
        });
        if (args.dataType == undefined || args.dataType == null) {
            args.dataType = "VipCardGrade";
        }
        store.proxy.url = "/Framework/Javascript/Biz/Handler/VipCardGradeHandler.ashx?method=" + args.dataType;
        store.load({
            //limit: 10,
            //page: 0
        });
        defaultConfig = {
            store: store
            , valueField: 'value'
            , displayField: 'text'
            , listeners:{
                select: function(combo, record, index) {
                     try {
                         var d = record[0].data;
                         if (typeof args.fnCallback == "function") {
                            args.fnCallback(d);
                         }
                     }
                     catch (ex) {
                         Ext.MessageBox.alert("错误", "错误:" + ex);
                     }
                }
            }
        };
        args = Ext.applyIf(args, defaultConfig);

        var result = Ext.create('Jit.form.field.ComboBox', args);
        result.store = store;
        result.setDefaultValue = function (defValue) {
            store.load({
                params: {}
                , callback: function (r, options, success) {
                    for (var i = 0; i < r.length; i++) {
                        var rawValue = r[i].data.id;
                        if (rawValue == defValue) {
                            result.setValue(rawValue);
                        }
                    }
                }
            });
        };

        return result;
    }
})