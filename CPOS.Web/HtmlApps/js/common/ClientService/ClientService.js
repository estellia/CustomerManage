/*定义页面*/
Jit.AM.defindPage({
    name: 'Categroy',
    Wrapper:null,
    ele: {
        pullDown: document.getElementById('pullDown'),
        pullUp: document.getElementById('pullUp'),
        section: $("#section"),
        categoryMainList: $("#categoryMainList"),
        CSHeadImage: '../../../images/common/ClientService/tupian.png',//客服头像默认地址
        CHeadImage: '../../../images/common/ClientService/tupian02.png',//客户头像默认地址
        HeadImage:''  //客服头像
    },
    page:{
        pageIndex:1,
        pageSize:30,
        allPage:2
    },
    status: {
        isfirstload: true,                 //是否首次加载数据
        endtime: '',
        RefreshIntervalTime: 10000, //刷新的时间间隔
        starttime: '',
        Current_Time:'',
        Next_Time: '',
        isgetmessage:false  //是否在获取数据
    },
    onPageLoad: function () {

        Jit.WX.OptionMenu(false);

        //当页面加载完成时触发
        Jit.log('进入'+this.name);

        this.isSending = false;
        this.isStartLoad = true;
        this.loadData();
        this.initEvent();
    },
    loadData: function () {
        this.receiveMessageFirst();

        $("#scrollContainer").attr("style", "overflow-y:visible");
    },
    //是否为json格式
    IsJSON: function (data) {
        if(Number(data))
        try
        {
            $.parseJSON(data);
            return true;
        }
        catch (e)
        {
            return false;
        }
    }
    ,
    //会员金矿集客内容转换
    convertContent: function (data) {
        debugger;
        if (this.IsJSON(data) && Number(data)==NaN) {
            var jsonobject = $.parseJSON(data);
            var reusltstr = "";
            var hname = "点击打开";
            if (jsonobject.content)
            {
                reusltstr += jsonobject.content
            }
            if (jsonobject.hName) {
                hname = jsonobject.hName
            }
            if (jsonobject.hURL) {
                reusltstr += "<br/><a style='color:#038efe;' href='" + jsonobject.hURL + "'>" + hname + "</a>";
            }
            return reusltstr;

        } else {
            return data;
        }
    }
    ,
    initEvent: function() {
        var self = this;
        $('.SendMessageBtn').on('click', function () {
            
            self.sendMessage();

        });



        
    }
   ,
    againdata:function()
    {
        var self = this;
        this.receiveMessageNews();
        var ss = setTimeout(function ()
        {
            self.againdata();

        }, self.status.RefreshIntervalTime);
    }
    ,
    //发送数据
    sendMessage: function () {
        var self = this;
        var Messagetext = $("#Messagetext").val();
        ///^\s$/.test(Messagetext)
        if (Messagetext.trim()=='') {
            Jit.UI.Dialog({
                'content':'不能发布空白信息！',
                'type': 'Dialog',
                'isDpi':1,
                'times': 1500
            });
            return;
        }


        $(".liaot").html($(".liaot").html() + ' <dl class="liaot2 temp" ><dd><span ontouchstart=" window.event.stopPropagation()"    >' + Messagetext + '</span><font class="jiantou"></font></dd><dt><div class="imgbg"><img src="../../../images/common/ClientService/imgbg1.png"></div><img style=" height: 90px;width: 90px;" src="' + this.ele.HeadImage + '"></dt></dl>');
        $("#Messagetext").val('');
        self.refreshIscroll();
        self.Wrapper.scrollTo(0, self.Wrapper.maxScrollY);

        self.ajax({
            url: '/CustomerService/Data.aspx',
            type: 'get',
            data: {
                'action': 'sendMessage',
                'isCS': 0,
                'messageContent': Messagetext,
                'csPipelineId': 1,
                'IsPopMsg': 1
            },
            beforeSend: function () {
                if (self.status.isfirstload) {
                    Jit.UI.Loading(true);
                    self.isSending = true;
                }
            },
            success: function (data) {
                
                    if (data.code == "200")
                    {
                        if (self.status.Current_Time == "" || self.status.Current_Time == null) {
                            self.status.Current_Time = data.CurrentServerDateTime;
                        }
                    }
                    self.receiveMessageNews();
               
            },
            complete: function () {
                    self.isSending = false;
                    Jit.UI.Loading(false);

            }
        });
    },
    //转换时间
    getDate:function(time)
    {
        var CreateTime = new Date(time.replace('T', ' '));
        if (CreateTime == 'Invalid Date') {
            var temp = time.replace('T', ',').replace('-', ',').replace('-', ',').replace(':', ',').replace(':', ',').split(',');
            if (temp.length > 5)
                CreateTime = new Date(parseInt(temp[0]), parseInt(temp[1]), parseInt(temp[2]), parseInt(temp[3]), parseInt(temp[4]), parseInt(temp[5]));
            
        }

        return CreateTime;

    }
    ,
    //获取信息html格式 messagetype;(1:初次加载，2：新数据，3：历史数据)
    GetMessageHtml: function (conversationList, messagetype) {
        var self = this;
        var htmlStr = '';
        var showdate = "";

        if (conversationList == null)
        {
            return "";
        }

        for (var i = conversationList.length - 1; i > -1; i--) {
                var CreateTime = "";//self.getDate(conversationList[i].CreateTime.substr(0, 19));
                var CreateTime1 = "";
                var HeadImage1 = self.ele.CSHeadImage;
                var HeadImage = self.ele.CHeadImage;

                var isaddHmtlstr = true;//是否加载数据
                
                
                if (messagetype == 3)//是否是加载历史数据
                {
                    if (i > 0 && i < (conversationList.length - 1)) {
                        CreateTime = self.getDate(conversationList[i - 1].CreateTime.substr(0, 19));
                    }
                    CreateTime1 = self.getDate(conversationList[i].CreateTime.substr(0, 19));
                } else {

                    CreateTime1 = self.status.endtime;
                    CreateTime = self.getDate(conversationList[i].CreateTime.substr(0, 19));

                 

                    self.status.endtime = CreateTime;
                }

                if (CreateTime != "" && CreateTime1 != "") {
                    var diffTimespan = CreateTime - CreateTime1;

                    if (diffTimespan > 600000) {//大于十分钟显示日期时间
                        showdate = '<span class="sjian">' + conversationList[i].CreateTime.substr(5, 11).replace('T', ' ').replace('-', '/') + '</span>';
                    } else {
                        showdate = "";
                    }
                } else {
                    showdate = '<span class="sjian">' + conversationList[i].CreateTime.substr(5, 11).replace('T', ' ').replace('-', '/') + '</span>';
                }

                if (conversationList[i].VipHeadImage != null && conversationList[i].VipHeadImage.length>4) {
                    HeadImage = conversationList[i].VipHeadImage;
                    self.ele.HeadImage = conversationList[i].VipHeadImage;
                }
                if (conversationList[i].UserHeadUrl != null && conversationList[i].UserHeadUrl.length>4) {
                    HeadImage1 = conversationList[i].UserHeadUrl;
                }
                
               

            
                if (isaddHmtlstr) {
                    htmlStr += showdate;
                    $(".temp").remove();
                    switch (conversationList[i].IsCS) {
                        case 0: htmlStr += ' <dl class="liaot2" ><dd><span ontouchstart=" window.event.stopPropagation()"    >' + this.convertContent(conversationList[i].Content) + '</span><font class="jiantou"></font></dd><dt><div class="imgbg"><img src="../../../images/common/ClientService/imgbg1.png"></div><img style=" height: 90px;width: 90px;" src="' + HeadImage + '"></dt></dl>'; break;
                        case 1: htmlStr += ' <dl class="liaotl"  ><dt><div class="imgbg"><img src="../../../images/common/ClientService/imgbg1.png"></div><img  style=" height: 90px;width: 90px;" src="' + HeadImage1 + '"></dt><dd><font ></font><span ontouchstart=" window.event.stopPropagation()"  >' + this.convertContent(conversationList[i].Content) + '</span></dd></dl>'; break;
                    }
                }

                
                
               
                
        }
        return htmlStr;
                   
    },
    //接收第一次加载信息
    receiveMessageFirst: function () {
        var self = this;
        self.ajax({
            url: '/CustomerService/Data.aspx',
            type: 'get',
            data: {
                'action': 'receiveMessageNew',
                 'isCS': 0,
                 'pageIndex': 1,
                 'ReceiveType':1,
                 'pageSize': self.page.pageSize
            },
            beforeSend: function () {
                
            },
            success: function (data) {
                if (data.code == "200" && data.content) {
                    debugger;
                    var htmlStr = self.GetMessageHtml(data.content.conversations, 1);
                    $(".liaot").html($(".liaot").html() + htmlStr);
                    self.getWelcomelanguage();
                    self.status.Current_Time= data.content.CurrentGetTime;
                    self.status.Next_Time = data.content.NextTime;
                    self.refreshIscroll();
                    if (htmlStr != "") {

                        self.Wrapper.scrollTo(0, self.Wrapper.maxScrollY);
                    }
                    self.againdata();

                }

            },
            complete: function () {
                

            }
        });
    }
    ,
   
    //接收最新信息
    receiveMessageNews: function () {
        var self = this;
        if (self.status.Current_Time != "") {
            if (!self.status.isgetmessage) {

                self.status.isgetmessage = true;
                self.ajax({
                    url: '/CustomerService/Data.aspx',
                    type: 'get',
                    data: {
                        'action': 'receiveMessageNew',
                        'isCS': 0,
                        'ReceiveType': 2,
                        'CurrentGetTime': self.status.Current_Time
                    },
                    beforeSend: function () {

                    },
                    success: function (data) {
                        if (data.code == "200" && data.content) {
                            debugger;
                            var htmlStr = self.GetMessageHtml(data.content.conversations, 2);
                            self.status.Current_Time = data.content.CurrentGetTime;
                            if (htmlStr != "") {
                                $(".liaot").html($(".liaot").html() + htmlStr);
                                self.refreshIscroll();
                                self.Wrapper.scrollTo(0,self.Wrapper.maxScrollY);
                            }

                        }

                    },
                    complete: function () {
                        self.status.isgetmessage = false;

                    }
                });
            }
        } else {
            this.receiveMessageFirst();
        }
    },

   
    //加载历史数据
    receiveMessagemore: function () {
        var self = this;
        self.page.pageIndex++;
        self.ajax({
            url: '/CustomerService/Data.aspx',
            type: 'get',
            data: {
                'action': 'receiveMessageNew',
                'isCS': 0,
                'ReceiveType': 3,
                'NextTime': self.status.Next_Time,
                'pageSize': self.page.pageSize
            },
            beforeSend: function () {

            },
            success: function (data) {
                if (data.code == "200" && data.content) {
                    var htmlStr = self.GetMessageHtml(data.content.conversations, 3);
                    self.status.Next_Time = data.content.NextTime;
                    $(".liaot").html(htmlStr + $(".liaot").html());

                }

                self.refreshIscroll();

            },
            complete: function () {


            }
        });
    },
    refreshIscroll: function () {
        var self = this;
        if (null != document.getElementById("scrollContainer")) {
            if (!self.storeWrapper) {
                var pullDownEl = self.ele.pullDown;
                var pullUpEl = self.ele.pullUp;
                var pullDownOffset = pullDownEl.offsetHeight;
                var pullUpOffset = pullUpEl.offsetHeight;


                self.Wrapper = new iScroll('scrollContainer', {
                    hScrollbar: true,
                    vScrollbar: true,
                    onRefresh: function () {
                        if (pullDownEl.className.match('loading')) {
                            pullDownEl.className = '';
                            //pullDownEl.querySelector('.pullDownLabel').innerHTML = '下拉刷新...';
                        } else if (pullUpEl.className.match('loading')) {
                            pullUpEl.className = '';
                            //ullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉显示更多...';
                        }
                    },
                    onBeforeScrollStart: function (e) {
                        var nodeType = e.explicitOriginalTarget ? e.explicitOriginalTarget.nodeName.toLowerCase() : (e.target ? e.target.nodeName.toLowerCase() : '');
                        if (nodeType != 'select' && nodeType != 'option' && nodeType != 'input' && nodeType != 'textarea') {
                            e.preventDefault();
                        }

                    },
                    onScrollMove: function () {

                        if (this.y > 10 && !pullDownEl.className.match('flip')) {
                            pullDownEl.className = 'flip';
                            //pullDownEl.querySelector('.pullDownLabel').innerHTML = '准备刷新...';
                            this.minScrollY = 0;

                            //console.log(111111);
                        } else if (this.y < 5 && pullDownEl.className.match('flip')) {
                            pullDownEl.className = '';
                            //pullDownEl.querySelector('.pullDownLabel').innerHTML = '准备刷新...';
                            this.minScrollY = -pullDownOffset;
                            //console.log(222222);
                        } else if (this.y < (this.maxScrollY - 5) && !pullUpEl.className.match('flip')) {
                            pullUpEl.className = 'flip';
                            // if(!self.nomoreData){
                            // pullUpEl.querySelector('.pullUpLabel').innerHTML = '准备加载...';	
                            // }else{
                            // pullUpEl.querySelector('.pullUpLabel').innerHTML = '';
                            // }
                            //this.maxScrollY = this.maxScrollY;
                            //console.log(333333);
                        } else if (this.y > (this.maxScrollY + 5) && pullUpEl.className.match('flip')) {
                            pullUpEl.className = '';
                            //pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉显示更多...';
                            //this.maxScrollY = pullUpOffset;
                            //console.log(444444);
                        }
                        //解决按住抖动拖动的bug
                        //if (Math.abs(this.y - this.lastY) > 100) {
                        //    this.refreshScroll = true;
                        //} else {
                        //    this.refreshScroll = false;
                        //}
                        console.log(this.y);
                        if (this.y < 60) {
                            //$("#scrollContainer").css("margin-top","40px");
                        }
                    },
                    onScrollEnd: function () {
                        //console.log("lastY:"+this.y);
                        this.lastY = this.y;
                        //解决按住抖动拖动的bug
                        if (this.refreshScroll) {
                            self.refreshIscroll();
                        }
                        if (pullDownEl.className.match('flip')) {
                            //console.log("pullDownAction");
                            pullDownEl.className = 'loading';
                            //pullDownEl.querySelector('.pullDownLabel').innerHTML = 'Loading...';
                            self.pullDownAction(); // Execute custom function (ajax call?)
                        } else if (pullUpEl.className.match('flip')) {
                            //console.log("pullUpAction");
                            pullUpEl.className = 'loading';
                            document.body.scrollTop = document.body.scrollTop - this.lastY;
                            //pullUpEl.querySelector('.pullUpLabel').innerHTML = '努力加载中...';
                            self.pullUpAction(); // Execute custom function (ajax call?)
                        }
                    }

                });
            } else {
                self.storeWrapper.refresh();
            }
        }
    },
    pullDownAction: function () {
        if (document.body.scrollTop < 2) {
            this.receiveMessagemore();

        }
    },
    pullUpAction: function (callback) {
        
        if ((document.body.scrollHeight - document.body.scrollTop)<20) {
            window.scrollTo(0, document.body.scrollHeight);
        }
    },


    //加载欢迎语数据
    getWelcomelanguage: function () {
        var self = this;
        self.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            type: 'get',
            data: {
                'action': 'Customer.Basic.GetCustomerBasicSetting',
                "SettingCode": "CustomerGreeting"
            },
            beforeSend: function () {

            },
            success: function (data) {
                debugger;
                if (data.Data) {
                    var htmlStr = '<dl class="liaotl"><dt><div class="imgbg"><img src="../../../images/common/ClientService/imgbg1.png"></div><img  style=" height: 90px;width: 90px;" src="../../../images/common/ClientService/tupian.png"></dt><dd><font ></font><span ontouchstart=" window.event.stopPropagation()"  >' + data.Data.SettingValue + '</span></dd></dl>';
                    $(".liaot").html($(".liaot").html() + htmlStr);

                    self.refreshIscroll();
                    if (htmlStr != "") {
                        self.Wrapper.scrollTo(0, self.Wrapper.maxScrollY);
                    }
                }


            },
            complete: function () {


            }
        });
    },
    
    alert:function(text,callback){
        Jit.UI.Dialog({
            type : "Alert",
            content : text,
            CallBackOk : function() {
                Jit.UI.Dialog("CLOSE");
                if(callback){
                    callback();
                }
            }
        });
    }
});