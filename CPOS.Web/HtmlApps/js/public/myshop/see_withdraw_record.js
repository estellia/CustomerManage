Jit.AM.defindPage({
	name: 'SeeWithdrawRecord',
	pageIndex: 0,
	totalPageCount: 0,
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this;
		Jit.WX.OptionMenu(false);
		that.initData();
	},
	initData: function() {
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/EveryoneSale/EveryoneGetway.ashx',
            data: {
                'action': 'GetWithdrawDeposit',
				'PageSize': 20,
				'PageIndex': that.pageIndex
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
				   that.totalPageCount = data.TotalPageCount;	
				   var  htmlStr = '<ul>';
				   		listData = data.Data.WithdrawDepositList;
				   if(!listData || !listData.length){
				   		$('.seeWithdrawRecordList').html("<div class='commonDataHint'>您还没有提现记录了！</div>");
				   }else{
				   		for(var i=0;i<listData.length;i++){
							htmlStr += bd.template('tpl_withdrawRecord', listData[i]);  	
					   }
					   htmlStr += '</ul>';
					   $('.commonDataHint').hide();
					   $('.seeWithdrawRecordList').append(htmlStr);
				   }
				}else{
				   alert(data.Message);
				}
            }
        });
		window.onscroll = function(){
			if(getScrollTop() + getWindowHeight() == getScrollHeight()){
				if(that.pageIndex<that.totalPageCount) {
                    that.pageIndex++;
                    that.initData();
                }
            }
		};
	}
});

//滚动条在Y轴上的滚动距离
function getScrollTop(){
　　var scrollTop = 0, bodyScrollTop = 0, documentScrollTop = 0;
　　if(document.body){
　　　　bodyScrollTop = document.body.scrollTop;
　　}
　　if(document.documentElement){
　　　　documentScrollTop = document.documentElement.scrollTop;
　　}
　　scrollTop = (bodyScrollTop - documentScrollTop > 0) ? bodyScrollTop : documentScrollTop;
　　return scrollTop;
}

//文档的总高度
function getScrollHeight(){
　　var scrollHeight = 0, bodyScrollHeight = 0, documentScrollHeight = 0;
　　if(document.body){
　　　　bodyScrollHeight = document.body.scrollHeight;
　　}
　　if(document.documentElement){
　　　　documentScrollHeight = document.documentElement.scrollHeight;
　　}
　　scrollHeight = (bodyScrollHeight - documentScrollHeight > 0) ? bodyScrollHeight : documentScrollHeight;
　　return scrollHeight;
}

//浏览器视口的高度
function getWindowHeight(){
　　var windowHeight = 0;
　　if(document.compatMode == "CSS1Compat"){
　　　　windowHeight = document.documentElement.clientHeight;
　　}else{
　　　　windowHeight = document.body.clientHeight;
　　}
　　return windowHeight;
}