Jit.AM.defindPage({
    elems: {
        leadCardBefore : $('#leadCardBefore'),
        leadCardAfter : $('#leadCardAfter'),
        vipCardPic : $('.vipCardPic'),
        vipName : $('.vipName span'),
        vipNo : $('.vipNo span'),
        balanceMoney : $('#balanceMoney'),//余额
        bonusMoney : $('#bonusMoney'),//红利
        integralCount : $('#integralCount'),//积分 
        cartFixedCount : $('.cartFixedBtn span'),//购物车数
        showSetPass:$("#showSetPass"), //显示设置密码
        mask:$("#mask")
    },
    message: {
        PageIndex: 0,
        PageSize:3,
        passwordFlag:0,  //是否设置支付密码  1为设置  0为未设置
        lockFlag:0,      //是否账户锁定   1为锁定  0为未锁定  
        phone:null,
        phoneStr:null
    },
    newsDetails: {
        GroupNewsID: ''
    },
    onPageLoad:function(){
        var that=this;
        this.initPage();
        this.initEvent();
    },
    initWithParam:function(param){
		Jit.WX.OptionMenu(false);
    	//this.param=param;
    },
    hideMask:function(){
        $('#masklayer').hide();
    },
    initPage: function () {
        if(window != top){
            top.location.href = location.href;
        }
        this.checkIsRegister();
        this.getVipInfo();
    },
    initEvent:function(){
        var that=this;

        $('.boundCardItem a').on('click',function(){
            that.getVipCardType();
        });
        //查看消息
        $(".PromptMessage").on("click", function () {
            $(".nl_contents").html("");
            $('.noticeList').show();
            that.message.PageIndex = 0;
            that.GetInnerGroupNewsList(function (_data) {
                var html = template.render('tpl_noticeList', _data);
                $(".nl_contents").html(html);
            });
        });
        //设置密码
        $(".settingPass").on("click", function () {
            var balanceVal = Number($('#balanceMoney').html());
            $('#phone').val(that.message.phoneStr).attr('data-val',that.message.phone);
            if(that.message.lockFlag==0){  //账户未锁定
                //弹出支付密码的层
                that.elems.mask.show();
                that.elems.showSetPass.show();

            }else{
                that.alert("支付账户被冻结!");
            }
        });
        
        //设置密码的取消事件
        that.elems.showSetPass.find("#cancel").bind("click",function(){
            that.elems.mask.hide();
            that.elems.showSetPass.hide();
            //停止倒计时
            that.showTimer("#getCode",true); //第二个参数为是否停止定时器
        });
        //设置密码的取消事件
        that.elems.showSetPass.find("#sureSet").bind("click",function(){
            //内容验证
            var flag=that.validateData();
            if(flag){//数据合法
                var phone=$("#phone").data('val');
                var passagain=$("#passAgain").val();
                var password=$("#pass").val();
                var code=$("#code").val();
                that.setPayPass({
                    mobile:phone,
                    password:JitPage.MD5(password),
                    passAgain:JitPage.MD5(passagain),
                    authCode:code,
                    callback:function(data){
                        Jit.UI.Loading(false);
                        //设置支付密码状态成功
                        that.alert('密码设置成功');
                        that.message.passwordFlag=1;
                        that.elems.showSetPass.hide();
                        that.elems.mask.hide();
                        $("#passAgain,#pass,#code").val('');

                        //that.elems.showPay.show();
                        //me.elems.showSetPass.hide();
                    }
                });
            }
        });
        //获取验证码
        that.elems.showSetPass.find("#getCode").bind("click",function(){
            if(that.authCode==undefined||that.authCode){
                var phone=$("#phone").data('val');
                if(phone.length==0){
                    JitPage.alert("手机号不能为空!");
                    return;
                }
                if(phone.toString().length!=11){
                    JitPage.alert("手机号长度为11位!");
                    return;
                }
                if(!/^[0-9]*$/.test(phone)){
                    JitPage.alert("手机号只能为数字!");
                    return ;
                }
                that.getAuthCode(phone);
            }
        });
        //更多消息
        $(".nexticon").on("click", function () {
            ++that.message.PageIndex;
            that.GetInnerGroupNewsList(function (_data) {
                if (_data) {
                    if (_data.InnerGroupNewsList.length == 0) {
                        Jit.UI.Dialog({
                            type: "Alert",
                            content: "已经没有更多的数据了！",
                            isDpi: 1,
                            CallBackOk: function (data) {
                                Jit.UI.Dialog("CLOSE");
                            }
                        });
                    }else{
                        var html =$(".nl_contents").html();
                        html += template.render('tpl_noticeList', _data);
                        $(".nl_contents").html(html);
                        $(".nl_contents").animate({ scrollTop: ($(".nl_contents")[0].scrollHeight - $(".nl_contents").height()) });
                    }
                }
            });
        });

        //查看消息详情
        $(".nl_contents").on("click", ".infor_view", function () {
            var id = $(this).data("id");
            that.newsDetails.GroupNewsID = id;
            that.GetInnerGroupNewsById(0, function (_data) {
                if (_data.NewsInfo) {
                    that.newsDetails.GroupNewsID = _data.NewsInfo.GroupNewsId;
                    $(".nl_content[data-id='" + _data.NewsInfo.GroupNewsId + "'] .isread").remove();
                }
                var html = template.render('tpl_noticedetails', _data);
                $(".Ndetails").html(html);
                $('.noticedetails').show();
            });

        });
        //查看消息
        $(".NDetailsBtn").on("click", function () {
            var operatingtype = $(this).data("operatingtype");
            that.GetInnerGroupNewsById(operatingtype, function (_data) {
                if (_data.NewsInfo) {
                    that.newsDetails.GroupNewsID = _data.NewsInfo.GroupNewsId;
                    $(".nl_content[data-id='" + _data.NewsInfo.GroupNewsId + "'] .isread").remove();
                }
                var html = template.render('tpl_noticedetails', _data);
                $(".Ndetails").html(html);
            });
        });
        //关闭消息详情
        $(".Ndetails").on("click", ".nd_close", function () {
            $('.noticedetails').hide();
        });

    },
    getVipCardType:function(){
        var that = this;
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.VipGolden.GetVipCardType',
                "Phone":""
            },
            success: function (data) {
                if(data.ResultCode==0){
                    $('.boundCardItem a').attr('href','javascript:Jit.AM.toPage("BindEntityCard");');
                }else{
                    that.hintTips(data.Message);
                }
            }
        });
    },
    //判断是否是注册会员
    checkIsRegister:function(){
        var me = this;
        $('.noticeList').hide();
        me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.GetMemberInfo',
                'VipSource':3
            },
            success: function (data) {
                me.hideMask();
                if(data.ResultCode == 330 || (data.Data && data.Data.MemberInfo && data.Data.MemberInfo.Status==1)){
					//未注册
					$('.cartFixedBtn').hide();
                    $('.gainTntegralBtn').hide();
                    me.elems.leadCardBefore.show();
                    $('#leadCardBefore .vipCardPic').attr('src',data.Data.MemberInfo.CardTypeImageUrl || "../../../images/common/vipCard/defaultCard.png");
                }else if(data.ResultCode == 0 && data.Data && data.Data.MemberInfo.Status==2){
					//已注册
                    var info = data.Data.MemberInfo,
                        isNeedCard = info.IsNeedCard,
                        vipCardPic = vipCardImg= info.CardTypeImageUrl || "../../../images/common/vipCard/defaultCard.png";
                    me.elems.vipName.text(info.VipRealName||info.VipName);
                    me.elems.vipNo.text(info.VipNo||'卡号未知');
                    $('#balaneUrl').attr('href','javascript:Jit.AM.toPage("NewsBalance","vipNo='+info.VipNo+'")');
                    $('#myScoreUrl').attr('href','javascript:Jit.AM.toPage("MyScore","vipNo='+info.VipNo+'")');
                    me.elems.vipCardPic.attr('src',vipCardPic);//会员卡图片
                    me.elems.balanceMoney.html(info.Balance.toFixed(2));//余额
                    me.elems.bonusMoney.html(info.ProfitAmount.toFixed(2));//红利
                    me.elems.integralCount.html(info.Integration);//积分
					me.elems.leadCardAfter.show();
                    if(isNeedCard==2){
                        $('#boundCardItem').hide();
                    }
                    if(info.ShopCartCount>0){
                        me.elems.cartFixedCount.text(info.ShopCartCount);
                        $('.cartFixedBtn span').show();
                    }
                }


                //集客信息
                if (data.Data.MessageInfo){
                    if (data.Data.MessageInfo.InnerGroupNewsCount > 0) {
                        $(".ReadMessageIcorn").show();
                    } else {
                        $(".ReadMessageIcorn").hide();
                    }
                    if (data.Data.MessageInfo.SetoffToolsCount > 0) {
                        $(".myGCcount").html(data.Data.MessageInfo.SetoffToolsCount);
                        $(".myGCcount").show();
                    } else {
                        $(".myGCcount").hide();
                    }

                    var upGradeSucess = data.Data.MessageInfo.UpGradeSucess;
                    if(data.Data.MessageInfo.UpGradeSucess!=''){
                        me.hintTips(upGradeSucess);
                    }
                }
                //分销信息
                if(!!data.Data.MemberInfo.SuperRetailTraderID){
                    $('.myDistribution a').attr('href','javascript:Jit.AM.toPage("DistributionDown")');
                }

            }
        });
    },
    //提示内容
    showTips:function(content){
    	Jit.UI.Dialog({
            'content':content+"",
            'type': 'Alert',
            'isDpi':1,
            'CallBackOk': function () {
                Jit.UI.Dialog('CLOSE');
            }
        });
    	return false;
    },
    //查看消息列表数据
    GetInnerGroupNewsList: function (callback) {
        var me = this;
        Jit.UI.Loading(true);
        me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'Sys.InnerGroupNews.GetInnerGroupNewsList',
                'NoticePlatformTypeId': 1,
                'PageIndex':me.message.PageIndex,
                'PageSize': me.message.PageSize
            },
            success: function (data) {
                if (data.IsSuccess && data.ResultCode == 0) {
                    var data = data.Data;
                    if (callback) {
                        callback(data);
                    }
                } else {
                    me.showTips(data.Message);
                }
            },
            complete:function(){
                Jit.UI.Loading(false);
            }
        });
    },
    //获得会员信息
    getVipInfo:function(){
        var me=this;
        JitPage.ajax({
            url:"/ApplicationInterface/Vip/VipGateway.ashx",
            data:{
                "action":'GetVipInfo' //获得VIP action
            },
            success:function(data){
                 if (data.ResultCode == 0) {
                    me.message.phone = data.Data.Phone;
                    //表示成功
                    me.message.status=data.Data.Status;
                    //是否设置了支付密码
                    me.message.passwordFlag=data.Data.PasswordFlag;
                    //是否账户锁定
                    me.message.lockFlag=data.Data.LockFlag;
                    var balanceVal = Number($('#balanceMoney').html());
                    var phone = data.Data.Phone,
                        phoneStr = phone.substr(0,4)+'****'+phone.substr(8);
                    me.message.phoneStr = phoneStr;
                    $('#phone').val(phoneStr).attr('data-val',phone);
                    if(data.Data.EndAmount>0){
                        //弹出支付密码的层
                        if(data.Data.PasswordFlag!=1){///未设置支付密码   则弹出设置支付密码的层
                            me.elems.mask.show();
                            me.elems.showSetPass.show();
                        }
                        
                    }else{
                        $('.settingPass').hide();
                        $('.settingPassTxt').hide();
                    }
                }
            }
        });
    },
    //获得验证码
    getAuthCode:function(mobile){
        Jit.UI.Loading(true);
        var me=this;
        JitPage.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.GetAuthCode',
                'Mobile': mobile,
                'VipSource':3
            },
            success: function (data) {
                Jit.UI.Loading(false);
                if(data.ResultCode==310){
                    Jit.UI.Dialog({
                        'content': data.Message,
                        'type': 'Alert',
                        'CallBackOk': function () {
                            Jit.UI.Dialog('CLOSE');
                        }
                    });
                }else{
                    //表示已经发送   1分钟后可以再次发送
                    me.authCode=false;
                    me.showTimer("#getCode");
                }
                //需要添加60秒倒计时代码
            }
        });
    },
    //设置支付密码的验证合法性
    validateData:function(){
        var flag=true;
        var phone=$("#phone").data('val');
        if(phone.length==0){
            JitPage.alert("手机号不能为空!");
            flag=false;
            return flag;
        }
        if(phone.toString().length!=11){
            JitPage.alert("手机号长度为11位!");
            flag=false;
            return flag;
        }
        if(!/^[0-9]*$/.test(phone)){
            JitPage.alert("手机号只能为数字!");
            return false;
        }
        var code=$("#code").val();
        if(code.length==0){
            JitPage.alert("验证码不能为空!");
            flag=false;
            return flag;
        }
        if(isNaN(parseInt(code))){
            JitPage.alert("验证码只能为数字!");
            flag=false;
            return flag;
        }
        var password=$("#pass").val();
        if(password.length==0){
            JitPage.alert("密码不能为空!");
            flag=false;
            return flag;
        }
        if(password.length<6&&password>0){
            JitPage.alert("密码最少为6位!");
            flag=false;
            return flag;
        }
        if(password.length>16){
            JitPage.alert("密码最多为16位!");
            flag=false;
            return flag;
        }
        var passagain=$("#passAgain").val();
        if(passagain.length==0){
            JitPage.alert("密码不能为空!");
            flag=false;
            return flag;
        }
        if(passagain!=password){
            JitPage.alert("两次密码不一致!");
            flag=false;
            return flag;
        }
        if(passagain.length<6&&passagain>0){
            JitPage.alert("密码最少6位!");
            flag=false;
            return flag;
        }
        if(passagain.length>16){
            JitPage.alert("密码最多为16位!");
            flag=false;
            return flag;
        }
        
        return true;
    },
    //倒计时显示
    showTimer:function(id,flag){
        var count=60;
        var that=this;
        if(flag){//是否停止定时器
            $(id).html("发送验证码");
            //表示已经发送   1分钟后可以再次发送
            that.authCode=true;
            clearInterval(this.timerId);
        }else{
            this.timerId=setInterval(function(){
                if(count>0){
                    --count;
                    $(id).html("<font size='2'>"+count+"秒</font>后发送");
                }else{
                    clearInterval(that.timerId);
                    $(id).html("发送验证码");
                    //表示已经发送   1分钟后可以再次发送
                    that.authCode=true;
                }
            },1000);
        }
    },
    //查看消息详情数据
    GetInnerGroupNewsById: function (operatingtype,callback) {
        var me = this;
        Jit.UI.Loading(true);
        if (!operatingtype)
        {
            operatingtype = 0;
        }
        me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'Sys.InnerGroupNews.GetInnerGroupNewsById',
                'NoticePlatformTypeId': 1,
                'GroupNewsID': me.newsDetails.GroupNewsID,
                'Operationtype': operatingtype
            },
            success: function (data) {
                if (data.IsSuccess && data.ResultCode == 0) {
                    var data = data.Data;
                    if (callback) {
                        callback(data);
                    }
                } else {
                    me.showTips(data.Message);
                }
            },
            complete: function () {
                Jit.UI.Loading(false);
            }
        });
    },
    //设置支付密码
    setPayPass:function(obj){
        Jit.UI.Loading(true);
        var me=this;
        JitPage.ajax({
            url:"/ApplicationInterface/Vip/VipGateway.ashx",
            data:{
                "action":"SetVipPayPassword", //支付密码action
                "Mobile":obj.mobile,  //手机号
                "AuthCode":obj.authCode,  //验证码
                "Password":obj.password, //密码
                "PasswordAgain":obj.passAgain  //再次的密码
            },
            success:function(data){
                Jit.UI.Loading(false);
                if (data.ResultCode == 0) {
                    //表示成功
                    if (obj.callback) {
                        obj.callback(data);
                    }
                }
                else {
                    JitPage.alert(data.Message);
                }
                
            }
        });
    },
    hintTips:function(content){
        Jit.UI.Dialog({
            'content':content,
            'type': 'Dialog',
            'isDpi':1,
            'times': 1500
        });
        return false;
    },
    alert :function(text){
        Jit.UI.Dialog({
            type:"Dialog",
            content:text,
            isDpi:1,
            times:2000
        });
    },
    MD5 :function(string) {
        /*
         *MD5 (Message-Digest Algorithm)
         *http://www.webtoolkit.info/
         */
        function RotateLeft(lValue, iShiftBits) {
            return (lValue<<iShiftBits) | (lValue>>>(32-iShiftBits));
        }
        function AddUnsigned(lX,lY) {
            var lX4,lY4,lX8,lY8,lResult;
            lX8 = (lX & 0x80000000);
            lY8 = (lY & 0x80000000);
            lX4 = (lX & 0x40000000);
            lY4 = (lY & 0x40000000);
            lResult = (lX & 0x3FFFFFFF)+(lY & 0x3FFFFFFF);
            if (lX4 & lY4) {
                return (lResult ^ 0x80000000 ^ lX8 ^ lY8);
            }
            if (lX4 | lY4) {
                if (lResult & 0x40000000) {
                    return (lResult ^ 0xC0000000 ^ lX8 ^ lY8);
                } else {
                    return (lResult ^ 0x40000000 ^ lX8 ^ lY8);
                }
            } else {
                return (lResult ^ lX8 ^ lY8);
            }
        }
        function F(x,y,z) { return (x & y) | ((~x) & z); }
        function G(x,y,z) { return (x & z) | (y & (~z)); }
        function H(x,y,z) { return (x ^ y ^ z); }
        function I(x,y,z) { return (y ^ (x | (~z))); }
        function FF(a,b,c,d,x,s,ac) {
            a = AddUnsigned(a, AddUnsigned(AddUnsigned(F(b, c, d), x), ac));
            return AddUnsigned(RotateLeft(a, s), b);
        };
        function GG(a,b,c,d,x,s,ac) {
            a = AddUnsigned(a, AddUnsigned(AddUnsigned(G(b, c, d), x), ac));
            return AddUnsigned(RotateLeft(a, s), b);
        };
        function HH(a,b,c,d,x,s,ac) {
            a = AddUnsigned(a, AddUnsigned(AddUnsigned(H(b, c, d), x), ac));
            return AddUnsigned(RotateLeft(a, s), b);
        };
        function II(a,b,c,d,x,s,ac) {
            a = AddUnsigned(a, AddUnsigned(AddUnsigned(I(b, c, d), x), ac));
            return AddUnsigned(RotateLeft(a, s), b);
        };
        function ConvertToWordArray(string) {
            var lWordCount;
            var lMessageLength = string.length;
            var lNumberOfWords_temp1=lMessageLength + 8;
            var lNumberOfWords_temp2=(lNumberOfWords_temp1-(lNumberOfWords_temp1 % 64))/64;
            var lNumberOfWords = (lNumberOfWords_temp2+1)*16;
            var lWordArray=Array(lNumberOfWords-1);
            var lBytePosition = 0;
            var lByteCount = 0;
            while ( lByteCount < lMessageLength ) {
                lWordCount = (lByteCount-(lByteCount % 4))/4;
                lBytePosition = (lByteCount % 4)*8;
                lWordArray[lWordCount] = (lWordArray[lWordCount] | (string.charCodeAt(lByteCount)<<lBytePosition));
                lByteCount++;
            }
            lWordCount = (lByteCount-(lByteCount % 4))/4;
            lBytePosition = (lByteCount % 4)*8;
            lWordArray[lWordCount] = lWordArray[lWordCount] | (0x80<<lBytePosition);
            lWordArray[lNumberOfWords-2] = lMessageLength<<3;
            lWordArray[lNumberOfWords-1] = lMessageLength>>>29;
            return lWordArray;
        };
        function WordToHex(lValue) {
            var WordToHexValue="",WordToHexValue_temp="",lByte,lCount;
            for (lCount = 0;lCount<=3;lCount++) {
                lByte = (lValue>>>(lCount*8)) & 255;
                WordToHexValue_temp = "0" + lByte.toString(16);
                WordToHexValue = WordToHexValue + WordToHexValue_temp.substr(WordToHexValue_temp.length-2,2);
            }
            return WordToHexValue;
        };
        function Utf8Encode(string) {
            string = string.replace(/\r\n/g,"\n");
            var utftext = "";
            for (var n = 0; n < string.length; n++) {
                var c = string.charCodeAt(n);
                if (c < 128) {
                    utftext += String.fromCharCode(c);
                }
                else if((c > 127) && (c < 2048)) {
                    utftext += String.fromCharCode((c >> 6) | 192);
                    utftext += String.fromCharCode((c & 63) | 128);
                }
                else {
                    utftext += String.fromCharCode((c >> 12) | 224);
                    utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                    utftext += String.fromCharCode((c & 63) | 128);
                }
            }
            return utftext;
        };
        var x=Array();
        var k,AA,BB,CC,DD,a,b,c,d;
        var S11=7, S12=12, S13=17, S14=22;
        var S21=5, S22=9 , S23=14, S24=20;
        var S31=4, S32=11, S33=16, S34=23;
        var S41=6, S42=10, S43=15, S44=21;
        string = Utf8Encode(string);
        x = ConvertToWordArray(string);
        a = 0x67452301; b = 0xEFCDAB89; c = 0x98BADCFE; d = 0x10325476;
        for (k=0;k<x.length;k+=16) {
            AA=a; BB=b; CC=c; DD=d;
            a=FF(a,b,c,d,x[k+0], S11,0xD76AA478);
            d=FF(d,a,b,c,x[k+1], S12,0xE8C7B756);
            c=FF(c,d,a,b,x[k+2], S13,0x242070DB);
            b=FF(b,c,d,a,x[k+3], S14,0xC1BDCEEE);
            a=FF(a,b,c,d,x[k+4], S11,0xF57C0FAF);
            d=FF(d,a,b,c,x[k+5], S12,0x4787C62A);
            c=FF(c,d,a,b,x[k+6], S13,0xA8304613);
            b=FF(b,c,d,a,x[k+7], S14,0xFD469501);
            a=FF(a,b,c,d,x[k+8], S11,0x698098D8);
            d=FF(d,a,b,c,x[k+9], S12,0x8B44F7AF);
            c=FF(c,d,a,b,x[k+10],S13,0xFFFF5BB1);
            b=FF(b,c,d,a,x[k+11],S14,0x895CD7BE);
            a=FF(a,b,c,d,x[k+12],S11,0x6B901122);
            d=FF(d,a,b,c,x[k+13],S12,0xFD987193);
            c=FF(c,d,a,b,x[k+14],S13,0xA679438E);
            b=FF(b,c,d,a,x[k+15],S14,0x49B40821);
            a=GG(a,b,c,d,x[k+1], S21,0xF61E2562);
            d=GG(d,a,b,c,x[k+6], S22,0xC040B340);
            c=GG(c,d,a,b,x[k+11],S23,0x265E5A51);
            b=GG(b,c,d,a,x[k+0], S24,0xE9B6C7AA);
            a=GG(a,b,c,d,x[k+5], S21,0xD62F105D);
            d=GG(d,a,b,c,x[k+10],S22,0x2441453);
            c=GG(c,d,a,b,x[k+15],S23,0xD8A1E681);
            b=GG(b,c,d,a,x[k+4], S24,0xE7D3FBC8);
            a=GG(a,b,c,d,x[k+9], S21,0x21E1CDE6);
            d=GG(d,a,b,c,x[k+14],S22,0xC33707D6);
            c=GG(c,d,a,b,x[k+3], S23,0xF4D50D87);
            b=GG(b,c,d,a,x[k+8], S24,0x455A14ED);
            a=GG(a,b,c,d,x[k+13],S21,0xA9E3E905);
            d=GG(d,a,b,c,x[k+2], S22,0xFCEFA3F8);
            c=GG(c,d,a,b,x[k+7], S23,0x676F02D9);
            b=GG(b,c,d,a,x[k+12],S24,0x8D2A4C8A);
            a=HH(a,b,c,d,x[k+5], S31,0xFFFA3942);
            d=HH(d,a,b,c,x[k+8], S32,0x8771F681);
            c=HH(c,d,a,b,x[k+11],S33,0x6D9D6122);
            b=HH(b,c,d,a,x[k+14],S34,0xFDE5380C);
            a=HH(a,b,c,d,x[k+1], S31,0xA4BEEA44);
            d=HH(d,a,b,c,x[k+4], S32,0x4BDECFA9);
            c=HH(c,d,a,b,x[k+7], S33,0xF6BB4B60);
            b=HH(b,c,d,a,x[k+10],S34,0xBEBFBC70);
            a=HH(a,b,c,d,x[k+13],S31,0x289B7EC6);
            d=HH(d,a,b,c,x[k+0], S32,0xEAA127FA);
            c=HH(c,d,a,b,x[k+3], S33,0xD4EF3085);
            b=HH(b,c,d,a,x[k+6], S34,0x4881D05);
            a=HH(a,b,c,d,x[k+9], S31,0xD9D4D039);
            d=HH(d,a,b,c,x[k+12],S32,0xE6DB99E5);
            c=HH(c,d,a,b,x[k+15],S33,0x1FA27CF8);
            b=HH(b,c,d,a,x[k+2], S34,0xC4AC5665);
            a=II(a,b,c,d,x[k+0], S41,0xF4292244);
            d=II(d,a,b,c,x[k+7], S42,0x432AFF97);
            c=II(c,d,a,b,x[k+14],S43,0xAB9423A7);
            b=II(b,c,d,a,x[k+5], S44,0xFC93A039);
            a=II(a,b,c,d,x[k+12],S41,0x655B59C3);
            d=II(d,a,b,c,x[k+3], S42,0x8F0CCC92);
            c=II(c,d,a,b,x[k+10],S43,0xFFEFF47D);
            b=II(b,c,d,a,x[k+1], S44,0x85845DD1);
            a=II(a,b,c,d,x[k+8], S41,0x6FA87E4F);
            d=II(d,a,b,c,x[k+15],S42,0xFE2CE6E0);
            c=II(c,d,a,b,x[k+6], S43,0xA3014314);
            b=II(b,c,d,a,x[k+13],S44,0x4E0811A1);
            a=II(a,b,c,d,x[k+4], S41,0xF7537E82);
            d=II(d,a,b,c,x[k+11],S42,0xBD3AF235);
            c=II(c,d,a,b,x[k+2], S43,0x2AD7D2BB);
            b=II(b,c,d,a,x[k+9], S44,0xEB86D391);
            a=AddUnsigned(a,AA);
            b=AddUnsigned(b,BB);
            c=AddUnsigned(c,CC);
            d=AddUnsigned(d,DD);
        }
        var temp = WordToHex(a)+WordToHex(b)+WordToHex(c)+WordToHex(d);
        return temp.toLowerCase();
    }

});