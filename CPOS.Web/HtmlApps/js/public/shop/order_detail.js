Jit.AM.defindPage({
	name : 'OrderDetail',
	onPageLoad : function() {
		//当页面加载完成时触发
		Jit.log('进入OrderDetail.....');
		
		this.loadData();
		this.initEvent();
	},
	loadData:function(){
		var self = this;
		self.ajax({
			url : '/ApplicationInterface/Gateway.ashx',
			data : {
				'action' : 'Order.Order.GetOrderDetail',
				'orderId' : self.getUrlParam('orderId')
			},
			success : function(data) {				
				if (data.IsSuccess) {
					var order = data.Data.OrderListInfo,
						tpl = $('#Tpl_order_info').html(), 
						html = '';
					//评价要用的数据	
					self.standbyData=order.OrderDetailInfo;
					self.renderPage(order);

					for (var i = 0; i < order.OrderDetailInfo.length; i++) {
						var dataStr = '';
						for(var j=1;j<6;j++){
							var ary1 = 'PropName'+j,
								ary2 = 'PropDetailName'+j;
							if(order.OrderDetailInfo[i]['GG'][ary1]!='' && order.OrderDetailInfo[i]['GG'][ary2]!=''){
								dataStr += order.OrderDetailInfo[i]['GG'][ary1]+':'+order.OrderDetailInfo[i]['GG'][ary2]+';';
							}
							
						}
						order.OrderDetailInfo[i].dataStr = dataStr;
						order.OrderDetailInfo[i].clinchTime = order.ClinchTime;
						
						if(order.OrderDetailInfo[i].SalesReturnFlag>0){
							$('#goEvaluateBtn').hide();
						}
						
						order.OrderDetailInfo[i].SalesReturnFlag = order.OrderDetailInfo[i].SalesReturnFlag==0?'':2;
						//对虚拟商品的处理
						order.OrderDetailInfo[i].IfService = order.OrderDetailInfo[i].IfService==0?'':'hide';
						order.OrderDetailInfo[i].ToPage = order.OrderDetailInfo[i].IfService==0?'GoodsDetail':'VirtualDetail';
						
						
						if(order.OrderStatus===700){
							order.OrderDetailInfo[i].statusHide = '';
						}else{
							order.OrderDetailInfo[i].statusHide = 'hide';
						}
						//console.log(order.OrderDetailInfo[i]);
						html += Mustache.render(tpl, order.OrderDetailInfo[i]);
					}
					$('#order_list').append(html);
					if(data.Data.OrderReasonTypeId == '21B88CE9916A4DB4A1CAD8E3B4618C10'){
						$('.refundBtn').remove();
					}
				if(data.Data.DeliveryID==4&&(data.Data.OrderStatus==100||data.Data.OrderStatus==800)){
					$('.refundBtn').remove();
				}

					//到店服务商品，订单状态
				} else {
					self.alert(data.Message);
				}
			}
		});
	},
	initEvent:function(){
		var self = this;
		$('body').delegate('#goEvaluateBtn','click',function(){
			Jit.AM.setPageParam('evaluateGoodsData',self.standbyData);
		});
		$('#order_list').delegate('.refundBtn','click',function(){
			//Jit.AM.setPageParam('evaluateGoodsData',self.standbyData);
			var $this = $(this),
				ind = $($this.parents('li'),$('#order_list')).index();
			Jit.AM.toPage('RefundApply','&orderId='+self.getUrlParam('orderId')+'&ind='+ind);
		});
		
	},
	renderPage : function(data) {
		//debugger;
		//  状态 1未付款/2待处理/3已发货/0已取消 可为空
		var me = this;
		$('#totalprice,#ordersAmount').html("￥" + data.ActualDecimal);
		//配送费
		$("#devilyAmount,.freightBox span").html("￥"+(data.DeliveryAmount?data.DeliveryAmount:0));
		$(".orderTotalBox span").html(data.TotalAmount);
		$("#goodsAmount").html("￥"+data.Total_Retail)//商品金额不算配送费。
		//$("#goodsAmount").html("￥"+Math.subtr(data.TotalAmount,data.DeliveryAmount));
		//$("#ordersAmount").html("￥" + data.ActualDecimal);
		//$('#totalqty').html(data.TotalQty+'件');
		//$('#ordersCount').html(data.TotalQty);
		$("#returnAmount").html('-￥'+data.ReturnAmount);//返现金额
		$("#couponAmount").html("-￥" +data.CouponAmount);//优惠券
		$("#orderIntegral").html("-￥"+data.UseIntegralToAmount);//+"("+data.OrderIntegral+")"积分
		$("#vipEndAmount").html("-￥" +data.VipEndAmount);//余额
		var accDiv = Math.div(data.OrderDetailInfo[0].DiscountRate,100),
			totalNum = Math.subtr(data.TotalAmount,data.DeliveryAmount),
			couponNum = Math.subtr(totalNum,data.CouponAmount),//减优惠券
			returnNum = Math.subtr(couponNum,data.ReturnAmount),//减返现
			vipEndNum = Math.subtr(returnNum,data.VipEndAmount),//减余额
			integralNum = Math.subtr(vipEndNum,data.UseIntegralToAmount),//减余额
			accDiscount = Math.mul(accDiv,data.Total_Retail),
			discountAmount = '-￥'+parseFloat(Math.subtr(data.Total_Retail,accDiscount)).toFixed(2);
		$("#vipDiscount").html(discountAmount);//会员折扣
		$('#createTime').html(data.OrderDate);
		//货到付款
		if(data.PaymentTypeCode=="GetToPay"){
			$("#getToPay").show();
			if(data.IsPayment!="1"){
				$('#paystatus').html('未支付(货到付款)');
			}else{
				$('#paystatus').html('已支付(货到付款)');
			}
		}else{
			if(data.IsPayment!="1"){
			
				$('#paystatus').html('未支付');
				
			}else{
			
				$('#paystatus').html('已支付');
			}
		}
		data.status = parseInt(data.Status);
		
		$('.expressName').text(data.CarrierName || '无');
		$('.expressNo').text(data.CourierNumber || '无');
		
		$('#paymentText').text(data.PaymentTypeName || '无');
		$('#invoiceText').text(data.Invoice || '无');
		$('#remarkText').text(data.Remark || '无');////备注信息
		

		// 提货信息
		// $('#linkMan').html(data.linkMan);
		// $('#linkTel').html(data.linkTel);
		// $('#addr').html(data.address);
		// $('#date').html(data.receiptTime);

		var tpl = '',
			html = '' ;

		if(data.DeliveryID==1){

		    tpl = $("#TplReceipt").html();
		    $(".deliveryInfo").show();


		}else if(data.DeliveryID==2 || data.DeliveryID==4){
			if(data.DeliveryID==2){
				data.DeliveryName = "提货人";
				data.DeliveryTime = "提货时间";
			}else if(data.DeliveryID==4){
				data.DeliveryName = "下单人";
				data.DeliveryTime = "服务时间";
			}
		    tpl = $("#TplPickup").html();
		    $(".deliveryInfo").hide();
		}
	/*	if(data.DeliveryID==4&&(data.OrderStatus==100||data.OrderStatus==800)){
		 data.hideStatus = "hide";
		 data.statusHide = "hide";
		 }
		 if(data.OrderStatus == "300" || data.OrderStatus == "400"){
		 data.hideStatus = "hide";
		 me.getTimeRun(data);
		 }else{
		 data.statusHide = "hide";
		 }*/
		//默认预约改期的功能都不显示。
		data.statusHide = "hide";
		data.hideStatus = "hide";
		if((data.DeliveryID==4||data.DeliveryID==2)&&data.OrderStatus==100){
			data.hideStatus = "hide";
			data.statusHide = "";

		}
		//审核通过的三种状态520 510 410    700是未完成的单子
	    if((data.OrderStatus==520||data.OrderStatus==510||data.OrderStatus==410)&&data.status!=700){
			data.statusHide = "hide";  //联系客服改期
			data.hideStatus = ""
		}
      /* if(data.statusHide =="hide"){   //联系客服改期 和预约改期只能显示一个 联系客服级别高于预约改期
		   data.hideStatus="";
	   } else{
		   data.hideStatus="hide";
	   }*/
		if(data.DeliveryID==1||data.status == 800){//送货上门，没有预约改期功能。和已经取消的订单。
			data.hideStatus = "hide";
			data.statusHide = "hide";
		}
		if(data.statusHide!="hide"){   //如果预约改期按钮显示绑定时间选择控件。
			me.getTimeRun(data);
		}
		html = Mustache.render(tpl, data);



		//$('#deliveryContianer').html(html);
		$('.consigneeInfo').html(html);

		//$('#orderStatus').html(data.StatusDesc);



		/*if(data.OrderStatus==800 || data.OrderStatus==900){
			//已结束订单
			return;
		}*/

		var htmls = [];
		if(data.IsPayment == '0'){
			//未付款订单
			if(data.status==100 || data.status==500 || data.status==510 || data.status==410){

				htmls.push('<a href="javascript:JitPage.ProcessOrder(\''+data.OrderID+'\',800);" class="grey" id="orderCancel">取消订单</a>');
				
				if(!data.PaymentTypeCode){
					htmls.push('<a href="javascript:Jit.AM.toPage(\'OrderPay\',\'orderId='+data.OrderID+'\');" class="red" id="orderCancel">确认付款</a>');
				}

			}else {
				if (!(data.status == 800 || data.status == 900)) {
					htmls.push('<a href="javascript:Jit.AM.toPage(\'OrderPay\',\'orderId=' + data.OrderID + '\');" class="red" id="orderCancel">确认付款</a>');
				}
			}
			
			$('#btnList').html(htmls.join(''));
			
		}else if(data.IsPayment == '1'){
			//已付款订单
			if(data.status==100 || data.status==500 || data.status==510){

				htmls.push('<a href="javascript:JitPage.ProcessOrder(\''+data.OrderID+'\',800);" class="grey" id="orderCancel">取消订单</a>');

			}else if(data.status==600 || data.status==610){
				
				htmls.push('<a href="javascript:JitPage.ProcessOrder(\''+data.OrderID+'\',700);" class="red" id="orderCancel">确认收货</a>');
			}else if(data.status==700){
				if(data.IsEvaluation){
					htmls.push('<a href=javascript:; class="red">已评价</a>');
				}else{
					htmls.push('<a href=javascript:Jit.AM.toPage("MallComment","orderId='+data.OrderID+'"); class="red" id="goEvaluateBtn">去评价</a>');
				}
			}

			$('#btnList').html(htmls.join(''));
		}
		//货到付款				

		if(data.PaymentTypeCode=="GetToPay"){
			//让确认付款隐藏
			$("#btnList").find(".red").hide();
		}
	},
	//获取配送日期和时间
    getTimeRun: function(dataOrder){
    	var me = this;    	
    	me.ajax({
    		url: "/ApplicationInterface/Gateway.ashx",
    		data: {
    			'action': 'Order.Delivery.GetOrderDeliveryDateTimeRange',
    			'DeliveryId': dataOrder.DeliveryID
    		},
    		success: function(data){   			
    			var result = data.Data;
    			me.setFetchTime(result,dataOrder);
    		}
    	})
    },
    //初始化日期插件
    setFetchTime: function(dataType,dataOrder){
    	var me = this;
		var data = dataType.DateRange;
    	var a = [],
    	b = {},
    	t = [];
    	for(var i = 0;i<data.length;i++){
    		var c = {
    			"data": data[i].dataRange,"value": i
    		}
    		a.push(c);
    		for(var k = 0;k < data[i].timeRange.length;k++){
    			var y = {
	    			"data": data[i].timeRange[k],"value": k
	    		}
    			t.push(y)
    		}
     		b[i] = t; 
     		t = [];
    	}
    	var area1 = new LArea();
    	area1.init({
	        'trigger': '.visityTime',
	        'keys': {
	            id: 'value',
	            name: 'data'
	        },
	        'type': 2,
	        'data': [a, b],
	        'callBack': function(dateText,timeText){	
        		var fetchDate = dateText;
        		var fetchTime = timeText;
        		var meDataOder = dataOrder;
        		var receiveAddress = meDataOder.DeliveryAddress + meDataOder.CarrierName;
        		me.ajax({
					url:'/ApplicationInterface/Gateway.ashx',
					data:{
						'action': 'Order.Delivery.UpdateOrderDeliveryInfo',
						'OrderID': meDataOder.OrderID, //订单Id
						'DeliveryTypeID': meDataOder.DeliveryID,
						'StoreID': meDataOder.StoreID,//me.orderInfo.StoreID老的me.orderInfo.CarrierID
						'ReceiverAddress': receiveAddress,
						'Mobile': meDataOder.Mobile,
						'ReceiverName': meDataOder.ReceiverName,
						'PickingDate':fetchDate,
						'PickingTime':fetchTime
					},
					success:function(data){						
						if(data.IsSuccess){
							var str = "提货时间：" + fetchDate +" "+ fetchTime;
							$(".deliveryTime").html(str);
						}else{
							Jit.UI.Loading(false);
							me.alert(data.Message);
						}
					}
				});
        	}
	    });
    },
	ProcessOrder:function(OrderID,ActionCode){
    	var self =this;
    	if(!OrderID){
    		self.alert("订单ID不能为空！");
    		return false;
    	}
    	if(!ActionCode){
    		self.alert("操作码错误！");
    		return false;
    	}
    	if(ActionCode==800){
    		self.confirm("确认取消订单？",function(){
				//self.ProcessAction(OrderID,ActionCode);
				self.submitOrderCancel(OrderID);
    		});
    	}else if(ActionCode==700){
    		self.confirm("确认您已收货？",function(){
				self.ProcessAction(OrderID,ActionCode);
    		});
    	}else{
    		self.ProcessAction(OrderID,ActionCode);
    	}
    },
    //新的取消订单接口
    submitOrderCancel:function(OrderID){
    	var self =this;

    	self.ajax({
            url: '/ApplicationInterface/Vip/VipGateway.ashx',
			interfaceType:'Product',
            data: {
                action: "SetCancelOrder",
                OrderId:OrderID
            },
            beforeSend: function() {

				Jit.UI.AjaxTips.Tips({show:true,tips:"正在操作"});
            },
            success: function(data) {

                if(data.IsSuccess){
					Jit.AM.toPage('MyOrder');
                	//location.reload();
                }else{

                	self.alert(data.Message);
                }                
            },
            complete:function(){

				Jit.UI.AjaxTips.Tips({show:false,tips:"正在操作"});	
            }
        });	
    },
    ProcessAction:function(OrderID,ActionCode){
    	
    	var self =this;

    	self.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
			interfaceType:'Product',
            data: {
                action: "Order.Order.ProcessAction",
                OrderID:OrderID,
                ActionCode:ActionCode
            },
            beforeSend: function() {

				Jit.UI.AjaxTips.Tips({show:true,tips:"正在操作"});
            },
            success: function(data) {

                if(data.IsSuccess){

                	location.reload();
                }else{

                	self.alert(data.Message);
                }                
            },
            complete:function(){

				Jit.UI.AjaxTips.Tips({show:false,tips:"正在操作"});	
            }
        });	
    },
    confirm:function(text,OKCallback,CancelCallback){
		Jit.UI.Dialog({
			'type': 'Confirm',
			'content': text,
			'LabelOk': '确认',
			'LabelCancel': '取消',
			'CallBackOk': function() {
				if(OKCallback){
					Jit.UI.Dialog("CLOSE");
					OKCallback();
				}
			},
			'CallBackCancel': function() {
				if(CancelCallback){
					Jit.UI.Dialog("CLOSE");
					CancelCallback();
				}else{
					Jit.UI.Dialog("CLOSE");
				}
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