Jit.AM.defindPage({
	name : 'EditAddress',
	onPageLoad : function() {
		if(Jit.AM.getUrlParam('channelId') == 6){
			$('#topNav').hide();
			$('.goods_wrap').css('margin-top','0px');
		}
		var me = this;
		var province, cityName;

		if (me.getUrlParam('type') == 'edit') {
			$('#pagetitle').html('修改收货地址');
			//me.addressId = me.getUrlParam('addressId');
			var data = me.getParams('editAddressData');
			me.initPageData(data);
		} else {
			$('#pagetitle').html('新增收货地址');
		}
		me.fnGetProvince();
		
		setTimeout(function(){
			//遍历数据
			me.boundAddrData();
		},1000);
	},
	fnGetProvince : function() {
		var self = this;
		$.area.getProvince(function(data) {
			var itemlists = data.provinceList;
			var $provinceSelect = $("#province");
			$provinceSelect.empty();
			var optionStr = "<option value=''>省</option>";
			$.each(itemlists, function(i, obj) {
				if (obj.Province == decodeURIComponent(self.getUrlParam('province'))) {
					optionStr += "<option value='" + obj.Province + "' selected=\"true\">" + obj.Province + "</option>";
				} else {
					optionStr += "<option value='" + obj.Province + "'>" + obj.Province + "</option>";
				}
			});
			$provinceSelect.html(optionStr);
			self.fnGetCityByProvince($provinceSelect.val());
		});
	},
	fnGetCityByProvince : function(province) {
		var self = this;
		$.area.getCityByProvince(province, function(data) {
			var itemlists = data.cityList;
			var $citySelect = $("#city");
			$citySelect.empty();
			var optionStr = "<option value=''>市</option>";
			$.each(itemlists, function(i, obj) {
				if (obj.CityName == decodeURIComponent(self.getUrlParam('city'))) {
					optionStr += "<option name='"+obj.CityName+"' value='" + obj.CityID + "' selected=\"true\">" + obj.CityName + "</option>";
				} else {
					optionStr += "<option name='"+obj.CityName+"' value='" + obj.CityID + "'>" + obj.CityName + "</option>";
				}
			});
			$citySelect.html(optionStr);
			self.fnGetDistrictByCity($citySelect.val());
		});
	},
	fnGetDistrictByCity : function(city) {
		var self = this;
		$.area.getDistrictByCity(city, function(data) {
			var itemlists = data;
			var $districtSelect = $("#district");
			$districtSelect.empty();
			var optionStr = "<option value=''>区</option>";
			$.each(itemlists, function(i, obj) {
				console.log(obj.Name +"%%%"+ decodeURIComponent(self.getUrlParam('districtName')));
				if (obj.Name == decodeURIComponent(self.getUrlParam('districtName'))) {
					optionStr += "<option value='" + obj.DistrictID + "' selected=\"true\">" + obj.Name + "</option>";
				} else {
					optionStr += "<option value='" + obj.DistrictID + "'>" + obj.Name + "</option>";
				}
			});
			$districtSelect.html(optionStr);
		});
	},
	initPageData : function(data) {
		var me = this;
		$('#username').val(data.linkMan);
		$('#phone').val(data.linkTel);
		$('#address').val(data.address);
		Jit.WX.OptionMenu(false);
	},
	boundAddrData:function(){
		var me = this,
		    addrId = Jit.AM.getUrlParam('addrId');
		me.ajax({
			url : '/ApplicationInterface/Gateway.ashx',
			data : {
				'action':'Order.Delivery.GetVipAddressInfo',
				'VipAddressID':addrId
			},
			success : function(data) {
				if(data.ResultCode == 0) {
					var result = data.Data.VipAddress;
						/*
						result.vipAddressID
						result.linkMan
						result.linkTel
						result.cityID
						
						
						result.province
						result.cityName
						result.districtName
						result.address
					    */
					$('#username').val(result.LinkMan);	
					$('#phone').val(result.LinkTel);
					$('#address').val(result.Address);
					
					
					setTimeout(function(){
						$('#province option[value="'+result.Province+'"]').attr('selected','selected');
						$('#province').trigger('change');
						setTimeout(function(){
							$('#city option[name="'+result.CityName+'"]').attr('selected','selected');
							$('#city').trigger('change');
							setTimeout(function(){
								$('#district option[value="'+result.CityID+'"]').attr('selected','selected');
								$('#district').trigger('change');
							},1000);
						},1000);
					},1000);
					
					//me.pageBack();
				} else {
					$.alert(data.Message);
				}
			}
		});	
	},
	saveAddress : function(isDelete) {
		var me = this, 
			provinceArea = $('#province').val(), 
			cityArea = $('#city').val(),
			districtArea = $("#district").val(),
			detailArea = $('#address').val();
		var adrdata = {
			'action' : 'setVipAddress',
			'linkMan' : $('#username').val(),
			'linkTel' : $.trim($('#phone').val()),
			'cityID' : 	$('#district').val(),
			'address' : detailArea,
			'vipAddressID' : Jit.AM.getUrlParam('addrId') || ''
		};

		if (!adrdata.linkMan) {
			$.alert('请输入您的联系人姓名');
			return;
		}
		var _reMobile = /^1\d{10}$/;
		if (!adrdata.linkTel) {
			$.alert('请输入您的手机号码');
			return;
		} else if (!_reMobile.test(adrdata.linkTel)) {
			$.alert('您输入的手机号码不正确');
			return;
		}
		if (!provinceArea.length||!cityArea.length||!districtArea.length) {
			$.alert('请选择省市区');
			return;
		}
		if (!detailArea.length) {
			$.alert('请输入您的联系地址');
			return;
		}
		Jit.UI.Loading(true);
		if (isDelete) {
			adrdata.IsDelete = 1;
		}
		me.ajax({
			url : '/OnlineShopping/data/Data.aspx',
			data : adrdata,
			success : function(data) {
				if (data.code == 200) {
					var _data = {
						'linkMan' : adrdata.linkMan,
						'linkTel' : adrdata.linkTel,
						'address' : adrdata.address
					};
					if (!me.getUrlParam('orderId')) {
						//当有没orderId时，不需要将地址绑定到订单
						me.setParams('select_address', _data);
					}
					me.pageBack();
				} else {
					Jit.UI.Loading(false);
					$.alert(data.description);
				}
			}
		});
	},
	setIsDelete:function(){
		var me = this;
		Jit.UI.Dialog({
			'content': '是否要删除该地址？',
			'type': 'Confirm',
			'LabelOk': '确定',
			'LabelCancel': '取消',
			'CallBackOk': function() {
				me.saveAddress(1);
				Jit.UI.Dialog('CLOSE');
			},
			'CallBackCancel': function(){
				Jit.UI.Dialog('CLOSE');
			}
		});
	}
}); 