Jit.AM.defindPage({
    ele:{
        total:$("#total"),
        payOut:$("#payOut"),
        scoreList:$("#dataList")
    },
    page:{
        pageIndex:0,
        pageSize:10,
        allPage:2
    },
    onPageLoad: function () {
        this.initPage();
        this.initEvent();
    },
    initPage: function () {
    	//传递页面的内容
        var that = this;
        $('.dataNav ul li em').on('click',function(){
            var type = $(this).attr('data-type');
            $('.dataNav ul li em').removeClass('current');
            $(this).addClass('current');
            that.page.pageIndex = 0;
            that.getScore(parseInt(type));
        })
    },
    reachBottom:function(vars) {
	    var scrollTop = 0;
	    var clientHeight = 0;
	    var scrollHeight = 0;
	    if (document.documentElement && document.documentElement.scrollTop) {
	        scrollTop = document.documentElement.scrollTop;
	    } else if (document.body) {
	        scrollTop = document.body.scrollTop;
	    }
	    if (document.body.clientHeight && document.documentElement.clientHeight) {
	        clientHeight = (document.body.clientHeight < document.documentElement.clientHeight) ? document.body.clientHeight: document.documentElement.clientHeight;
	    } else {
	        clientHeight = (document.body.clientHeight > document.documentElement.clientHeight) ? document.body.clientHeight: document.documentElement.clientHeight;
	    }
	    scrollHeight = Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
	    if (scrollTop + clientHeight>= scrollHeight-vars) {
	        return true;
	    } else {
	        return false;
	    }
	},
    initEvent:function(){
    	var me=this;
    	$(window).bind("scroll",function(){
    		if(me.reachBottom(100)){
                if(!me.isSending){
    			    me.page.pageIndex++;
                    var type = $('.dataNav ul li em.current').attr('data-type');
    			    me.getScore(parseInt(type));
                }
    		}
    	});
        $($('.dataNav ul li em').get(0)).trigger('click');
    },
    //获得积分
    getScore:function(type){
        var self = this;
        if(self.page.pageIndex>self.page.allPage-1){
            return false;
        }
        this.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.VipIntegral.GetVipIntegralList',
				'VipCardCode':Jit.AM.getUrlParam('vipNo') || '',//会员卡号
                'IntegralType':type,
                'PageIndex':self.page.pageIndex,
                'PageSize':self.page.pageSize
            },
            beforeSend:function(){
                Jit.UI.Loading(true);
                self.isSending = true;
            },
            success: function (data) {


                if (data && data.IsSuccess) {
                    self.ele.total.html(data.Data.IncomeAmount);
                    self.ele.payOut.html(data.Data.ExpenditureAmount)
                    if( data.Data.IntegralList&&data.Data.TotalCount!=0){
                        var htmlList = template.render('tplList', {'list': data.Data.IntegralList});
                        if(self.page.pageIndex==0){
                            self.ele.scoreList.html(htmlList);
                        }else{
                            self.ele.scoreList.append(htmlList);
                        }
                    }else{
                        if(self.page.pageIndex==0){
                            self.ele.scoreList.html('<div style="width:320px;height:100px;text-align: center;position: absolute;left:50%;top:50%;margin:-50px 0 0 -160px;;">没有查找到您的积分信息!</div>');
                        }
                    }


                }else{
                    self.ele.scoreList.html('<div style="width:320px;height:100px;text-align: center;position: absolute;left:50%;top:50%;margin:-50px 0 0 -160px;;">'+data.Message+'</div>');
                }
            },
            complete:function(){
                self.isSending = false;
                Jit.UI.Loading(false);
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