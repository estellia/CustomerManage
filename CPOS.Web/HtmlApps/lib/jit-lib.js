﻿var JitCfg = {
	'baseUrl':'../../../',
	'ajaxUrl':'', //http://182.254.156.57 http://api.dev.chainclouds.com
	'shareIco':'',
	'statisticsCode':''
};
String.prototype.colorRgb = function(){
	var reg = /^#([0-9a-fA-f]{3}|[0-9a-fA-f]{6})$/;
	var sColor = this.toLowerCase();
	if(sColor && reg.test(sColor)){
		if(sColor.length === 4){
			var sColorNew = "#";
			for(var i=1; i<4; i+=1){
				sColorNew += sColor.slice(i,i+1).concat(sColor.slice(i,i+1));
			}
			sColor = sColorNew;
		}
		//处理六位的颜色值
		var sColorChange = [];
		for(var i=1; i<7; i+=2){
			sColorChange.push(parseInt("0x"+sColor.slice(i,i+2)));
		}
		return "rgba(" + sColorChange.join(",") + ",0.6)";
	}else{
		return sColor;
	}
};
(function(global){
	function trim(str){
		return str.replace(/(\s)|(\r\n)|(\r)|(\n)/gi, "");
	}

	function strToJson(jsonStr){
		jsonStr = trim(jsonStr);
		return eval( '(' + jsonStr + ')' );
	}

	function getOffsetPos(obj){
    	var _top=obj.offsetTop,
			_left=obj.offsetLeft;
    	if(obj.offsetParent!=null){
    		var opos = getOffsetPos(obj.offsetParent);
    		_top += opos.top;
    		_left += opos.left;
    	}
    	return {'top':_top,'left':_left};
	}

	/*
	 #随机数函数
	 parameter：
	 	(number)section 随机区间
	 	(number)start 随机起步值
	 return:
	 	(int)
	 @memberOf gc.fn
	 */
	function random(section,start){
		if(start != null){
			return Math.floor(Math.random()*section) + 1 + start;
		}else{
			return Math.floor(Math.random()*section) + 1;
		}
	}

    /**
     * 随机数函数（值>=start&&值<=end）
     * @param start 起始值
     * @param end 结束值
     * @returns {*}
     */
    function randomTo(start,end){
        var max=end-start+1;
        return Math.floor(Math.random()*max) + start;
    }

	function bind(func, scope){
		return function(){
			return func.apply(scope, arguments);
		}
	}
	
	function loadFiles(urls,callback){
		var _file,
			_Head = document.getElementsByTagName("HEAD").item(0); 
		function createElement(tag){
			var element = document.createElement(tag == 'script' ? 'script' : 'link');
			if(tag == 'script'){
				element.setAttribute('type','text/javascript');
			}else{
				element.setAttribute('rel','stylesheet');
				element.setAttribute('type','text/css');
			}
			return element;
		}
		if(typeof urls == 'string'){
			urls = [urls];
		}
		var hasloadfilescount = 0,
			needloadfilescount = urls.length;
		for(var i in urls){
			if(urls[i].indexOf('.js')!=-1){
				_file = createElement('script');
				_Head.appendChild(_file);
				_file.src = urls[i];
			}
			if(urls[i].indexOf('.css')!=-1){
				_file = createElement('link');
				_Head.appendChild(_file);
				_file.href = urls[i];
			}
			if(typeof callback == 'function'){
				_file.onload = function(){
					hasloadfilescount++;
					if(hasloadfilescount>=needloadfilescount){
						callback();
					}
				};
				_file.onerror = function(){
					hasloadfilescount++;
					if(hasloadfilescount>=needloadfilescount){
						callback();
					}
				}
			}
		}
	}
	
	function deviceType() {  
		var sUserAgent= navigator.userAgent.toLowerCase();  
		var bIsIpad= sUserAgent.match(/ipad/i) == "ipad";  
		var bIsIphoneOs= sUserAgent.match(/iphone os/i) == "iphone os";  
		var bIsMidp= sUserAgent.match(/midp/i) == "midp";  
		var bIsUc7= sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";  
		var bIsUc= sUserAgent.match(/ucweb/i) == "ucweb";  
		var bIsAndroid= sUserAgent.match(/android/i) == "android";  
		var bIsCE= sUserAgent.match(/windows ce/i) == "windows ce";  
		var bIsWM= sUserAgent.match(/windows mobile/i) == "windows mobile";  
		if (bIsIpad || bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM) {  
			return 'mobile';
		} else {
			return 'pc';
		}  
	}
	
	var logmsg = [],_dtype = deviceType();
	function log(str,type){
		var cfg = Jit.AM.getAppVersion();
		if(cfg.APP_DEBUG_PANEL){
			if(logmsg.length>=200){
				logmsg = logmsg.splice(1,logmsg.length-1);
			}
			logmsg.push('-> '+str);
			var logstr = '';
			for(var i=0;i<logmsg.length;i++){
				logstr += logmsg[i]+'<br>';
			}
			$('.jit-debug-panel').html(logstr);
		}else{
			console.log(str);
		}
	};
	
	function setCookie(name,value,expires,path){
		var expdate=new Date();
		expdate.setTime(expdate.getTime()+(expires*1000));
		document.cookie = name+"="+escape(value)
						+ ";expires="+expdate.toGMTString()
						+ ( path ? ";path=" + path : "" )
	}

	function getCookie(name){
	    var arr,reg=new RegExp("(^| )"+name+"=([^;]*)(;|$)");
	    if(arr=document.cookie.match(reg)){
	        return unescape(arr[2]); 
	    }else{
	        return null; 
	    }
	} 

	function deleteCookie(name){
       	var exp = new Date(); 
	    exp.setTime(exp.getTime() - 1); 
	    var cval=this.GetCookie(name); 
	    if(cval!=null){
	    	document.cookie= name + "="+cval+";expires="+exp.toGMTString();
	    }
	}

	var cookie = {
		set:function(key,val){
			if(!appManage.CUSTOMER_ID){
				alert('cookie 操作出错，需要customerId');
				return;
			}
			var appcookie = getCookie('jit_'+appManage.CUSTOMER_ID);
			try{
				if(appcookie){
					appcookie = JSON.parse(appcookie);
				}else{
					appcookie = {};
				}
			}catch(e){
				appcookie = {};
			}
			if(val!=null){
				appcookie[key] = val;
			}else{
				delete appcookie[key];
			}
			appcookie = JSON.stringify(appcookie);
			setCookie('jit_'+appManage.CUSTOMER_ID,appcookie,3600*24*7);
		},
		get:function(key){
			if(!appManage.CUSTOMER_ID){
				alert('cookie 操作出错，需要customerId');
				return;
			}
			var appcookie = getCookie('jit_'+appManage.CUSTOMER_ID);
			if(appcookie == '' || appcookie == null){
				return null;
			}
			appcookie = JSON.parse(appcookie);
			return (appcookie[key]==undefined?null:appcookie[key]);
		},
		getAuth:function(name){
			var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg))
                return unescape(arr[2]);
            else
                return null;
		},
		del:function(name){
			document.cookie = name+"=;expires="+(new Date(0)).toGMTString();
			// var exp = new Date(); 
		 //    exp.setTime(exp.getTime() - 1); 
		 //    var cval=getCookie(name); 
		 //    if(cval!=null){
		 //    	document.cookie= name + "="+cval+";expires="+exp.toGMTString();
		 //    }
		}
	};
	
	var locStorage = {
		set:function(key,val){
			if(key){
				if(val){
					localStorage.setItem(key,val);
				}else{
					localStorage.removeItem(key);
				}
			}else{
				log('set localStorage Error:key is null','error');
			}
		},
		get:function(key){
			return localStorage.getItem(key);
		}
	};
	
	var store = function(){
		var args = arguments;
		if(args.length == 1){
			return locStorage.get(args[0]);
		}else if(args.length == 2){
			locStorage.set(args[0],args[1]);
		}
	};
	
	var sesStorage = {
		set:function(key,val){
			if(key){
				sessionStorage.setItem(key,val);
			}else{
				log('set sessionStorage Error:key is null','error');
			}
		},
		get:function(key){
			return sessionStorage.getItem(key);
		}
	};
	
	var session = function(){
		var args = arguments;
		if(args.length == 1){
			return sesStorage.get(args[0]);
		}else if(args.length == 2){
			if(args[1] == null){
				sessionStorage.removeItem(args[0]);
			}else{
				sesStorage.set(args[0],args[1]);
			}
		}
	};
	
	var validator = {
		IsEmail : function(str){
			var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
			return reg.test(str)
		},
		isPhoneNumber : function (str){
			var regAee  = /^(13\d[1]{0,9}|14\d[1]{0,9}|15\d[1]{0,9}|18\d[1]{0,9})\d{8}$/;
			return regAee.test(str);
		}
	};

	var fn = {
		random : random,
		trim : trim,
		strToJson : strToJson,
		loadFiles : loadFiles,
		log : log,
		store : store,
		valid : validator,
		cookie : cookie,
		getPostion : getOffsetPos
	};

	/*
	@@页面管模块
	appManage = {
		//设置应用的配置信息
		setAppVersion(配置信息)，
		//获取应用的配置信息
		getAppVersion(),
		//设置应用交互时ajax 携带的基本信息
		setBaseAjaxParam(json object),
		//获取应用交互时ajax 携带的基本信息
		getBaseAjaxParam(),
		//设置页面之间通信的数据信息
		setPageParam(key , value [object | string] ),
		//获取页面之间通信的数据信息
		getPageParam(key)
	}
	*/
	var appManage = {
		'APP_CODE':'',
		//获得url上的参数
		getUrlParam:function(key){
			var value = "",itemarr = [],
				urlstr = window.location.href.split("?");
			if (urlstr[1]) {
				var item = urlstr[1].split("&"),rst={};
				for (i = 0; i < item.length; i++) {
					itemarr = item[i].split("=");
					rst[itemarr[0]] = itemarr[1];
				}
			}else{
				return null;
			}
			if(key){
				return rst[key];
			}else{
				return rst;
			}
		},
		buildUserId:function() {
			var guid = '';
			for (var i = 1; i <= 32; i++){
				var n = Math.floor(Math.random()*16.0).toString(16);
				guid += n;
			}
			return guid;
		},
		setAppVersion:function(cfg){
			var me = this;
			if(!me.CUSTOMER_ID){
				me.CUSTOMER_ID = me.getUrlParam('customerId');
			}
			if(!me.CUSTOMER_ID){
				log('Error: customer Id 丢失');
				return;
			}
			var ver = me.getAppVersion();
			if(ver){
				for(var key in cfg){
					ver[key] = cfg[key];
				}
			}else{
				ver = cfg;
			}
			if(!me.APP_CODE){
				for(var key in ver){
					if(key = 'APP_CODE'){
						me.APP_CODE = cfg[key];
						break;
					}
				}
			}
			store(me.CUSTOMER_ID,JSON.stringify(ver));
		},
		getAppVersion:function(){
			var me = this;
			if(!me.CUSTOMER_ID){
				me.CUSTOMER_ID = me.getUrlParam('customerId');
			}
			if(!me.CUSTOMER_ID){
				return null;
			}
			var rst = store(me.CUSTOMER_ID);
			if(rst){
				rst = eval('(' + rst + ')');
				return rst;
			}else{
				return null;
			}
		},
		/*
		设置ajax交互时的基本数据 (需要和config.js 中的AJAX_PARAMS 相匹配)
		@param : {
			'openId':'xxx',
			'userId':'xxx',
			'locale':'xxx',
			'customerId':''
		}
		*/
		setBaseAjaxParam:function(param,oncookie){
			var me = this,
				appcfg = me.getAppVersion(),
				ajaxKeys = appcfg['AJAX_PARAMS'].split(',');
			if(oncookie){
				for(var i in ajaxKeys){
					param[ajaxKeys[i]] = param[ajaxKeys[i]]?param[ajaxKeys[i]]:null;
				}
				cookie.set('baseInfo',param);
			}else{
				for(var i in ajaxKeys){
					store(this.APP_CODE+'_AJAX_PARAM_'+ajaxKeys[i] , param[ajaxKeys[i]]);
				}
			}
		},
		getBaseAjaxParam:function(){
			var me = this,
				appcfg = me.getAppVersion(),
				ajaxKeys = appcfg['AJAX_PARAMS'].split(','),
				param = {},
				baseInfo = cookie.get('baseInfo');
			if(baseInfo){
				return baseInfo;
			}
			for(var i in ajaxKeys){
				param[ajaxKeys[i]] = store(this.APP_CODE+'_AJAX_PARAM_'+ajaxKeys[i]);
			}
			return param;
		},
		setAppParam:function(type,key,val){
			var rkey = this.APP_CODE + '_' + type + '_' + key;
			if(val == null){
				store(rkey , null );
			}else if((typeof val) == 'object'){
				store(rkey , 'o_' + JSON.stringify(val) );
			}else{
				store(rkey , 's_' + val );
			}
		},
		getAppParam:function(type,key){
			var rkey = this.APP_CODE + '_' + type + '_' + key,
				val = store(rkey);
			if(!val){
				return null;
			}
			var dtype = val.substr(0,1),
				rval = val.substring(2,val.length);
			if(dtype == 's'){
				return rval;
			}else if(dtype == 'o'){
				return eval('(' + rval + ')');
			}
		},
		setAppSession:function(type,key,val){
			var rkey = this.APP_CODE + '_' + type + '_' + key;
			if(val == null){
				session(rkey , null );
			}else if((typeof val) == 'object'){
				session(rkey , 'o_' + JSON.stringify(val) );
			}else{
				session(rkey , 's_' + val );
			}
		},
		getAppSession:function(type,key){
			var rkey = this.APP_CODE + '_' + type + '_' + key,
				val = session(rkey);
			if(!val){
				return null;
			}
			var dtype = val.substr(0,1),
				rval = val.substring(2,val.length);
			if(dtype == 's'){
				return rval;
			}else if(dtype == 'o'){
				return eval('(' + rval + ')');
			}
		},
		setPageParam:function(key,val){
			this.setAppParam('PageParam',key,val);
		},
		getPageParam:function(key){
			return this.getAppParam('PageParam',key);
		},
		setPageHashParam:function(key,val){
			this.setAppSession('PageParam',key,val);
		},
		getPageHashParam:function(key){
			return this.getAppSession('PageParam',key);
		},
		checkAppPageConfig:function(_needReLoad){
			var cfg = this.getAppVersion(),
				pcfg = this.getAppPageConfig();
			if(!pcfg || _needReLoad){
				var isGloble = Jit.AM.getAppParam('isGloble'),
					_cfgname = (isGloble=='true'?'_globle':this.CUSTOMER_ID),
					rst = $.ajax({
						url: '/HtmlApps/config/'+_cfgname+'.js',
						async:false,
						cache:false
					}),
					vcfg = Jit.strToJson(rst.responseText);
				Jit.AM.setAppPageConfig(vcfg);
			}
		},
		setAppPageConfig:function(cfg){
			this.setAppParam('PageCfg','',cfg);
		},
		getAppPageConfig:function(){
			return this.getAppParam('PageCfg','');
		},
		pageHistoryPush:function(pagename){
			var history = this.getAppSession('PageHistory','');
			if(history){
				var list = history.split(',');
				if(list.length>=12){
					list.splice(0,1);
				}
				list.push(pagename);
				this.setAppSession('PageHistory','',list.join(','));
			}else{
				this.setAppSession('PageHistory','',pagename);
			}
		},
		pageHistoryPop:function(){
			var history = this.getAppSession('PageHistory','');
			if(history){
				var list = history.split(',');
				list.pop();
				this.setAppSession('PageHistory','',list.join(','));
			}
		},
		pageHistoryClear:function(){
			this.setAppSession('PageHistory','',null);
		},
		hasHistory:function(){
			var history = this.getAppSession('PageHistory','');
			if(history){
				var list = history.split(',');
				if(list.length >= 2){
					return true;
				}
			}
			return false;
		},
		pageBack:function(){
			var history = this.getAppSession('PageHistory','');
			if(history){
				var list = history.split(',');
				if(list.length >= 2){
					log('返回上一页');
					list.pop();
					var tarpage = list.pop(),
						pages = tarpage.split(':');
					this.setAppSession('PageHistory','',list.join(','));
					if(pages.length >1){
						this.toPage(pages[0],pages[1]);
					}else{
						this.toPage(pages[0]);
					}
				}
			}
		},
		toPage:function(pagename,param){
			var pagecfg = this.getAppPageConfig(),
				page = pagecfg[pagename];
			var htmlpath = page.path.replace(/%(\S*)%/,function(str){
				return pagecfg['Config']['Shorthand'][str.substring(1,str.length-1)];
			});
			if(page){
				/*
				if(param){
					this.pageHistoryPush(pagename+':'+param);
				}else{
					this.pageHistoryPush(pagename);
				}
				*/
				var cfg = Jit.AM.getAppVersion(),
					version = (cfg.APP_CACHE?cfg.APP_VERSION:((new Date()).getTime()));
				location.href = '/HtmlApps/html/'+htmlpath+'?customerId='+Jit.AM.CUSTOMER_ID+(param?('&'+param):'')+'&version='+version;
			}
		},
		getPageUrl:function(pageName,param){
			var url = location.host+'/WXOAuth/NoAuthGoto.aspx?'
				+ 'customerId='+Jit.AM.CUSTOMER_ID
				+ '&pageName='+pageName+'&'+param
				+ 'Url=api.dev.chainclouds.com/HtmlApps/html/_pageName_';
			return url;
		},
		toPageWithParam:function(pagename,param){
			var value = "",
				itemarr = [],
				params = {},
				urlstr = window.location.href.split("?");
			if (urlstr[1]) {
				var items = urlstr[1].split("&");
				for (i = 0; i < items.length; i++) {
					itemarr = items[i].split("=");
					params[itemarr[0]] = itemarr[1];
				}
			}
			if(param){
				var temps = param.split("&"),tempparam;
				for(var i in temps){
					tempparam = temps[i].split('=');
					params[tempparam[0]] = tempparam[1];
				}
			}
			delete params['customerId'];
			var paramslist = [];
			for(var key in params){
				paramslist.push(key + '=' + params[key]);
			}
			this.toPage(pagename,paramslist.join('&'));
		},
		buildAjaxParams:function(param){
			var _param = {
				type: "post",
				dataType: "json",
				url: "",
				data: null,
				beforeSend: function () {
					//UI.Loading('SHOW');
				},
				success: null,
				error: function (XMLHttpRequest, textStatus, errorThrown){
					//UI.Loading("CLOSE");
				}
			};
			$.extend(_param,param);
			var baseInfo = this.getBaseAjaxParam(),
				baseInfoCustomerId = this.getUrlParam("customerId"),
				baseInfoOpenId = Jit.cookie.getAuth('openId_'+baseInfoCustomerId),
				baseInfoUserId = Jit.cookie.getAuth('userId_'+baseInfoCustomerId);
			var hostName = location.host;
			if(hostName=='api.dev.chainclouds.cn' || hostName=='api.dev.chainclouds.com'){
				location.hash = '&openId='+baseInfoOpenId+'&userId='+baseInfoUserId;
			}
			//通过浏览器地址栏把内容填充
			if((!baseInfo.customerId)&&baseInfoCustomerId){
				baseInfo.customerId=baseInfoCustomerId;
			}
			if((!baseInfo.userId)||baseInfoUserId){
				baseInfo.userId=baseInfoUserId;
			}
			if((!baseInfo.openId)||baseInfoOpenId){
				baseInfo.openId=baseInfoOpenId;
			}
			if(!baseInfo.ChannelID){//2代表
				if($('#channelIdSize').val() == 6){
					baseInfo.ChannelID="6";
					appManage.ChannelID="6";
				}else if($('#channelIdSize').val() == 7){
					baseInfo.ChannelID="7";
					appManage.ChannelID="7";
				}else if($('#channelIdSize').val() == 11){
					baseInfo.ChannelID="11";
					appManage.ChannelID="11";
				}else{
					baseInfo.ChannelID="2";
				}
			}
			//TODO:delete
			/*
            var isALD=Jit.AM.getPageParam("_aldfrom_")==1?1:0;
            if(!baseInfo.isALD){
                baseInfo.isALD=isALD;
            }
            */
			var _data = {
				'action':param.data.action,
				'ReqContent':JSON.stringify({
					'common':(param.data.common?$.extend(baseInfo,param.data.common):baseInfo),
					'special':(param.data.special?param.data.special:param.data)
				})
			};
			_param.data = _data;
			return _param;
		},
		buildNewAjaxParams:function(param){
			var _param = {
				type: "post",
				dataType: "json",
				url: "",
				data: null,
				beforeSend: function () {
					
				},
				success: null,
				error: function (XMLHttpRequest, textStatus, errorThrown){
					
				}
			};
			$.extend(_param,param);
			var baseInfo = this.getBaseAjaxParam(),
				baseInfoCustomerId = this.getUrlParam("customerId"),
				baseInfoOpenId = Jit.cookie.getAuth('openId_'+baseInfoCustomerId),
				baseInfoUserId = Jit.cookie.getAuth('userId_'+baseInfoCustomerId);
			var hostName = location.host;
			if(hostName=='api.dev.chainclouds.cn' || hostName=='api.dev.chainclouds.com'){
				location.hash = '&openId='+baseInfoOpenId+'&userId='+baseInfoUserId;
			}
			//通过浏览器地址栏把内容填充
			if((!baseInfo.customerId)&&baseInfoCustomerId){
				baseInfo.customerId=baseInfoCustomerId;
			}
			if((!baseInfo.userId)||baseInfoUserId){
				baseInfo.userId=baseInfoUserId;
			}
			if((!baseInfo.openId)||baseInfoOpenId){
				baseInfo.openId=baseInfoOpenId;
			}
			if(!baseInfo.ChannelID){
				if($('#channelIdSize').val() == 6){
					baseInfo.ChannelID="6";
					appManage.ChannelID = '6';
				}else if($('#channelIdSize').val() == 7){
					baseInfo.ChannelID="7";
					appManage.ChannelID="7";
				}else if($('#channelIdSize').val() == 11){
					baseInfo.ChannelID="11";
					appManage.ChannelID="11";
				}else{
					baseInfo.ChannelID="2";
				}
			}
			//TODO:delete
			/*
            var  isALD=Jit.AM.getPageParam("_aldfrom_")==1?1:0;
            if(!baseInfo.isALD){
                baseInfo.isALD=isALD;
            }
            */
			var action = param.data.action,
				interfaceType = param.interfaceType||'Product',
				_req = {
					'Locale':baseInfo.locale,
					'CustomerID':baseInfo.customerId,
					'UserID':baseInfo.userId,
					'OpenID':baseInfo.openId,
					'ChannelID':baseInfo.ChannelID,
                    'isALD': baseInfo.isALD,
					'Token':null,
					'Parameters':param.data
				};
			delete param.data.action;
			var _data = {
				'req':JSON.stringify(_req)
			};
			
			_param.data = _data;
			_param.url = _param.url+'?type='+interfaceType+'&action='+action;
			return _param;
		},
		ajax:function(param){
			var action = param.data.action,
				_param;
			if(param.url.indexOf('Gateway.ashx')!=-1 || param.url.indexOf('Getway.ashx')!=-1 || param['interfaceMode'] == 'V2.0'){
				_param = this.buildNewAjaxParams(param);
			}else{
				_param = this.buildAjaxParams(param);
			}
			_param.url = JitCfg.ajaxUrl + _param.url;
			_param.beforeSend = function(){
				if(param.beforeSend){
					param.beforeSend();
				}
				global.timer = new Date().getTime();
			};
			_param.complete = function(){
				if(param.complete){
					param.complete();
				}
				console.log(
					"\r\n"+
					"页面名称："+Jit.AM.getAppPageConfig()[$("title").attr("name")].title+"|"+$("title").attr("name")
					+"\r\n"+
					"请求地址："+_param.url
					+"\r\n"+
					"请求方法："+action
					+"\r\n"+
					"请求耗时："+(new Date().getTime()- global.timer)+"毫秒"+"\r\n"
				);
			};
			$.ajax(_param);
		},
		isPageNeedLog:function(){
			var cfg = Jit.AM.getAppVersion(),
				htmlname = $('title').attr('name');
			if(htmlname && cfg['LOG_PAGE'] && cfg['LOG_PAGE'].indexOf(htmlname) != -1){
				return true;
			}
			return false;
		},
		logToServer:function(type){
			if(type == 'browser' || type == 'forward' || type == 'browserForward'){
				var _param = this.buildAjaxParams({
					url: '/Module/BrowserRecord.ashx',
					data: {
						'action': type,
						'webPage':$('title').attr('name')
					},
					success: function(){}
				});
				$.ajax(_param);
			}
		},
		openShareFunction:function(urlParams,onVisitCallBack){
			/*设置页面分享时的推荐链接*/
			var urls = [];
			urls.push(location.host+'/HtmlApps/Auth.html?pageName='+$('title').attr('name'));
			if(urlParams && urlParams.length>0){
				for(var i in urlParams){
					var val = Jit.AM.getUrlParam(urlParams[i]);
					if(val){
						urls.push('&'+urlParams[i]+'='+val);
					}
				}
			}
			urls.push('&customerId='+Jit.AM.CUSTOMER_ID);
			urls.push('&recommender=1&recommenderId='+Jit.AM.getBaseAjaxParam().userId);
			var url = urls.join('');
			Jit.WX.shareFriends("好友推荐",'',url,null);
			/*设置浏览分享页面时的行为逻辑*/
			if(Jit.AM.getUrlParam('recommender') == 1 && typeof onVisitCallBack == 'function'){
				onVisitCallBack(Jit.AM.getUrlParam('recommenderId'));
			}
		},
		//分享给微信好友和朋友圈
		initShareEvent:function(shareInfo){
			Jit.WX.shareFriends(shareInfo);
        	Jit.WX.shareTimeline(shareInfo);
		},
		
		defindPage:function(page){
			window.scrollTo(0, 0);
			if(!page.initWithParam){
				page.initWithParam = function(){};
			}
			page.getBaseInfo = bind(this.getBaseAjaxParam,this);
			page.setParams = bind(this.setPageParam,this);
			//页面默认支持分享到微信和朋友圈
			page.initShareEvent=bind(this.initShareEvent,this);
			page.getParams = bind(this.getPageParam,this);
			page.setHashParam = bind(this.setPageHashParam,this);
			page.getHashParam = bind(this.getPageHashParam,this);
			page.getUrlParam = bind(this.getUrlParam,this);
			page.pageBack = bind(this.pageBack,this);
			page.toPage = bind(this.toPage,this);
			page.ajax = bind(this.ajax,this);
			page.buildAjaxParams = bind(this.buildAjaxParams,this);
			page.toPageWithParam = bind(this.toPageWithParam,this);
			page.openShareFunction = bind(this.openShareFunction,this);
			page.weiXinOptionMenu = bind(this.weiXinOptionMenu,this);
			page.weiXinToolBar = bind(this.weiXinToolBar,this);
			page._initShare = function(){
				var me = this;
				var param = this.pageParam;
				if(param && param['WX_TITLE']){
					var shareInfo = {
                        'title':(param['WX_TITLE']||'好友推荐'),
                        'desc':(param['WX_DES']||'大奖等你抢！'),
                        'link':location.href,
                        'imgUrl':(param['WX_ICO'])
                    }
                    Jit.WX.shareFriends(shareInfo);
				}
			}
			Jit.AM.onLoad = function(){
				//获取页面配置的参数（页面订制化）
				var pagecfg = Jit.AM.getAppPageConfig()[$('title').attr('name')]
				if(pagecfg && pagecfg['param']){
					page.pageParam=pagecfg['param'];
					page.initWithParam(pagecfg['param']);
				}
				//初始化分享功能
				page._initShare();
				//初始化事件
				page.eventType=Jit.deviceType()=="mobile"?"tap":"click";
				//初始化页面
				page.onPageLoad();
				
				//
				debugger;
				//Jit.AM.piwikScript();//piwik统计页面访问量
			}
			window.JitPage = page;
		},
		checkHasContact:function(){
			if(JitCfg.CheckOAuth == 'unAttention'){
				return false;
			}
			return true;
		},
		piwikScript:function () {
			    //
			    var _d = window.document,
                    _script,
                    _scriptFist,
                    //_url = (("https:" == document.location.protocol) ? "https" : "http") + "://127.0.0.1:3456/piwik/",
					//_url = (("https:" == document.location.protocol) ? "https" : "http") + "://tj.chainclouds.cn/",
					_url,
                    //_piwikUrl = _url + 'piwik.php',
					_piwikUrl,
                    //_siteId = '2',
					_siteId ,
                    //_pageGoal,
					//_pageHost = (("https:" == document.location.protocol) ? "https" : "http") + '://192.168.14.1:1234/HtmlApps/html/',
					_pageHost = (("https:" == document.location.protocol) ? "https" : "http") + '://api.test.chainclouds.cn/HtmlApps/html/',
                    _pageTile,
                    _pageUrl,
					_hostStr,
                    _pageClass;

			    //
			    function _piwikInit() {
					if(_hostStr == 'api.test.chainclouds.cn' || _hostStr == 'api.test.chainclouds.com' 
					   || _hostStr == 'api.dev.chainclouds.cn' || _hostStr == 'api.dev.chainclouds.com'
					   || _hostStr == 'api.uat.chainclouds.cn' || _hostStr == 'api.uat.chainclouds.com'){
						_url = (("https:" == document.location.protocol) ? "https" : "http") + "://tjtest.chainclouds.com/";
						_siteId = '4';
					} else if(_hostStr == 'api.chainclouds.cn' || _hostStr == 'api.chainclouds.com'){
						_url = (("https:" == document.location.protocol) ? "https" : "http") + "://tj.chainclouds.cn/";
						_siteId = '2';
					}
					_piwikUrl = _url + 'piwik.php',
					
			        window._paq = window._paq || [];
			        _paq.push(['setTrackerUrl', _piwikUrl]);
			        _paq.push(['setSiteId', _siteId]);
			        _paq.push(['setDocumentTitle', _pageTile]);
			        //_paq.push(['setCustomUrl', _pageHost + _pageUrl]);
					_paq.push(['setCustomUrl', _pageUrl]);
			        _paq.push(['setIgnoreClasses', _pageClass]);
			        _paq.push(['enableLinkTracking', true]);
			        _paq.push(['trackPageView']);
			    }

			    //
			    var init = function() {
					debugger;
			        _pageTile = Jit.AM.getAppPageConfig()[$('title').attr('name')].title || "";
			        //_pageUrl = (Jit.AM.getAppPageConfig()[$('title').attr('name')].path || "") + "{&vipid&}" + Jit.AM.getBaseAjaxParam().userId;
					_pageUrl = window.location.href + "{&vipid&}" + Jit.AM.getBaseAjaxParam().userId;
					_hostStr=window.location.hostname;
			        _pageClass = "";
			        _piwikInit();
			        _script = _d.createElement('script');
			        _script.type = 'text/javascript';
			        _script.async = true;
			        _script.defer = true;
			        _script.src = _url + 'piwik.js';
			        _scriptFist = _d.getElementsByTagName('script')[0];
			        _scriptFist.parentNode.insertBefore(_script, _scriptFist);
			    };
				
				return  init();
		},
		piwik:function(){
			$('body').append(JitCfg.statisticsCode);
		}
	};
	
	var WeiXin = {
		shareInfo:{},
		OptionMenu:function(flag){
			if(typeof WeixinJSBridge == 'object'){
				WeixinJSBridge.call(flag?'showOptionMenu':'hideOptionMenu');
			}else{
				document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
					WeixinJSBridge.call(flag?'showOptionMenu':'hideOptionMenu');
				});
			}
		},
		ToolBar:function(flag){
			if(typeof WeixinJSBridge == 'object'){
				WeixinJSBridge.call(flag?'showToolbar':'hideToolbar');
			}else{
				document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
					WeixinJSBridge.call(flag?'showToolbar':'hideToolbar');
				});
			}
		},
		fnShare:function(o){
			var map = {
				'Friends':{
					'mKey':'appmessage',
					'invoke':'sendAppMessage'
				}
			};
			function share(title, desc, link, imgUrl, isAuth, Authparam) {
				var customerId=Jit.AM.getUrlParam("customerId"),
					applicationId=Jit.AM.getUrlParam("applicationId");
				if(!!!customerId){
					customerId=Jit.AM.getBaseAjaxParam().customerId;
				}
				var applicationId=Jit.AM.getUrlParam("applicationId");
				if(!!!applicationId){
					applicationId=Jit.AM.getBaseAjaxParam().applicationId;
				}
				//是否需要高级auth认证
				if(isAuth) {
					var info = Jit.AM.getBaseAjaxParam(),
						shareUrl = link.replace('userId=' + info.userId, ''),
						shareUrl = shareUrl.replace('openId=' + info.openId, ''),
						shareUrl = shareUrl.replace('customerId=' + info.customerId, ''),
					/*shareUrl = shareUrl.replace('applicationId='+info.applicationId,''),
					 shareUrl = shareUrl.replace('weixinId='+info.weixinId,''),*/
						linkUrl = shareUrl + '&sender=' + info.userId;
				    //个性化auth页面参数集合

					var Authparamstr = "";
					if (Authparam && Authparam.length > 0) {
					    $.each(Authparam, function (index, self) {
					        if (self.paramname != null) {
					            Authparamstr += "&" + self.paramname + "=" + self.paramvalue;
					        }
					    });
					}

					var shareType="",ObjectID="";
				/*	CTW：创意仓库
					Coupon：优惠券
					SetoffPoster：集客海报
					Goods：商品*/
					switch ($("title").attr("name")){
						case "GoodsDetail": shareType="&objectType=Goods"; break;  //商品
						case "GCCoupon": shareType="&objectType=Coupon"; break;  //优惠券
						case "poster": shareType="&objectType=SetoffPoster"; break;  //商品
						case "RedPacket":   //红包
							shareType="&objectType=CTW"; break;  //
					}
					var SourceId=3;
					if(Jit.AM.getUrlParam('pushType')=='IsSuperRetail'){

						SourceId=4;
					}

					if(applicationId){
					    link = location.host + '/WXOAuth/AuthUniversal.aspx?scope=snsapi_userinfo&SourceId='+ SourceId+ Authparamstr + '&ShareVipID=' + info.userId+shareType+ '&customerId=' + customerId + '&applicationId=' + applicationId + '&goUrl=' + encodeURIComponent(linkUrl);
					}else{
					    link = location.host + '/WXOAuth/AuthUniversal.aspx?scope=snsapi_userinfo&SourceId='+SourceId + Authparamstr + '&ShareVipID=' + info.userId+shareType + '&customerId=' + customerId + '&goUrl=' + encodeURIComponent(linkUrl);
					}
				}
				if(typeof WeixinJSBridge == 'object'){
					WeixinJSBridge.on('menu:share:appmessage', function(argv){
						WeixinJSBridge.invoke('sendAppMessage',{
							//"appid":appId,
							"img_url":imgUrl||('http://'+location.host+'/HtmlApps/images/common/icon_timeline.png'),
							//"img_width":"320",
							//"img_height":"320",
							"link":link,
							"desc":desc,
							"title":title
						}, function(res) {
							//分享成功
							if(res.err_msg.indexOf('ok')!=-1||res.err_msg.indexOf('confirm')!=-1){
								//转发给好友统计
								if(window._paq){
									var baseInfo = appManage.getBaseAjaxParam();
                                    var title =document.title?document.title+'---转发到好友':'转发到好友';

									_paq.push(['trackEvent', title,baseInfo.customerId]);
								}
							}
							if(res.err_msg.indexOf('ok')!=-1 && Jit.AM.isPageNeedLog()){
								Jit.AM.logToServer('forward');
							}
						})
					});
					WeixinJSBridge.on('menu:share:timeline', function(argv){
						WeixinJSBridge.invoke('shareTimeline',{
							//"appid":appId,
							"img_url":imgUrl||('http://'+location.host+'/HtmlApps/images/common/icon_timeline.png'),
							//"img_width":"320",
							//"img_height":"320",
							"link":link,
							"desc":desc,
							"title":title
						}, function(res) {
							//分享成功
                            if(res.err_msg.indexOf('ok')!=-1||res.err_msg.indexOf('confirm')!=-1){
								//转发到朋友圈统计
								if(window._paq){
									var baseInfo = appManage.getBaseAjaxParam(),
										title =document.title?document.title+'---转发到朋友圈':'转发到朋友圈';
									_paq.push(['trackEvent', title,baseInfo.customerId]);
								}
							}
							if(res.err_msg.indexOf('ok')!=-1 && Jit.AM.isPageNeedLog()){
								Jit.AM.logToServer('forward');
							}
						})
					});
					return true;
				}else{
					return false;
				}
			}
			var runCount = 20;
			function dofn(o,count){
				return (function(){
				    if (share(o.title, o.desc, o.link, o.imgUrl, o.isAuth, o.Authparam) || count <= 0) {
						clearInterval(window.WX_Share_timer);
						window.WX_Share_timer = null;
					}
					count--;
				});
			};
			var hashdo = dofn(o,20);
			if(window.WX_Share_timer){
				clearInterval(window.WX_Share_timer);
			}
			window.WX_Share_timer = setInterval(hashdo,300);
		},
		initShare : function(){
			var me = this,
				version = Jit.AM.getAppVersion(),
				isAuth=false;
			if($("title").attr("name")=="IndexShopApp"){
				isAuth=true;
			}
			if(version['APP_WX_TITLE']){
				me.fnShare({
					'link':location.href,
					'title':version['APP_WX_TITLE'],
					'desc':version['APP_WX_DES'],
					'imgUrl':version['APP_WX_ICO'],
					'isAuth':isAuth  //默认不需要高级auth认证
				});
			}else{
				me.fnShare({
					'link':location.href,
					'title':"商城首页",
					'desc':"发现一个很不错的商城",
					'imgUrl':$("img").eq(0).attr("src"),
					'isAuth':isAuth  //默认不需要高级auth认证
				});
			}
		},
		//分享到朋友圈
		shareTimeline: function (title, desc, link, imgUrl, isAuth, Authparam) {
			var me = this;
			if(arguments.length==4){
				me.fnShare({
					'title':title,
					'desc':desc,
					'link':link,
					'imgUrl':imgUrl,
					'isAuth': isAuth ? isAuth : false,    //是否需要高级auth认证  默认false
                    'Authparam': Authparam
				});
			}else if(typeof arguments[0] == 'object'){
				me.fnShare(arguments[0]);
			}
		},
		//发送给好友
		shareFriends: function (title, desc, link, imgUrl, isAuth, Authparam) {
			var me = this;
			if(arguments.length==4){
				me.fnShare({
					'title':title,
					'desc':desc,
					'link':link,
					'imgUrl':imgUrl,
					'isAuth': isAuth ? isAuth : false,    //是否需要高级auth认证  默认false
                    'Authparam': Authparam
				});
			}else if(typeof arguments[0] == 'object'){
				me.fnShare(arguments[0]);
			}
		},
		//添加关注
		addContact : function(name,callback){
			if(typeof WeixinJSBridge == 'object'){
				WeixinJSBridge.invoke('addContact', {webtype: '1',username: name}, function(e) {
					WeixinJSBridge.log(e.err_msg);
					//e.err_msg:add_contact:added 已经添加
					//e.err_msg:add_contact:cancel 取消添加
					//e.err_msg:add_contact:ok 添加成功
					if(e.err_msg == 'add_contact:added' || e.err_msg == 'add_contact:ok'){
						//关注成功，或者已经关注过
						callback(true);
					}
				})
			}
		},
		/**
		 * 调起微信Native的图片播放组件。
		 * 这里必须对参数进行强检测，如果参数不合法，直接会导致微信客户端crash
		 *
		 * @param {String} curSrc 当前播放的图片地址
		 * @param {Array} srcList 图片地址列表
		 */
		imagePreview:function (curSrc, srcList) {
			if (!curSrc || !srcList || srcList.length == 0) {
				return;
			}
			if(typeof WeixinJSBridge == 'object'){
				WeixinJSBridge.invoke('imagePreview', {
					'current' : curSrc,
					'urls' : srcList
				});
			}
		}
	};

	var UI = {
		Prompt:function(params){
			if(typeof params != 'object'){
				return false;
			}
			var htmlStr = '<div class="ui-mask" style="display:block;">\
				<div class="ui-prompt-area">\
					<p class="ui-title">'+params.title+'</p>\
					<div class="ui-des">'+params.des+'</div>\
					<div class="ui-btn-box">'+params.html+'</div>\
				</div>\
			</div>';
			$('body').prepend(htmlStr);
			$('.ui-prompt-area .cancelBtn').bind('click',function(){
				$('.ui-mask').remove();
				if(params.callback){
					params.callback();
				}
			});
		},
		ReachBottom: function(inHeight, target) {
			var nDivHight, nScrollHight, scrollTop;
			if (target && $(target).length > 0) {
				target = $(target);
				nDivHight = target[0].clientHeight;
				nScrollTop = target[0].scrollTop;
				nScrollHight = target[0].scrollHeight;
			} else {
				var doc = document.documentElement,
					body = document.body,
					flag = (doc.scrollTop == 0);
				nDivHight = self.innerHeight || doc && doc.clientHeight || body.clientHeight;
				nScrollTop = flag ? body.scrollTop : doc.scrollTop;
				nScrollHight = flag ? body.scrollHeight : doc.scrollHeight;
			}
			if ((nScrollTop + nDivHight) >= nScrollHight - (inHeight || 0)) {
				return true;
			}
			return false;
		},
		//提示框方法
		Dialog:function(cfg){
			if(cfg == 'CLOSE'){
				var panel = $('.jit-ui-panel');
				if(panel){
					(panel.parent()).remove();
				}
			}else{
				cfg.LabelOk = cfg.LabelOk?cfg.LabelOk:'确定';
				cfg.LabelCancel = cfg.LabelOk?cfg.LabelCancel:'取消';
				var panel,btnstr;
				if(cfg.type == 'Alert' || cfg.type == 'Confirm'){
				    btnstr = (cfg.type == 'Alert') ? '<a id="jit_btn_ok">' + cfg.LabelOk + '</a>' : '<a id="jit_btn_cancel" style="width:50%;">' + cfg.LabelCancel + '</a><a id="jit_btn_ok"  style="width:50%;">' + cfg.LabelOk + '</a>';
				    if(cfg.isDpi===1){
				    	panel = $('<div id="dialog_div" class="isDpiBoxAlert"><div class="jit-ui-panel"></div><div name="jitdialog" id="isDpiBoxAlert" class="popup br-5"><div class="dislog__top"><em class="dislog__close"></em></div>'
						  + '<p class="ac" id="dialog__content">'+cfg.content+'</p><div class="popup_btn">'
						  + btnstr + '</div></div></div>');
				    }else{
				    	panel = $('<div id="dialog_div"><div class="jit-ui-panel"></div><div name="jitdialog" style="margin-top:120px" class="popup br-5"><div class="dislog__top"><em class="dislog__close"></em></div>'
						  + '<p class="ac" id="dialog__content">'+cfg.content+'</p><div class="popup_btn">'
						  + btnstr + '</div></div></div>');
				    }
				}else if(cfg.type == 'Dialog'){
					if(cfg.isAppend){  //追加内容
						if($("#dialog__content").length){
							$("#dialog__content").append("<br/>"+cfg.content);
						}else{
							panel = $('<div id="dialog_div"><div class="jit-ui-panel"></div><div style="margin-top:120px" class="popup br-5"><div class="dislog__top"><em class="dislog__close"></em></div><p class="ac" id="dialog__content">'+cfg.content+'</p></div></div>');
						}
					}else if(cfg.isDpi===1){
						panel = $('<div id="dialog_div"><div class="jit-ui-panel"></div><div id="isDpiBox" class="popup br-5"><p class="ac" id="dialog__content">'+cfg.content+'</p></div></div>');

					}else if(cfg.isDpi===2){
						panel = $('<div id="dialog_div"><div class="jit-ui-panel"></div><div id="unDpiBox" class="popup br-5"><p class="ac" id="dialog__content">'+cfg.content+'</p></div></div>');

					}else{
						panel = $('<div id="dialog_div"><div class="jit-ui-panel"></div><div style="margin-top:120px" class="popup br-5"><div class="dislog__top"><em class="dislog__close"></em></div><p class="ac" id="dialog__content">'+cfg.content+'</p></div></div>');
					}
					if(cfg.times){
						setTimeout(function(){
							$("#dialog_div").hide();
						},cfg.times);
					}
				}
				if(panel){
					panel.css({
						'position':'fixed',
						'left':'10px',
						'right':'10px',
						'top':'0',
						'bottom':'0',
						'z-index':'99999'
					});
					if($("#dialog_div").length){
						$("#dialog_div").remove();
					}
					panel.appendTo($('body'));
					(function(panel,cfg){
						setTimeout(function(){
							$(panel.find('.dislog__close')).bind('click',function(){Jit.UI.Dialog('CLOSE');});
							if(cfg.CallBackOk){
								$(panel.find('#jit_btn_ok')).bind('click',cfg.CallBackOk);
							}
							if(cfg.CallBackCancel){
								$(panel.find('#jit_btn_cancel')).bind('click',cfg.CallBackCancel);
							}else{
								$(panel.find('#jit_btn_cancel')).bind('click',function(){Jit.UI.Dialog('CLOSE');});
							}
						},16);
					})(panel,cfg);
				}
				/*
				var dialogdom =$('[name=jitdialog]');
				dialogdom.css({
					'left':(Jit.winSize.width-dialogdom.width())/2,
					'top':(Jit.winSize.height-dialogdom.height())/2,
				});
				*/
			}
		},
        Masklayer:{
            show:function(){
			alert($('#masklayer').length);
				if($('#masklayer').length<=0){
					var mask = $('<div id="masklayer" style="position:fixed;background-color:#ECECEC;width:100%;height:100%;line-height:100%;z-index:9999;top:0;left:0;text-align:center"><img src="../../../images/common/loading.gif" style="margin:30px auto;" alt="" /></div>');
					mask.appendTo('body');
				}
				$('#masklayer').css({'opacity':'0.6'}).show();
            },
            hide:function(){
                $('#masklayer').hide();
            }
        },
        Loading:function(display,msg){
        	if(display||arguments.length==0){
        		msg = msg || '正在加载...';
        		var _html = '<div id="wxloading" class="wx_loading">'
        				  + 	'<div class="wx_loading_inner">'
        				  + 		'<i class="wx_loading_icon"></i>'
        				  + 		'<span>'+ msg +'</span>'
        				  +		'</div>'
        				  +	'</div>'

        		$('body').append(_html);
        	}else{
        		$('#wxloading').remove();
        	}
        },
        AjaxTips:{
        	//显示ajax加载数据的时候   出现加载图标
        	Loading:function(flag){
        		if(flag||arguments.length==0){
        			//显示loading
        			UI.Loading(true);
        		}else{
        			//隐藏loading
        			UI.Loading(false);
        		}
        	},
        	//加载数据
        	Tips:function(options){
        		var left="50%",
        			top="50%";
        		if(options.left){
        			left=options.left;
        		}
        		if(options.top){
        			top=options.top;
        		}
        		if(options.show){//显示tips
					if($("#ajax__tips").length>0){
        				$("#ajax__tips").remove();
        			}
        			var style="position:fixed;top:"+top+";  left:"+left+";  width:100px;  height:100px;margin-top:-50px;margin-left:-50px;text-align: center;line-height100px;";
        			var $div=$("<div id='ajax__tips' style='"+style+"'>"+(options.tips?options.tips:"暂无数据")+"</div>");
        			$("body").append($div);
	        		
        		}else{  //隐藏tips
					$("#ajax__tips").hide();
        		}
        	}
        },
		Image:{
			getSize:function(src,size){
				return src;
				if(!src){
					return '/HtmlApps/images/common/misspic.png';
				}
				var _src = src.replace(/(.png)|(.jpg)/,function(s){
					return '_'+size+s;
				});
				return _src;
			}
		},
		showPicture:function(className){
			if (!className || className.length == 0	|| $("img."+className).length ==0) {
				return;
			}
			var imgList = [];
			for(var i=0,idata;i<$("img."+className).length;i++){
				idata = $("img."+className)[i];
				if(idata.src.length){
					imgList.push(idata.src);
				}
			}
			$("body").delegate("img."+className,JitPage.eventType,function(){
				WeiXin.imagePreview(this.src,imgList);
			});
		},
		'Nav':{
			init:function(){
				var items = $('#topNav li'),
					cfg = Jit.AM.getAppPageConfig(),
					navcfg = null;
				//是否显示顶部导航
				if(cfg.Config && cfg.Config.Navigation){
					navcfg = cfg.Config.Navigation;
				}
				if(!navcfg){
					return;
				}
				//动态配置导航信息
				if(items.length != navcfg.count){
					var htmls = '';
					for(var i=0;i<navcfg.count;i++){
						htmls += "<li><a href=\""+(navcfg.href[i]||'')+"\"></a></li>";
					}
					$('#topNav ul').html(htmls);
				}else{
					for(var i in navcfg.href){
						$(items.eq(i).find('a')).attr('href',(navcfg.href[i]||''));
					}
				}
			},
			setItemHref:function(idx,href){
				var items = $('#topNav li');
				$(items.get(idx)).attr('href',href);
			},
			showItemTips:function(idx,msg){
				var item = $('#topNav a').eq(idx);
				item.html('<span style="display: inline;">'+msg+'</span>');
			},
			displayItem:function(idx,display){
				if(display){
					$('#topNav li').eq(idx).show();
				}else{
					$('#topNav li').eq(idx).hide();
				}
			}
		}
	};
	
	global.Jit = (function(){
		var _jit = new Function();
		_jit.prototype = fn;
		_jit = new _jit();
		_jit.fn = fn;
		_jit.appManage = appManage;
		_jit.AM = appManage;
		_jit.WX = WeiXin;
		_jit.UI = UI;
		_jit.deviceType = deviceType;
		return _jit;
	})();

})(window,undefined);

