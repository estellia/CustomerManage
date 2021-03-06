﻿define(['jquery', 'template', 'tools', 'langzh_CN', 'easyui', 'kkpager', 'artDialog', 'noviceGuideJS'], function ($) {
    var page = {
        elems: {
            sectionPage:$("#section"),
            simpleQueryDiv: $("#simpleQuery"),     //简单查询条件的层dom
            allQueryDiv: $("#allQuery"),             //所有的查询条件层dom
            uiMask: $("#ui-mask"),
            tabel:$("#gridTable"),                   //表格body部分
            tabelWrap:$('#tableWrap'),
            thead:$("#thead"),                    //表格head部分
            showDetail: $('#showDetail'),         //弹出框查看详情部分
            operation:$('#opt,#Tooltip'),              //弹出框操作部分
            vipSourceId:'',
            click:true,
            dataMessage:  $("#pageContianer").find(".dataMessage"),
            panlH:116                           // 下来框统一高度
        },
        detailDate:{},
        ValueCard:'',//储值卡号
        select:{
            isSelectAllPage:false,                 //是否是选择所有页面
            tagType:[],                             //标签类型
            tagList:[]                              //标签列表
        },
        init: function () {
            //debugger;
            this.initEvent();
            this.loadPageData();
        },
        initEvent: function () {
            var that = this;
            //点击查询按钮进行数据查询
            //
            if ($.util.getUrlParam("stepIndex") != null && $.util.getUrlParam("GuId") != null) {
                var stepIndex = parseInt($.util.getUrlParam("stepIndex"));
                var GuId = $.util.getUrlParam("GuId");

                that.GetModuleNextStep({
                    "action": 'Basic.Guide.GetModuleNextStep',
                    "ModuleCode": "CloudShop",
                    "ParentModule": GuId,//"BAB72601-4801-4403-B97F-F97CC0B9A564",
                    "Step": stepIndex,

                }, function (data) {

                    if (data.NowModule) {
                        var step1 = new noviceGuide("step1", {
                            top: 250,
                            left: 320,
                            isShow: true,
                            title: "查看商品列表",
                            content: "查询和修改商品信息</br>可以进行上架或下架，同时可批量修改分组",
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

            that.elems.sectionPage.delegate(".queryBtn","click", function (e) {
                //调用设置参数方法   将查询内容  放置在this.loadData.args对象中
                that.setCondition();
                //查询数据
                that.loadData.getCommodityList(function(data){
                    //写死的数据
                    //data={"ResultCode":0,"Message":null,"IsSuccess":true,"Data":{"DicColNames":{"UserName":"姓名","Phone":"手机","Email":"邮箱","Col9":"人数","Col8":"职位","Col7":"公司","Col3":"性别"},"SignUpList":[{"SignUpID":"60828091-F8F4-4C97-8F6C-6AC9E627DF97","EventID":"16856b2950892b62473798f3a88ee3e3","UserName":"王孟孟","Phone":"18621865591","Email":"mengmeng.wang@jitmarketing.cn","Col9":"1","Col8":"研发总监","Col7":"上海杰亦特有限公司","Col3":"男"}],"TotalCountUn":1,"TotalCountYet":9,"TotalPage":1}};
                    //渲染table
		
                    that.renderTable(data);

                });
                $.util.stopBubble(e);

            });
            that.elems.operation.delegate(".commonBtn","click",function(e){
                var  selectList= that.elems.tabel.datagrid("getSelections");
                var  type= $(this).data("flag");
                if(type=="add"){
                   // var mid = JITMethod.getUrlParam("mid");
                  //  location.href = "?&mid=" + mid;
                    $.util.toNewUrlPath("release.aspx");
                    return false;
                }
                if(selectList.length==0){
                    alert("必须选择一个商品");
                    return false;
                }

                var parms={};
                parms.ItemInfoList=selectList;
                if(type=="sales"){
                    var nodes=$("#Tooltip").find(".treeNode").tree('getChecked');
                  /*  if(nodes.length==0){
                        alert("至少选择一个促销分组");
                        return false;
                    }*/
                    parms.SalesPromotionList=nodes
                }
                if(type!="salesTooltip"&&type!="cannel"&&type!="add"){
                    that.loadData.operation(parms,type,function(data){
                        alert("操作成功");
                        that.loadPageData(e);
                    });
                } else if(type=="cannel"){
                    $('#sales').tooltip("hide");
                }


            });
            $('#sales').tooltip({
                content: function(){
                    return  $("#Tooltip");
                },
                showEvent: 'click',
                onShow: function(){
                    var nodes=$("#Tooltip").find(".treeNode").tree('getChecked');
                    $.each(nodes,function(){
                        var me=this;
                        $("#Tooltip").find(".treeNode").tree('uncheck',me.target);
                    });
                    var t = $(this);
                    t.tooltip('tip').unbind().bind('mouseenter', function(){
                        t.tooltip('show');
                    }).bind('mouseleave', function(){
                        t.tooltip('hide');
                    });
                }
            });
            /**************** -------------------弹出easyui 控件 start****************/
            var  wd=200,H=30;
            $('#item_status').combobox({
                width:wd,
                height:H,
                panelHeight:that.elems.panlH,
                valueField: 'id',
                textField: 'text',
                data:[{
                    "id":1,
                    "text":"上架",
                    "selected":true
                },{
                    "id":-1,
                    "text":"下架"

                },{
                    "id":0,
                    "text":"全部"
                }]
            });
            $('#item_status').combobox("setValue",1);
             // 分类
            that.loadData.getClassify(function (data) {
                debugger;
                if (!data[0].children) data[0].children = [];
                data[0].children.push({id:0,text:"全部"});
                $('#item_category_id').combotree({
                    width:wd,
                    height:H,
                    editable:true,
                    lines:true,
                    panelHeight:that.elems.panlH,
                    valueField: 'id',
                    textField: 'text',
                    data:data[0].children
                });
            });
            that.loadData.args.bat_id = '2';
            debugger;
            that.loadData.getClassify(function(data) {
                $("#Tooltip").find(".treeNode").tree({
                   // animate:true,
                    checkbox:true,
                    valueField: 'id',
                    textField: 'text',
                    data:data

                });
                data.push({id:0,text:"全部"});
                $('#SalesPromotion_id').combobox({
                    width: wd,
                    height: H,
                    panelHeight: that.elems.panlH,
                    valueField: 'id',
                    textField: 'text',
                    data: data
                });

            });
            $('.datebox').datebox({
                width:wd,
                height:H
            });

          
            /**************** -------------------弹出easyui 控件  End****************/


            /**************** -------------------弹出窗口初始化 start****************/
            $('#win').window({
                modal:true,
                shadow:false,
                collapsible:false,
                minimizable:false,
                maximizable:false,
                closed:true,
                closable:true
            });
            $('#panlconent').layout({
                fit:true
            });
            $('#win').delegate(".saveBtn","click",function(e){

                if ($('#payOrder').form('validate')) {

                    var fields = $('#payOrder').serializeArray(); //自动序列化表单元素为JSON对象

                    that.loadData.operation(fields,that.elems.optionType,function(data){

                        alert("操作成功");
                        $('#win').window('close');
                        that.loadPageData(e);

                    });
                }
            });
            /**************** -------------------弹出窗口初始化 end****************/

            /**************** -------------------列表操作事件用例 start****************/
            that.elems.tabelWrap.delegate(".fontC","click",function(e){
                var rowIndex=$(this).data("index");
                var optType=$(this).data("oprtype");
                that.elems.tabel.datagrid('selectRow', rowIndex);
                var row = that.elems.tabel.datagrid('getSelected');
                if(optType=="payment") {
                    if(row.IsPaid!=1&&row.Status!=10&&row.Status!=11) {
                        that.payMent(row);
                    }
                }
                if(optType=="cancel"){
                    that.cancelOrder(row);
                }
            })
            /**************** -------------------列表操作事件用例 End****************/


          

           

        },
        //获取新手引导步数
        GetModuleNextStep: function (datas, callback) {
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


        //设置查询条件   取得动态的表单查询参数
        setCondition:function(){
            ////debugger;
            var that=this;
            //查询每次都是从第一页开始
            that.loadData.args.start=0;
            var fileds=$("#seach").serializeArray();
            $.each(fileds,function(i,filed){
                filed.value=filed.value=="0"?"":filed.value;
                that.loadData.seach[filed.name]=filed.value;
                that.loadData.seach.form[filed.name]=filed.value;
            });





        },

        //加载页面的数据请求
        loadPageData: function (e) {
            //debugger;
            var that = this;
            $(that.elems.sectionPage.find(".queryBtn").get(0)).trigger("click");
           
            $.util.stopBubble(e);
        },
       
        //渲染tabel
        renderTable: function (data) {
            ////debugger;
            var that=this;
            if(!data.topics){

                data.topics=[];
            }
            //jQuery easy datagrid  表格处理
            that.elems.tabel.datagrid({

                method : 'post',
                iconCls : 'icon-list', //图标
                singleSelect : false, //多选
                // height : 332, //高度
                fitColumns : true, //自动调整各列，用了这个属性，下面各列的宽度值就只是一个比例。
                striped : true, //奇偶行颜色不同
                collapsible : true,//可折叠
                //数据来源
                data:data.topics,
                sortName : 'brandCode', //排序的列
                /*sortOrder : 'desc', //倒序
                 remoteSort : true, // 服务器排序*/
                idField : 'Item_Id', //主键字段
                /*  pageNumber:1,*/
                /* frozenColumns : [ [ {
                 field : 'brandLevelId',
                 checkbox : true
                 } //显示复选框
                 ] ],*/
                frozenColumns:[[
                    {
                        field : 'ck',
                        title:'全选',
                        checkbox : true,
                        width:50,
                    },//显示复选框
                    {field : 'Image_Url',title : '图片',width:50,align:'center',resizable:false,
                        formatter:function(value ,row,index){
                            var html=' <img src="images/商品.png" width="40" height="40"  />';
                            if(value){
                                html=' <img src="'+value+'" width="40" height="40"  />'
                            }

                            return html;
                        }

                    },
                ]],
                columns : [[
                    {field : 'Item_Name',title : '商品名称',width:120,align:'left',resizable:false,
                        formatter:function(value ,row,index){
                            var long=56;
                            if(value&&value.length>long){
                                return '<div class="rowText" title="'+value+'">'+value.substring(0,long)+'...</div>'
                            }else{
                                return '<div class="rowText">'+value+'</div>'
                            }
                        }
                    },
                    {field : 'ifservice',title : '商品类型',width:50,align:'center',resizable:false,
                        formatter:function(value ,row,index){
                            var long=56;
                            if(value==1){
                                return '虚拟商品'
                            }else{
                                return '实物商品'
                            }
                        }
                    } ,
                    {field : 'minPrice',title : '价格(元)',width:50,resizable:false,align:'center'},
                    {field : 'stock',title : '库存',width:40,align:'center',resizable:false,
                        formatter:function(value,row,index){
                           if(isNaN(parseInt(value))){
                             return 0;
                           }else{
                              return parseInt(value);
                           }
                        }
                    },
                    {field : 'SalesCount',title : '销量',width:40,align:'center',resizable:false,
                        formatter:function(value,row,index){
                            if(isNaN(parseInt(value))){
                                return 0;
                            }else{
                                return parseInt(value);
                            }
                        }},
                    {field : 'SalesPromotion',title : '商品分组',width:100,align:'left',resizable:false,
                        formatter:function(value ,row,index){
                            var long=12;
                            var html=""
                            if(value&&value.length>long){
                                html=  '<div class="rowText" title="'+value+'">'+value.substring(0,long)+'...</div>'
                            }else{
                                 html='<div class="rowText">'+value+'</div>'
                            }

                            return  html
                    }},
                    {field : 'Item_Category_Name',title : '商品品类',width:50,align:'left',resizable:false} ,
                    {
                        field: 'Modify_Time', title : '更新时间',width:80,align:'left',resizable:false,
                        formatter:function(value ,row,index){
                            return new Date(value).format("yyyy-MM-dd hh:mm");
                        }
                    },
                    {field : 'Status',title : '状态',width:50,align:'center',resizable:false,
                        formatter:function(value ,row,index){
                            ////debugger;
                            var staus;
                            switch (value){
                                case "1": staus="上架";break;

                                case "-1": staus= "下架"; break;
                            }
                            return staus;
                        }
                    },
                    {
                        field: 'Item_Id', title: '下载二维码', width:50, align: 'left', resizable: false,
                        formatter: function (value, row, index) {

                            return value ? '<a target="_blank" style="padding-left:20px" href="/Module/Basic/Item/Handler/ItemHandler.ashx?method=download_qrcode&item_id=' + value + '&item_name=' + row.Item_Name + '"><img width="16" height="16" src="images/QRcode.png"></a>' : '';
                        }
                    },
					{
                        field: 'customer_id', title: '操作', width:30, align: 'left', resizable: false,
                        formatter: function (value, row, index) {
							/*var hast = location.host,
								hastname = '',
								goodsUrl = '';
							if(hast == 'bs.test.chainclouds.com' || hast == 'bs.test.chainclouds.cn'){
								hastname = 'api.test.chainclouds.cn';
							}else if(hast == 'bs.dev.chainclouds.com' || hast == 'bs.dev.chainclouds.cn'){
								hastname = 'api.dev.chainclouds.cn';
							}else if(hast == 'bs.chainclouds.com' || hast == 'bs.chainclouds.cn'){
								hastname = 'api.chainclouds.cn';
							}*/
							var goodsUrl =window.commodity+'/HtmlApps/html/public/shop/goods_detail.html?customerId='+value+'&goodsId='+row.Item_Id+'&isPreview=1&APP_TYPE=SUBSCIBE&version=111';
                            return 1 ? '<a  target="_blank" href="'+goodsUrl+'"><img width="18" height="12" src="images/icon-preview.png"  style="margin:3px 0 0 3px;"></a>' : '';
                        }
                    }
                ]],

                onLoadSuccess : function(data) {
                    //debugger;
                    that.elems.tabel.datagrid('clearSelections'); //一定要加上这一句，要不然datagrid会记住之前的选择状态，删除时会出问题
                    if(data.rows.length>0) {
                        that.elems.dataMessage.hide();
                    }else{
                        that.elems.dataMessage.show();
                    }
                },
                onClickRow:function(rowindex,rowData){
                     ////debugger;
                     if(that.elems.click){
                     that.elems.click = true;
                     ////debugger;

                     //var mid = JITMethod.getUrlParam("mid");
                     var url = "commodityExit.aspx?Item_Id=" + rowData.Item_Id;
                         $.util.toNewUrlPath(url);
                     }

                },onClickCell:function(rowIndex, field, value){
                    if (field == "ck" || field == "Item_Id" || field =="customer_id") {    //在每一列有操作 而点击行有跳转页面的操作  才使用该功能。 此处不注释 与注释都可以。
                     that.elems.click=false;
                     }else{
                     that.elems.click=true;
                     }
                }

            });



            //分页
            data.Data={};
            data.Data.TotalPageCount = data.totalCount%that.loadData.args.limit==0? data.totalCount/that.loadData.args.limit: data.totalCount/that.loadData.args.limit +1;
            var page=parseInt(that.loadData.args.start/that.loadData.args.limit);
            kkpager.generPageHtml({
                pno: page?page+1:1,
                mode: 'click', //设置为click模式
                //总页码
                total: data.Data.TotalPageCount,
                totalRecords: data.totalCount,
                isShowTotalPage: true,
                isShowTotalRecords: true,
                //点击页码、页码输入框跳转、以及首页、下一页等按钮都会调用click
                //适用于不刷新页面，比如ajax
                click: function (n) {
                    //这里可以做自已的处理
                    //...
                    //处理完后可以手动条用selectPage进行页码选中切换
                    this.selectPage(n);
                    //让  tbody的内容变成加载中的图标
                    //var table = $('table.dataTable');//that.tableMap[that.status];
                    //var length = table.find("thead th").length;
                    //table.find("tbody").html('<tr ><td style="height: 150px;text-align: center;vertical-align: middle;" colspan="' + (length + 1) + '" align="center"> <span><img src="../static/images/loading.gif"></span></td></tr>');
                    //debugger;
                    that.loadMoreData(n);
                },
                //getHref是在click模式下链接算法，一般不需要配置，默认代码如下
                getHref: function (n) {
                    return '#';
                }

            }, true);


            if((that.loadData.opertionField.displayIndex||that.loadData.opertionField.displayIndex==0)){  //点击的行索引的  如果不存在表示不是显示详情的修改。
                that.elems.tabel.find("tr").eq(that.loadData.opertionField.displayIndex).find("td").trigger("click",true);
                that.loadData.opertionField.displayIndex=null;
            }
        },
        //加载更多的资讯或者活动
        loadMoreData: function (currentPage) {
            debugger;
            var that = this;
            this.loadData.args.start = (currentPage - 1) * that.loadData.args.limit;
            debugger;
            that.loadData.getCommodityList(function (data) {
                debugger;
                that.renderTable(data);
            });
        },


        //取消订单
        cancelOrder:function(data){
            var that=this;
            that.elems.optionType="cancel";
            $('#win').window({title:"取消订单",width:360,height:260});
            //改变弹框内容，调用百度模板显示不同内容
            $('#panlconent').layout('remove','center');
            var html=bd.template('tpl_OrderCancel');
            var options = {
                region: 'center',
                content:html
            };
            $('#panlconent').layout('add',options);
            this.loadData.tag.orderID=data.OrderID;
            $('#win').window('open');
        },
        loadData: {
            args: {
                bat_id:"1",
                PageIndex: 0,
                PageSize: 6,
                SearchColumns:{},    //查询的动态表单配置
                OrderBy:"",           //排序字段
                SortType: 'DESC',    //如果有提供OrderBy，SortType默认为'ASC'
                Status:-1,
                page:1,
                start:0,
                limit:10
            },
            tag:{
                VipId:"",
                orderID:''
            },
            seach:{
                item_category_id:null,
                SalesPromotion_id:null,
                form:{
                    item_code:"",
                    item_name:"",
                    item_status:null,
                    item_can_redeem:null
                }
            },
            opertionField:{},

            getCommodityList: function (callback) {
                debugger;
                $.util.oldAjax({
                    url: "/module/basic/Item/Handler/ItemHandler.ashx",
                      data:{
                          action:'search_item',
                          item_category_id:this.seach.item_category_id,
                          SalesPromotion_id:this.seach.SalesPromotion_id,
                          page:this.args.page,
                          start:this.args.start,
                          limit:this.args.limit,
                          form:{
                              item_code:"",
                              item_name:this.seach.form.item_name,
                              item_status:this.seach.form.item_status,
                              item_can_redeem:null
                          }
                      },
                      success: function (data) {
                        debugger;
                        if (data.topics) {
                            if (callback) {
                                callback(data);
                            }

                        } else {
                            alert("加载数据不成功");
                        }
                      },
                      error: function (data) {
                          debugger;
                          alert(data);
                      }
                });
            },
            getClassify: function (callback) {
                debugger;
                $.util.oldAjax({
                    url: "/module/basic/ItemCategoryNew/Handler/ItemCategoryTreeHandler.ashx",
                    data:{
                        node:"root",
                        isAddPleaseSelectItem:true,
                        pleaseSelectText:"请选择",
                        pleaseSelectID:"0",
                        bat_id:this.args.bat_id,
                        Status:"1"


                    },
                    success: function (data) {
                        debugger;
                        if (data) {
                            if (callback)
                                callback(data);
                        }
                        else {
                            alert("分类加载异常请联系管理员");
                        }
                    }
                });
            },
            operation:function(pram,operationType,callback){
                //debugger;
                var prams={data:{action:""}};
                prams.url="/ApplicationInterface/Module/Item/ItemNewHandler.ashx";
                //根据不同的操作 设置不懂请求路径和 方法


                prams.data.ItemInfoList=[];
                $.each( pram.ItemInfoList,function(){
                           var me=this;
                    prams.data.ItemInfoList.push({Item_Id:me.Item_Id})
                });

                if(pram.SalesPromotionList) {
                    prams.data.SalesPromotionList=[];
                    $.each(pram.SalesPromotionList, function () {
                        var me = this;
                        prams.data.SalesPromotionList.push({ItemCategoryId: me.id})
                    });
                }
                switch(operationType){
                    case "putaway":prams.data.action="ItemToggleStatus";  //上架
                        prams.data.Status="1";
                        break;
                    case "soldOut":prams.data.action="ItemToggleStatus";  //下架
                        prams.data.Status="-1";
                        break;
                    case "sales":prams.data.action="UpdateSalesPromotion";  //更改促销分组
                        break;
                }


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
            }


        }

    };
    page.init();
});

