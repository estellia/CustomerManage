(function () {
    var debugInfo = {
        'openId': 'owuD8w5RucIbAPelqVgg-6uZ0X30',//oQ6Y9t92Za96QeBX3zHZ4VQhpNtc
        'userId': '837a5468201c4818aef79fc1cf71de8f',//17a3a58b4c7c4128a37e8242686dc187
        'locale': 'ch'
    };
    var appcfg = Jit.AM.getAppVersion();
    Jit.WX.ToolBar(appcfg['APP_TOOL_BAR']);
    Jit.WX.OptionMenu(appcfg['APP_OPTION_MENU']);
    //分享功能初始化
    Jit.WX.initShare();
    /*
    //TODO:delete
    if (appcfg['APP_DEBUG_PANEL']) {
        $('body').append('<div class="jit-debug-panel"></div>');
    }
	*/
    var version = ((appcfg.APP_CACHE === false || appcfg.APP_CACHE == 'False') ? ((new Date()).getTime()) : appcfg.APP_VERSION);
    Jit.log('APP_VERSION : ' + version);
    Jit.log('APP_NAME: ' + appcfg['APP_NAME']);

    var PageConfigs = Jit.AM.getAppPageConfig();
    //获取页面的KTY值
    var htmlname = $('title').attr('name');
    if(!htmlname) {
        alert('Html 中页面名称未定义');
        return;
    }
    //获取KEY值页面的配置参数
    var pageconfig = PageConfigs[htmlname];
    if(!pageconfig) {
        alert('Config.js 中未找到对应的HTML');
        return;
    }

    var urlparam = location.href.split('?')[1];
    if(urlparam){
        Jit.AM.pageHistoryPush(htmlname + ':' + urlparam);
    }else{
        Jit.AM.pageHistoryPush(htmlname);
    }

    //设置页面的title
    $('title').html(Jit.AM.getUrlParam('title') || pageconfig.title);

    var JsLoad = false,
        CssLoad = false,
        styles = [],
        styl,
        common = null;
    /*
    //TODO:delete目前发现PageConfigs对象中没有config属性
    if (PageConfigs['Config'] && PageConfigs['Config']['Common']) {
        common = PageConfigs['Config']['Common'];
    }
    if (common && common['style']) {
        var commonstyles = common['style'];
        for (var i = 0; i < commonstyles.length; i++) {
            styl = commonstyles[i];
            styl = styl.replace(/%(\S*)%/, function (str) {
                return PageConfigs['Config']['Shorthand'][str.substring(1, str.length - 1)];
            })
            styles.push(JitCfg.baseUrl + 'css/' + styl + '.css?version=' + version);
        }
    }
    */
    //遍历需要加载的css文件
    if (!!pageconfig['style']) {
        for (var i = 0; i < pageconfig.style.length; i++) {
            styl = pageconfig.style[i];
            styl = styl.replace(/%(\S*)%/, function (str) {
                //TODO:delete
                return PageConfigs['Config']['Shorthand'][str.substring(1, str.length - 1)];
            })
            if (styl) {
                styles.push(JitCfg.baseUrl + 'css/' + styl + '.css?version=' + version);
            }
        }
    }

    //校验css文件是否加载完成
    function checkCssLoadComplete() {
        var cssTimer = setInterval(function () {
			debugger;
            var loadnum = 0 , cssList = $('link');
            if (!cssList || !cssList.size()) {
                clearInterval(cssTimer);
                CssLoad = true;
                checkFileHasLoad();
                return false;
            }
            cssList.each(function () {
                var me = this;
                /*
                 link 标签如果不带 type='text/css' 会导致无法检测到css加载完成，所以该段代码先注释掉
                 if(me.type != 'text/css'){
                 return false;
                 }
                 */
                if (me.sheet && me.sheet.cssRules) {
                    loadnum++;
                }
            });
            if (loadnum >= styles.length) {
                clearInterval(cssTimer);
                CssLoad = true;
                checkFileHasLoad();
            }
        }, 500);
    }
    //加载css文件
    Jit.loadFiles(styles);
    checkCssLoadComplete();

    //加载js文件
    function loadJs(list, type, callback) {
        if (!$.isArray(list)){
            return;
        }
        var js_arr = [],
            type = ( type == 'plugin' ? JitCfg.baseUrl + 'plugin/' : JitCfg.baseUrl + 'js/' );
        $.each(list, function (key, val) {
            val = val.replace(/%(\S*)%/, function (str) {
                return PageConfigs['Config']['Shorthand'][str.substring(1, str.length - 1)];
            })
            if (val) {
                js_arr.push(type + val + '.js?version=' + version);
            }
        });
        require(js_arr, callback);
    }

    //先加载plugin中的js文件，再校验js文件是否加载完成
    function loadPageScript(allscripts) {
        if ($.isArray(allscripts) && allscripts.length > 0) {
            loadJs(allscripts, 'script', function () {
                Jit.log('script files load complete');
                JsLoad = true;
                checkFileHasLoad();
            });
        } else {
            JsLoad = true;
            checkFileHasLoad();
        }
    }

    //遍历需要加载的js文件
    if (pageconfig) {
        var plugins = pageconfig.plugin ? pageconfig.plugin : [],
			allscripts = pageconfig.script ? pageconfig.script : [];
        if (common && common['script'] && common['script'].length > 0) {
            allscripts = common['script'].concat(allscripts);
        }
        if ($.isArray(plugins) && plugins.length > 0) {
            loadJs(plugins, 'plugin', function () {
                loadPageScript(allscripts);
            });
        } else {
            loadPageScript(allscripts);
        }
    }

    //校验css，js是否都load完成
    function checkFileHasLoad() {
        if (CssLoad && JsLoad) {
            main();
        }
    }

    //网速不给力，css,js没有load完成时，给以提示
    var fileloadtimer = null, timeout = 12000;
    function loadstatus() {
        if (fileloadtimer) {
            var c = confirm('您的网络不给力，页面加载失败！是否继续等待？');
            if (c) {
                fileloadtimer = setTimeout(loadstatus, timeout);
            } else {
                clearTimeout(fileloadtimer);
                $('.wx_loading_inner').html('装载页面样式及脚本失败，请稍后重试！').css({
                    'width': '152px',
                    'padding': '12px'
                });
            }
        }
    }
    fileloadtimer = setTimeout(loadstatus, timeout);

    //css,js文件load完成之后执行main函数
    function main() {
        clearTimeout(fileloadtimer);
        var cfg = Jit.AM.getAppVersion();
        Jit.log('current run model :' + (cfg.APP_DEBUG ? 'Debug' : 'Release'));
        Jit.log('customer id : ' + Jit.AM.CUSTOMER_ID);

        //如果URL中有设置就重置APP_TYPE的值
        if (Jit.AM.getUrlParam('APP_TYPE')) {
            cfg['APP_TYPE'] = Jit.AM.getUrlParam('APP_TYPE');
        }

        //Debug：false(正式)，true（测试）
        //正式环境存储url中的参数
        //测试环境存储假的debugInfo的参数
        var openId = Jit.cookie.getAuth('openId_'+Jit.AM.CUSTOMER_ID),
            userId = Jit.cookie.getAuth('userId_'+Jit.AM.CUSTOMER_ID);
        var initBase = Jit.AM.getBaseAjaxParam();
        if (cfg.APP_DEBUG != 'False' && cfg.APP_DEBUG) {
            debugInfo.customerId = Jit.AM.CUSTOMER_ID;
            var opendIdUrl = Jit.AM.getUrlParam('openId'),
                userIdUrl = Jit.AM.getUrlParam('userId');
            if(opendIdUrl && userIdUrl){
                debugInfo.openId = Jit.AM.getUrlParam('openId');
                debugInfo.userId = Jit.AM.getUrlParam('userId');
            }
            Jit.AM.setBaseAjaxParam(debugInfo);
        } else if (openId && userId) {
            var keys = cfg.AJAX_PARAMS.split(',');
            var param = {};
            for (var key in keys) {
                hash = Jit.AM.getUrlParam(keys[key]);
                if(hash){
                    param[keys[key]] = hash;
                }
            }
            param.customerId = Jit.AM.CUSTOMER_ID;
            param.openId = openId;
            param.userId = userId;
            Jit.AM.setBaseAjaxParam(param);
            //Jit.cookie.del('openId_'+Jit.AM.CUSTOMER_ID);
            //Jit.cookie.del('userId_'+Jit.AM.CUSTOMER_ID);
        }
        var info = Jit.AM.getBaseAjaxParam();
        //判断微信平台：SERVICE:服务号，SUBSCIBE:订阅号，SUBSCIBE_NOINFO:订阅号-不自动生成用户ID
        if (!cfg.APP_DEBUG && cfg['APP_TYPE'] && cfg['APP_TYPE'] == 'SERVICE') {
            Jit.log('公共帐号类型：服务号');
            var hasAuth = Jit.AM.getUrlParam('CheckOAuth');
            if ((!info.userId && hasAuth != 'unAttention')
                || (Jit.AM.getAppParam('Launch', 'CheckOAuth') == 'unAttention' && hasAuth != 'unAttention' && !Jit.AM.getUrlParam('openId'))) {
                var url = window.location.href.replace('http://', ''),
                    surl = '',
                    customerId = Jit.AM.getUrlParam('customerId'),
                    authType = Jit.AM.getUrlParam('authType');
                url = url.replace(/&+/gi, '%26');
                url = url.replace('customerId=' + customerId, '');
                surl = '/WXOAuth/AuthUniversal.aspx?&customerId=' + customerId;
                if (authType) {
                    surl = surl + '&scope=' + authType;
                }
                surl = surl + '&goUrl=' + url;
                window.location.href = surl;
                return;
            } else if (!info.userId && hasAuth == 'unAttention') {
                Jit.log('未关注用户，并且没有userId');
                Jit.AM.setAppParam('Launch', 'CheckOAuth', 'unAttention');
                if (!info.userId) {
                    var userId = Jit.AM.buildUserId();
                    Jit.AM.setBaseAjaxParam({
                        'customerId': Jit.AM.CUSTOMER_ID,
                        'userId': userId,
                        'openId': userId
                    });
                }
                JitCfg.CheckOAuth = 'unAttention';
            } else {
                Jit.AM.setAppParam('Launch', 'CheckOAuth', 'Attention');
            }
        } else if (!cfg.APP_DEBUG && !info.userId && cfg['APP_TYPE'] && (cfg['APP_TYPE'] == 'SUBSCIBE' || cfg['APP_TYPE'] == 'SUBSCIBE_NOINFO')) {
            Jit.log('公共帐号类型：订阅号');
            if (cfg['APP_TYPE'] == 'SUBSCIBE_NOINFO') {
                Jit.AM.setBaseAjaxParam({
                    'customerId': Jit.AM.CUSTOMER_ID,
                    'userId': null,
                    'openId': null
                });
            } else {
                var userId = Jit.AM.buildUserId();
                Jit.AM.setBaseAjaxParam({
                    'customerId': Jit.AM.CUSTOMER_ID,
                    'userId': userId,
                    'openId': userId
                });
            }
        }
        delete debugInfo;

        //TODO:delete
        /*
        if(Jit.AM.getUrlParam("FromYun")) {
            userId=Jit.AM.getUrlParam("userId");
            Jit.AM.setBaseAjaxParam({
                'userId': userId
            });
        }
        */
        //重置公共参数变量
        info = Jit.AM.getBaseAjaxParam();
        Jit.log('user id : ' + info.userId);
        //load页面的元素，执行私有的js
        if (Jit.AM.onLoad) {
            Jit.AM.onLoad();
            Jit.log('Jit.AM.onLoad start');
        }
        //是否记录页面的日志
        if (Jit.AM.isPageNeedLog()) {
            if (Jit.AM.getUrlParam('isShare') == 'YES') {
                Jit.AM.logToServer('browserForward');
            } else {
                Jit.AM.logToServer('browser');
            }
        }
        //初始化顶部导航
        Jit.UI.Nav.init();
        if (pageconfig.param && pageconfig.param.Navigation) {
            var items = $('#topNav li');
            var navcfg = pageconfig.param.Navigation;
            if (!navcfg) {
                return;
            }
            if (items.length != navcfg.count) {
                var htmls = '';
                for (var i = 0; i < navcfg.count; i++) {
                    htmls += "<li><a href=\"" + (navcfg.href[i] || '') + "\"></a></li>";
                }
                $('#topNav ul').html(htmls);
            } else {
                for (var i in navcfg.href) {
                    $(items.eq(i).find('a')).attr('href', (navcfg.href[i] || ''));
                }
            }
        }
        //页面对象JitPage存在hideMask方法时,由页面本身控制MaskLayer的显示与隐藏
        try {
            if (typeof JitPage != "undefined") {
                if (!JitPage.hideMask) {
                    $('#masklayer').hide();
                }
            } else {
                $('#masklayer').hide();
            }
        } catch (e) {
            alert(e.message);
        }
    }
})();

