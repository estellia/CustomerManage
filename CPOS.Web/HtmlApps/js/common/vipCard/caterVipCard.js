Jit.AM.defindPage({
    elems:{},
	hideMask:function(){
        $('#masklayer').hide();
    },
    initWithParam:function(param){
    	this.param=param;
		//console.log(param);
        if(!!!param.vipCardImg){
            param.vipCardImg="../../../images/common/vipCard/defaultCard.png";
        }
        //根据config中参数设置客户的会员卡图片
        $('[name=vipCardImg]').attr('src',param.vipCardImg);
        //设置字体颜色
		if(param.infoColor){$('.vipClipBanner .infoWrap').css('color',param.infoColor)};
    },
	onPageLoad: function () {
        var that=this;
        this.initPage();
        this.initEvent();
    },
    initPage:function(){
		this.checkIsRegister();
		Jit.WX.OptionMenu(false);
    },
    initEvent:function(){
        var that=this;
        //跳转页面事件
        $("#gotoPage").delegate(".menuItem",this.eventType,function(){
        	var $this=$(this);
        	var num=$this.find(".num").html();
        	var money=$("[info=vip_balance]").html();
			var vipNo=$('[info=vip_phone]').html();
        	var href=$this.attr("data-href");
        	var args="";
        	if(href=="MyScore"){
        		args+="num="+num;
        	}
        	if(href=="Balance"){
        		args+="money="+money;
        	}
        	Jit.AM.toPage(href,'vipNo='+vipNo);
        });
        //点击领取事件
        //Jit.AM.toPage('UserRegister');
		
		//添加点击效果
        $("body").delegate('.togo',this.eventType,function(){
        	var $this=$(this);
        	var parent=$this.parent();
        	parent.addClass("on");
        	that.timerID=setTimeout(function(){
        		parent.removeClass("on");
        		clearTimeout(that.timerID);
        		location.href=$this.attr("$data-href");
        	},200);
        });
        
    },
    //判断是否是注册会员
    checkIsRegister:function(){
        var me = this;
        me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.GetMemberInfo',
                'VipSource':3
            },
            success: function (data) {
                me.hideMask();
				me.updateVipCardPageInfo(data.Data.MemberInfo);
				/*
				//判断是否注册
                if(data.ResultCode == 330 || (data.Data.MemberInfo.Status==1)){
					Jit.UI.Prompt({
						'title':'亲，你还没有注册哦！',
						'des':'您现在可以：',
						'html':'<a href=""; class="btn01" style="width:70%;">无卡，注册</a>'
					});
                }else if(data.ResultCode == 0 && data.Data && data.Data.MemberInfo.Status==2){
					$('#getCard').hide();
                }
				*/
            }
        });
    },
    updateVipCardPageInfo: function(info){
        $('.vipRealName').html(info.VipRealName||info.VipName);//会员名称
		$('.vipLevelName').html(info.VipLevelName);//卡的等级类型
		//微信头像
		if(info.ImageUrl){
			$('.headPic img').attr('src',info.ImageUrl);
        }
		//会员电话
		if(info.Mobile && info.Mobile.length==11){
			var mobileStr = info.Mobile,
				newMobileStr = '';
			for(var i=0;i<3;i++){
				if(i==0){
					newMobileStr += mobileStr.substr(0,3)+'-';
				}else if(i==1){
					newMobileStr += mobileStr.substr(3,4)+'-';
				}else if(i==2){
					newMobileStr += mobileStr.substr(7,4);
				}
			}
			$('.vipPhone').attr('href','tel:'+mobileStr).html(newMobileStr);
		}
		//已注册==已认证
		if(info.Status==2){
			//$('.authItem').attr('onclick','javascript:;');
			$('.isAuthTag').text('已认证');
		}
		
        //$('[info=vip_balance]').html(info.Balance+'.00');//余额
        $('[info=vip_coupon]').html(info.CouponsCount);//优惠券
		$('[info=vip_returnAmount]').html(info.ReturnAmount);//返现
        $('[info=vip_integration]').html(info.Integration);//积分
       
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
    }

});