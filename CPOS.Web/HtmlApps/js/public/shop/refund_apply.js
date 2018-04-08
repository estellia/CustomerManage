Jit.AM.defindPage({
    name: 'RefundApply',
	params:{
		servicesType: 1,
		deliveryType: 1,
		count: 1
	},
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
		//that.evaluateData = Jit.AM.getPageParam('evaluateGoodsData');
		//evaluateData = that.evaluateData;
		
		/*
		for(var i=0;i<evaluateData.length;i++){
			htmlStr += bd.template('tpl_evaluateList', evaluateData[i]);
		}
		$('.goodsCommentList').html(htmlStr);
		*/
		
		that.getOrderShop();
        that.initEvent();
    },
    initEvent:function(){
        var that = this,
			$numBox = $('.numBox');
		$('.plusBox').on('click',function(){
			var num = $numBox.text()-0;
			if(num==that.params.count){
				return;
			}else{
				$numBox.text(num+1);
			}
		});
		$('.minusBox').on('click',function(){
			var num = $numBox.text()-0;
			if(num==1){
				return;
			}else{
				$numBox.text(num-1);
			}
		});
		
		$('.menuTag a').on('click',function(){
			var $this = $(this);
			if($this.hasClass('on')){
				return;
			}else{
				$('.menuTag a').removeClass('on');
				$this.addClass('on');
				if($this.text()=='退货'){
					that.params.servicesType = 1;
				}else{
					that.params.servicesType = 2;
				}
			}
		});
		
		$('.footer .submitBtn').on('click',function(){
			var $this = $(this),
				reMobile = /^1\d{10}$/,
				refundCount = $('.numBox').text()-0,
				proposer = $('#proposer').val(),
				mobileNum = $('#mobileNum').val(),
				remandExp = $('.remandExp').val(),
				$evaluateList = $('.goodsCommentList .itemBox'),
				evaluateData = that.evaluateData,
				evaluationInfo = [];
			
			if(proposer==''){
				alert('亲，你还没有填写申请人!');
				return;
			}else if(mobileNum==''){
				alert('亲，你还没有填写手机号!');
				return;
			}else if(!reMobile.test(mobileNum)){
				alert('亲，请输入正确的手机号!');
				return;
			}else if(remandExp==''){
				var  connection= "亲，你还没有填写退货原因!";
				if(that.params.servicesType==2) {//Int		是	服务类型（1=退货；2=换货）
					connection='亲，你还没有填写换货原因!'
				}
				alert(connection);
				return;
			}
			Jit.UI.Loading();
			var params = {
				'action': 'Order.SalesReturn.SetSalesReturn',
				'OrderID': Jit.AM.getUrlParam('orderId') || '', //String	是	订单ID
				'OperationType':1,		//Int 是	操作类型（1=申请；2=取消申请；3=确认收货；4=确认退款；5=App退换货）
				'ServicesType':that.params.servicesType,	//Int		是	服务类型（1=退货；2=换货）
				'DeliveryType':that.params.deliveryType,	//Int		是	退回商品方式（1=送回门店；2=快递送回）
				'ItemID':that.params.itemId,				//String	是	商品ID
				'SkuID':that.params.skuId,					//String	是	最小库存ID
				'Qty':refundCount,							//Int		是	申请数量
				'UnitID':that.storeParams.StoreID,		    //String	否	门店ID
				'UnitName':that.storeParams.StoreName,		//String	否	门店名称
				'UnitTel':that.storeParams.StoreTel,		//String	否	门店电话
				'Address':that.storeParams.StoreAddress,	//String	是	门店/收件地址
				'Contacts':proposer,						//String	否	联系人
				'Phone':mobileNum,							//String	否	联系电话
				'Reason':remandExp							//String	否	退货原因
			};
			
			that.submitApply(params);
		})
    },
	getOrderShop:function(){
		var that = this,
			ind = parseInt(Jit.AM.getUrlParam('ind'));
		that.ajax({
			url : '/ApplicationInterface/Gateway.ashx',
			data : {
				'action' : 'Order.Order.GetOrderDetail',
				'orderId' : that.getUrlParam('orderId')
			},
			success : function(data) {
				if (data.IsSuccess) {
					var order =  data.Data.OrderListInfo,
						htmlStr = '';
					//评价要用的数据	
					//self.standbyData=order.OrderDetailInfo;
					//self.renderPage(order);
					that.storeParams =  order;
					
					//配送方式
					that.params.deliveryType = order.DeliveryID;
					//总件数
					that.params.count = order.OrderDetailInfo[ind].Qty;
					that.params.itemId = order.OrderDetailInfo[ind].ItemID;//商品ID
					that.params.skuId = order.OrderDetailInfo[ind].SkuID;//最小库存ID
					
					
					
					for (var i = ind; i < ind+1; i++) {
						var dataStr = '';
						for(var j=1;j<6;j++){
							var ary1 = 'PropName'+j,
								ary2 = 'PropDetailName'+j;
							if(order.OrderDetailInfo[i]['GG'][ary1]!='' && order.OrderDetailInfo[i]['GG'][ary2]!=''){
								dataStr += order.OrderDetailInfo[i]['GG'][ary1]+':'+order.OrderDetailInfo[i]['GG'][ary2]+';';
							}
						}
						order.OrderDetailInfo[i].dataStr = dataStr;
						//order.OrderDetailInfo[i].clinchTime = order.ClinchTime;
						htmlStr += bd.template('TplRefundApply',order.OrderDetailInfo[i]);
					}
					$('.refundItem').html(htmlStr);
					
					if(that.params.deliveryType == 1){
						order.DeliveryName = '快递送回';
					}else if(that.params.deliveryType == 2){
						order.DeliveryName = '送回门店';
					}
					
					$('.remandGoodsArea').html(bd.template('TplStoreInfo',order));
					//将申请人与电话从接口中带出来
					$('#proposer').val(order.ReceiverName || '');
					$('#mobileNum').val(order.Mobile || '');
					
				} else {
					alert(data.Message);
				}
			}
		});
	},
	submitApply: function(params){
		var that = this;
			
		that.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			data: params,
			success: function (data){
				if (data.ResultCode == 0){
					//Jit.AM.toPage('MyOrder','groupingType=3');
					Jit.UI.Loading(false);
					 var  connection= "亲，商品已提交退货申请！";
					if(that.params.servicesType==2) {//Int		是	服务类型（1=退货；2=换货）
						connection='亲，商品已提交换货申请！'
					}
					Jit.UI.Dialog({
						type:"Alert",
						content:connection,
						CallBackOk:function(){
							//Jit.UI.Dialog("CLOSE");
							Jit.AM.toPage('RefundList');
						}
					});
				}else{
					Jit.UI.Loading(false);
					alert(data.Message);
				}
			}
		})

	}
})