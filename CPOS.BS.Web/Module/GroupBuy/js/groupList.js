define(['newJquery', 'tools', 'template', 'kkpager', 'artDialog', 'datetimePicker', 'noviceGuideJS'], function () {
    var page =
        {
            pageSize: 2,
            //默认控制条数
            currentPage: 0,
            //图文素材ID
            url: "/ApplicationInterface/Events/EventsGateway.ashx",
            //关联到的类别
            elems:
            {
                //选择活动详情的下拉框
                uiMask: $("#ui-mask")
            },
            clearInput: function () {
                $("#name").val("");
                $("#beginTime").val("");
                $("#endTime").val("");
                $("#statusText").html("在商城中显示").data("status", "");
            },
            init: function () {
                var that = this;
                //团购  抢购 市场都是一套逻辑  故用pageType标识
                var pageType = $.util.getUrlParam("pageType");
                var pageName = decodeURIComponent($.util.getUrlParam("pageName"));

               
                var pageStr = "";
                if (pageName) {
                    pageStr = pageName;
                    document.title = pageStr;
                } else {
                    var d = dialog({
                        fixed: true,
                        title: '提示:哎呀妈呀',
                        content: "未配置活动类别!"
                    });
                    d.showModal();
                    document.title = pageStr + "管理";
                }
                that.pageType = pageType;
                window.pageStr = that.pageStr = pageStr;
                $("#addText").html("添加新" + pageStr + "");
                $("#addNew").html("添加新" + pageStr + "");
                this.initTables();
                this.initEvent();
                this.monthStr();

            },
            monthStr: function () {
                var arr = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"];
                window.Month = arr;
            },
            stopBubble: function (e) {
                if (e && e.stopPropagation) {
                    //因此它支持W3C的stopPropagation()方法 
                    e.stopPropagation();
                }
                else {
                    //否则，我们需要使用IE的方式来取消事件冒泡 
                    window.event.cancelBubble = true;
                }
                e.preventDefault();
            },
            //显示遮罩层
            showMask: function (flag, type) {
                if (!!!flag) {
                    this.elems.uiMask.hide();
                    this.elems.chooseEventsDiv.hide();
                }
                else {
                    this.elems.uiMask.show();
                    //动态的填充弹出层里面的内容展示
                    this.loadPopUp(type);
                    this.elems.chooseEventsDiv.show();
                }
            },
            //显示弹层
            showElements: function (selector) {
                this.elems.uiMask.show();
                $(selector).slideDown(500);
            },
            hideElements: function (selector) {
                this.elems.uiMask.fadeIn(500);
                $(selector).fadeIn(500);
            },
            initTables: function () {
                var that = this;
                $("#loading").show();
                //请求结果
                this.loadData.getEventsList(function (data) {
                    var list = data.Data.PanicbuyingEventList;
                    list = list ? list : [];
                    $.each(list, function (i) {
                        list[i].pageStr = (that.pageStr ? that.pageStr : "团购");
                    });
                    //传mid
                    var mid = window.mid;
                    var pMenuID = window.PMenuID;
                    ////debugger;;
                    var html = bd.template("tpl_content", { list: list, Mid: mid, PMenuID: pMenuID })
                    $("#goodsList").html(html);
                    kkpager.generPageHtml({
                        pno: 1,
                        mode: 'click', //设置为click模式
                        //总页码  
                        total: data.Data.TotalPage,
                        isShowTotalPage: false,
                        isShowTotalRecords: false,
                        //点击页码、页码输入框跳转、以及首页、下一页等按钮都会调用click
                        //适用于不刷新页面，比如ajax
                        click: function (n) {
                            //这里可以做自已的处理
                            //...
                            //处理完后可以手动条用selectPage进行页码选中切换
                            this.selectPage(n);

                            that.loadMoreData(n);
                        },
                        //getHref是在click模式下链接算法，一般不需要配置，默认代码如下
                        getHref: function (n) {
                            return '#';
                        }

                    }, true);

                });
            },
            //加载更多的资讯或者活动
            loadMoreData: function (currentPage) {
                var that = this;
                this.loadData.args.PageIndex = currentPage - 1;
                this.loadData.getEventsList(function (data) {
                    var list = data.Data.PanicbuyingEventList;
                    list = list ? list : [];
                    $.each(list, function (i) {
                        list[i].pageStr = (that.pageStr ? that.pageStr : "团购");
                    });
                    var mid = window.mid;
                    var pMenuID = window.PMenuID;
                    var html = bd.template("tpl_content", { list: list, Mid: mid, PMenuID: pMenuID })
                    $("#goodsList").html(html);
                });
            },
            alert: function (content) {
                var d = dialog({
                    fixed: true,
                    title: '提示',
                    content: content
                });
                d.showModal();
                setTimeout(function () {
                    d.close().remove();
                }, 3500);
            },
            initEvent: function () {
                //初始化事件集
                var that = this;
                $('#beginTime').datetimepicker({
                    lang: "ch",
                    format: 'Y-m-d H:i',
                    step: 5 //分钟步长
                });
                $('#endTime').datetimepicker({
                    lang: "ch",
                    format: 'Y-m-d H:i',
                    step: 5 //分钟步长
                });
                //添加新团购事件
                $("#addNew").click(function (e) {
                    that.clearInput();
                    that.showElements("#addNewDiv");
                    that.stopBubble(e);
                });
                $(".selectBox").bind("mouseenter click", function (e) {
                    $(this).find(".dropList").show();
                    that.stopBubble(e);
                });
                //选择上架状态
                $(".dropList").delegate("span", "click", function (e) {
                    var $this = $(this);
                    $("#statusText").html($this.html());
                    $("#statusText").data("shopstatus", $this.data("status"));
                    $this.parent().hide();
                    that.stopBubble(e);
                }).bind("mouseleave", function (e) {
                    $(this).hide();
                    that.stopBubble(e);
                });
               
              

                //确定添加新团购
                $("#btnSureAdd").click(function (e) {
                    if (!!!page.isSending) {
                        page.isSending = true;   //正在请求
                        var beginTime = $("#beginTime").val(),
                            endTime = $("#endTime").val();
                        if ($("#name").val().length == 0) {
                            page.isSending = false;
                            that.alert("活动名称不能为空!");
                            return;
                        }
                        if (beginTime.length == 0) {
                            page.isSending = false;
                            that.alert("活动开始时间不能为空!");
                            return;
                        }
                        if (endTime.length == 0) {
                            page.isSending = false;
                            that.alert("活动结束时间不能为空!");
                            return;
                        }
                        if (beginTime >= endTime) {
                            page.isSending = false;
                            that.alert("活动开始时间不能大于等于结束时间!");
                            return;
                        }
                        that.loadData.args.EventName = $("#name").val();
                        that.loadData.args.BeginTime = $("#beginTime").val();
                        that.loadData.args.EndTime = $("#endTime").val();
                        that.loadData.args.EventStatus = $("#statusText").data("shopstatus");
                        
                        //添加新团购
                        that.loadData.addEvent(function (data) {
                            var EventId = data.Data.EventID;
                            $(".hintClose").trigger("click");
                            setTimeout(function () {
                                $.util.toNewUrlPath("GroupManage.aspx?pageType=" + (that.pageType ? that.pageType : 1) + "&eventId=" + EventId + "&showPage=shopManage" + "&pageName=" + (that.pageStr ? that.pageStr : "团购"));
                            }, 800);

                        });
                    }
                    //跳转到商品管理界面
                    that.stopBubble(e);
                });
                //关闭弹出层
                $(".hintClose").bind("click", function () {
                    that.elems.uiMask.slideUp();
                    $(this).parent().parent().fadeOut();
                });
             
                if ($.util.getUrlParam("stepIndex") != null && $.util.getUrlParam("GuId") != null) {
                    var pageType = $.util.getUrlParam("pageType");
                    var stepIndex = parseInt($.util.getUrlParam("stepIndex"));
                    var GuId = $.util.getUrlParam("GuId");
                    var title = "";
                    var content = "";
                    if (stepIndex == 3) {
                       
                        title = "设置团购活动";
                        content = "使用场景有打造爆款商品、通过商品<br/>批量销售来提升营业额和人气等等";
                    } else if (stepIndex == 2) {
                        
                        title = "设置秒杀活动";
                        content = "使用场景有低价商品吸引客户流量、<br/>用物美价廉商品回馈优质会员等等";
                    } else if (stepIndex ==4) {
                     
                        title = "设置热销商品";
                        content = "推荐畅销的新品或旧品，<br />引导会员购买某款爆款商品等等";
                    }
                    
                    that.loadData.GetModuleNextStep({
                        "action": 'Basic.Guide.GetModuleNextStep',
                        "ModuleCode": "Marketing",
                        "ParentModule": "46F16BCD-76A3-4737-B8CE-38A213E188BA",
                        "Step": stepIndex,

                    }, function (data) {
                     
                        if (data.NowModule.Url) {
                            var step1 = new noviceGuide("step1", {
                                top: 80,
                                left: 1000,
                                isShow: true,
                                title: title,
                                content: content,
                                step: data.NowModule.ModuleStep,
                                sumStep: 4,
                                video: {
                                    top: 200,
                                    left: 400,
                                    src: "http://guide.chainclouds.cn" + data.NowModule.VideoUrl// "../static/jkb/movie/sczx.mp4",
                                },
                                direction: {
                                    angle: -50,
                                    top: "10",
                                    left: "150"
                                }
                            }, function () {
                                if (data.PreModule.Url == "/module/couponManage/querylist.aspx") {
                                    window.location.href = data.PreModule.Url + "#stepIndex=" + data.PreModule.ModuleStep + "&GuId=" + data.PreModule.ParentModule;
                                } else {
                                    window.location.href = data.PreModule.Url + "&stepIndex=" + data.PreModule.ModuleStep + "&GuId=" + data.PreModule.ParentModule;
                                }
                                
                            }, function () {

                                if (data.NextModule.ModuleStep == 0) {
                                    that.loadData.SetModuleLastStep({
                                        "action": 'Basic.Guide.SetModuleLastStep',
                                        "ModuleCode": "Marketing",
                                        "ParentModule": GuId,//"BAB72601-4801-4403-B97F-F97CC0B9A564",
                                        "Step": stepIndex,
                                        "Url": data.NowModule.Url,
                                        "FinishedStatus": 1
                                    }, function () {
                                        window.location.href = data.NextModule.Url + "#stepIndex=" + data.NextModule.ModuleStep + "&GuId=" + data.NowModule.ParentModule;
                                    });
                                } else {
                                    that.loadData.SetModuleLastStep({
                                        "action": 'Basic.Guide.SetModuleLastStep',
                                        "ModuleCode": "Marketing",
                                        "ParentModule": GuId,//"BAB72601-4801-4403-B97F-F97CC0B9A564",
                                        "Step": stepIndex,
                                        "Url": data.NowModule.Url,
                                        "FinishedStatus": 0
                                    }, function () {
                                        window.location.href = data.NextModule.Url + "&stepIndex=" + data.NextModule.ModuleStep + "&GuId=" + data.NextModule.ParentModule;
                                    });
                                }
                                
                            });
                        }
                        });
                    }
            },
         
        };

    page.loadData =
    {
        args: {
            PageIndex: 0,
            PageSize: 10,
            Status: "-1"
        },
        //获取活动列表
        getEventsList: function (callback) {
            console.log(page.url)
            $.util.ajax({
                url: page.url,
                type: "post",
                data:
                {
                    'action': 'GetPanicbuyingEvent',
                    'EventTypeId': page.pageType, //类别ID
                    'PageIndex': this.args.PageIndex,
                    'PageSize': this.args.PageSize
                },
                success: function (data) {

                    if (data.ResultCode == 0) {
                        //表示成功
                        if (callback) {
                            callback(data);
                        }
                    }
                    else {
                        alert(data.Message);
                    }
                }
            });
        },
        //添加新团购
        addEvent: function (callback) {
            $.util.ajax({
                url: page.url,
                type: "post",
                data:
                {
                    'action': 'AddPanicbuyingEvent',
                    'EventName': this.args.EventName,
                    'EventTypeId': page.pageType,
                    'BeginTime': this.args.BeginTime,
                    'EndTime': this.args.EndTime,
                    'EventStatus': this.args.EventStatus
                },
                success: function (data) {

                    if (data.ResultCode == 0) {
                        //表示成功
                        if (callback) {
                            callback(data);
                        }
                    }
                    else {
                        alert(data.Message);
                    }
                    page.isSending=false;
                }
            });
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
    }
    //初始化
    page.init();
});