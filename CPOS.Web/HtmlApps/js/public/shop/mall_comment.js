Jit.AM.defindPage({
    name: 'MallComment',
	initWithParam: function(param){
		//前端配置显示层
		//if(param['showMiddleArea']=='false'){
			//$('.noticeList').hide();
		//}
	},
    onPageLoad: function () {
		var that = this,
			evaluateData = [],
			htmlStr = '';
		that.isAnonymity = 1;
		that.evaluateData = Jit.AM.getPageParam('evaluateGoodsData');
		evaluateData = that.evaluateData;
		$('.ordersTime').html('成交时间'+evaluateData[0].clinchTime);
		for(var i=0;i<evaluateData.length;i++){
			htmlStr += bd.template('tpl_evaluateList', evaluateData[i]);
		}
		$('.goodsCommentList').html(htmlStr);
        that.initEvent();
    },
    initEvent:function(){
        var that = this;
		$('#setEvaluateBtn').on('click',function(){
			that.submitEvaluate();
		})
		$('.heartBox a').on('click',function(){
			var $this = $(this),
				$thisPar = $this.parents('.heartBox'),
				index = $this.index();
			if(!$this.hasClass('on')){
				index = index+1;
			}
			$('a',$thisPar).removeClass('on');
			for(var i=0;i<index;i++){
				$('a',$thisPar).eq(i).addClass('on');
			}
			$thisPar.attr('data-val',index);
		})
		
		
		//$('.goodsCommentList .itemBox')
		$('.commentPicBox a').on('click',function(){
			var $this = $(this),
				$thisPar = $this.parent(),
				$fillComment = $('.fillComment textarea'),
				level = $this.data('level');
			if($this.hasClass('on')){
				return false;
			}else{
				$('a',$thisPar).removeClass('on');
				$this.addClass('on');
				if(level==3){
					$fillComment.text('买到就是赚到 这么好的东西系统帮你给你好评啦！');
				}else{
					$fillComment.text('');
				}
			}
		})
		
		$('.anonymitySwitch').on('click',function(){
			var $this = $(this);
			if($this.hasClass('on')){
				$this.removeClass('on');
				that.isAnonymity = 1;
			}else{
				$this.addClass('on');
				that.isAnonymity = 2;
			}
		})
		
		$('.fillComment textarea').on('focus',function(){
			var $this = $(this);
			$this.parent().addClass('on');
		})
    },
	submitEvaluate: function(){
		var that = this,
			$evaluateList = $('.goodsCommentList .itemBox'),
			evaluateData = that.evaluateData,
			evaluationInfo = [],
			starLevel1 = parseInt($('#describeSame').attr('data-val')),
			starLevel2 = parseInt($('#serveManner').attr('data-val')),
			starLevel3 = parseInt($('#expressSpeed').attr('data-val')),
			starLevel = starLevel1+starLevel2+starLevel3;
		if(!starLevel1){
			that.alert('亲，描述相符没有评分哦！');
			return false;
		}else if(!starLevel2){
			that.alert('亲，服务态度没有评分哦！');
			return false;
		}else if(!starLevel3){
			that.alert('亲，发货速度没有评分哦！');
			return false;
		}
		
		for(var i=0;i<evaluateData.length;i++){
			/*
			var remarkStr = '';
			for(var j=1;j<6;j++){
				var ary1 = 'PropName'+j,
					ary2 = 'PropDetailName'+j;
				if(order.OrderDetailInfo[i]['GG'][ary1]!='' && order.OrderDetailInfo[i]['GG'][ary2]!=''){
					remarkStr += order.OrderDetailInfo[i]['GG'][ary1]+':'+order.OrderDetailInfo[i]['GG'][ary2]+';';
				}
				
			}
			*/
			evaluationInfo[i] = {};
			evaluationInfo[i]['ObjectID'] = evaluateData[i].ItemID;
			evaluationInfo[i]['StarLevel'] = $('.commentBox01.on',$evaluateList.eq(i)).data('level');
			evaluationInfo[i]['Content'] = $('.fillComment textarea',$evaluateList.eq(i)).val();
			evaluationInfo[i]['Remark'] = evaluateData[i].dataStr;
		}
		
		that.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			data: {
				'action': 'VIP.Evaluation.SetEvaluationItem',
				'OrderID': Jit.AM.getUrlParam('orderId') || '',//Jit.AM.getUrlParam('unitId'), //商品，门店id 必须UnitID
				'Type': 1, //1-商品，2-门店 必须
				//'Content': '很好',  //评价内容，必须
				'StarLevel': starLevel, // 3:好评，2：中评，1：差评  总评价(没有好中差评选项时，传累计评分)必须 
				'StarLevel1': starLevel1, //描述相符
				'StarLevel2': starLevel2, //服务态度
				'StarLevel3': starLevel3, //发货速度
				'IsAnonymity': that.isAnonymity,//是否匿 1=不匿名；2=匿名
				'ItemEvaluationInfo': evaluationInfo //商品评论项
			},
			success: function (data){
				if (data.ResultCode == 0){
					//history.back();
					Jit.AM.toPage('MyOrder','groupingType=3');
					//Jit.AM.toPage('MyCarsOrder','action=finish');
					//GoodsDetail
					/*
					Jit.UI.Prompt({
						'title':'亲，您给你好评！',
						'des':'您现在可以去：',
						'html':'<a href=javascript:Jit.AM.toPage("MyCenter"); class="btn02" style="width:40%;">个人中心</a>'
					})
					*/
				}else{
					alert(data.Message);
				}
			}
		})
	},
	alert: function(msg){
		Jit.UI.Dialog({
            type: "Alert",
            content: msg,
            isDpi: 2,
            CallBackOk: function (data) {
                Jit.UI.Dialog("CLOSE");
            }
        });
	}
})