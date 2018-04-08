Jit.AM.defindPage({

	name:'SelectAddress',
	
	onPageLoad:function(){
		if(Jit.AM.getUrlParam('channelId') == 6){
			$('#topNav').hide();
			$('.goods_wrap').css('margin-top','0px');
		}
		this.loadPageData();
		this.initEvent();
	},
	loadPageData:function(){
		Jit.UI.Loading(true);
		var me = this,
			cityCode = '-99',
			positionStr = '',
			point = new BMap.Point(116.331398,39.897445),
			geolocation = new BMap.Geolocation();
		geolocation.getCurrentPosition(function(r){
			if(this.getStatus() == BMAP_STATUS_SUCCESS){
				//var mk = new BMap.Marker(r.point);
				//map.addOverlay(mk);
				//map.panTo(r.point);
				//alert('您的位置：'+r.point.lng+','+r.point.lat);
				cityCode = '-00';
				positionStr = r.point.lng+','+r.point.lat;
				me.storeList(cityCode,positionStr);
			}
			else {
				me.storeList(cityCode);
				setTimeout(function(){
					Jit.UI.Loading(false);
				},500);
				alert('failed'+this.getStatus());
			}        
		},{enableHighAccuracy: true});
		me.provinceSel();
		/*
		var me = this,
			cityCode = '-99',
			positionStr = '';
		//获取坐标
		me.geoLocation(function(position){
			if(position){
				cityCode = '-00';
				positionStr = position.coords.longitude+','+position.coords.latitude;
				//me.coords = {longitude:position.longitude,latitude:position.latitude};
				me.storeList(cityCode,positionStr);
				//alert(me.coords.longitude+','+me.coords.latitude);
				//self.SearchStoreList(self.coords.longitude+","+self.coords.latitude);
			}else{
				me.storeList(cityCode);
				setTimeout(function(){
					Jit.UI.Loading(false);
				},500);
				//Jit.UI.AjaxTips.Tips({show:true,tips:""});
				alert('您拒绝了使用位置共享服务，查询已取消');
			}				
			
		});
		me.provinceSel();
		*/
	},
	initEvent:function(){
		var me = this;
		$('#address_list').delegate(".address_store",'click',function(e){
			var that = $(this);
			if(e.target.className!="telephone"){
				if(that.hasClass('cur')){
					return;
				}else{
					$('.address_store').removeClass('cur');
					that.addClass('cur');
				}
				setTimeout(me.affirmAddress,100);
			}
		});
		
	},
	storeList:function(cityCode,positionStr){
		var me = this;
		me.ajax({
			url:'/ApplicationInterface/Gateway.ashx',
			data:{
				'action':'UnitAndItem.Unit.SearchStoreList',
				//'orderId':me.getUrlParam('orderId'),
				//'CityID':cityId,
				'CityCode':cityCode,
				'PageIndex':0,
				'PageSize':99,
				'StoreID':'',
				'Position':positionStr,
				'IncludeHQ':''
			},
			success:function(data){
				if(data.ResultCode == 0){
					var list = data.Data.StoreListInfo,
						tpl = $('#Tpl_address_item').html(),
						html = '';
					me.data = list;
					if(list.length<=0){
						//me.storeList(-99);
						html = '<div style="text-align:center;line-height:60px;">暂无附近门店！</div>';
					}else{
						for(var i=0;i<list.length;i++){
							if(list[i].DistanceDesc=='0米'){
								list[i].DistanceDesc = '';
							}
							html += Mustache.render(tpl,list[i]);
						}
					}
					$('#address_list').html(html);
					Jit.UI.Loading(false);
					//me.initEvent();
					//$('.address_store').eq(0).trigger('click');
				}
			}
		});
	},
	provinceSel: function(){
		var me = this;
		me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getProvince'
            },
            success: function (data) {
				var itemlists = data.content.provinceList;
				var provinceStrList=[];
				$("#province").html("");
				$("#province").append("<option value=''>请选择</option>");
				$.each(itemlists, function (i, obj) {
					if (obj.Province == decodeURIComponent(me.getUrlParam('province'))) {
						provinceStrList.push("<option value='" + obj.Province + "' selected=\"true\">" + obj.Province + "</option>");
					} else {
						provinceStrList.push("<option value='" + obj.Province + "'>" + obj.Province + "</option>");
					}
				});
				$("#province").append(provinceStrList.join(""));
				JitPage.fnSelCityByProvince($("#province").val());
            }
        });
	},
	fnSelCityByProvince: function (province) {
        var me = this;
        //城市下拉框改变事件
        me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getCityByProvince',
                'Province': province
            },
            success: function (data) {
                var itemlists = data.content.cityList;
                var cityStrList = [];
                $("#city").html("");
                $("#city").append("<option value=''>请选择</option>");
                $.each(itemlists, function (i, obj) {
                    if (obj.CityName == decodeURIComponent(me.getUrlParam('city'))) {
                        cityStrList.push("<option value='" + obj.CityCode + "' selected=\"true\">" + obj.CityName + "</option>");
                    } else {
                    	cityStrList.push("<option value='" + obj.CityCode + "'>" + obj.CityName + "</option>");
                    }
                });
                $("#city").append(cityStrList.join(""));
            }
        });
    },
	affirmAddress:function(){
		var me = JitPage;
		var storeid = $('#address_list .cur').data('storeid'),
			deliveryAddress = $('#address_list .cur').data('address'),
			mobile =$('#address_list .cur').data('tel');
		me.ajax({
			url:'/OnlineShopping/data/Data.aspx',
			data:{
				'action':'setUpdateOrderDelivery',
				'storeId':storeid,
				'mobile':mobile,
				'deliveryAddress':deliveryAddress,
				'deliveryId':2,
				'orderId':Jit.AM.getUrlParam('orderId')
			},
			success:function(data){
				if(data.code == 200){
					//me.pageBack(); 原生方法被人干掉了， 需要统一处理，
					history.back();//history.back()用和这个防止页面填写的数据刷新。
				}
			}
		});
	},
	geoLocation:function(callback){
		Jit.UI.Loading(true);
		var geol,me=this;
		try {
			if ( typeof (navigator.geolocation) != 'undefined') {
				geol = navigator.geolocation;
			} else {
				geol = google.gears.factory.create('beta.geolocation');
			}
		} catch (error) {
			
			Jit.UI.Loading(false);
			alert(error.message);
		}
		if (geol) {//&&!$.native.isNative()
			geol.getCurrentPosition(function(position) {
				if(callback){
					callback(position);
				}
			}, function(error) {
				$("#storeList").empty();
				me.storeList(-99);
				switch(error.code) {
					case error.TIMEOUT :
						alert("获取定位超时，请重试");
						//$.util.UI.AjaxTips.Tips({show:true,tips:"获取定位超时，请重试"});
						break;
					case error.PERMISSION_DENIED :
						alert("您拒绝了使用位置共享服务，查询已取消");
						//$.util.UI.AjaxTips.Tips({show:true,tips:"您拒绝了使用位置共享服务，查询已取消"});
						break;
					case error.POSITION_UNAVAILABLE :
						alert("非常抱歉，我们暂时无法通过浏览器获取您的位置信息");
						//$.util.UI.AjaxTips.Tips({show:true,tips:"非常抱歉，我们暂时无法通过浏览器获取您的位置信息"});
						break;
				}
				Jit.UI.Loading(false);
			}, {
				//设置十秒超时
				timeout : 10000
			});
		}else{
			//$.native.getLocation(callback);
			Jit.UI.Loading(false);
		}
	}
});