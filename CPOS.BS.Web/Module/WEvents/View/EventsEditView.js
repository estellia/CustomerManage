﻿function InitView() {

    //editPanel area

    //jifeng.cao begin
    //Ext.create('Jit.biz.Options', {
    //    id: "cmbIsTicketRequired",
    //    OptionName: 'YES/NO',
    //    renderTo: "cmbIsTicketRequired",
    //    labelWidth: 0, labelPad: 0,
    //    width: 100,
    //    listeners: {
    //        "select": fnSelectIsTicketRequired
    //    }
    //});

    Ext.create('Jit.biz.Options', {
        id: "cmbMailSendInterval",
        OptionName: 'MailSendInterval',
        renderTo: "cmbMailSendInterval",
        labelWidth: 0, labelPad: 0,
        width: 100
    });

    //    Ext.create('Jit.biz.Options', {
    //        id: "cmbIsDefault",
    //        OptionName: 'YES/NO',
    //        renderTo: "cmbIsDefault",
    //        labelWidth: 0, labelPad: 0,
    //        width: 100
    //    });

    //    Ext.create('Jit.biz.Options', {
    //        id: "cmbIsTop",
    //        OptionName: 'YES/NO',
    //        renderTo: "cmbIsTop",
    //        labelWidth: 0, labelPad: 0,
    //        width: 100
    //    });

    Ext.create('Jit.biz.LEventsType', {
        id: "cmbEditEventTypeID",
        renderTo: "cmbEventTypeID",
        isDefaultLoad: true,
        isDefault: true,
        labelWidth: 0, labelPad: 0,
        width: 100
    });

    //    Ext.create('Jit.biz.MobileModule', {
    //        id: "cmbMobileModuleID",
    //        renderTo: "cmbMobileModuleID",
    //        isDefaultLoad: true,
    //        isDefault: true,
    //        labelWidth: 0, labelPad: 0,
    //        width: 100
    //    });

    Ext.create('Jit.form.field.ComboBox', {
        id: "cmbMobileModuleID",
        renderTo: "cmbMobileModuleID",
        valueField: "MobileModuleID",
        displayField: "ModuleName",
        store: Ext.getStore("Module1Store"),
        width: 100

    });


    Ext.create('Jit.form.field.TextArea', {
        id: 'txt_Intro',
        name: 'Intro',
        renderTo: 'divIntro'
        , width: 405
        , height: 100
        , labelPad: 0
        , labelWidth: 0
        , margin: '0 0 10 0'
    });

    Ext.create('Jit.biz.MapSelect', {
        id: 'txtLongitudeLatitude'   //panel 的id 默认为mapSelect
        , fieldLabel: ''  //textField 的fieldLabel
        , text: '选 择'  //button的显示文本
        , mapTitle: 'Map'   //地图的显示title
        , width: 150
        , renderTo: 'txtLongitudeLatitude' //panel的renderTo   没有则不需要添加   
        , labelWidth: 0, labelPad: 0
    });
    //活动分类
    //    Ext.create('Jit.biz.Options', {
    //        xtype: "jitcombobox",
    //        fieldLabel: "<font color='red'>*</font>活动类型",
    //        emptyText: '--请选择--',
    //        store: Ext.getStore("titleStore"),
    //        valueField: "Title",
    //        displayField: "cmbIsTop",
    //        name: "cmbIsTop",
    //        id: "cmbIsTop"
    //    });
    Ext.create('Jit.form.field.Text', {
        id: "txtbootURL",
        text: "",
        renderTo: "txtbootURL",
        width: 405
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtOrganizer",
        text: "",
        renderTo: "txtOrganizer",
        width: 405
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtShareRemarke",
        text: "",
        renderTo: "txtShareRemarke",
        width: 405
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtOver",
        text: "",
        renderTo: "txtOver",
        width: 405
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtPoserImegUrl",
        text: "",
        renderTo: "txtPoserImegUrl",
        width: 405
    });
    //Jermyn20140706--------------------------------------------------------
    Ext.create('Jit.form.field.Checkbox', {
        id: "IsPointsLottery",
        text: "",
        renderTo: "IsPointsLottery",
        width: 10,
        value: true,
        listeners: {
            change: function () {
                if (Ext.getCmp("IsPointsLottery").value == true) {
                    $("#txtPointsLottery").show();
                    $("#PointsLottery").show();
                    Ext.getCmp("PointsLottery").jitSetValue("1");
                }
                else {
                    $("#txtPointsLottery").hide();
                    $("#PointsLottery").hide();
                }
            }
        }

    });

    Ext.create('Jit.form.field.Number', {
        id: "PointsLottery",
        renderTo: "PointsLottery",
        width: 100
    });
    Ext.create('Jit.form.field.Number', {
        id: "RewardPoints",
        renderTo: "RewardPoints",
        width: 100
    });
    //------------------------------------------------------
    Ext.create('Jit.form.field.Checkbox', {
        id: "txtIsShare",
        text: "",
        renderTo: "txtIsShare",
        width: 10,
        value: true,
        listeners: {
            change: function () {
   
                if (Ext.getCmp("txtIsShare").value == true) {
                    $("#trPoserImegUrl").removeAttr("style");
                    $("#trShareRemarke").removeAttr("style");
                    $("#trOver").removeAttr("style");
                    var x = $("#hasChoosed").html();
                    var model = Ext.getCmp("txtModelId").getValue();
                    if (model == "1") {
                      //  $("#tabInfo").height(1390);
                    }
                    else {
                       // $("#tabInfo").height(1390 + x * 80);
                    }
                    $("#txtRewardPoints").show();
                    $("#RewardPoints").show();
                    Ext.getCmp("txtOver").jitSetValue("");
                    Ext.getCmp("txtPoserImegUrl").jitSetValue("");
                    Ext.getCmp("txtShareRemarke").jitSetValue("");
                    Ext.getCmp("RewardPoints").jitSetValue("1");



                }
                else {
                    $("#trPoserImegUrl").attr("style", "display:none");
                    $("#trShareRemarke").attr("style", "display:none");
                    $("#trOver").attr("style", "display:none");
                    $("#txtRewardPoints").hide();
                    $("#RewardPoints").hide();
                    var x = $("#hasChoosed").html();
                    if (model == "1") {
                      //  $("#tabInfo").height(1290);
                      //  $("#tabInfo").height(1350);
                    }
                    else {
                        //$("#tabInfo").height(1290 + x * 80);
                      //  $("#tabInfo").height(1300 + x * 80);
                    }

                }
            }
        }
    });
    //jifeng.cao end


    //    Ext.create('Jit.Biz.LEventSelectTree', {
    //        id: "txtParentEvent",
    //        text: "",
    //        renderTo: "txtParentEvent",
    //        width: 100
    //    });

   
    Ext.create('Jit.form.field.Date', {
        id: "txtStartDate",
        text: "",
        renderTo: "txtStartDate",
        width: 100
    });
    Ext.create('Jit.form.field.Date', {
        id: "txtEndDate",
        text: "",
        renderTo: "txtEndDate",
        width: 100
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtStartTime",
        text: "",
        value: "00:00",
        renderTo: "txtStartTime",
        maxLength: 5,
        width: 100
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtEndTime",
        text: "",
        value: "00:00",
        renderTo: "txtEndTime",
        maxLength: 5,
        width: 100
    });


    Ext.create('Jit.form.field.Text', {
        id: "txtTitle",
        text: "",
        renderTo: "txtTitle",
        width: 405
    });

    //    Ext.create('Jit.form.field.Text', {
    //        id: "txtEventId",
    //        text: "",
    //        renderTo: "txtEventId",
    //        width: 405
    //    });

    Ext.create('Jit.form.field.Text', {
        id: "txtCityId",
        text: "",
        renderTo: "txtCityId",
        width: 100
    });
    //Ext.create('Jit.Biz.CitySelectTree', {
    //    id: "txtCityId",
    //    text: "",
    //    renderTo: "txtCityId",
    //    width: 100
    //});

    Ext.create('Jit.form.field.Text', {
        id: "txtContact",
        text: "",
        renderTo: "txtContact",
        width: 100
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtPhoneNumber",
        text: "",
        renderTo: "txtPhoneNumber",
        width: 100
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtEmail",
        text: "",
        renderTo: "txtEmail",
        width: 100
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtAddress",
        text: "",
        renderTo: "txtAddress",
        width: 405
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtImageUrl",
        text: "",
        renderTo: "txtImageUrl",
        //readOnly: true,
        width: 405
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtUrl",
        text: "",
        renderTo: "txtUrl",
        //readOnly: true,
        width: 405
    });

    //Ext.create('jit.biz.EventCheckinType', {
    //    id: "txtCheckinType",
    //    text: "",
    //    renderTo: "txtCheckinType",
    //    width: 100
    //});

    //Ext.create('jit.biz.EventRange', {
    //    id: "txtCheckinRange",
    //    text: "",
    //    renderTo: "txtCheckinRange",
    //    width: 100
    //});

    //    Ext.create('jit.biz.QuestionnaireType', {
    //        id: "txtApplyQues",
    //        text: "",
    //        renderTo: "txtApplyQues",
    //        width: 100
    //    });

    //    Ext.create('jit.biz.QuestionnaireType', {
    //        id: "txtPollQues",
    //        text: "",
    //        renderTo: "txtPollQues",
    //        width: 100
    //    });
    Ext.create('Jit.form.field.ComboBox', {
        id: "txtPollQues",
        renderTo: "txtPollQues",
        valueField: "MobileModuleID",
        displayField: "ModuleName",
        store: Ext.getStore("Module2Store"),
        width: 100


    });
    Ext.create('jit.biz.WEventAdmin', {
        id: "txtWEventAdmin",
        text: "",
        renderTo: "txtWEventAdmin",
        width: 100
    });

    //Ext.create('jit.biz.WApplicationInterface2', {
    //    id: "txtWeiXinPublic",
    //    text: "",
    //    renderTo: "txtWeiXinPublic",
    //    width: 100
    //        , fn: function () {
    //            //                Ext.getCmp("txtModelId").setDefaultValue("");
    //            Ext.Ajax.request({
    //                url: "/Module/WEvents/Handler/EventsHandler.ashx?method=get_events_by_id",
    //                params: { EventID: getUrlParam("EventID") },
    //                method: 'POST', sync: true, async: false,
    //                success: function (response) {
    //                    //var d = Ext.decode(response.responseText).topics;
    //                    //if (d != null) {
    //                    //    Ext.getCmp("txtModelId").jitSetValue(getStr(d.ModelId));
    //                    //}
    //                    var d = Ext.decode(response.responseText).topics;
    //                    if (d != null) {
    //                        //Ext.getCmp("txtModelId").setDefaultValue(getStr(d.ModelId));
    //                        //                            Ext.getCmp("txtModelId").fnLoad(function () {
    //                        //                               if (Ext.getCmp("txtWeiXinPublic").jitGetValue() != "")
    //                        //                                  Ext.getCmp("txtModelId").jitSetValue(getStr(d.ModelId));
    //                        //                               else
    //                        //                                Ext.getCmp("txtModelId").jitSetValue("");
    //                        //                            });
    //                        Ext.getCmp("txtWXCode").fnLoad(function () {
    //                            if (Ext.getCmp("txtWeiXinPublic").jitGetValue() != "")
    //                                Ext.getCmp("txtWXCode").jitSetValue(getStr(d.QRCodeTypeId));
    //                            else
    //                                Ext.getCmp("txtWXCode").jitSetValue("");
    //                        });
    //                    }
    //                },
    //                failure: function () {
    //                    Ext.Msg.alert("提示", "获取参数数据失败");
    //                }
    //            });
    //        }
    //        , listeners: {
    //            select: function (store) {
    //                //Ext.Ajax.request({
    //                //    url: "/Module/WApplication/Handler/WApplicationHandler.ashx?method=search_wapplication",
    //                //    params: { form: "{ \"WeiXinID\":\"" + Ext.getCmp("txtWeiXinPublic").jitGetValue() + "\" }" },
    //                //    method: 'POST',
    //                //    sync: true, async: false,
    //                //    success: function (response) {
    //                //        var d = Ext.decode(response.responseText).data;
    //                //        if (d != null) {
    //                //            //Ext.getCmp("txtModelId").setDefaultValue(getStr(d.ModelId));
    //                //        }
    //                //    },
    //                //    failure: function () {
    //                //        Ext.Msg.alert("提示", "获取参数数据失败");
    //                //    }
    //                //});
    //                //Ext.getCmp("txtModelId").setDefaultValue("");
    //                Ext.getCmp("txtWXCode").setDefaultValue("");
    //            }
    //        }
    //});
    Ext.create('Jit.form.field.ComboBox', {
        id: "txtFlag",
        multiSelect: true,
        renderTo: "cmbFlag",
        valueField: "Id",
        displayField: "Name",
        store: Ext.getStore("FlagStore"),
        jitSize: 'small',
        isDefault: true,
        width: 100,
        listeners: {
            change: selectFlag
        }

    });


    Ext.create('Jit.form.field.ComboBox', {
        id: "txtModelId",
        renderTo: "txtModelId",
        valueField: "Id",
        displayField: "Name",
        store: Ext.getStore("ModelStore"),
        jitSize: 'small',
        isDefault: true,
        listeners: {
            select: selectModel
        }
    });

    Ext.create('Jit.form.field.ComboBox', {
        id: "cmbPersonCount",
        renderTo: "txtPersonCount",
        store: Ext.getStore("PersonCountStore"),
        valueField: "Id",
        displayField: "Name",
        width: 100,
        isDefault: true
    });



    Ext.create('Jit.form.field.ComboBox', {
        id: "cmbDrawMethod",
        renderTo: "txtDrawMethod",
        store: Ext.getStore("drawMethodStore"),
        valueField: "DrawMethodID",
        displayField: "DrawMethodName",
        width: 100,
        isDefault: true
    });



    //Ext.create('Jit.form.field.Number', {
    //    id: "txtCanSignUpCount",
    //    renderTo: "txtCanSignUpCount",
    //    width: 100
    //});

    Ext.create('Jit.form.field.Number', {
        id: "txtRange",
        renderTo: "txtRange",
        width: 100,
        value: '1',
        maxValue: 99,
        minValue: 0
    });

    Ext.create('Jit.form.field.Text', {
        id: "txtLongitude",
        text: "",
        renderTo: "txtLongitude",
        width: 100
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtLatitude",
        text: "",
        renderTo: "txtLatitude",
        width: 100
    });
    Ext.create('jit.biz.EventStatus', {
        id: "txtEventStatus",
        text: "",
        renderTo: "txtEventStatus",
        width: 100
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtDisplayIndex",
        text: "",
        renderTo: "txtDisplayIndex",
        width: 100
    });
    //Ext.create('Jit.form.field.Number', {
    //    id: "txtPersonCount",
    //    value: "0",
    //    renderTo: "txtPersonCount",
    //    width: 100
    //});
    Ext.create('Jit.form.field.Text', {
        id: "txtWXCode2",
        text: "",
        renderTo: "txtWXCode2",
        readOnly: true,
        width: 100
    });
    Ext.create('Jit.button.Button', {
        text: "获取二维码",
        renderTo: "btnWXImage",
        handler: function () {
            fnGetWXCode();
        }
    });
    Ext.create('Jit.form.field.Text', {
        id: "txtDimensionalCodeURL",
        text: "",
        renderTo: "txtDimensionalCodeURL",
        readOnly: true,
        width: 330
    });
    //Ext.create('jit.biz.WQRCodeType', {
    //    id: "txtWXCode",
    //    text: "",
    //    renderTo: "txtWXCode",
    //    width: 100,
    //    c: true,
    //    parent_id: "txtWeiXinPublic"
    //});

    var content = new Ext.form.TextArea({
        height: 10,
        id: 'txtContent',
        renderTo: "txtContent",
        anchor: '80%',
        listeners: {
            "render": function (f) {
                K = KindEditor;
                htmlEditor = K.create('#txtContent', {
                    uploadJson: '/Framework/Javascript/Other/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Framework/Javascript/Other/kindeditor/asp.net/file_manager_json.ashx',
                    height: 270,
                    width: '100%'
                });
            }
        }
    });


    //operator area
    Ext.create('Ext.form.Panel', {
        title: null,
        renderTo: "divBtn",
        id: "editBtnPanel",
        width: "100%",
        height: "100%",
        border: 1,
        layout: {
            type: 'table',
            columns: 3,
            align: 'right'
        },
        defaults: {},
        items: [],
        buttonAlign: "left",
        buttons: [{
            xtype: "jitbutton",
            id: "btnSave",
            text: "保存",
            formBind: true,
            disabled: true,
            hidden: false,
            handler: fnSave
            , jitIsHighlight: true
            , jitIsDefaultCSS: true
        },
        {
            xtype: "jitbutton",
            text: "关闭",
            handler: fnClose
        }]
    });
}