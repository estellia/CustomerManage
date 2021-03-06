﻿/*所有的图片浏览特效*/
Jit.AM.defindPage({
    name: 'Introduce',
    elements: {
        intoListArea: ''
    },
    onPageLoad: function() {
        //当页面加载完成时触发
        Jit.log('页面进入' + this.name);
        this.initLoad();
        this.initEvent();

    }, //加载数据
    initLoad: function() {
        var self = this;
        self.elements.intoListArea = $('#intoListArea');
        var curWindow=$(window);
        // self.elements.intoListArea.find('img').height(curWindow.height());
    },

    //绑定事件
    initEvent: function() {
        var self = this;
        var keys = {
            toWidth: 'width',
            toHeight: 'height',
            curWidth: 'curwidth',
            curHeight: 'curheight'
        };
        // Jit.UI.DomView.init(['#subInto1', '#subInto2', 
        // '#subInto3','#subInto4','#subInto5',
        // '#subInto6','#subInto7','#subInto8']);
       var type=Jit.AM.getUrlParam("type");
        var data={
        	itemList:self.pageParam.itemList
        };
        if(self.pageParam.hasFooter==undefined||self.pageParam.hasFooter){
        	$(".footer").show();
        }else{
        	$(".footer").hide();
        }
		var html=bd.template("tmplImgs",data);
		$("#intoListArea").html(html);
		 Jit.UI.DomView.init(".intoListArea");
		
        $('.into_sub_text').bind('touchstart', function() {
			
            var el = $(this),
                toWidth = el.data(keys.toWidth),
                toHeight = el.data(keys.toHeight),
                curWidth = el.data(keys.curWidth),
                curHeight = el.data(keys.curHeight);

            if (el.hasClass('on')) {
                el.animate({
                    width: curWidth,
                    height: curHeight
                }, 300);
                el.removeClass('on');
            } else {
                el.animate({
                    width: toWidth,
                    height: toHeight
                }, 300);
                el.addClass('on');
            }

        });
    }

});