(function(){
	/***************************      添加加减乘除操作，解决js浮点运算的bug  ---begin    ***************************/
	//除法函数，用来得到精确的除法结果
	//说明：javascript的除法结果会有误差，在两个浮点数相除的时候会比较明显。这个函数返回较为精确的除法结果。
	//调用：accDiv(arg1,arg2)
	//返回值：arg1除以arg2的精确结果
	function accDiv(arg1, arg2) {
		var t1 = 0, t2 = 0, r1, r2;
		try {
			t1 = arg1.toString().split(".")[1].length;
		} catch(e) {
		}
		try {
			t2 = arg2.toString().split(".")[1].length;
		} catch(e) {
		}
		with (Math) {
			r1 = Number(arg1.toString().replace(".", ""));
			r2 = Number(arg2.toString().replace(".", ""));
			return (r1 / r2) * pow(10, t2 - t1);
		}
	}
	//乘法函数，用来得到精确的乘法结果
	//说明：javascript的乘法结果会有误差，在两个浮点数相乘的时候会比较明显。这个函数返回较为精确的乘法结果。
	//调用：accMul(arg1,arg2)
	//返回值：arg1乘以arg2的精确结果
	function accMul(arg1, arg2) {
		var m = 0,
			s1 = arg1.toString(),
			s2 = arg2.toString();
		try {
			m += s1.split(".")[1].length;
		} catch(e) {
		}
		try {
			m += s2.split(".")[1].length;
		} catch(e) {
		}
		return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
	}
	//加法函数，用来得到精确的加法结果
	//说明：javascript的加法结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的加法结果。
	//调用：accAdd(arg1,arg2)
	//返回值：arg1加上arg2的精确结果
	function accAdd(arg1, arg2) {
		var r1, r2, m;
		try {
			r1 = arg1.toString().split(".")[1].length;
		} catch(e) {
			r1 = 0;
		}
		try {
			r2 = arg2.toString().split(".")[1].length;
		} catch(e) {
			r2 = 0;
		}
		m = Math.pow(10, Math.max(r1, r2));
		return (arg1 * m + arg2 * m) / m;
	}
	//减法函数，用来得到精确的减法结果
	//说明：javascript的减法结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的减法结果。
	//调用：accSubtr(arg1,arg2)
	//返回值：arg1减去arg2的精确结果
	function accSubtr(arg1, arg2) {
		var r1, r2, m, n;
		try {
			r1 = arg1.toString().split(".")[1].length;
		} catch(e) {
			r1 = 0;
		}
		try {
			r2 = arg2.toString().split(".")[1].length;
		} catch(e) {
			r2 = 0;
		}
		m = Math.pow(10, Math.max(r1, r2));
		//动态控制精度长度
		n = (r1 >= r2) ? r1 : r2;
		return ((arg1 * m - arg2 * m) / m).toFixed(n);
	}
	/*
	给基本类型添加原型方法添加不上。
	
	//给Number类型增加一个div方法，调用起来更加方便。
	Number.prototype.div = Number.prototype.divided = function(arg) {
		return accDiv(this, arg);
	};
	//给Number类型增加一个mul方法，调用起来更加方便。
	Number.prototype.mul =Number.prototype.multiplied = function(arg) {
		return accMul(arg, this);
	};
	//给Number类型增加一个add方法，调用起来更加方便。
	Number.prototype.add = function(arg) {
		return accAdd(arg, this);
	};
	//给Number类型增加一个subtr 方法，调用起来更加方便。
	Number.prototype.subtr = Number.prototype.subtract = function(arg) {
		return accSubtr(arg, this);
	};
	*/
	Math.div = Math.divided = accDiv;
	Math.mul = Math.multiplied = accMul;
	Math.add = accAdd;
	Math.subtr = accSubtr;
	/***************************      添加加减乘除操作，解决js浮点运算的bug  ---end    ***************************/
})(window);

