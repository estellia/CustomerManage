Jit.AM.defindPage({
    name: 'ZhaoPin',
	initWithParam: function(param){
		//$("#wrapper").html("<img class='img' src="+param.mainBg+" />");
	},
    onPageLoad: function() {
    	this.initData();
    },
    initData:function(){
        var title = '有正念，有梦想',
			desc= '加入正念团队做梦想合伙人吧！',
			link = location.href,
			imgUrl = 'http://'+location.host+'/HtmlApps/images/special/zhaopin/titPic.jpg';
        Jit.WX.shareFriends(title,desc,link,imgUrl);
        Jit.WX.shareTimeline(title,desc,link,imgUrl);
    }
});