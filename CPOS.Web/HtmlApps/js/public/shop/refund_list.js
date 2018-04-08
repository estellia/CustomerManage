Jit.AM.defindPage({
    name: 'RefundList',
	page:{
		'pageIndex':0,
		'totalPage':0,
		'pageSize':20
	},
    onPageLoad: function() {
        this.loadData();
		this.initEvent();
    },
    loadData:function(){
    	var that = this;
		that.getRefundList();
    },
	initEvent: function() {
        var that = this;
		$('#refundOrderList').delegate('.cancelRefundBtn','click',function(event){
			var $this = $(this),
				$thisParent = $this.parents('li'),
				refundId = $thisParent.data('refundid');
			that.confirm('您确定要取消申请吗？',function(){
				that.setRefundApply(refundId);
			});
			event.stopPropagation();
		});
		
		$('#refundOrderList').delegate('li','click',function(event){
			var $this = $(this),
				refundId = $this.data('refundid');
			Jit.AM.toPage('RefundStatus','refundId='+refundId);	
		});
		
		$(window).bind('scroll',function(){
			if(Jit.UI.ReachBottom(20) && that.page.pageIndex<that.page.totalPage){
				that.getRefundList();
			}
		});
    },
    getRefundList:function(){
    	var that =this;
    	that.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			data: {
				"action": "Order.SalesReturn.GetSalesReturnList",
				"PageIndex": that.page.pageIndex,
				"PageSize": that.page.pageSize
			},
			beforeSend: function() {
				//self.isSending = true;
				//if(that.page.pageIndex==0){
					//$('#refundOrderList ul').html('<div class="order_mod order_goods_list">正在加载,请稍后...</div>');
				//}
			},
			success: function(data) {
				if(data.IsSuccess){
					var result = data.Data.SalesReturnList,
						htmlStr = '';
					that.page.totalPage = data.Data.TotalPageCount;
					if(result.length==0){
						$('#refundOrderList ul').html('<p style="padding-top:100px;text-align:center;">亲，您暂时还没有退换货列表哦！</p>');
						return false;
					}
					for(var i=0;i<result.length;i++){
						var resultData = result[i],
							SkuDeta = resultData.SkuDetail,
							status = resultData.Status,
							servicesType = resultData.ServicesType,//1=退货；2=换货
							dataStr = '';
								
						if(status == 1){
							resultData.Status = '待审核';
						}else if(status == 2){
							resultData.Status = '已取消';
						}else if(status == 3){
							resultData.Status = '审核不通过';
						}else if(status == 4){
							resultData.Status = '待发货';
						}else if(status == 5){
							resultData.Status = '拒绝收货';
						}else if(status == 6){
							resultData.Status = (servicesType == 2 ? '已完成' : '待退款');
						}else if(status == 7){
							resultData.Status = (servicesType == 2 ? '已完成' : '已退款');
						}
						
						if(!!SkuDeta){
							for(var j=1;j<4;j++){
								var ary1 = 'PropName'+j,
									ary2 = 'PropDetailName'+j;
								if(SkuDeta[ary1]!='' && SkuDeta[ary2]!=''){
									dataStr += SkuDeta[ary1]+':'+SkuDeta[ary2]+';';
								}
							}
						}
						resultData.SkuDetail = dataStr;
						htmlStr += bd.template('tplRefundList',resultData);
					}
					if(that.page.pageIndex==0){
						$('#refundOrderList ul').html(htmlStr);
					}else{
						$('#refundOrderList ul').append(htmlStr);
					}
					that.page.pageIndex++;
				}else{
					alert(data.Message);
				}
			},
			complete:function(){
				
			}
		});
    },
	setRefundApply:function(id){
    	var that =this;
    	that.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			data: {
				"action": "Order.SalesReturn.SetSalesReturn",
				'OperationType':2,	//Int 是	操作类型（1=申请；2=取消申请；3=确认收货；4=确认退款；5=App退换货）
				"SalesReturnID": id //退换货申请ID
			},
			beforeSend: function() {
			},
			success: function(data) {
				if(data.IsSuccess){
					$("li[data-refundid='"+id+"'] .cancelRefundBtn").addClass('on');
					$("li[data-refundid='"+id+"'] .refundMark span").text('已取消');
				}else{
					alert(data.Message);
				}
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
