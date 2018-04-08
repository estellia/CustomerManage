﻿
// CitySelectTree 业务控件
Ext.define('Jit.Biz.CitySelectTree', {
    alias: 'widget.jitbizcityselecttree',
    constructor: function (args) {
        var instance = '';
        if (args.parentId == null) {
            args = Ext.applyIf(args, { parentId: "" });
        }
        Ext.Ajax.request({
            url: "/Framework/Javascript/Biz/Handler/CitySelectTreeHandler.ashx?method=&parent_id=" + args.parentId,
            method: 'GET',
            async: false,
            success: function (response) {
                defaultConfig = {
                    url: '/Framework/Javascript/Biz/Handler/CitySelectTreeHandler.ashx?method=&parent_id=' + args.parentId   //树的数据从后台加载
                    , multiSelect: false                 //树是否为多选
                    , rootText: ''                  //树的根节点的文本
                    , rootID: ''                      //树的根节点的值
                    , isSelectLeafOnly: true           //只能选择树的叶子节点
                    , isRootVisible: false
                    , pickerCfg: {
                        minHeight: 230
                        , maxHeight: 230
                    }
                }
                args = Ext.applyIf(args, defaultConfig);

                instance = Ext.create('Jit.form.field.ComboTree', args);
            }
        });

        //根据value自动赋值text
        instance.jitSetValue = function (value) {
            if (typeof (value) == "object") {
                instance.setValues(value, false);
            }
            else {
                if (value != null && value.toString() != "") {
                    Ext.Ajax.request({
                        url: "/Framework/Javascript/Biz/Handler/CitySelectTreeHandler1.ashx?method=GetCityByCityCode&citycode=" + value,
                        method: 'POST',
                        callback: function (options, success, response) {
                            if (success == true) {
                                instance.setValues([{ "id": value, "text": response.responseText}], false);
                            }
                        }
                    });
                }
                else { //为空的情况
                    instance.setValues([{ "id": "", "text": ""}], false);
                }
            }
        }

        return instance;
    }
})
