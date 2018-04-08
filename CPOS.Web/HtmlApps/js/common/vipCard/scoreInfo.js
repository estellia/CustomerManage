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
        that.getScore();
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
    			    me.getScore();
                }
    		}
    	});
    },
    //获得积分说明
    getScore:function(){
        var self = this;
        if(self.page.pageIndex>self.page.allPage-1){
            return false;
        }
        this.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.VipGolden.GetVipIntegralInfoList'				
            },
            beforeSend:function(){
                Jit.UI.Loading(true);
                self.isSending = true;
            },
            success: function (data) {


                if (data && data.IsSuccess) {
                    var list = data.Data.VipIntegralInfoList;
                    for(var i=0;i<list.length;i++){
                        if(list[i].CustomerBaseSettingDesc == '积分抵扣比例'){
                            if(list[i].CustomerBaseSettingValue=="0"){
                                $('.dataMessage').eq(0).find('li').eq(0).hide();    
                            }else{
                                $('.dataMessage').eq(0).find('span').eq(0).html(list[i].CustomerBaseSettingValue);   
                            }
                        }else if(list[i].CustomerBaseSettingDesc == '账户积分累计满'){
                            if(list[i].CustomerBaseSettingValue=="0"){
                                $('.dataMessage').eq(0).find('li').eq(1).hide();    
                            }else{
                                $('.dataMessage').eq(0).find('span').eq(1).html(list[i].CustomerBaseSettingValue);
                            }
                        }else if(list[i].CustomerBaseSettingDesc == '每单可使用积分上限'){
                            if(list[i].CustomerBaseSettingValue=="0"){
                                $('.dataMessage').eq(0).find('li').eq(2).hide();  
                            }else{
                                $('.dataMessage').eq(0).find('span').eq(2).html(list[i].CustomerBaseSettingValue);
                                
                            }
                        }else if(list[i].CustomerBaseSettingDesc == '积分有效期'){
                            if(list[i].CustomerBaseSettingValue=="0"){
                                 $('.dataMessage').eq(0).find('li').eq(3).hide();  
                                   
                            }else{
                                $('.dataMessage').eq(0).find('span').eq(3).html(list[i].CustomerBaseSettingValue);
                            }
                        }else if(i=4){
                            if(list[i].CustomerBaseSettingValue=="0"){
                                 $('.dataMessage').eq(1).find('li').eq(0).hide(); 
                            }else{
                                $('.dataMessage').eq(1).find('li').eq(0).html(list[i].CustomerBaseSettingDesc);
                            }
                        }
                    }
                    if(list.length<5){
                        $('.dataHeader').eq(1).hide();
                        $('.dataMessage').eq(1).hide();
                    }
                    self.ele.total.html(data.Data.IncomeAmount);
                    self.ele.payOut.html(data.Data.ExpenditureAmount)
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