Jit.AM.defindPage({
    name: 'RefundStatus',
	initWithParam: function(param){
		//前端配置显示层
		//if(param['showMiddleArea']=='false'){
			//$('.noticeList').hide();
		//}
	},
    onPageLoad: function () {
		var that = this,
			$oddNum = $('.refundMark strong'),//退换数量
			$oddStatus = $('.refundMark span'),//退换状态
			$deliveryWay = $('.deliveryWay'),//配送方式
			$reasonText = $('.reasonText'),//退换原因
			$statusTagArea = $('.statusTagArea'),
			$tagBox = $('.tagBox'),
			$dateTime = $('.dateTime'),//时间节点
			$expText = $('.expText'),//进度描述
			$refundCause = $('.refundCause'),
			$storeInfo = $('.storeInfo'),
			$name = $('.storeInfo .name'),
			$addr = $('.storeInfo .addr'),
			$tel = $('.storeInfo .tel'),
			htmlStr = '';
		that.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			data: {
				'action': 'Order.SalesReturn.GetSalesReturnDetail',
				'SalesReturnID': Jit.AM.getUrlParam('refundId') || ''
			},
			success: function (data){
				if (data.ResultCode == 0){
					var result = data.Data,
						servicesType = result.ServicesType,//1=退货；2=换货
						historyData = result.HistoryList;
					if(result.Status==1){
						result.Status = '待审核';
					}else if(result.Status==2){
						result.Status = '已取消';
					}else if(result.Status==3){
						result.Status = '审核不通过';
					}else if(result.Status==4){
						result.Status = '待发货';
					}else if(result.Status==5){
						result.Status = '拒绝收货';
					}else if(result.Status == 6){
						result.Status = (servicesType == 2 ? '已完成' : '待退款');
					}else if(result.Status == 7){
						result.Status = (servicesType == 2 ? '已完成' : '已退款');
					}
					
					$oddNum.text(result.SalesReturnNo);
					$oddStatus.text(result.Status);
					$deliveryWay.text(result.DeliveryType==1?'快递送回':'送回门店');
					$reasonText.text(result.Reason);
					
					if(result.DeliveryType==1){
						$refundCause.hide();
						$name.text(result.UnitName);
						$addr.text(result.Address);
						$tel.text(result.UnitTel);
					}else{
						$storeInfo.hide();
					}

					htmlStr = bd.template('tplRecordList',result);
					$statusTagArea.html(htmlStr);
					
				}else{
					alert(data.Message);
				}
			}
		})
		
        that.initEvent();
    },
    initEvent:function(){
        var that = this;
		
    }
})