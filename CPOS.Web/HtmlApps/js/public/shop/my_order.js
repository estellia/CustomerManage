Jit.AM.defindPage({

    name: 'my_order',

	ele:{
		pullDown:document.getElementById('pullDown'),
		pullUp:document.getElementById('pullUp'),
		orderList:$('#orderList')
	},
	page:{
		'pageIndex':0,
		'pageSize':10,
		'totalPage':0
	},
    onPageLoad: function() {
		var self = this;
		if(Jit.AM.getUrlParam('orderType')=="appointment") {
			var $body = $('body')
			document.title ="我的预约";
// hack在微信等webview中无法修改document.title的情况
			var $iframe = $('<iframe src="/favicon.ico"></iframe>').on('load', function() {
				setTimeout(function() {
					$iframe.off('load').remove()
				}, 0)
			}).appendTo($body)
			      self.getAppointment();

		}else {
			self.standbyData = [];
			if (Jit.AM.getUrlParam('channelId') == 6) {
				$('#topNav').hide();
				$('body').prepend('<input id="channelIdSize" type="hidden" value="6">');
			}
			//当页面加载完成时触发
			Jit.log('进入' + this.name);
			template.isEscape = false;
			//debugger;
			this.initEvent();
			this.loadData();
			var type = Jit.AM.getUrlParam('groupingType');
			if (type) {
				var timer = setTimeout(function () {
					$('#jsOrderStateList li[data-code' + type + ']').trigger('click');
					clearTimeout(timer);
				}, 200);
			}
		}
		
		
    },
    initEvent: function() {
        var self = this;
        $("#jsOrderStateList li").click(function(){
        	if(!self.isSending){
        		var $this=$(this);  
	        	self.ele.orderList.html("");
	        	$this.addClass("cur").siblings().removeClass("cur");
	        	self.GroupingTypeId = $this.data("code");
	        	if($this.find("span").attr("id") == "waitCount2"){
	        		$(".completeOrder").hide();
	        		$(".order_list,.myorder").addClass("marginTop");
	        		$(".acceptanceOrder").show();
	        		//alert($this.find("span").attr("id") == "waitCount2" || $this.find("span").attr("id") == "waitCount5")
	        	}else if($this.find("span").attr("id") == "waitCount5"){
	        		$(".acceptanceOrder").hide();
					$(".order_list,.myorder").addClass("marginTop");
	        		$(".completeOrder").show();
	        	}else{
					$(".order_list,.myorder").removeClass("marginTop");
	        		$(".acceptanceOrder").hide();
	        		$(".completeOrder").hide();
	        	}
	        	if(self.tabFlag){
	        		clearTimeout(self.tabFlag);
	        	}
	        	self.tabFlag = setTimeout(function(){      	
		    		self.nomoreData = false;
		    		self.page.pageIndex = 0;
		    		self.GetOrders();
	        	},300);
        	}
        });
        //带服务
        $(".acceptanceOrder ul li").click(function(){        	
        	var $this=$(this);
        	$this.addClass("changeColor").siblings().removeClass("changeColor");
        	self.GetOrders();
        });
        //完成
        $(".completeOrder ul li").click(function(){
        	var $this=$(this);
        	$this.addClass("changerColor").siblings().removeClass("changerColor");
        	self.GetOrders();
        });
		//点击评价
		$('body').delegate('.goEvaluateBtn','click',function(){
			Jit.AM.setPageParam('evaluateGoodsData','');
			var $this = $(this),
				orderId = $this.parents('.my_order_list').data('orderid'),
				objData = self.standbyData,
				index = $('.goEvaluateBtn').index($this),
				order = objData[index];
			if($this.text()=='已评价'){
				return false;
			}
			if($this.data("type")=="verify"){  //调用交易完成的接口，然后跳转评价页面
				JitPage.ajax({
					type:"get",
					url: '/ApplicationInterface/Gateway.ashx',
					interfaceType:'Product',
					data: {
						action: "Order.Order.ProcessAction",
						OrderID:orderId,
						ActionCode:700
					},
					beforeSend: function() {
						Jit.UI.AjaxTips.Tips({show:true,tips:"正在操作"});
					},
					success: function(data) {
						if(data.IsSuccess){
							for (var i = 0; i < order.OrderDetails.length; i++) {
							 var dataStr = '';
							 for(var j=1;j<6;j++){
							 var ary1 = 'PropName'+j,
							 ary2 = 'PropDetailName'+j;
							 if(order.OrderDetails[i]['GG'][ary1]!='' && order.OrderDetails[i]['GG'][ary2]!=''){
							 dataStr += order.OrderDetails[i]['GG'][ary1]+':'+order.OrderDetails[i]['GG'][ary2]+';';
							 }

							 }
							 order.OrderDetails[i].dataStr = dataStr;
							 order.OrderDetails[i].clinchTime = order.OrderDate;
							 order.OrderDetails[i].ImageInfo = [];
							 order.OrderDetails[i].ImageInfo[0]={};
							 order.OrderDetails[i].ImageInfo[0].ImageUrl = order.OrderDetails[i].ImageUrl;

							 }
							 Jit.AM.setPageParam('evaluateGoodsData',order.OrderDetails);
							 Jit.AM.toPage('MallComment','orderId='+orderId);

						}else{
							self.alert(data.Message);
						}
					},
					complete:function(){
						Jit.UI.AjaxTips.Tips({show:false,tips:"正在操作"});
					}
				});



				return false
			}

			for (var i = 0; i < order.OrderDetails.length; i++) {
				var dataStr = '';
				for(var j=1;j<6;j++){
					var ary1 = 'PropName'+j,
						ary2 = 'PropDetailName'+j;
					if(order.OrderDetails[i]['GG'][ary1]!='' && order.OrderDetails[i]['GG'][ary2]!=''){
						dataStr += order.OrderDetails[i]['GG'][ary1]+':'+order.OrderDetails[i]['GG'][ary2]+';';
					}
					
				}
				order.OrderDetails[i].dataStr = dataStr;
				order.OrderDetails[i].clinchTime = order.OrderDate;
				order.OrderDetails[i].ImageInfo = [];
				order.OrderDetails[i].ImageInfo[0]={};
				order.OrderDetails[i].ImageInfo[0].ImageUrl = order.OrderDetails[i].ImageUrl;
				
			}
			Jit.AM.setPageParam('evaluateGoodsData',order.OrderDetails);
			Jit.AM.toPage('MallComment','orderId='+orderId);
			
		});
		//取消申请
		$('#orderList').delegate('.cancelRefundBtn','click',function(event){
			var $this = $(this),
				$thisParent = $this.parents('li'),
				refundId = $thisParent.data('refundid');
			self.confirm('您确定要取消申请吗？',function(){
				self.setRefundApply(refundId);
			});
			event.stopPropagation();
		});
		//页面
		$('#orderList').delegate('li','click',function(event){
			var $this = $(this),
				refundId = $this.data('refundid');
			Jit.AM.toPage('RefundStatus','refundId='+refundId);	
		});
    },
    loadData:function(){
    	this.GetOrders();
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
				self.ProcessAction(OrderID,ActionCode);
    		});
    	}else{
    		self.ProcessAction(OrderID,ActionCode);
    	}
    },
    ProcessAction:function(OrderID,ActionCode){
    	var self =this;
    	//取消订单
    	if(ActionCode==800){
    		JitPage.ajax({
	    		type:"get",
	            url:"/ApplicationInterface/Vip/VipGateway.ashx",
	            data: {
	                action:"SetCancelOrder",
	                OrderID:OrderID,
	                ActionCode:ActionCode
	            },
	            beforeSend: function() {
					Jit.UI.AjaxTips.Tips({show:true,tips:"正在操作"});
	            },
	            success: function(data) {
	                if(data.IsSuccess){
	                	self.GetOrders();
	                }else{
	                	self.alert(data.Message);
	                }               
	            },
	            complete:function(){
					Jit.UI.AjaxTips.Tips({show:false,tips:"正在操作"});
	            }
	        });	
    	}else{
	    	JitPage.ajax({
	    		type:"get",
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
	                	self.GetOrders();
	                }else{
	                	self.alert(data.Message);
	                }                
	            },
	            complete:function(){
					Jit.UI.AjaxTips.Tips({show:false,tips:"正在操作"});	
	            }
	        });
	     }
    },
    GetOrders:function(callback,type){
    	var self =this,
    		GroupingTypeId = type||self.GroupingTypeId||$("#jsOrderStateList li.cur").data("code"),
        	orderList = self.ele.orderList;
        	if(GroupingTypeId == "10" || GroupingTypeId == "11" || GroupingTypeId == "12"){
        		GroupingTypeId = $(".acceptanceOrder ul li.changeColor").data("code");
        	}else if(GroupingTypeId == "13" || GroupingTypeId == "14" || GroupingTypeId == "2"){
        		GroupingTypeId = $(".completeOrder ul li.changerColor").data("code");
        	}
        	console.log(GroupingTypeId)
		//加载更多的时候需要追加数据	
		if(!self.page.pageIndex){
			self.standbyData = [];
		}
		if(GroupingTypeId!=4){
			var isRefund = "false";
	    	if(!self.isSending){
	    		//获取各项分类下面数据个数,当前分类数据
	    		JitPage.ajax({
		            url: '/ApplicationInterface/Gateway.ashx',
					interfaceType:'Product',
		            data: {
		                action: "VIP.Order.GetOrders",
		                GroupingType:GroupingTypeId,
		                PageIndex: self.page.pageIndex,
		                PageSize: self.page.pageSize
		            },
		            beforeSend: function(){
		            	self.isSending = true;
		            	if(self.page.pageIndex==0){
		            		orderList.html('<div class="order_mod order_goods_list">正在加载,请稍后...</div>');
		            	}
		            },
		            success: function(data) {
		                if(data.IsSuccess){
		                	//渲染订单状态
		                	if (data.Data.GroupingOrderCounts && data.Data.GroupingOrderCounts.length > 0) {
			                    self.renderOrderStatus(data.Data.GroupingOrderCounts);
			                }
			                
		                	//渲染订单数据
		                	if (data.Data.Orders && data.Data.Orders.length > 0) {
			                    self.renderOrderList(data.Data.Orders,GroupingTypeId,isRefund);
			                    
			                    if(data.Data.Orders.length!=self.page.pageSize){
			                    	self.nomoreData = true;
			                    }
			                } else {
			                	self.nomoreData = true;
			                	if(self.page.pageIndex==0){
			                		orderList.html('<div style="padding-top:100px;text-align:center;">亲，您还没有订单哦！</div>');
			                	}
			                }
							
							//评价要用的数据
							self.standbyData = self.standbyData.concat(data.Data.Orders);
	    					self.refreshIscroll();
		                }else{
		                	self.alert(data.Message);
		                }       
		            },
		            complete:function(){
		            	self.isSending = false;
		            	self.GroupingTypeId = GroupingTypeId;
		            }
		        });
	    	}
    	}else{
    		self.getRefundList();
    	}
    },

	//获取预约的订单
	getAppointment:function(callback){
	$(".nav_myorder").hide();
		$("#orderPanel").css({"padding-top":"0"})
		var self=this,
			orderList = self.ele.orderList;
		JitPage.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			interfaceType: 'Product',
			data: {
				action: "VIP.Order.GetOrders",
				GroupingType: 10,
				IsShowUnitName:1,//是否返回门店名称
				PageIndex: self.page.pageIndex,
				PageSize: self.page.pageSize
			},
			beforeSend: function () {
				self.isSending = true;
				if (self.page.pageIndex == 0) {
					orderList.html('<div class="order_mod order_goods_list">正在加载,请稍后...</div>');
				}
			},
			success: function (data) {
				if (data.IsSuccess) {
					//渲染订单状态
					if (data.Data.GroupingOrderCounts && data.Data.GroupingOrderCounts.length > 0) {
						self.renderOrderStatus(data.Data.GroupingOrderCounts);
					}

					//渲染订单数据
					if (data.Data.Orders && data.Data.Orders.length > 0) {
						//self.renderOrderList(data.Data.Orders, GroupingTypeId, isRefund);
						var itemlists=data.Data.Orders;
						for(var i in itemlists){

							itemlists[i]['OrderDate'] = itemlists[i]['OrderDate'].substr(0,itemlists[i]['OrderDate'].length-3);
						}
						var html=template.render('tplListItemAppointment',{itemlists:itemlists});
						if(self.page.pageIndex == 0){
							self.ele.orderList.html(html);
						}else{
							self.ele.orderList.append(html);
						}
						if(Jit.AM.getUrlParam('channelId') == 6){
							//$('#scrollContainer').css('margin-top','-40px');
							$('#orderList .ogl_handle').hide();
							$('.my_order_list>a').attr('href','javascript:;');
						}
						if (data.Data.Orders.length != self.page.pageSize) {
							self.nomoreData = true;
						}
					} else {
						self.nomoreData = true;
						if (self.page.pageIndex == 0) {
							orderList.html('<div style="padding-top:100px;text-align:center;">亲，您还没有订单哦！</div>');
						}
					}
					/*
					 //评价要用的数据
					 self.standbyData = self.standbyData.concat(data.Data.Orders);*/
					self.refreshIscroll();
				} else {
					self.alert(data.Message);
				}
			},
		});
	},
    //退换货数据
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
						$('#orderList').html('<p style="padding-top:100px;text-align:center;">亲，您暂时还没有退换货列表哦！</p>');
						return false;
					}
					if(result.length!=that.page.totalPage){
						that.nomoreData = true;
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
						htmlStr +=template('tplListItem',resultData);
					}
					if(that.page.pageIndex==0){
						$('#orderList').html(htmlStr);
					}else{
						$('#orderList').append(htmlStr);
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
    renderOrderList:function(itemlists,type,isRefund){
        for(var i in itemlists){

            itemlists[i]['OrderDate'] = itemlists[i]['OrderDate'].substr(0,itemlists[i]['OrderDate'].length-3);
        }

		var html=template.render('tplListItem',{itemlists:itemlists,GroupingTypeId:type,isRefund:isRefund});
    	if(this.page.pageIndex == 0){
        	this.ele.orderList.html(html);
    	}else{
        	this.ele.orderList.append(html);
    	}
		if(Jit.AM.getUrlParam('channelId') == 6){
			//$('#scrollContainer').css('margin-top','-40px');
			$('#orderList .ogl_handle').hide();
			$('.my_order_list>a').attr('href','javascript:;');
		}
    },
    //提示订单数量
    renderOrderStatus:function(statusList){
		var state_1 = 0 , state_2 = 0 , state_3 = 0,state_4 = 0,state_5 = 0 ,
			state_6 = 0 , state_7 = 0,state_8 = 0,state_9 = 0,state_10 = 0,state_11 = 0;
        for (var i = 0; i < statusList.length; i++) {
            var statusInfo = statusList[i];
			if(statusInfo.GroupingType == 1){
				state_1 = statusInfo.OrderCount;
			}else if(statusInfo.GroupingType == 10){
				state_2 = statusInfo.OrderCount;				
			}else if(statusInfo.GroupingType == 11){
				state_3 = statusInfo.OrderCount;
			}else if(statusInfo.GroupingType == 12){
				state_4 = statusInfo.OrderCount;
			}else if(statusInfo.GroupingType == 13){
				state_5 = statusInfo.OrderCount;
			}else if(statusInfo.GroupingType == 14){
				state_6 = statusInfo.OrderCount;
			}else if(statusInfo.GroupingType == 2){
				state_7 = statusInfo.OrderCount;
			}else if(statusInfo.GroupingType == 3){
				state_8 = statusInfo.OrderCount;
			}else if(statusInfo.GroupingType == 5){
				state_9 = statusInfo.OrderCount;
			}
        }
        state_10 = state_2 + state_3 + state_4;
        state_11 = state_5 + state_6 + state_7;
        $('#waitCount1').html(state_1);//代付款总数
        $('#waitCount2').html(state_10);//代受理总数
        $('#waitCount3').html(state_8);//已完成总数
        $('#waitCount4').html(state_9);//退换货总数
        $('#waitCount5').html(state_11);//待确认总数
        $('#waitCount6').html(state_2);//待服务
        $('#waitCount7').html(state_3);//待提货
        $('#waitCount8').html(state_4);//待发货
        $('#waitCount9').html(state_5);//以服务
        $('#waitCount10').html(state_6);//已提货
        $('#waitCount11').html(state_7);//已发货
        
        if(state_7>0){
			$('#waitCount11').show();
		}else{
			$('#waitCount11').hide();
		}
        if(state_6>0){
			$('#waitCount10').show();
		}else{
			$('#waitCount10').hide();
		}
        if(state_5>0){
			$('#waitCount9').show();
		}else{
			$('#waitCount9').hide();
		}
        if(state_4>0){
			$('#waitCount8').show();
		}else{
			$('#waitCount8').hide();
		}
        if(state_3>0){
			$('#waitCount7').show();
		}else{
			$('#waitCount7').hide();
		}
        if(state_2>0){
			$('#waitCount6').show();
		}else{
			$('#waitCount6').hide();
		}
		if(state_1>0){
			$('#waitCount1').show();
		}else{
			$('#waitCount1').hide();
		}
		if(state_10>0){
			$('#waitCount2').show();
		}else{
			$('#waitCount2').hide();
		}
		if(state_8>0){
			$('#waitCount3').show();
		}else{
			$('#waitCount3').hide();
		}
		if(state_9>0){
			$('#waitCount4').show();
		}else{
			$('#waitCount4').hide();
		}
		if(state_11>0){
			$('#waitCount5').show();
		}else{
			$('#waitCount5').hide();
		}
    },
    refreshIscroll:function(){
		var self = this;
		if(null!=document.getElementById("scrollContainer")){
			if (!self.storeWrapper) {
                var pullDownEl = self.ele.pullDown;
                var pullUpEl = self.ele.pullUp;
                var pullDownOffset = pullDownEl.offsetHeight;
                var pullUpOffset = pullUpEl.offsetHeight;
                
                
				self.storeWrapper = new iScroll('scrollContainer', {
					hScrollbar : false,
					vScrollbar : false,
					onRefresh : function() {
                    	if (pullDownEl.className.match('loading')) {
                            pullDownEl.className = '';
                            //pullDownEl.querySelector('.pullDownLabel').innerHTML = '下拉刷新...';
                        } else if (pullUpEl.className.match('loading')) {
                            pullUpEl.className = '';
                            //ullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉显示更多...';
                        }
                    },
                    onBeforeScrollStart:function(e){
						var nodeType = e.explicitOriginalTarget ? e.explicitOriginalTarget.nodeName.toLowerCase():(e.target ? e.target.nodeName.toLowerCase():'');
					    if(nodeType !='select'&& nodeType !='option'&& nodeType !='input'&& nodeType!='textarea'){
					    	e.preventDefault();
					    } 
					               
					}, 
                    onScrollMove : function() {
                        if (this.y > 5 && !pullDownEl.className.match('flip')) {
                                pullDownEl.className = 'flip';
                                //pullDownEl.querySelector('.pullDownLabel').innerHTML = '准备刷新...';
                                this.minScrollY = 0;
                        } else if (this.y < 5 && pullDownEl.className.match('flip')) {
                                pullDownEl.className = '';
                                //pullDownEl.querySelector('.pullDownLabel').innerHTML = '准备刷新...';
                                this.minScrollY = -pullDownOffset;
                        } else if (this.y < (this.maxScrollY - 5) && !pullUpEl.className.match('flip')) {
                                pullUpEl.className = 'flip';
                                // if(!self.nomoreData){
                                	// pullUpEl.querySelector('.pullUpLabel').innerHTML = '准备加载...';	
                                // }else{
                                	// pullUpEl.querySelector('.pullUpLabel').innerHTML = '';
                                // }
                                this.maxScrollY = this.maxScrollY;
                        } else if (this.y > (this.maxScrollY + 5) && pullUpEl.className.match('flip')) {
                                pullUpEl.className = '';
                                //pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉显示更多...';
                                this.maxScrollY = pullUpOffset;
                                //console.log(444444);
                        }
                        //解决按住抖动拖动的bug
                        if(Math.abs(this.y-this.lastY)>100){
                        	this.refreshScroll = true;
                        }else{
                        	this.refreshScroll = false;
                        }
                        
                        //if(this.y<60){
							//$("#scrollContainer").css("margin-top","40px");
                        //}

                        if(this.y>60){
                        	return this.y=60;
                        }
                        if(this.y<this.lastY-60){
                        	return this.y = this.lastY-60;
                        }
                    },
                    onScrollEnd : function() {
                    		//console.log("lastY:"+this.y);
                    		this.lastY = this.y;
                        	//解决按住抖动拖动的bug
                    		if(this.refreshScroll){
                    			self.refreshIscroll();
                    		}
                            if (pullDownEl.className.match('flip')) {
                            		//console.log("pullDownAction");
                                    pullDownEl.className = 'loading';
                                    //pullDownEl.querySelector('.pullDownLabel').innerHTML = 'Loading...';
                                    self.pullDownAction(); // Execute custom function (ajax call?)
                            }else if (pullUpEl.className.match('flip')) {
                            		//console.log("pullUpAction");
                                    pullUpEl.className = 'loading';
                                    //pullUpEl.querySelector('.pullUpLabel').innerHTML = '努力加载中...';
                                    self.pullUpAction(); // Execute custom function (ajax call?)
                            }
                    }
                    
				});
			} else {
				self.storeWrapper.refresh();
			}
	    }
	},
	pullDownAction:function(){
		
	},
	pullUpAction:function(callback){
		//$("#scrollContainer").css("margin-top","0");
		if(!this.isSending&&!this.nomoreData){
			this.page.pageIndex+=1;
			this.GetOrders(function(){
				if(callback){
					callback();
				}			
			});
		}else{
			this.storeWrapper.refresh();
			if(callback){
				callback();
			}
		}
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
