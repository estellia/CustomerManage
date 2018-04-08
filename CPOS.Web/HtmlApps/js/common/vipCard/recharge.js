Jit.AM.defindPage({
    name: 'Recharge',
	submit: true,
	initWithParam: function(param){
		//前端配置显示层
		//if(param['showMiddleArea']=='false'){
			//$('.noticeList').hide();
		//}
	},
    onPageLoad: function () {
        this.loadData();
        this.initEvent();
    },
    initEvent:function(){
        var that=this;
        $('.moneyType').delegate('a','click',function(){
			var $this = $(this);
			if($this.hasClass('on')){
				return false;
			}
			$('.moneyType a').removeClass('on');
			$this.addClass('on');
		})
       	
		$('.paymentList').delegate('.itemBox','click',function(){
			var $this = $(this);
			if($this.hasClass('on')){
				return false;
			}
			$('.paymentList .itemBox').removeClass('on');
			$this.addClass('on');
		})
		
		
		//提交支付方式
		$('#paymentConfirmBtn').on('click',function(){
            if(that.submit) {
                that.submit=false;
                var typeCode = $('.paymentList .itemBox.on').data('typecode'),
                  	payId = $('.paymentList .itemBox.on').data('payid'),
					totalAmount = $('.moneyType a.on').data('amount'),
					returnAmount = $('.moneyType a.on').data('returnamount');
					
				if(!totalAmount){
					alert('请选择充值金额！');
					return false;
				}
				if(!payId){
					alert('请选择支付方式！');
					return false;
				}
				
                that.setPayMethod(payId,totalAmount,returnAmount,typeCode);
            }
		})
		
    },
    loadData:function(){
        var that = this,
			accountBalance = Jit.AM.getUrlParam('accountBalance') || 0;
        Jit.UI.Loading(true);

		$('#balanceSum').html('￥'+accountBalance);
        that.ajax({
            url: '/ApplicationInterface/Services/ServiceGateway.ashx',
            data: {
                'action': 'GetRechargeStrategy'
            },
            success: function (data) {
				var strHtml = '',
					result = data.Data.RechargeStrategyList;
				for(var i=0;i<result.length;i++){
					strHtml += '<a class="'+(i==1?'on':'')+'" href="javascript:;" data-amount='+result[i].RechargeAmount+'   data-returnamount='+result[i].GiftAmount+'>'+result[i].RechargeAmount+'元<span class="cashTip">返现'+result[i].GiftAmount+'元</span></a>';
				}
				$('.moneyType').html(strHtml);
            }
        });
		
		
		that.ajax({
            url: '/Interface/data/OrderData.aspx',
            data: {
                'action': 'GetPaymentListBycId'
            },
            success: function (data) {
				if(data.code=='200'){
					var listData = data.content.paymentList,
						htmlStr = '';
					for(var i=0;i<listData.length;i++){
						if(listData[i].paymentTypeCode=='GetToPay'){
							continue;
						}
						htmlStr += bd.template('tpl_payWayList', listData[i]);
					}
					$('.paymentList').html(htmlStr);
					
				}else{
					alert(data.description);
				}
				Jit.UI.Loading(false);
            }
        });
		
		
		
		
		
    },
	setPayMethod: function(payId,amount,returnAmount,typeCode){
		var that = this;
        Jit.UI.Loading(true);
		//if(Jit.AM.getUrlParam('wxPay')==1){
			//continueExpensesStatus = 1;
		//}
		
        that.ajax({
            url: '/ApplicationInterface/EveryoneSale/EveryoneGetway.ashx',
            data: {
                'action': 'SetRecharge',
				'OrderDesc':'充值',	//String	订单描述，默认传“充值“
				'Amount':amount,		//Decimal	充值金额
				'ReturnAmount':returnAmount,	//Decimal	返现金额
				'PayerID':'',		//String	支付人ID(扩展，以前可存代付人OpenID，可为空)
				'PayID':payId			//String	支付方式ID
            },
            success: function (data){
                Jit.UI.Loading(false);
				if(data.ResultCode==0){
					//Jit.AM.setPageParam('orderID',data.Data.OrderID);//vipCardId
                    //var typeCode = $('.payWayList .payWay.on').data('typecode');
                    if(typeCode=='WXJS'){
                        debugger;
                        that.weixinPayment(data.Data,payId);
                    }else if(typeCode=='AlipayWap'){
						window.location = data.content.PayUrl;
					}
				}else if(data.ResultCode==340){
					that.weixinPayment(data.Data,payId);
					/*
					Jit.UI.Prompt({
						'title':'提示',
						'des':data.Message,
						'html':'<a href=javascript:Jit.AM.toPage("MyCenter"); class="btn02" style="width:70%;">个人中心</a>'
					});
					*/
				}else{
					alert(data.Message);
                    that.submit=true;
				}
            }
        });
	},
    weixinPayment:function(Data,payId){
        var me=this;
        var baseInfo=me.getBaseInfo();
        var toUrl="http://"+location.host+"/HtmlApps/auth.html?pageName=PaySuccess&eventId="+baseInfo.eventId+"&customerId="+baseInfo.customerId+"&openId="+baseInfo.openId+"&userId="+baseInfo.userId+"&orderId"+payId;
        //var isvipCard="";
        //me.setParams('VipCard_ID'+baseInfo.userId,Data.OrderID);
		/*
         if(Data.VipCardInfo.VipRealName == ''){
             isvipCard="PapersInfo";
         } else if(Data.VipAddressList.length == 0){
             isvipCard= "FamilyAddress"
         }else{
             isvipCard= "Appointment"
         }
		*/
        var toUrl="http://"+location.host+"/HtmlApps/html/public/shop/pay_success.html?customerId="+me.getUrlParam('customerId');
		
        me.totalAmount=Data.Amount;
		
        //var payType=payId;
        //"orderPay",
        var hashdata = {
            action: 'setPayOrder',
            paymentId: payId,
            orderID: Data.OrderID,
            returnUrl: toUrl,
            mobileNo: payId == '7730ABEECF3048BE9E207D7E83C944AF' ?"": "", //为实现 语音支付  phonenum.val();
            amount:me.totalAmount,
            actualAmount:me.totalAmount,
            dataFromId:2
        };
        //hashdata.orderID=Data.VipCardInfo.VipCardID;
        me.ajax({
            url: '/Interface/data/OrderData.aspx',
            data: hashdata,
            success: function(data) {
                if (data.code == "200" && parseInt(payId) != 2) {
						
                    if(payId == 'DFD3E26D5C784BBC86B075090617F44B'){
						
                        var wxpackage = JSON.parse(data.content.WXPackage);
                        WeixinJSBridge.invoke('getBrandWCPayRequest',wxpackage,function(res){
                            if(res.err_msg == "get_brand_wcpay_request:ok" ) {
								Jit.AM.toPage('MyCenter');
                                //window.location = toUrl+"&isVipCard="+isvipCard;
                            }else if(res.err_msg=="get_brand_wcpay_request:cancel"){
                            }else{
                                alert("支付失败");
                                me.submit=true;
                            }
                        });

                    }
                }else{
                    alert(data.description);
                    me.submit=true;
                }
            }
        });
    }
})