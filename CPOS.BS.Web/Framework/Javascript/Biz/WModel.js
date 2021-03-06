﻿Ext.define("ContorlWModelEntity", {
    extend: "Ext.data.Model",
    fields: [{
        name: "id",
        mapping:"ModelId",
        type: "string"
    }, {
        name: "value",
        mapping:"ModelId",
        type: "string"
    }, {
        name: "text",
        mapping:"ModelName",
        type: "string"
    }]
});

// WModel 业务控件
Ext.define('jit.biz.WModel', {
    alias: 'widget.jitbizwmodel',
    constructor: function (args) {
        var store = new Ext.data.Store({
            storeId: args.storeId,
            model: "ContorlWModelEntity",
            proxy: {
                type: 'ajax',
                reader: {
                    type: 'json',
                    root: 'data'
                }
            },
            listeners: {
                load: function (store) {
                    if (!args.multiSelect) {
                        store.insert(0, {
                            "name": args.Name,
                            "value": '',
                            "text": "--请选择--"
                        });
                    }
                },
                expand: function(combo, record, index) {
                     try {
                         store.load({
                            params: { pid: Ext.getCmp(args.parent_id).jitGetValue(), type: args.type }
                         });
                     }
                     catch (ex) {
                         Ext.MessageBox.alert("错误", "错误:" + ex);
                     }
                }
            }
        }); 
        if (args.dataType == undefined || args.dataType == null) {
            args.dataType = "get_list";
        }
        store.proxy.url = "/Framework/Javascript/Biz/Handler/WModelHandler.ashx?method=" + args.dataType;
        if (!args.c) {
            store.load({
                //limit: 10,
                //page: 0
                params: { pid: getUrlParam("ApplicationId") == "" || getUrlParam("ApplicationId").length == 0 ? 
                                    Ext.getCmp(args.parent_id).jitGetValue() : getUrlParam("ApplicationId"), type: args.type }
            });
        }
        defaultConfig = {
            store: store
            , valueField: 'value'
            , displayField: 'text'
        };
        args = Ext.applyIf(args, defaultConfig);
        
        if (args.listeners) {
            args.listeners.expand = function (combo, record, index) {
                //Ext.getCmp(args.id).jitSetValue("");
                //try {
                //    store.load({
                //        params: { 
                //            pid: getUrlParam("ApplicationId") == "" || getUrlParam("ApplicationId").length == 0 ? 
                //                Ext.getCmp(args.parent_id).jitGetValue() : getUrlParam("ApplicationId"), type: args.type }
                //    });
                //}
                //catch (ex) {
                //    Ext.MessageBox.alert("错误", "错误:" + ex);
                //}
            }
        }

        var result= Ext.create('Jit.form.field.ComboBox', args);
        result.store = store; 
        result.fnLoad = function(fn) {
            store.load({
                params: { pid: getUrlParam("ApplicationId") == "" || getUrlParam("ApplicationId").length == 0 ? 
                                Ext.getCmp(args.parent_id).jitGetValue() : getUrlParam("ApplicationId"), type: args.type }
                ,callback: function(r, options, success) {
                    if (fn != undefined && fn != null) {
                        fn();
                    }
                }
            });
        };
        result.setDefaultValue = function(defValue, fn, fn2) {
            store.load({
                //sync: true, async: false,
                params: { pid: getUrlParam("ApplicationId") == "" || getUrlParam("ApplicationId").length == 0 ? 
                                Ext.getCmp(args.parent_id).jitGetValue() : getUrlParam("ApplicationId"), type: args.type }
                ,callback: function(r, options, success) {
                    if (success) {
                        //if (r.length == 0) return;
                        //alert(Ext.getCmp(args.parent_id) != null && Ext.getCmp(args.parent_id).jitGetValue() == "");
                        if (Ext.getCmp(args.parent_id) != null && (Ext.getCmp(args.parent_id).jitGetValue() == "")) {
                            result.setValue("");
                            return;
                        }
                        var flag = false;
                        for (var i = 0; i < r.length; i++) {
                            var rawValue = r[i].data.id;
                            if (rawValue == defValue) {
                                flag = true;
                                result.setValue(rawValue);
                            }
                        }
                        if (!flag) {
                            result.setValue("");
                        }
                        if (fn2 != undefined && fn2 != null) {
                            fn2();
                        }
                    }
                }
            });
            if (fn != undefined && fn != null) {
                fn();
            }
        };

        return result;
  }
})