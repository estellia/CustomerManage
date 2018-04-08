Jit.AM.defindPage({
    elems: {
    },
    onPageLoad:function(){
        var that=this;
        that.initPage();
    },
    initWithParam:function(param){
		Jit.WX.OptionMenu(false);
    },
    initPage: function () {
        var that = this;
        that.getVipCardType();
        that.initEvent();
        //that.checkIsRegister();
    },
    initEvent:function(){
        var that=this;

        $('.vipCardList').delegate('.radioBox','click',function(){
            var $this = $(this),
                isBool = $this.hasClass('on'); 
            if(isBool){
                //return false;
                $this.removeClass('on');
            }else{
                $('.radioBox').removeClass('on');
                $this.addClass('on');
            }
        });

        $('.bindCardBtn').on('click',function(){
            var id = $('.radioBox.on').data('id'),
                vid = $('.radioBox.on').data('vid');
            if(id){
                that.updateVipCard(id,vid);
            }else{
                that.hintTips('请选择实体卡');
            }
        });

        $('.unBindCardBtn').on('click',function(){
            var registerType = Jit.AM.getUrlParam('registerType') || '';
            if(registerType==1){
                that.hintTips('领取成功');
                setTimeout(function(){
                    Jit.AM.toPage('GetVipCard');
                },1500);
            }else{
                Jit.AM.toPage('GetVipCard');
            }
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
                    var result = data.Data.VipCardTypeList;
                    var htmlList = template.render('tpVipCardList', {
                        'datas': result
                    });
                    $('.vipCardList').html(htmlList);
                }else{
                    that.hintTips(data.Message);
                }
            }
        });
    },
    updateVipCard:function(id,vid){
        var that = this;
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.VipGolden.UpdateVipCardType',
                "VipCardTypeID":id,
                "BindVipID":vid
            },
            success: function (data) {
                if(data.ResultCode==0){
                    that.hintTips('绑定成功');
                    setTimeout(function(){
                        Jit.AM.toPage('GetVipCard');
                    },1500);
                }else{
                    that.hintTips(data.Message);
                }
            }
        });

    },
    //判断是否是注册会员
    checkIsRegister:function(){
        var that = this;
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.GetMemberInfo',
                'VipSource':3
            },
            success: function (data){
                if(data.ResultCode == 330 || (data.Data && data.Data.MemberInfo && data.Data.MemberInfo.Status==1)){
					//未注册
                    
                }else if(data.ResultCode == 0 && data.Data && data.Data.MemberInfo.Status==2){
					//已注册
                    var info = data.Data.MemberInfo;
                }
            }
        });
    },
    //提示内容
    showTips:function(content){
    	Jit.UI.Dialog({
            'content':content+"",
            'type': 'Alert',
            'CallBackOk': function () {
                Jit.UI.Dialog('CLOSE');
            }
        });
    	return false;
    },
    hintTips:function(content){
        Jit.UI.Dialog({
            'content':content,
            'type': 'Dialog',
            'isDpi':1,
            'times': 1500
        });
    }
});