Jit.AM.defindPage({
	name: 'WithdrawDeposit',
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this;
		Jit.WX.OptionMenu(false);
		that.initData();
		that.initEvent();
	},
	originalBankInfo: {},
	loadData:{
		WithDrawNum:"",
		WithDrawNumType:"",
		MinAmountCondition:"",
		WithDrawMaxAmount:"",
		totalAmount:'',
		isSending:true
	},
	
	initData: function(){
		var that = this,
			$totalAmount = $('#totalAmount'),
			$bankCardInfo = $('#bankCardInfo'),
			$withdrawNum = $('.withdrawNum span'),
			$withdrawMonth = $('.withdrawMonth span'),
			$bankName = $('#bankCardInfo .bankName'),
			$cardNo = $('#bankCardInfo .cardNo'),
			$hintBox = $('#withdrawLayer .hintBox span');
			that.getWDBasicInfo(function(data){
				var result = data.Data,
					WithDrawNum = result.VipWithDrawRule.WithDrawNum,
					wdApplyCount = result.wdApplyCount,
					WithDrawNumType = result.VipWithDrawRule.WithDrawNumType
					WdTime = WithDrawNum - wdApplyCount;
				that.loadData.WithDrawNum = WithDrawNum;
				that.loadData.totalAmount = result.WDCurrentAmount||0.00;
				that.loadData.WithDrawNumType = WithDrawNumType;
				that.loadData.MinAmountCondition = result.VipWithDrawRule.MinAmountCondition;
				that.loadData.WithDrawMaxAmount = result.VipWithDrawRule.WithDrawMaxAmount; 
				
				if(WithDrawNum){
					if(WdTime>0){
						WdTime = WdTime;
					}else{
						WdTime = 0;
					}
					$withdrawNum.html(WdTime);
					$withdrawMonth.html(WithDrawNum);
				}else{
					WdTime = "无限制";
					$withdrawNum.parents('.withdrawNum').html("");
					$withdrawMonth.parents('.withdrawMonth').html("");
					//$withdrawNum.html(WdTime);
					//$withdrawMonth.html(WdTime);
				}
				if(that.loadData.WithDrawMaxAmount){
					$hintBox.text("每次最多可提现"+that.loadData.WithDrawMaxAmount+"元");
				}else{
					$hintBox.parents('.hintBox').html("");
				}
				var $tt = result.WDCurrentAmount;
				if(!result.WDCurrentAmount){
					$tt = 0
					$tt = $tt.toFixed(2);
				}
				$totalAmount.html($tt);//可提现金额
				//$withdrawNum.html(WdTime);//每天提现次
				//$hintBox.text(result.WDAmountDayDesc||'无');//日累计提现金额描述
				if(!result.VipWithDrawRule.MinAmountCondition){
					$('.withdrawNotice').hide();
				}
				$('.withdrawNotice').html("账户金额累计满"+result.VipWithDrawRule.MinAmountCondition+"元才可提现");
				if(!!result.VipBankID){
	 				that.originalBankInfo = result;
	 				$('#withdrawLayer .bankCardBox').addClass('isCard'); 
	 				$cardNo.html(result.CardNo.substring(result.CardNo.length-4));//银行卡号
	 				$bankName.html(result.BankName);//银行名称
	 				//VipBankID//银行标示
	 			}else{
	 				$('.inputAmountBox').hide();
					$('.withdrawMonth').hide();
					$('.withdrawNum').hide();
					$('.withdrawBtn').hide();
					$('.withdrawNotice').hide();
					$('.seeWithdraw').hide();
					$('.hintBox').hide();
	 			}
	 			if(Number(result.WDCurrentAmount)<result.VipWithDrawRule.MinAmountCondition&&Number(result.WDCurrentAmount)!=0){
	 				
					$('.withdrawBtn').addClass('disab');
					$('.withdrawNotice').show();
					that.loadData.isSending = false;
					return false;
	 			}else if(Number(result.WDCurrentAmount)==0){
	 				if(!result.VipWithDrawRule.MinAmountCondition){
	 					$('.withdrawNotice').hide();
	 				}
	 				$('.withdrawBtn').addClass('disab');
	 				that.loadData.isSending = false;
					return false;
					
	 			}
	 			if(WdTime==0){
	 				//$('.withdrawNotice').html("账户本月提现次数已用完");
					$('.withdrawBtn').addClass('disab');
					that.loadData.isSending = false;
					return false;
	 			}
			})
		// that.ajax({
  //           url: '/ApplicationInterface/EveryoneSale/EveryoneGetway.ashx',
  //           data: {
  //               'action': 'GetWDBasicInfo',
  //           },
  //           success: function (data) {
  //               //Jit.UI.Loading(false);
		// 		if(data.ResultCode == 0){
		// 			var result = data.Data;
		// 			$totalAmount.html(result.WDCurrentAmount);//可提现金额
		// 			$withdrawNum.html(result.WDTime);//每天提现次数
		// 			$hintBox.text(result.WDAmountDayDesc);//日累计提现金额描述
		// 			if(!!result.VipBankID){
		// 				that.originalBankInfo = result;
		// 				$('#withdrawLayer .bankCardBox').addClass('isCard'); 
		// 				$cardNo.html(result.CardNo.substring(result.CardNo.length-4));//银行卡号
		// 				$bankName.html(result.BankName);//银行名称
		// 				//VipBankID//银行标示
		// 			}
					
		// 		}else{
		// 			alert(data.Message);
		// 		}
				
				
  //           }
  //       });
	},
	initEvent: function() {
		var that = this,
			$withdrawLayer = $('#withdrawLayer'),
			$addBankcardLayer = $('#addBankcardLayer'),
			$modifyBankcardLayer = $('#modifyBankcardLayer');
		
		//绑定添加银行卡，修改银行卡事件	
		$('.bankCardBox',$withdrawLayer).on('click',function(){
			var $this = $(this),
				logoUrl = '../../../images/public/my_shop/icon_bankCard2.png';
			if($this.hasClass('isCard')){
				document.title = '修改银行卡';
				$withdrawLayer.hide();
				$modifyBankcardLayer.show();
				if(that.originalBankInfo.LogoUrl!=''){//洗e客修改过
					logoUrl = that.originalBankInfo.LogoUrl;
				}
				$('.bankHead img',$modifyBankcardLayer).attr('src',logoUrl);
				$('.bankName',$modifyBankcardLayer).html(that.originalBankInfo.BankName);
				$('.showCardNo',$modifyBankcardLayer).html(that.originalBankInfo.CardNo.substring(that.originalBankInfo.CardNo.length-4));
				//获取我的银行卡,添加多卡的时候用
				//that.getMyBankCard();
			}else{
				document.title = '添加银行卡';
				$withdrawLayer.hide();
				$addBankcardLayer.show();
				//获取银行列表
				that.getBankList($('.addBank select',$addBankcardLayer))
			}
		})
		
		//提交添加银行卡
		$('.confirmBtn',$addBankcardLayer).on('click',function(){
			//document.title = '提现';
			//$addBankcardLayer.hide();
			//$withdrawLayer.show();
			var bankId = $('.addBank select',$addBankcardLayer).find("option:selected").data('bankid'),
				accountName = $('.accountName',$addBankcardLayer).val(),
				cardNo =  $('.cardNo',$addBankcardLayer).val(),
				cardNoNew =  $('.cardNoNew',$addBankcardLayer).val();
				
			if(!bankId){
				return that.promptHint2('请选择添加银行卡！');
			}else if(accountName==''){
				return that.promptHint2('请输入开户人姓名！');
			}else if(!/[\u4E00-\u9FA5]{2,5}/.test(accountName)){
				return that.promptHint2('开户人姓名是2-5位汉字！');
			}else if(cardNo==''){
				return that.promptHint2('请输入储蓄卡卡号！');
			}else if(!/^[0-9]*$/.test(cardNo)){
				return that.promptHint2('储蓄卡卡号只能为数字！');
			}else if(cardNo.length < 12 || cardNo.length > 19){
				return that.promptHint2('银行卡号是12－19位数字！');
			}else if(cardNoNew==''){
				return that.promptHint2('请重输入一遍卡号！');
			}else if(cardNo != cardNoNew){
				return that.promptHint2('两次输入卡号不一致！');
			}
			that.submitAddCard(bankId,accountName,cardNo);
		})
		
		
		//提交修改银行卡
		$('.confirmModifyBtn',$modifyBankcardLayer).on('click',function(){
			//document.title = '提现';
			var bankId = $('.selectBank select',$modifyBankcardLayer).find("option:selected").data('bankid'),
				accountName = $('.accountName',$modifyBankcardLayer).val(),
				cardNo =  $('.cardNo',$modifyBankcardLayer).val(),
				cardNoNew =  $('.cardNoNew',$modifyBankcardLayer).val(),
				vipBankId = that.originalBankInfo.VipBankID;
				
			if(!bankId){
				return that.promptHint2('请选择添加银行卡！');
			}else if(accountName==''){
				return that.promptHint2('请输入开户人姓名！');
			}else if(!/[\u4E00-\u9FA5]{2,5}/.test(accountName)){
				return that.promptHint2('开户人姓名是2-5位汉字！');
			}else if(cardNo==''){
				return that.promptHint2('请输入储蓄卡卡号！');
			}else if(!/^[0-9]*$/.test(cardNo)){
				return that.promptHint2('储蓄卡号只能为数字！');
			}else if(cardNo.length < 12 || cardNo.length > 19){
				return that.promptHint2('银行卡号是12－19位数字！');
			}else if(cardNoNew==''){
				return that.promptHint2('请重输入一遍卡号！');
			}else if(cardNo != cardNoNew){
				return that.promptHint2('两次输入卡号不一致！');
			}
			that.submitAddCard(bankId,accountName,cardNo,vipBankId);
		})
		
		//显示修改银行卡输入框
		$('.modifyBankBtn',$modifyBankcardLayer).on('click',function(){
			$('.modifyBankBtn').hide();
			$('.inputModifyArea',$modifyBankcardLayer).show();
			
			//获取银行列表
			that.getBankList($('.selectBank select',$modifyBankcardLayer),function(){
				var $option = $('.selectBank option',$modifyBankcardLayer);
				for(var i=0;i<$option.length;i++){
					if($option.eq(i).text() == that.originalBankInfo.BankName){
						$option.eq(i).attr('selected','');
						break;
					}
				}
				$('.accountName',$modifyBankcardLayer).val(that.originalBankInfo.AccountName);
			})
		})

		$('#withdrawAmount').on('keyup',function(){
			var txt = $(this).val(),
			WDCurrentAmount = $('#totalAmount').html(),
			isSending = that.loadData.isSending,
			MinAmountCondition = that.loadData.MinAmountCondition,
			WithDrawMaxAmount = that.loadData.WithDrawMaxAmount;
			if(isSending){
				if(Number(txt)>Number(WithDrawMaxAmount)&&Number(txt)>Number(MinAmountCondition)&&Number(txt)>Number(WDCurrentAmount)&&Number(WDCurrentAmount)!=0){
					//$('.withdrawNotice').html("每次最多提现金额为"+WithDrawMaxAmount+"");
					//$('.withdrawNotice').hide();
					$('.withdrawBtn').addClass('disab');
				}
				else if(Number(txt)>Number(WithDrawMaxAmount)&&Number(WithDrawMaxAmount)!=0){
					$('.withdrawBtn').addClass('disab');
				}
				else if(Number(WDCurrentAmount)==0){
					$('.withdrawBtn').addClass('disab');
					$('.withdrawNotice').show();
				}else if(Number(WDCurrentAmount)<Number(MinAmountCondition)){
					//$('.withdrawNotice').html("账户金额累计满"+MinAmountCondition+"元才可提现");
					$('.withdrawBtn').addClass('disab');
					$('.withdrawNotice').show();
				}else{
					//$('.withdrawNotice').hide();
					$('.withdrawBtn').removeClass('disab');
				}
			}
		})
		
		$('.withdrawBtn',$withdrawLayer).on('click',function(){

			var withdrawAmount = $('#withdrawAmount').val(),
			    isClicked = $(this).hasClass('disab'),
				WithdrawNum =$('.withdrawNum span').html(),
				WithDrawNum = that.loadData.WithDrawNum,
				WithDrawNumTypethat = that.loadData.WithDrawNumType,
				MinAmountCondition = that.loadData.MinAmountCondition,
				WithDrawMaxAmount = that.loadData.WithDrawMaxAmount,
				totalAmount = that.loadData.totalAmount;
			if(!isClicked){	
				if(withdrawAmount<=0){
					return that.promptHint2('金额不可输入为0');
				}else if(withdrawAmount>1000){
					return that.promptHint2('请输入小于1000的金额');	
				}else if(WithdrawNum=="0"&&WithDrawNum!='0'){
					if(WithDrawNumTypethat == '1'){
						return that.promptHint2('你当天的提现次数已经用完');
					}	
					else if(WithDrawNumTypethat == '2'){
						return that.promptHint2('你当周的提现次数已经用完');
					}
					else if(WithDrawNumTypethat == '3'){
						return that.promptHint2('你当月的提现次数已经用完');
					}
				}
				else if(totalAmount < parseInt(MinAmountCondition)&&MinAmountCondition!="0"){
					return that.promptHint2('可提现金额必须大于或等于'+MinAmountCondition+'才可提现');
				}
				else if(WithdrawNum > parseInt(WithDrawMaxAmount)&&WithDrawMaxAmount!="0"){
					return that.promptHint2('请输入小于或等于'+WithDrawMaxAmount+'的金额');
				}
				that.withdrawFun(withdrawAmount);
			}
		})
	},
	getBankList: function($dom,callback){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/EveryoneSale/EveryoneGetway.ashx',
            data: {
                'action': 'GetBank',
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
					var result = data.Data.BankList,
						htmlStr = '<option>请选择开户银行</option>';
					for(var i=0;i<result.length;i++){
						htmlStr = htmlStr + '<option data-bankid='+ result[i].BankID +'>'+ result[i].BankName +'</option>';
					}
					$dom.html(htmlStr);
					if(typeof callback==="function"){
						callback();
					}
				}else{
					alert(data.Message);
				}
				
				
            }
        });
	},
	getWDBasicInfo:function(callback){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/EveryoneSale/EveryoneGetway.ashx',
            data: {
                'action': 'GetWDBasicInfo',
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
					if (callback) {
                        callback(data);
                    }
				}else{
					alert(data.Message);
				}
            }
        });
	},
	submitAddCard: function(bankId,accountName,cardNo,vipBankId){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/EveryoneSale/EveryoneGetway.ashx',
            data: {
                'action': 'UpdateVipBank',
				'BankID': bankId,
				'AccountName': accountName,
				'CardNo': cardNo,
				"VipBankID": vipBankId || '' //修改银行卡时使用
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
					location.reload();
				}else{
					alert(data.Message);
				}
            }
        });
		
		
	},
	getMyBankCard: function(){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/EveryoneSale/EveryoneGetway.ashx',
            data: {
                'action': 'GetVipBank',
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
					
					var result = data.Data.VipBankList,
						htmlStr = '';
					for(var i=0;i<result.length;i++){
						var logoUrl = '../../../images/public/my_shop/icon_bankCard2.png';
						if(result[i].LogoUrl!=''){
							logoUrl = result[i].LogoUrl;
						}
						htmlStr = htmlStr + '<div class="bankShow"><p class="bankHead"><img src="'+logoUrl+'"></p><div class="bankInfo"><p class="bankName">'+result[i].BankName+'</p><span>**************'+result[i].CardNo.substring(result[i].CardNo.length-4)+'</span></div></div>';
					}
					$('#modifyBankcardLayer').prepend(htmlStr);
				}else{
					alert(data.Message);
				}
				
				
            }
        });	
	},
	withdrawFun: function(num){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/EveryoneSale/EveryoneGetway.ashx',
            data: {
                'action': 'ApplyWithdrawDeposit',
				"VipBankID":that.originalBankInfo.VipBankID,
				"Amount":num
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
					that.promptHint('本次申请提现金额:'+num+'元<br><span>如48小时未到账，请联系客服。</span>',function(){location.reload();});
				}else{
					that.promptHint2(data.Message);
				}
				
				
            }
        });	
	},
	promptHint: function(str,callback){
		var htmlStr = '<div class="ui-mask" style="display:block;">\
		<div class="ui-dialogs ui-dialogs-prompt" style="height:390px;">\
			<h2 class="dialogs-title">提交成功</h2>\
			<p class="dialogs-text" style="height:115px;margin-top:28px;font-size:26px;color:#424649;">'+str+'</p>\
			<div class="clearfix">\
				<a class="dialogs-btn" style="width:100%;display:block;" href="javascript:;">确定</a>\
			</div>\
		</div>\
		</div>';
		$('body').append(htmlStr);
		$('body').delegate('.dialogs-btn','click',function(){
			var $this = $(this);
			$this.parents('.ui-mask').remove();
			if(typeof callback==="function"){
				callback();
			}
		});
	},
	promptHint2: function(str,callback){
		var htmlStr = '<div class="ui-mask" style="display:block;">\
		<div class="ui-dialogs ui-dialogs-prompt">\
			<p class="dialogs-text">'+str+'</p>\
			<div class="clearfix">\
				<a class="dialogs-btn" style="width:100%;display:block;" href="javascript:;">确定</a>\
			</div>\
		</div>\
		</div>';
		$('body').append(htmlStr);
		$('body').delegate('.dialogs-btn','click',function(){
			var $this = $(this);
			$this.parents('.ui-mask').remove();
			if(typeof callback==="function"){
				callback();
			}
		});
		
		
	},
});