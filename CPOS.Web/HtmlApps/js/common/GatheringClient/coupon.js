/*定义页面*/
Jit.AM.defindPage({
	name: 'CouponDraw',
	elements: {
	    giverId: '',  //分享者id
	    ObjectId: '', //优惠卷标识
	    couponId: '',  //优惠卷id
	    SetoffToolID:'', //工具id
	    paraTmp: '',  //8位的随机数
	    info:[],
		PlatformType:3 //二维码来源统计。默认3会员 4 超级分销商 1 员工  2是客服；
	},
	share: {
	    title: document.title ? document.title : "优惠卷分享", //分享标题
	    desc: "赶快来看看吧",    //分享描述
	    imgUrl: ""   //分享图片
	},
	onPageLoad: function() {
		//当页面加载完成时触发
	    Jit.log('进入CouponDraw.....');

		this.initData();


	},
	initData: function() {
	    var that = this;
	    that.elements.giverId = that.getUrlParam('giverId');
		that.elements.ShareVipId = that.getUrlParam('ShareVipId');

	    that.elements.couponId = that.getUrlParam('couponId');
	    that.elements.SetoffToolID = that.getUrlParam('SetoffToolID');
	    that.elements.info = Jit.AM.getBaseAjaxParam();
		$(".attentionBtn").on("click", function () {
			var couponId =that.elements.couponId ;
			Jit.AM.toPage('UserRegister', "couponId=" + couponId);
		});
	    that.GetSetoffToolDetail(function (_data) {
	        if(_data.ServiceLife&&Number(_data.ServiceLife)>0){
				$(".couponValidity").html("有效期为 : "+_data.ServiceLife+"天")

			}else{
				$("#txtBeginDate").html(_data.StartTime);
				$("#txtEndDate").html(_data.EndTime);

			}

	        $(".couponName").html(_data.Name);
	        that.elements.paraTmp = _data.paraTemp;
	        $(".QRcode img").attr("src", _data.imageUrl);
	        $(".qrAttentionBtn img").attr("src", _data.imageUrl);


	        that.share.imgUrl = _data.imageUrl;
	        that.share.title = _data.Name;
	      /*  if (that.elements.giverId && that.elements.giverId != "") {    //没有注册

	            $(".bottomPicBox").show();

	        } else {
	            $(".couponInfo").show();
	        }*/
			$(".bottomPicBox").show();
			$(".qrAttentionBtn").hide();
			$(".regInfo").hide();
			that.ajax({
				url: '/ApplicationInterface/Gateway.ashx',
				data: {
					'action': 'VIP.Login.GetMemberInfo',
					'ToolType': "Coupon",
					'SetoffToolID': that.elements.SetoffToolID,
					'ShareVipId': that.elements.giverId,
					'ObjectId': that.elements.couponId,
					'PlatformType': 1,
					'VipDCode':5
				},
				success: function (data) {
					if (data.IsSuccess && data.Data) {

						if(data.Data.MemberInfo&&data.Data.MemberInfo.Status==1) { //已关注未注册
							$(".qrAttentionBtn").hide();
							$(".regInfo").show();
						} else{

							$(".qrAttentionBtn").show();
							$(".regInfo").hide();

						}


					} else {
						alert(data.Message);
					}
				}
			});
			/*if(that.getUrlParam('pushType')&&that.getUrlParam('pushType')!=""){    //如果是app推送的显示注册按钮


			}*/

	        that.initIsShareEvent();
	        if (!that.elements.giverId) {

	         }

			//pushType=IsSuperRetail

	        //等待扫一扫

			if(that.getUrlParam('pushType')=='IsSuperRetail'){

				that.elements.PlatformType=4;
			}
	        QrTimer = setInterval(function () {
	            that.getDimensionalCodeByVipInfo(function (_data1) {
	                if (_data1.ResultCode != 0) {
	                    clearInterval(QrTimer);
	                    Jit.UI.Dialog({
	                        type: "Alert", content: _data1.Message,
	                        CallBackOk: function (data) {
	                            Jit.UI.Dialog("CLOSE");

	                        }
	                    });
	                }
	            });
	        }, 3000)

	    });

	    

	   

	},
    //获取优惠卷信息及二维码信息及分享二维码
	GetSetoffToolDetail: function (callback)
	{
	    var that = this;
	    that.ajax({
	        url: '/ApplicationInterface/Gateway.ashx',
	        data: {
	            'action': 'VIP.VipGolden.GetSetoffToolDetail',
	            'ToolType': "Coupon",
	            'SetoffToolID': that.elements.SetoffToolID,
	            'ShareVipId': that.elements.ShareVipId,
	            'ObjectId': that.elements.couponId,
	            'PlatformType': 1,
	            'VipDCode':5
	        },
	        success: function (data) {
	            if (data.IsSuccess && data.Data) {
	                var data = data.Data;
	                if (callback) {
	                    callback(data);
	                }


	            } else {
	                alert(data.Message);
	            }
	        }
	    });

	},
   
	initIsShareEvent: function () {
	    var that = this;
	    var info = Jit.AM.getBaseAjaxParam();
		var urlLink=location.href.replace("&pushType=IsAppPush","");
	    var shareInfo = {
	        'title': "快来成为我的集客小伙伴！",
	        'desc': "打开后识别二维码，加入微信并注册会员，成为我的小伙伴，带你集客带你飞~",
	        'link': urlLink + "&giverId=" + info.userId,
	        'isAuth': true,//需要高级auth认证
	        "imgUrl": location.origin+"/HtmlApps/images/common/GatheringClient/CouponImage.jpg",
	        'Authparam': [{ "paramname": "ObjectID", "paramvalue": that.elements.couponId }]
	    };
        
	    Jit.WX.shareFriends(shareInfo);
	    Jit.WX.shareTimeline(shareInfo);
	},
    //扫描二维码触发事件
    getDimensionalCodeByVipInfo: function (callback) {
    var that = this;
    that.ajax({
        url: '/ApplicationInterface/Stores/StoresGateway.ashx',
        data: {
            'action': 'getDimensionalCodeByVipInfo',
            'special': {
                'ObjectID': that.elements.couponId,
                'Mode': "Coupon",
                'PlatformType':  that.elements.PlatformType,
                'paraTmp': that.elements.paraTmp
            }
            
        },
        success: function (data) {
            //Jit.UI.Loading(false);
            if (data) {
                if (data.IsSuccess && data.Data) {
                    if (callback) {
                        callback(data);
                    }


                } else {
                    clearInterval(QrTimer);
                    Jit.UI.Dialog({
                        type: "Alert", content: data.Message,
                        CallBackOk: function (data) {
                            Jit.UI.Dialog("CLOSE");

                        }
                    });
                }
            }

               
        }
    });
    }
  
});