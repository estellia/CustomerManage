﻿Ext.define("ContorlDeliveryUnitEntity", {
    extend: "Ext.data.Model",
    fields: [{
        name: "id",
        mapping:"Id",
        type: "string"
    }, {
        name: "value",
        mapping:"Id",
        type: "string"
    }, {
        name: "text",
        mapping:"Name",
        type: "string"
    }]
});

// DeliveryUnit 业务控件
Ext.define('jit.biz.DeliveryUnit', {
    alias: 'widget.jitbizdeliveryunit',
    constructor: function (args) {
        var store = new Ext.data.Store({
            storeId: args.storeId,
            //pageSize: args.pageSize || 15,
            model: "ContorlDeliveryUnitEntity",
            proxy: {
                type: 'ajax',
                reader: {
                    type: 'json',
                    root: 'data'
                }
            },
            listeners: {
                load: function (store) {
                    //if (args.isDefault != null && args.isDefault) {
                        store.insert(0, {
                            "name": args.Name,
                            "value": '',
                            "text": "--请选择--"
                        });
                    //}
                }
            }
        }); 
        store.proxy.url ="/Framework/Javascript/Biz/Handler/DeliveryUnitHandler.ashx?method=get_delivery_unit&parent_id=" + args.parent_id;
        store.load({
            //limit: 10,
            //page: 0
        });
        defaultConfig = {
            store: store
            , valueField: 'value'
            , displayField: 'text'
        };      
        args = Ext.applyIf(args, defaultConfig);
        return Ext.create('Jit.form.field.ComboBox', args);
  }
})