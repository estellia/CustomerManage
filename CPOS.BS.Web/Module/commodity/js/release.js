﻿define(['jquery', 'template', 'tools', 'langzh_CN', 'easyui', 'artDialog', 'kkpager', 'kindeditor', 'noviceGuideJS'], function ($) {


    //上传图片
    KE = KindEditor;
    /*///两种调用方式
        var template1="我是{0}，今年{1}了";
        var template2="我是{name}，今年{age}了";
        var result1=template1.format("loogn",22);
        var result2=template2.format({name:"loogn",age:22});
     /////两个结果都是"我是loogn，今年22了"*/
    String.prototype.format = function (args) {
        var result = this;
        if (arguments.length > 0) {
            if (arguments.length == 1 && typeof (args) == "object") {
                for (var key in args) {
                    if (args[key] != undefined) {
                        var reg = new RegExp("({" + key + "})", "g");
                        result = result.replace(reg, args[key]);
                    }
                }
            }
            else {
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i] != undefined) {
                        var reg = new RegExp("({[" + i + "]})", "g");
                        result = result.replace(reg, arguments[i]);
                    }
                }
            }
        }
        return result;
    };


    var page = {
        elems: {
            optPanel: $("#optPanel"), //页面的顶部li
            submitBtn: $("#submitBtn"),   //提交和下一步按钮
            section: $("#section"),    //
            editLayer: $("#editLayer"), //图片上传
            simpleQuery: $("#simpleQuery"),  //全部
            skuTable: $("#skuTable"),
            width: 200,
            height: 30,
            panlH: 200,
            sku: $("#sku"),
            loadAlert: null,
            priceFilde: "item_price_type_name_",//数据库价格相关的字段，一般有销量库存，价格实体价格
            allData: {} //页面所有存放对象基础数据
        },
        init: function () {
            this.distribution();
            this.loadDataPage();
            this.initEvent();
            this.onTouch();
            this.addItem();
            $("#leftMenu li").each(function () {
                $(this).removeClass("on");
                var urlPath = location.pathname.replace(/\//g, "_");
                var classNameList = $(this).find("em").attr("class").split(" ");
                if (classNameList.length > 1) {
                    if (urlPath.indexOf(classNameList[1]) != -1) {
                        $(this).addClass("on");
                    }
                }

            });
        },
        //刷新页面
        onTouch: function () {
            $("#plRefresh").click(function () {
                window.location.href = location.href;
            })
            $("#fzRefresh").click(function () {
                window.location.href = location.href;
            })
        },
        /**************** -------------------配送方式获取 控件 start****************/
        distribution: function () {
            console.log(11)
            var that = this;
            $.ajax({
                url: "/ApplicationInterface/Delivery/DeliveryEntry.ashx?type=Product&action=GetDeliveryList&req={}",
                type: "get",
                dataType: "json",
                success: function (data) {
                    if (data.IsSuccess == true) {
                        var datas = data.Data.DeliveryList;
                        var bkBox = $("#bulkBox")
                        for (var i = 0; i < datas.length ; i++) {
                            var str = '<div class="commonSelectWrap">' +
                                        '<div class="selectBox" style="line-height:16px;">' +
                                            '<div class="checkBox line14" data-name="isBulk" data-value="' + datas[i].deliveryId + '"><em></em><span>' + datas[i].deliveryName + '</span></div>' +
                                         '</div>' +
                                     '</div>';
                            bkBox.prepend(str);
                        }

                    } else {
                        $.messager.alert("提示", data.Message);
                    }
                }
            });
        },
        /**************** -------------------获取虚拟商品信息 控件 start****************/
        getVirtualItems: function () {
            var xnType = $(".xnType");
            var VritualItems = [];
            for (var i = 0; i < xnType.length; i++) {
                var str = {
                    Item_Count: $($(".xnType")[i]).find(".commonSelectWrap").eq(2).find(".easyui-validatebox").val(),
                    VirtualItemTypeId: $($(".xnType")[i]).find(".commonSelectWrap").eq(0).find(".easyui-combobox").combobox('getValue'),
                    ObjecetTypeId: $($(".xnType")[i]).find(".commonSelectWrap").eq(1).find(".easyui-combobox").combobox('getValue')
                }
                VritualItems.push(str)
            }
            return VritualItems;
        },
        /**************** -------------------添加虚拟产品 控件 start****************/
        addItem: function () {
            var that = this;

            $(".addItem").click(function () {
                var i = $(".addItems .xnType").length + 1;
                if (that.VirtualItemTypeInfo && that.VirtualItemTypeInfo.length > 0) {
                    var str = '<div class="xnType">' +
                        '<div class="commonSelectWrap" data-type="Clone">' +
                        '<em class="tit">选择种类：</em>' +
                        '<label class="selectBox">' +
                        '<input data-text="选择种类" class="itemType easyui-combobox" id="itemType' + i + '" data-flag="Item_Name"  data-options="required:true,width:200,height:30" name="VirtualItemTypeId" type="text" value="">' +
                        '</label>' +
                        '</div>' +
                        '<div class="commonSelectWrap">' +
                        '<em class="tit">内容：</em>' +
                        '<label class="selectBox">' +
                        '<input data-text="商品名称" id="ObjecetType' + i + '" data-flag="Item_Name"  class="easyui-combobox" data-options="required:true,width:200,height:30" name="ObjecetTypeId" type="text" value="请选择">' +
                        '</label>' +
                        '</div>' +
                        '<div class="commonSelectWrap">' +
                        '<em class="tit">数量：</em>' +
                        '<label class="selectBox">' +
                        '<input data-text="商品名称" id="mathType" data-flag="Item_Math"  class="easyui-numberbox" data-options="required:true,width:200,height:30,precision:0" name="mathTypeId" type="text" value="1">' +
                        '<em class="text">张</em>' +
                        '</label>' +
                        '</div>' +
                        '<div class="commonSelectWrap"><em class="tit deleteItem">删除</em></div>' +
                        '</div>';
                    $(".addItems").append(str);
                    that.deletItem();
                    if ($(".xnType").length > 1) {
                        $(".deleteItem").css({"background-color": "#00a0e8"})
                    }

                    var wd = 200, H = 30;
                    var xnTypeDom = $(".xnType:last");

                    /* that.VirtualItemTypeInfo.push({ "VirtualItemTypeId": -1, VirtualItemTypeName: "请选择", selected: true })*/
                    xnTypeDom.find(".commonSelectWrap").eq(2).find(".easyui-validatebox").validatebox({});
                    xnTypeDom.find(".commonSelectWrap").eq(1).find(".easyui-combobox").combobox({});
                    xnTypeDom.find(".commonSelectWrap").eq(0).find(".easyui-combobox").combobox({
                        width: wd,
                        height: H,
                        panelHeight: that.elems.panlH,
                        valueField: 'VirtualItemTypeId',
                        textField: 'VirtualItemTypeName',
                        data: that.VirtualItemTypeInfo,
                        onSelect: function (record) {
                            //

                            var cmbObjectType = xnTypeDom.find(".commonSelectWrap").eq(1).find(".easyui-combobox");

                            cmbObjectType.combobox({data: []});

                            if (record.VirtualItemTypeId == -1) {
                                cmbObjectType.combobox("setValue", "请选择");
                                return;
                            }

                            cmbObjectType.combobox("setValue", "请选择");

                            var selectData = record.VirtualItemTypeCode;
                            $.util.isLoading()
                            if (selectData == "VipCard") {
                                that.loadData.getSysVipCardTypeList(function (cardData) {
                                    if (cardData.Data.SysVipCardTypeList && cardData.Data.SysVipCardTypeList.length > 0) {
                                        cardData.Data.SysVipCardTypeList[0]["selected"] = true;
                                        cmbObjectType.combobox({
                                            width: wd,
                                            height: H,
                                            panelHeight: that.elems.panlH,
                                            valueField: 'VipCardTypeID',
                                            textField: 'VipCardTypeName',
                                            data: cardData.Data.SysVipCardTypeList
                                        });
                                    } else {
                                        console.log("未创建任何类型的卡")
                                    }

                                });

                            } else if (selectData == "Coupon") {

                                that.loadData.getCouponTypeList(function (couponData) {
                                    //
                                    if (couponData.Data.CouponTypeList && couponData.Data.CouponTypeList.length > 0) {
                                        couponData.Data.CouponTypeList[0]["selected"] = true;
                                        cmbObjectType.combobox({
                                            width: wd,
                                            height: H,
                                            panelHeight: that.elems.panlH,
                                            valueField: 'CouponTypeID',
                                            textField: 'CouponTypeName',
                                            data: couponData.Data.CouponTypeList
                                        });
                                    } else {
                                        console.log("我创建任何优惠券类型")
                                    }

                                });
                            }

                        }

                    });

                } else {

                    that.loadData.getItemType(function (data) {
                        //
                        that.VirtualItemTypeInfo = data.Data.VirtualItemTypeInfo;
                        that.VirtualItemTypeInfo.push({
                            "VirtualItemTypeId": -1,
                            VirtualItemTypeName: "请选择",
                            selected: true
                        })

                    });
                }

            });

        },
        /**************** -------------------删除虚拟产品 控件 start****************/
        deletItem: function () {


            $(".deleteItem").click(function () {
                if ($(".xnType").length > 1) {
                    $(this).parent().parent().remove();
                } else{
                    $(".deleteItem").css({"background-color":"#ccc"})
                }
            });

        },
        //动态创建页面元素
        createDom: function (data) {
            var that = this;
            //
            var html = bd.template("tpl_commodityForm", data);
            $("#nav02").html(html);
            if (data && data.Children) {
                for (var i = 0; i < data.Children.length; i++) {
                    that.drawDom(data.Children[i].Prop_Input_Flag);
                }
            }

        },
        createSellDom: function (data) {
            var html = bd.template("tpl_commoditySellForm", data);
            $("#textList").html(html);
            $("#textList").find(".load .easyui-numberbox").numberbox({ width: 80, max: 1000000000 });  //初始化控件。；
        },

        drawDom: function (objType, name) {
            var that = this;
            switch (objType) {
                case "htmltextarea":
                    $(".info").css({ width: $("#optPanel").width() + "px", height: "480px" });
                    that.elems.editor = KE.create('.info', {
                        allowFileManager: true,
                        fileManagerJson: "/Framework/Javascript/Other/kindeditor/asp.net/file_manager_json.ashx", //打开图片空间路径
                        uploadJson: "/Framework/Javascript/Other/kindeditor/asp.net/upload_homepage_json.ashx?width=600" //上传图片
                    });
                    break;
            }

        },

        initEvent: function () {
            var that = this;
            if ($.util.getUrlParam("stepIndex") != null && $.util.getUrlParam("GuId") != null) {
                var stepIndex = parseInt($.util.getUrlParam("stepIndex"));
                var GuId = $.util.getUrlParam("GuId");

                that.GetModuleNextStep({
                    "action": 'Basic.Guide.GetModuleNextStep',
                    "ModuleCode": "CloudShop",
                    "ParentModule": GuId,//"BAB72601-4801-4403-B97F-F97CC0B9A564",
                    "Step": stepIndex,

                }, function (data) {

                    console.log(data.NowModule.VideoUrl);
                    if (data.NowModule) {
                        var step1 = new noviceGuide("step1", {
                            top: 200,
                            left: 350,
                            isShow: true,
                            title: "发布商品",
                            content: "编辑商品基础和详情信息</br>对应商品规格，设置销售价格和库存",
                            step: data.NowModule.ModuleStep,
                            sumStep: 9,
                            video: {
                                top: 200,
                                left: 700,
                                src: "http://guide.chainclouds.cn" + data.NowModule.VideoUrl// "../static/jkb/movie/sczx.mp4",
                            },
                            direction: {
                                angle: -160,
                                top: "10",
                                left: "50"
                            }
                        }, function () {
                            window.location.href = data.PreModule.Url + "#stepIndex=" + data.PreModule.ModuleStep + "&GuId=" + data.PreModule.ParentModule;
                        }, function () {
                            that.SetModuleLastStep({
                                "action": 'Basic.Guide.SetModuleLastStep',
                                "ModuleCode": "CloudShop",
                                "ParentModule": "BAB72601-4801-4403-B97F-F97CC0B9A564",
                                "Step": stepIndex,
                                "Url": data.NowModule.Url,
                                "FinishedStatus": 0
                            }, function () {
                                window.location.href = data.NextModule.Url + "#stepIndex=" + data.NextModule.ModuleStep + "&GuId=" + data.NextModule.ParentModule;
                            });

                        });
                    }

                });
            };
            that.elems.simpleQuery.find(".panelDiv").fadeOut(0).eq(0).fadeIn("slow");
            $("#nav0_1").delegate(".radio", "click", function () {
                var me = $(this), name = $(this).data("name");
                $("[data-name=" + name + "]").removeClass("on");
                me.addClass('on');
                if (me.data("value") == 1) {
                    $("[data-flag=" + name + "]").show();
                    $("#ObjecetType").combobox({
                        novalidate: false
                    });
                    $("#itemType").combobox({
                        novalidate: false
                    })
                    $(".commonBtn.addSKU").hide();
                    $('#bulkBox').hide();
                } else {
                    $("[data-flag=" + name + "]").hide();
                    $("#ObjecetType").combobox({
                        novalidate: true
                    });
                    $("#itemType").combobox({
                        novalidate: true
                    });
                    $(".commonBtn.addSKU").show();
                    $('#bulkBox').show();
                }
            });


            $("#nav0_1").delegate(".checkBox", "click", function () {
                var me = $(this);
                if (!me.hasClass('on')) {
                    me.addClass('on');
                } else {
                    me.removeClass('on');
                }
            });

            /**************** -------------------初始化easyui 控件 start****************/
            var wd = 200, H = 30;
            $('[data-name="ifservice"]').eq(0).trigger("click");
            $.util.isLoading()
            that.loadData.getItemType(function (data) {
                //

                if (data.Data.VirtualItemTypeInfo && data.Data.VirtualItemTypeInfo.length > 0) {
                    data.Data.VirtualItemTypeInfo.push({ "VirtualItemTypeId": -1, VirtualItemTypeName: "请选择", selected: true })
                   that.VirtualItemTypeInfo= data.Data.VirtualItemTypeInfo;
                    $(".itemType").combobox({
                        width: wd,
                        height: H,
                        panelHeight: that.elems.panlH,
                        valueField: 'VirtualItemTypeId',
                        textField: 'VirtualItemTypeName',
                        data: data.Data.VirtualItemTypeInfo,
                        onSelect: function (record) {

                            $("#ObjecetType").combobox({ data: [] });
                            if (record.VirtualItemTypeId == -1) {
                                $("#ObjecetType").combobox("setValue", "请选择");
                            } else {
                                $.util.isLoading();
                                $("#ObjecetType").combobox("setValue", "请选择");
                                var selectData = record.VirtualItemTypeCode;
                                if (selectData == "VipCard") {
                                    that.loadData.getSysVipCardTypeList(function (cardData) {
                                        if (cardData.Data.SysVipCardTypeList && cardData.Data.SysVipCardTypeList.length > 0) {
                                            cardData.Data.SysVipCardTypeList[0]["selected"] = true;
                                            $("#ObjecetType").combobox({
                                                width: wd,
                                                height: H,
                                                panelHeight: that.elems.panlH,
                                                valueField: 'VipCardTypeID',
                                                textField: 'VipCardTypeName',
                                                data: cardData.Data.SysVipCardTypeList
                                            });
                                        } else {
                                            console.log("未创建任何类型的卡")
                                        }

                                    });

                                } else if (selectData == "Coupon") {

                                    that.loadData.getCouponTypeList(function (couponData) {
                                        //
                                        if (couponData.Data.CouponTypeList && couponData.Data.CouponTypeList.length > 0) {
                                            couponData.Data.CouponTypeList[0]["selected"] = true;
                                            $("#ObjecetType").combobox({
                                                width: wd,
                                                height: H,
                                                panelHeight: that.elems.panlH,
                                                valueField: 'CouponTypeID',
                                                textField: 'CouponTypeName',
                                                data: couponData.Data.CouponTypeList
                                            });
                                        } else {
                                            console.log("我创建任何优惠券类型")
                                        }

                                    });
                                }
                            }
                        }

                    });
                    $("#itemType").combobox({ "setText": "请选择" });
                    $(".tooltip").remove();//验证提示会一直显示问题
                } else {
                    $('[data-name="ifservice"]').eq(1).remove();
                    $(".tooltip").remove()
                }

            });
            that.loadData.getClassify(function (data) {
                debugger;
                $('#Category').combotree({
                    width: wd,
                    height: H,
                    panelHeight: that.elems.panlH,
                    lines: true,
                    valueField: 'id',
                    textField: 'text',
                    data: data[1].children
                });
                $('#Category').combotree('setText', "请选择");
            });
            // 获取促销分组
            that.loadData.getClassifySeach.bat_id = "2"; //促销分组  1代表品类
            that.loadData.getClassifySeach.isAddPleaseSelectItem = false;
            that.loadData.getClassify(function (data) {
                debugger;
                $('#ItemCategoryId').combotree({
                    width: wd,
                    height: H,
                    panelHeight: that.elems.panlH,

                    animate: true,
                    multiple: true,
                    valueField: 'id',
                    textField: 'text',
                    data: data
                });
            });
            /**************** -------------------初始化easyui 控件  End****************/
            that.elems.optPanel.delegate("li", "click", function (e) {
                //
                var me = $(this), flag = $(this).data("flag"), issubmit = true;
                if (flag != "#nav01") {
                    issubmit = $("#nav0_1").form('validate');
                    if (issubmit && $('[data-name="ifservice"]').eq(1).hasClass("on")) {
                        var itemTypeValue = $("#itemType").combobox("getValue");
                        var objectIdValue = $("#ObjecetType").combobox("getValue");
                        if (itemTypeValue == "-1" || itemTypeValue == "请选择") {
                            issubmit = false;
                            alert("选择种类填写无效，请重新选择");
                            return false;
                        }
                        if (objectIdValue == "-1" || objectIdValue == "请选择") {
                            issubmit = false;
                            alert("关联填写无效，请重新选择");
                            return false;
                        }


                    }
                    if (issubmit) {
                        var nodes = $('#Category').combotree('tree').tree('getSelected');	// 获取树对象   	// 获取选择的节点
                        if (!nodes) {
                            issubmit = false;
                            alert("商品品类填写无效，请重新选择");
                            return false;
                        }

                        nodes = $('#ItemCategoryId').combotree('tree').tree('getSelected');
                        if (nodes && nodes.length > 0) {
                            issubmit = false;
                            alert("促销分组填写无效，请重新选择");
                            return false;
                        }
                    }
                    if (issubmit && that.elems.editLayer.find(".imglist img").length == 0) {
                        issubmit = false;
                        alert("至少上传一张商品图片");
                    }
                }
                if (issubmit) {
                    that.elems.submitBtn.data("flag", flag);
                    if (me.index() == that.elems.optPanel.find("li").length - 1) {
                        that.elems.submitBtn.html("提交");
                        that.elems.submitBtn.data("issubMit", true);
                    } else {
                        that.elems.submitBtn.html("下一步");
                        that.elems.submitBtn.data("issubMit", false);
                    }
                    that.elems.simpleQuery.find(".panelDiv").fadeOut(0);
                    $(flag).fadeIn("slow");
                    that.elems.optPanel.find("li").removeClass("on");
                    $(this).addClass("on")
                }

            });
            that.elems.section.delegate("#submitBtn", "click", function (e) {
                //
                var index = 0;
                if ($("#dataState").is(':hidden')) {
                    $("#SKUForm").form("disableValidation");//隐藏禁用，
                } else {
                    $("#SKUForm").form("enableValidation"); //显示就启用
                }
                if ($(this).data("issubMit")) {

                    if (that.getAddData() && $("#SKUForm").form("validate")) { //获取添加商品的参数，

                        that.loadData.addCommodity(function (data) {
                            $.messager.confirm("商品操作提示", "商品已添加成功,确定要继续添加商品吗？", function (r) {
                                var mid = JITMethod.getUrlParam("mid");
                                window.notShow = true;
                                if (r) {//location.href = "queryList.aspx?Item_Id=" + rowData.Item_Id + "&mid=" + mid;
                                    //  location.href = "release.aspx?&mid=" + mid;
                                    $.util.toNewUrlPath("release.aspx");
                                } else {
                                    $.util.toNewUrlPath("queryList.aspx");
                                    //  location.href = "queryList.aspx?&mid=" + mid;
                                }
                            });


                        });
                    }


                } else {
                    that.elems.simpleQuery.find(".panelDiv").each(function (e) {
                        //
                        if (!($(this).is(":hidden"))) {
                            index = $(this).data("index") + 1;
                        }

                    });
                    that.elems.optPanel.find("li").eq(index).trigger("click");
                }
                $.util.stopBubble(e);
            });

            //data-flag="CarNumber"

            /**************** -------------------弹出窗口初始化 start****************/
            $('#win').window({
                modal: true,
                shadow: false,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                closed: true,
                closable: true
            });
            $('#panlconent').layout({
                fit: true
            });
            $('#win').delegate(".saveBtn", "click", function (e) {

                if ($('#payOrder').form('validate')) {

                    var fields = $('#payOrder').serializeArray(); //自动序列化表单元素为JSON对象

                    that.loadData.operation(fields, that.elems.optionType, function (data) {

                        alert("操作成功");
                        $('#win').window('close');
                        that.loadPageData(e);

                    });
                }
            });
            /**************** -------------------弹出窗口初始化 end****************/
            $(".skuTable").delegate("p.fontC", "click", function () {
                //
                $("#countRepertory").html("0");
                var me = $(this), index = me.data("index");
                if (!isNaN(parseInt(index))) {
                    that.elems.skuTable.datagrid('acceptChanges');  //缓存修改过以后的数据
                    var gridData = that.elems.skuTable.datagrid('getData');//去缓存的数据


                    gridData.rows[index].bat_id = gridData.rows[index].bat_id == "-1" ? "1" : "-1";
                    that.elems.skuTable.datagrid({ data: gridData });
                    for (var index = 0; index < gridData.rows.length; index++) {
                        that.elems.skuTable.datagrid('beginEdit', index);
                    }
                }
            });

            $("#batch").delegate(".fontC", "click", function () {
                //
                $("#batch").find(".fontC").hide(0);
                $(this).show();
                var oldFontC = $("#batch").find(".mainpanl").parent();
                if (oldFontC.length > 0) {
                    oldFontC.html(oldFontC.data("item").item_price_type_name);
                }
                var me = $(this);
                var data = {};

                data.name = me.data("item").item_price_type_name;
                //
                if (data.name.indexOf("(元)") != -1) {
                    data.name = data.name.substring(0, data.name.indexOf("(元)"));
                }

                data.obj = me.data("item");
                var html = bd.template("tpl_AddBatch", data);
                $(this).html(html);
            }).delegate("input", "click", function (e) {
                $.util.stopBubble(e);
            }).delegate(".commonBtn", "click", function (e) {
                $("#batch").find(".fontC").show();
                var me = $(this);
                //
                var item = me.parents(".mainpanl").data("item");
                if (item.item_price_type_name.indexOf("(元)") != -1) {
                    item.item_price_type_name = item.item_price_type_name.substring(0, item.item_price_type_name.indexOf("(元)"));
                }
                if (me.data("type") == "save") {
                    $("#countRepertory").html("0")
                    item.price = me.parents(".mainpanl").find("input").val();
                    that.getProData(item);
                    me.parents(".fontC").html(item.item_price_type_name);
                } else if (me.data("type") == "cancel") {
                    $("#batch").find(".fontC").show();
                    //取消
                    me.parents(".fontC").html(item.item_price_type_name);
                }

                $.util.stopBubble(e);
            });

            that.registerUploadImgBtn();
            that.proOperationEvent();
        },

        //获取新手引导步数
        GetModuleNextStep: function (datas, callback) {
            var that = this;
            $.util.ajax({
                url: "/ApplicationInterface/Gateway.ashx",
                data: datas,
                success: function (data) {
                    if (data.IsSuccess && data.ResultCode == 0) {
                        //  $.util.isLoading(true);
                        if (callback) {
                            callback(data.Data);
                        }
                    } else {
                        //$.util.isLoading(true);
                        $.messager.alert("提示", data.Message);
                    }
                }
            });
        },
        //记录模块的最后一步
        SetModuleLastStep: function (datas, callback) {
            var that = this;
            $.util.ajax({
                url: "/ApplicationInterface/Gateway.ashx",
                data: datas,
                success: function (data) {
                    console.log(data);
                    if (data.IsSuccess && data.ResultCode == 0) {
                        //  $.util.isLoading(true);
                        if (callback) {
                            callback(data.Data);
                        }
                    } else {
                        //$.util.isLoading(true);
                        $.messager.alert("提示", data.Message);
                    }
                }
            });
        },
        //创建批量操作按钮
        optionPriceList: function () {
            var that = this;
            $("#batch").find("#option").html("");
            $.each(that.elems.allData.ItemPriceTypeList, function (index, Item) {

                if (Item.item_price_type_name != "销量") {
                    var obj = { item_price_type_name: Item.item_price_type_name, item_price_type_id: Item.item_price_type_id };
                    if (Item.item_price_type_name.indexOf("(元)") != -1) {
                        Item.item_price_type_name = Item.item_price_type_name.substring(0, Item.item_price_type_name.indexOf("(元)"));
                    }
                    var html = "<div data-id={0} data-item={1} class='fontC'>{2}</div>".format(Item.item_price_type_id, JSON.stringify(obj), Item.item_price_type_name);
                    $("#batch").find("#option").append(html)
                }

            });

        },


        //初始化加载的页面数据
        loadDataPage: function () {
            var that = this;
            that.loadData.getItemBaseData(function (data) {
                if (data.Data) {

                    that.elems.allData.ItemPropGroupList = data.Data.ItemPropGroupList; //商品属性组 商品详细信息的基础信息
                    that.elems.allData.SKUPropList = data.Data.SKUPropList; //Sku属性列表   sku属性的基础信息
                    that.elems.allData.ItemPriceTypeList = data.Data.ItemPriceTypeList; //商品价格类型库存列表

                    that.elems.allData.barcode = { filed: "barcode", title: "商品条码", Prop_Input_Flag: "text" }; //商品条码
                    that.createDom(that.elems.allData.ItemPropGroupList[0]);
                    that.createSellDom(data);
                    that.optionPriceList();
                } else {
                    console.log("基础数据加载不完善");
                }
            })


        },


        /*  mergeCellsByField()在DataGrid的onLoadSuccess中调用。
         * EasyUI DataGrid根据字段动态合并单元格
         * @param tableID 要合并table的id
         * @param colList 要合并的列,用逗号分隔(例如："name,department,office");*/
        //合并相同的值的属性行
        mergeCellsByField: function (table, colList) {


            var ColArray = colList.split(",");
            var tTable = table;
            var TableRowCnts = tTable.datagrid("getRows").length;
            var tmpA;
            var tmpB;
            var PerTxt = "";
            var CurTxt = "";
            var alertStr = "";
            //for (j=0;j<=ColArray.length-1 ;j++ )
            for (j = ColArray.length - 1; j >= 0 ; j--) {
                //当循环至某新的列时，变量复位。
                PerTxt = "";
                tmpA = 1;
                tmpB = 0;

                //从第一行（表头为第0行）开始循环，循环至行尾(溢出一位)
                for (i = 0; i <= TableRowCnts ; i++) {
                    if (i == TableRowCnts) {
                        CurTxt = "";
                    }
                    else {
                        CurTxt = tTable.datagrid("getRows")[i][ColArray[j]];
                    }
                    if (PerTxt == CurTxt) {
                        tmpA += 1;
                    }
                    else {
                        tmpB += tmpA;
                        tTable.datagrid('mergeCells', {
                            index: i - tmpA,
                            field: ColArray[j],
                            rowspan: tmpA,
                            colspan: null
                        });
                        tmpA = 1;
                    }
                    PerTxt = CurTxt;
                }
            }
        },

        //规格的操作事件绑定
        proOperationEvent: function () {
            var that = this;
            that.elems.sku.delegate(".icon", "click", function () {   //删除操作按钮操作

                var me = $(this);
                if (me.parent().hasClass('pro')) {
                    me.parents(".skuList").remove();
                    if (that.elems.sku.find(".skuList").length == 0) {
                        $("#dataState").fadeIn();
                    }
                }
                me.parent().remove();
                that.getProData();

            }).delegate(".pro,.btn", "mouseenter", function () {  //停靠显示和隐藏删除按钮
                $(this).find(".icon").show(0)
            }).delegate(".pro,.btn", "mouseleave", function () {
                $(this).find(".icon").hide(0)
            }).delegate(".addSKU", "click", function (data) {  //添加sku品类事件
                $("#dataState").fadeOut(10);
                var long = that.elems.sku.find(".skuList").length;
                if (long >= 2) {
                    alert("最多只能添加2个规格");
                    return;
                }
                if (that.elems.allData.SKUPropList.length == long && !data) {
                    alert("没有新的规格可供添加");
                    return;
                }
                var html = bd.template("tpl_AddPro");
                that.elems.sku.append(html);
                that.proAdd(long);


            }).delegate(".fontC", "click", function () {   //添加
                if ($(this).data("type") == "add") {
                    var id = $(this).parent().siblings().find(".proList").combobox("getValue"); //获取选择的规格。
                    var row = $(this).parents(".skuList").data("row");
                    if (!id || id == "0") {
                        alert("请选择一个规格");
                    } else {
                        if ($("#proDetail").length > 0) {
                            $("#proDetail").parents(".fontC").data("type", "add");
                            $("#proDetail").parents(".fontC").html("+添加");
                        }

                        var html = bd.template("tpl_AddProDetail");
                        $(this).html(html);
                        $("#proDetail").combobox({    //添加规格值；
                            width: that.elems.width,
                            height: that.elems.height,
                            panelHeight: that.elems.panlH,
                            valueField: 'Prop_Id',
                            textField: 'Prop_Name',
                            data: row.Children
                        });
                        $(this).data("type", "save");

                    }



                }

            }).delegate(".proDetailSave", "click", function (e) {   //确定
                var nondParent = $(this).parents(".fontC");
                var text = $("#proDetail").combobox("getText");
                if (nondParent.data("type") == "save") {
                    if (text && text != "请选择") {
                        var isAdd = true, isServerAdd = true;   //isAdd控制是否增加规格；isServerAdd 控制是否增加
                        var btnLitDiv = $(this).parents(".fontC").siblings(".proValue");
                        var btnList = btnLitDiv.find(".btn");//获取已经添加的
                        btnList.each(function (i, val) {
                            if ($(val).data("id") == $("#proDetail").combobox("getValue")) {
                                isAdd = false;
                                return false;//结束整个循环
                            }
                        });

                        for (var i = 0; i < $("#proDetail").combobox("getData").length; i++) {
                            var value = $("#proDetail").combobox("getData")[i];
                            if (value.Prop_Name == $("#proDetail").combobox("getText")) {
                                isServerAdd = false;
                                break;
                            }
                        }

                        if (isServerAdd) {
                            var id = nondParent.parent().siblings().find(".proList").combobox("getValue");

                            that.loadData.addSkuValue.parent_prop_id = id;
                            that.loadData.addSkuValue.prop_name = text;
                            that.loadData.SaveSkuValue(function (data) {
                                that.loadData.getItemBaseData(function (data) {
                                    if (data.Data) {
                                        that.elems.allData.SKUPropList = data.Data.SKUPropList; //Sku属性列表
                                        nondParent.parent().siblings().find(".proList").combobox({ data: that.elems.allData.SKUPropList });//   sku属性的基础信息
                                        nondParent.parent().siblings().find(".proList").combobox("setValue", id);
                                        var record = "";
                                        for (var i = 0; i < that.elems.allData.SKUPropList.length; i++) {
                                            if (id == that.elems.allData.SKUPropList[i].prop_id) {
                                                record = that.elems.allData.SKUPropList[i];
                                            }
                                        }
                                        if (record && record.Children && record.Children.length > 0) {
                                            nondParent.parent().siblings().find(".proList").parents(".skuList").data("row", record);
                                            $("#proDetail").combobox({    //添加规格值；
                                                width: that.elems.width,
                                                height: that.elems.height,
                                                panelHeight: that.elems.panlH,
                                                valueField: 'Prop_Id',
                                                textField: 'Prop_Name',
                                                data: record.Children
                                            });
                                            for (var i = 0; i < record.Children.length; i++) {
                                                if (text == record.Children[i].Prop_Name) {
                                                    $("#proDetail").combobox("setValue", record.Children[i].Prop_Id);
                                                }
                                            }
                                        }
                                        var item = { id: $("#proDetail").combobox("getValue"), name: text };
                                        var html = bd.template("tpl_AddBtn", item);
                                        btnLitDiv.append(html);
                                        nondParent.data("type", "add");
                                        nondParent.html("+添加");
                                        that.getProData();
                                    }

                                });

                            });

                        } else {
                            if (isAdd) {
                                var item = { id: $("#proDetail").combobox("getValue"), name: text };
                                var html = bd.template("tpl_AddBtn", item);
                                btnLitDiv.append(html);
                                nondParent.data("type", "add");
                                nondParent.html("+添加");
                                that.getProData();
                            } else {
                                alert("不可重复添加");
                            }
                        }
                    } else {
                        alert("必须选择一个规格值");
                    }
                }

                $.util.stopBubble(e);


            }).delegate(".proDetailCancel", "click", function (e) {
                var nondParent = $(this).parents(".fontC");
                nondParent.data("type", "add");
                nondParent.html("+添加");
                $.util.stopBubble(e);
            });

        },

        //计算总库存


        //绘制表格
        getProData: function (items) {
            $("#countRepertory").html("0");
            var logs = 1;
            var table = { options: {}, data: {} };
            var nodeData = {
                "total": 1,//总页数
                "rows": [
                   /*{"code":"001","name":"Name 1","addr":"Address 11","col4":"col4 data"},*/
                ]
            }
            var that = this;
            that.elems.Fields = "";
            var colLIst = "[[{field:'',title:'默认',hidden:true}]]";
            table.options.columns = eval(colLIst);
            /*var rowData='"{0}":"{1}","{2}":"{3}","{4}":"{5}","{6}":"{7}"';*/
            var rowData = [], skuLit = that.elems.sku.find(".skuList");
            var T_ItemSkuProp = "";
            for (var k = 0; skuLit.length > k; k++) {
                logs++;
                var val = skuLit.eq(k);
                var filed = val.data("filed");//规格字段名称  prop_1
                var filedName = filed + "_detail_name";//规格值的字段名称
                //{"pro1":"颜色","pro1_id":"pro1_id_001","pro1_detailName":"白色","shopcode":"","pice":"", "originalPice":"","repertory":""} 数据
                var title = val.find(".proList").combobox("getText");
                var row = val.data("row");
                if (row && filed) {
                    //添加jQuery 的列
                    var me, str, btnList = val.find(".proValue").find(".btn");
                    if (btnList.length > 0) {
                        table.options.columns[0].push(
                            {
                                field: filedName, title: title, width: title.length * 20
                            }
                        );
                    }
                    that.elems.Fields += filedName + ",";
                    //创建的数据对象字符串

                    if (rowData.length > 0) {
                        var oldRowData = [];
                        oldRowData = rowData;
                        if (btnList.length > 0) {
                            rowData = [];
                            for (var i = 0; i < oldRowData.length; i++) {
                                logs++;
                                var objStr = oldRowData[i];
                                //去除前后{}大括号
                                for (var j = 0; j < btnList.length; j++) {
                                    logs++;
                                    me = btnList.eq(j); str = '{6}"{0}":"{1}","{2}":"{3}","{4}":"{5}",';
                                    console.log(oldRowData[i]);
                                    str = str.format(filedName, me.data("name"), filed + "_detail_id", me.data("id"), filed + "_id", row.prop_id, oldRowData[i]);
                                    rowData.push(str);
                                }
                            }
                        };

                        delete oldRowData;

                    } else { //首次添加数据
                        for (var j = 0; j < btnList.length; j++) {
                            me = btnList.eq(j);
                            str = '"{0}":"{1}","{2}":"{3}","{4}":"{5}",';
                            str = str.format(filedName, me.data("name"), filed + "_detail_id", me.data("id"), filed + "_id", row.prop_id);
                            rowData.push(str)
                        }
                    }
                    T_ItemSkuProp += '"{0}":"{1}",'.format(filed + "_id", row.prop_id);
                }


            }
            that.loadData.addPram.T_ItemSkuProp = JSON.parse('{' + T_ItemSkuProp.substring(0, T_ItemSkuProp.length - 1) + '}');
            //
            //添加商品条码
            var protitle = that.elems.allData.barcode.title;
            table.options.columns[0].push(
                {
                    field: that.elems.allData.barcode.filed, title: protitle, width: 80,
                    editor: {
                        type: 'validatebox',
                        options: {
                            required: true,
                            height: 31,
                            width: 136,
                            validType: 'englishCheckSub'

                        }
                    }

                }
            );

            //构造 sku_price_list 对象数据 字符串
            var sku_price_list = '"sku_price_list" : [{0}]';
            var sku_price_listItem = "";
            str =
               '{"sku_id": "",' +
               '"item_id": "",' +
               '"unit_id": "",' +
               '"price": 0,  ' +
               '"item_price_type_id": "{1}",' +
               '"bat_id": "", ' +
               '"modify_user_id": "",' +
               '"modify_time": "", ' +
               '"customer_id": "", ' +
               '"sku_price_id": "", ' +
               '"sku_price":"{0}",' +
               '"status": "1",' +
               '"create_time": "",' +
               '"create_user_id": "", ' +
               '"if_flag": "0",' +
               '"item_price_type_name": "{2}"}'
               + ",";   //末尾加一个逗号；用于多跳json数据分开,price_list

            var pricefiledList = "";
            $.each(that.elems.allData.ItemPriceTypeList, function (index, Item) {
                logs++;
                var filed = that.elems.priceFilde + index;
                if (Item.item_price_type_name) {
                    if (Item.item_price_type_name == "库存") {
                        table.options.columns[0].push(
                            {
                                field: filed, title: Item.item_price_type_name, width: 60,
                                formatter: function (value, row, rowindex) {
                                    return row.sku_price_list[index].sku_price;
                                }, editor: {
                                    type: 'numberbox',
                                    options: {
                                        min: 1,
                                        required: true,
                                        precision: 0,
                                        height: 31,
                                        width: 136,
                                        onChange: function (newValue, oldValue) {
                                            var number = parseInt($("#countRepertory").html());
                                            newValue = parseInt(newValue);
                                            oldValue = parseInt(oldValue);
                                            if (isNaN(number)) { number = 0 }
                                            if (isNaN(newValue)) { newValue = 0 }
                                            if (isNaN(oldValue)) { oldValue = 0 }
                                            if ((number - oldValue) < 0) {
                                                number = newValue;
                                            } else {
                                                number = number - oldValue + newValue;
                                            }
                                            $("#countRepertory").html(number)
                                        }
                                        /* prefix:'￥'*/
                                    }
                                }
                            }
                        );

                    } else if (Item.item_price_type_code == "销量") {
                        //预留此处，防止销量做处理

                    } else {
                        table.options.columns[0].push(
                            {
                                field: filed, title: Item.item_price_type_name, width: 60,
                                formatter: function (value, row, rowindex) {
                                    return row.sku_price_list[index].sku_price;
                                }, editor: {
                                    type: 'numberbox',
                                    options: {
                                        min: 0,
                                        required: true,
                                        precision: 2,
                                        height: 31,
                                        width: 136,
                                        validType: 'nonzero'
                                        /* prefix:'￥'*/
                                    }
                                }
                            }
                        );
                    }
                    pricefiledList += '"{0}":"{1}",'.format(filed, 0);
                    //
                    sku_price_listItem += str.format(0, Item.item_price_type_id, Item.item_price_type_name);
                }

            });
            pricefiledList = pricefiledList.substring(0, pricefiledList.length - 1);
            sku_price_list = sku_price_list.format(sku_price_listItem.substring(0, sku_price_listItem.length - 1));//去掉最后一个逗号，让后格式化数据

            //最终数据形成
            var data = '{'
                + '"root":'
                + '['

                + '{0}'

                + ']'
                + '}';
            str = '';
            for (var s = 0; s < rowData.length; s++) {
                var item = rowData[s];
                if (item.length > 1) {
                    //item='{'+item+'}'+'"bat_id":"1"';
                    item += '"{0}":"{1}","{2}":"{3}","{4}":"{5}",{6},{7}'.format("status", "1", "bat_id", "1", that.elems.allData.barcode.filed, "", sku_price_list, pricefiledList);
                    item = '{' + item + '}';
                }
                str += item + ",";

            }
            table.options.columns[0].push(
                {
                    field: "bat_id", title: "操作", width: 40,
                    formatter: function (value, row, rowindex) {
                        if (value == 1) {
                            return '<p class="fontC" data-index="' + rowindex + '">停用</p>';
                        } else {
                            return '<p class="fontC red" data-index="' + rowindex + '" >启用</p>';
                        }
                    }

                }
            );
            data = data.format(str.substring(0, str.length - 1));
            data = JSON.parse(data);

            nodeData.rows = data.root;    // 不可直接data.root 直接等于table.data
            table.data = nodeData.rows;   //初始化数据绑定。
            //console.log(logs);
            //不是新增规格时保存以前修改的数据。
            if (table.options.columns[0].length == that.elems.skuTable.datagrid("getColumnFields").length) {
                that.elems.skuTable.datagrid('acceptChanges');  //缓存修改过以后的数据
                var gridData = that.elems.skuTable.datagrid('getData');//再次去缓存的数据
                if ($('[field="barcode"]').find(".datagrid-editable-input").length > 0) { //验证不通过时的商品条码的数据取出赋值。
                    $('[field="barcode"]').find(".datagrid-editable-input").each(function () {
                        var meDom = $(this);
                        var index = Number(meDom.parents("[datagrid-row-index]").attr("datagrid-row-index"));
                        gridData.rows[index]["barcode"] = meDom.val();
                    })
                }
                if (gridData && gridData.rows.length > 0) { //如果数据已经修改。初始化数据不绑定
                    if (table.data.length > 0) {
                        for (var rowIndex = 0; rowIndex < gridData.rows.length; rowIndex++) {
                            var TableIndex = -1, gridIndex = -1;
                            rowData = gridData.rows[rowIndex];
                            $.each(rowData, function (filedName, filedValue) {
                                if (filedName.indexOf(that.elems.priceFilde) != -1) {
                                    //
                                    var index = parseInt(filedName.substring(that.elems.priceFilde.length, filedName.length))
                                    gridData.rows[rowIndex].sku_price_list[index].price = filedValue;
                                    gridData.rows[rowIndex].sku_price_list[index].sku_price = filedValue;

                                    if (items) {
                                        if (items.item_price_type_id == gridData.rows[rowIndex].sku_price_list[index].item_price_type_id) {
                                            var name = that.elems.priceFilde + index;
                                            gridData.rows[rowIndex][name] = items.price;
                                            gridData.rows[rowIndex].sku_price_list[index].price = items.price;
                                            gridData.rows[rowIndex].sku_price_list[index].sku_price = items.price;
                                        }
                                    }


                                } else {
                                    //排除价相关的问题
                                    //onsole.log( typeof(filedValue)+"名称"+filedName);
                                    for (var rowTIndex = 0; rowTIndex < table.data.length; rowTIndex++) {
                                        var isCopy = true;
                                        
                                        for (var k = 0; skuLit.length > k; k++) {
                                            if (isCopy) {
                                                var val = skuLit.eq(k);
                                                var filed = val.data("filed");//规格字段名称  prop_1
                                                var name = filed + "_detail_name";//规格值的字段名称
                                                //filedNameList.push(name);
                                                if (gridData.rows[rowIndex][name] != table.data[rowTIndex][name]) {
                                                    isCopy = false; break;
                                                } else {
                                                    isCopy = true;
                                                }
                                            }
                                        }
                                        if (isCopy) {
                                            gridIndex = rowIndex;
                                            TableIndex = rowTIndex;
                                            break;
                                        }

                                        /*一列的时候             if (typeof(filedValue) == "string" && filedName.indexOf("_detail_") != -1) { //判定是否是规格字段
                                                         $.each(table.data[rowTIndex], function (tableFiledName, tableFiledValue) {
             
                                                             if (filedValue == tableFiledValue) {     // 规格具体值相等
                                                                 //
                                                                 gridIndex = rowIndex;
                                                                 TableIndex = rowTIndex;
                                                             }
                                                         });
                                                     }*/

                                    }
                                }


                            });
                            //

                            if (gridIndex != -1 && rowIndex != -1) {

                                table.data[TableIndex] = gridData.rows[gridIndex];//字段值的处理
                                //table.data[rowIndex].barcode = gridData.rows[rowIndex].barcode;//商品条码字段处理
                            }
                        }
                    }
                    //每次绑定初始化数据需要对 价格相关字段做特殊处理；
                }
            }


            if (table.data.length > 0) {
                $(".skuTable").show(0);
                that.elems.skuTable.datagrid({
                    method: 'post',
                    iconCls: 'icon-list', //图标
                    singleSelect: true, //多选
                    height: "auto", //高度
                    fitColumns: true, //自动调整各列，用了这个属性，下面各列的宽度值就只是一个比例。
                    striped: true, //奇偶行颜色不同
                    collapsible: true,//可折叠
                    //数据来源
                    data: table.data,
                    columns: table.options.columns,
                    /*sortOrder : 'desc', //倒序
                     remoteSort : true, // 服务器排序*/
                    onLoadSuccess: function (data) {

                        that.mergeCellsByField(that.elems.skuTable, that.elems.Fields);
                    }

                });
                for (var index = 0; index < table.data.length; index++) {
                    that.elems.skuTable.datagrid('beginEdit', index);
                }

            } else {
                $(".skuTable").hide(0);
                that.elems.skuTable.datagrid({
                    data: [],
                    columns: [],
                    height: 0
                });

                that.optionPriceList();
            }


        },
        //新增数据的整合
        getAddData: function () {
            $("#countRepertory").html("0");

            //
            var that = this, isSubmit = true, errorIndex = -1;


            that.elems.skuTable.datagrid('acceptChanges');  //缓存修改过以后的数据
            var gridData = that.elems.skuTable.datagrid('getData'); //去缓存的数据

            for (var index = 0; index < gridData.rows.length; index++) {
                if (!that.elems.skuTable.datagrid('validateRow', index)) {
                    isSubmit = false;
                    errorIndex = index;
                    break;
                }
            }

            if (gridData && gridData.rows.length > 0 && $("#dataState").is(':hidden')) {

                for (var rowIndex = 0; rowIndex < gridData.rows.length; rowIndex++) {
                    //每次绑定初始化数据需要对 价格相关字段做特殊处理；
                    rowData = gridData.rows[rowIndex];
                    $.each(rowData, function (filedName, filedValue) {
                        if (filedName.indexOf(that.elems.priceFilde) != -1) {
                            var index = parseInt(filedName.substring(that.elems.priceFilde.length, filedName.length));
                            if (isNaN(parseInt(filedValue))) {
                                filedValue = 0;
                            }
                            gridData.rows[rowIndex].sku_price_list[index].price = filedValue;
                            gridData.rows[rowIndex].sku_price_list[index].sku_price = filedValue;

                        }
                    });
                }
                for (var index = 0; index < gridData.rows.length; index++) {
                    that.elems.skuTable.datagrid('beginEdit', index);
                }
                //
                that.loadData.addPram.SkuList = gridData.rows;
            } else if (!$("#dataState").is(':hidden')) {
                that.loadData.addPram.SkuList = [{ bat_id: "1", sku_price_list: [] }];
                $("[data-flag='price']").each(function (index, dom) {
                    var me = $(this);
                    if (me.data("type") == 'price') {
                        var obj = me.data("flaginfo");
                        //
                        obj.sku_price = me.val();
                        that.loadData.addPram.SkuList[0].sku_price_list.push(obj)


                    } else {
                        that.loadData.addPram.SkuList[0].barcode = me.val();
                        console.log("商品编码" + me.val());
                    }
                });
            } else {
                isSubmit = false;
                alert("必须填写一组规格值");
            }





            //图片列表信息获取ItemImageList
            that.loadData.addPram.ItemImageList = [];

            /*1.6	ItemPropList   商品与属性关系集合数组里的对象数据
             //PropertyCodeId
             PropertyDetailId
             PropertyCodeValue
             IsEditor*/
            that.loadData.addPram.ItemPropList = [];
            $("[data-flag='details']").each(function (index, dom) {
                var me = $(this), flagInfo = me.data("flaginfo");
                var obj = { PropertyCodeId: flagInfo.Prop_Id, PropertyDetailId: "", PropertyCodeValue: "", IsEditor: false };

                if (me.data("type") == "text") {

                    obj.DisplayIndex = index;
                    obj.ImageURL = $(dom).attr("src");
                    obj.PropertyCodeValue = $(this).val();$('#nav0_1')
                    console.log($(this).val());
                    that.loadData.addPram.ItemPropList.push(obj);
                } else if (me.data("type") == "htmltextarea") {
                    obj.DisplayIndex = index;
                    obj.ImageURL = $(dom).attr("src");
                    obj.IsEditor = true;
                    obj.PropertyCodeValue = that.elems.editor.html();
                    that.loadData.addPram.ItemPropList.push(obj);
                }
            });

            that.elems.editLayer.find(".imglist img").each(function (index, dom) {
                var obj = { ImageId: "", ObjectId: "", ImageURL: "", DisplayIndex: "", Title: "", Description: "" };
                obj.DisplayIndex = index;
                obj.ImageURL = $(dom).attr("src");
                that.loadData.addPram.ItemImageList.push(obj);
            });
            var fields = $('#nav0_1').serializeArray();

            that.loadData.addPram.SalesPromotionList = []
            $.each(fields, function (i, field) {
                debugger;
                if (field.name == "ItemCategoryId") {
                    that.loadData.addPram.SalesPromotionList.push({ ItemCategoryId: field.value })
                } else {
                    if (field.value != "") {
                        that.loadData.addPram[field.name] = field.value; //提交的参数
                    }
                }
            });
            if (errorIndex !== -1) {
                var obj = { index: errorIndex, field: that.elems.allData.barcode.filed };
                var ed = that.elems.skuTable.datagrid('getEditor', obj);
                $(ed.target).focus();

            }

            //添加商品配送类型
            var itemDeliverySettingsArray = [];

            $("#bulkBox .checkBox.on").each(function (i, element) {
                var itemDelivery = {
                    DeliveryId: $(this).attr("data-value")
                };

                itemDeliverySettingsArray.push(itemDelivery);
            });

            that.loadData.addPram.ItemDeliverySettings = itemDeliverySettingsArray;

            //添加虚拟商品
            if ($('[data-name="ifservice"].on').data("value") == "1") {
                that.loadData.addPram.VirtualItems = that.getVirtualItems();
            }

            return isSubmit;
        },


        //添加规格
        proAdd: function (index) {
            //
            var that = this, wd = that.elems.width, H = that.elems.height;
            var skuList = that.elems.sku.find(".skuList");
            var proList = skuList.eq(index).find(".proList");
            $(".proList").combobox({
                width: wd,
                height: H,
                panelHeight: that.elems.panlH,
                valueField: 'prop_id',
                textField: 'prop_name',
                data: that.elems.allData.SKUPropList,
                onSelect: function (record) {
                    $("#proDetail").parents(".fontC").data("type", "add");
                    $("#proDetail").parents(".fontC").html("+添加");

                    var me = $(this), meDome = this;
                    //判断是否已经有添加过的
                    var isUpDate = true;
                    skuList.each(function (i, val) {
                        //
                        var row = $(this).data("row");
                        if (row && record.prop_id && row.prop_id == record.prop_id) {
                            alert("规格已经添加请从新选择");
                            // me.combobox("setValue", 0);//
                            isUpDate = false;
                            return false;
                        }

                    });
                    if (isUpDate) {
                        me.parents(".skuList").data("row", record);
                        me.parents(".skuList").data("filed", "prop_" + index);//每一个规格的具体名称在数据库的字段不一样
                        me.parents(".pro").siblings(".proDetailList").find(".proValue").html("");//清空已经添加的；

                    } else {
                        for (var i = 0; i < skuList.length; i++) {
                            //
                            var dome = skuList.eq(i);
                            var row = dome.data("row");
                            if (row) {
                                dome.find(".proList").combobox('setValue', row.prop_name)
                            } else {
                                dome.find(".proList").combobox('setValue', "");
                            }
                        }
                    }

                }


            });

            for (var index = 0; index < skuList.length; index++) {
                //
                var dome = skuList.eq(index);
                var row = dome.data("row");
                if (row) {
                    dome.find(".proList").combobox('setValue', row.prop_name)
                }
            }
            proList.combobox("showPanel");
        },

        //图片上传按钮绑定
        registerUploadImgBtn: function () {
            var self = this;
            // 注册上传按钮
            self.elems.editLayer.find(".uploadImgBtn").each(function (i, e) {
                self.addUploadImgEvent(e);
            });
        },
        //上传图片区域的各种事件绑定
        addUploadImgEvent: function (e) {
            var self = this;
            var imglist = self.elems.editLayer.find(".imglist");
            //上传图片左侧点击图片显示大图事件绑定
            imglist.delegate("img", "click", function () {
                imglist.find("img").removeClass("on");
                $(this).addClass("on");
                var src = $(this).attr("src");
                $(this).parent().siblings(".imgPanl").find("img").attr("src", src)
                $(this).parent().siblings(".imgPanl").find("img").show();
            });
            //设置默认和删除图片 事件绑定
            self.elems.editLayer.find(".btnPanel").delegate(".commonBtn", "click", function () {
                var me = $(this), imgobj;
                if (me.data("flag") == "default") {
                    var imgobj = self.elems.editLayer.find(".imglist .on");
                    self.elems.editLayer.find(".imglist .on").remove();
                    self.elems.editLayer.find(".imglist").prepend(imgobj);

                } else if (me.data("flag") == "del") {

                    var index = self.elems.editLayer.find(".imglist .on").index();

                    //移除选择的img
                    self.elems.editLayer.find(".imglist .on").remove();
                    //因为移除一个如果index是最后一个 元素个数和索引肯定相等
                    if (self.elems.editLayer.find(".imglist").length == index || index == 0) {
                        imgobj = self.elems.editLayer.find(".imglist img").eq(0);
                    } else {
                        imgobj = self.elems.editLayer.find(".imglist img").eq(index - 1);
                    }
                    //
                    imgobj.addClass("on");
                    var src = imgobj.attr("src");
                    if (src) {
                        me.parents(".imgPanl").find("img").attr("src", src);
                    } else {
                        me.parents(".imgPanl").find("img").attr("src", "");
                        me.parents(".imgPanl").find("img").hide();
                        self.elems.editLayer.find(".btnPanel").fadeOut();

                    }
                }
            });

            //上传图片并显示
            self.uploadImg(e, function (ele, data) {
                $(ele).parent().siblings(".imgPanl").find("img").attr("src", data.file.url);
                $(ele).parent().siblings(".imgPanl").find("img").show();
                $(ele).parent().siblings(".imglist").find("img").removeClass("on");
                $(ele).parent().siblings(".imglist").append('<img class="on" src="' + data.file.url + '" />');
                self.elems.editLayer.find(".imgPanl").hover(function () {
                    if (self.elems.editLayer.find(".imglist img").length > 0) {
                        self.elems.editLayer.find(".btnPanel").fadeIn();
                    }
                }, function () {
                    self.elems.editLayer.find(".btnPanel").fadeOut();
                });
            });

        },
        //上传图片

        uploadImg: function (btn, callback) {
            var uploadbutton = KE.uploadbutton({
                button: btn,
                width: 120,
                //上传的文件类型
                fieldName: 'file',
                //注意后面的参数，dir表示文件类型，width表示缩略图的宽，height表示高
                url: '/Framework/Upload/UploadFile.ashx?method=image',
                afterUpload: function (data) {
                    //
                    if (data.success) {
                        if (callback) {
                            //
                            callback(btn, data);
                        }
                        //取返回值,注意后台设置的key,如果要取原值
                        //取缩略图地址
                        //var thumUrl = KE.formatUrl(data.thumUrl, 'absolute');

                        //取原图地址
                        //var url = KE.formatUrl(data.url, 'absolute');
                    } else {
                        alert(data.msg);
                    }
                },
                afterError: function (str) {
                    alert('自定义错误信息: ' + str);
                }
            });

            uploadbutton.fileBox.change(function (e) {
                if ($(".imglist").find("img").length < 5) {
                    uploadbutton.submit();
                } else {
                    $.messager.alert("提示", "上传图不可超过五张")
                }
            });
        },

        loadData: {
            args: {},
            getClassifySeach: {
                Status: "1",
                node: "root",
                multiSelect: '',
                isAddPleaseSelectItem: 'true',
                pleaseSelectText: "请选择",
                pleaseSelectID: "0",
                bat_id: "1" //1 是商品品类 0是促销分组

            },
            addPram: {
                Item_Id: "",     //商品ID
                Item_Category_Id: "",   //商品品类ID
                Item_Name: "",      //商品名称
                Item_Code: "",     //商品编码
                SalesPromotionList: '', //促销分组[{"ItemCategoryId":"BD10C60F-8AC9-4BB6-BE64-6F196B3D36CB"},{"ItemCategoryId":"BD10C60F-8AC9-4BB6-BE64-6F196B3D36C3"}]
                ItemPropList: '',//商品与属性关系集合
                T_ItemSkuProp: '',//  商品sku名列 (基础数据)
                //ObjecetTypeId: '',// 虚拟商品关联的id
                //VirtualItemTypeId: '',//虚拟商品的种类标识
                SkuList: '',// 商品sku值列表
                ItemImageList: '', //图片信息
                OperationType: 'ADD', //add
                VirtualItems: [],
                ItemDeliverySettings: []
            },
            addSkuValue: {
                parent_prop_id: "",
                prop_name: ''
            },
            /*  operation:function(pram,operationType,callback){
                  var prams={data:{action:""}};
                  prams.url="";
                  //根据不同的操作 设置不懂请求路径和 方法
                  prams.data.OrderID=this.tag.orderID;
                  switch(operationType){
                      case "pay":prams.data.action="SetOrderPayment";  //收款
                          break;
                      case "cancel":prams.data.action="SetOrderCancel";  //收款
                          break;
                  }
                  var gridData=$("#coupon").length>0?$("#coupon").combogrid("grid").datagrid('getSelected'):null;
                  $.each(pram, function(i, field) {
                      //
                      if (field.value != "") {
                          prams.data[field.name] = field.value; //提交的参数
                      }
                      if(field.name=="CouponID"&&gridData){
                          prams.data[field.name] =gridData.CouponID;
                      }
  
                  });
                  $.util.ajax({
                      url: prams.url,
                      data:prams.data,
                      success: function (data) {
                          if (data.IsSuccess && data.ResultCode == 0) {
                              if (callback) {
                                  callback(data);
                              }
  
                          } else {
                              alert(data.Message);
                          }
                      }
                  });
              },*/

            getItemBaseData: function (callback) {
                $.util.ajax({
                    url: " /ApplicationInterface/Module/Item/ItemNewHandler.ashx",
                    data: {
                        action: 'GetItemBaseData'
                    },
                    success: function (data) {
                        if (data.IsSuccess && data.ResultCode == 0) {
                            if (callback)
                                callback(data);
                        }
                        else {
                            alert(data.Message);
                        }
                    }
                });

            },
            //获取商品种类
            getItemType: function (callback) {
                $.util.ajax({
                    url: " /ApplicationInterface/Module/Item/ItemNewHandler.ashx",
                    data: {
                        action: 'GetItemType'
                    },
                    success: function (data) {
                        if (data.IsSuccess && data.ResultCode == 0) {
                            if (callback)
                                callback(data);
                        }
                        else {
                            alert(data.Message);
                        }
                    }
                });
            },
            //获取优惠券
            getCouponTypeList: function (callback) {
                $.util.ajax({
                    url: " /ApplicationInterface/Gateway.ashx",
                    data: {
                        action: 'Marketing.Coupon.GetCouponTypeList',
                        "PageIndex": 1,
                        "PageSize": 100000,
                    },
                    success: function (data) {
                        if (data.IsSuccess && data.ResultCode == 0) {
                            if (callback)
                                callback(data);
                        }
                        else {
                            alert(data.Message);
                        }
                    }
                });
            },
            //获取卡类型
            getSysVipCardTypeList: function (callback) {
                $.util.ajax({
                    url: " /ApplicationInterface/Gateway.ashx",
                    data: {
                        action: 'VIP.SysVipCardType.GetSysVipCardTypeList'
                    },
                    success: function (data) {

                        if (data.IsSuccess && data.ResultCode == 0) {
                            if (callback)
                                callback(data);
                        }
                        else {
                            alert(data.Message);
                        }
                    }
                });
            },
            //获取商品品类
            getClassify: function (callback) {
                debugger;
                pram = '?Status={0}&_dc=1428913044379&node={1}&multiSelect={2}&isSelectLeafOnly={3}&isAddPleaseSelectItem={4}&bat_id={5}&pleaseSelectText={6}&pleaseSelectID={7}';
                pram = pram.format(this.getClassifySeach.Status, this.getClassifySeach.node, this.getClassifySeach.multiSelect, true, this.getClassifySeach.isAddPleaseSelectItem, this.getClassifySeach.bat_id, this.getClassifySeach.pleaseSelectText, this.getClassifySeach.pleaseSelectID);
                $.util.ajax({
                    url: "/module/basic/ItemCategoryNew/Handler/ItemCategoryTreeHandler.ashx" + pram,
                    data: {
                        oldInface: true
                    },
                    success: function (data) {
                        if (data) {
                            if (callback)
                                callback(data);
                        }
                        else {
                            alert("品类加载异常请联系管理员");
                        }
                    }
                });
            },


            addCommodity: function (callback) {
                $.util.isLoading();
                var isFalse = $('#bulkBox .checkBox').hasClass('on') && $('#bulkBox').css('display') != 'none';
                $.util.ajax({
                    url: "/ApplicationInterface/Module/Item/ItemNewHandler.ashx",
                    data: {
                        action: 'save_itemNew',
                        Item_Id: "",     //商品ID
                        Item_Category_Id: this.addPram.Item_Category_Id,   //商品品类ID
                        Item_Name: this.addPram.Item_Name,      //商品名称
                        Item_Code: this.addPram.Item_Code,     //商品编码
                        //VirtualItemTypeId: this.addPram.VirtualItemTypeId,  //虚拟商品种类
                        //ObjecetTypeId: this.addPram.ObjecetTypeId,//关联的id

                        ifservice: $('[data-name="ifservice"].on').data("value"),
                        SalesPromotionList: this.addPram.SalesPromotionList, //促销分组[{"ItemCategoryId":"BD10C60F-8AC9-4BB6-BE64-6F196B3D36CB"},{"ItemCategoryId":"BD10C60F-8AC9-4BB6-BE64-6F196B3D36C3"}]
                        ItemPropList: this.addPram.ItemPropList,//商品与属性关系集合
                        T_ItemSkuProp: this.addPram.T_ItemSkuProp,//  商品sku名列 (基础数据)
                        SkuList: this.addPram.SkuList,// 商品sku值列表
                        ItemImageList: this.addPram.ItemImageList, //图片信息
                        OperationType: 'ADD', //add
                        IsGB: isFalse ? 0 : 1,
                        VirtualItems: this.addPram.VirtualItems,//虚拟商品组合
                        ItemDeliverySettings: this.addPram.ItemDeliverySettings //商品配送方式
                    },
                    success: function (data) {
                        if (data.IsSuccess && data.ResultCode == 0) {
                            if (callback)
                                callback(data);
                        }
                        else {
                            // window.d.close();//关闭提示框;
                            alert(data.Message);
                        }
                    }
                });
            },
            SaveSkuValue: function (callback) {
                $.util.ajax({
                    url: "/ApplicationInterface/Module/Item/ItemNewHandler.ashx",
                    data: {
                        action: 'SaveSkuValue',
                        parent_prop_id: this.addSkuValue.parent_prop_id,
                        prop_name: this.addSkuValue.prop_name

                    },
                    success: function (data) {
                        if (data.IsSuccess && data.ResultCode == 0) {
                            if (callback)
                                callback(data);
                        }
                        else {
                            alert(data.Message);
                        }
                    }
                });
            }

        }

    };
    page.init();
});

