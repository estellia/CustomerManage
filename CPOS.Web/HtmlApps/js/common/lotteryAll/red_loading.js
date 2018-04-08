Jit.AM.defindPage({
    name: 'RedLoading',
	initWithParam: function(param) {
	},
	onPageLoad : function() {
		//当页面加载完成时触发
		
	    this.setShareAH();
		this.loadData();
	},
	loadData:function(){
		var that = this;
		that.CTWEventShareLog();
		if (Jit.AM.getUrlParam('H5URL') != null && Jit.AM.getUrlParam('H5URL') != "") {

		    window.location.href = "http://" + Jit.AM.getUrlParam('H5URL').replace(/_Slash_/g, "\\").replace( /_questionmark_/g,"\?").replace( /_equal_/g,"=");

		}
	},
	setShareAH: function () {
	    var that = this,
			params = {
			    'action': 'Event.Lottery.Share',
			    'EventId': Jit.AM.getUrlParam('EventId'),
			    'ShareUserId': Jit.AM.getUrlParam('sender') || '',
			    'TypeId': '',
			};

        

	    if (Jit.AM.getUrlParam('CTWEventId') != null)
	    {
	        params = {
	            'action': 'Event.Lottery.Share',
	            'EventId': Jit.AM.getUrlParam('CTWEventId'),
	            'ShareUserId': Jit.AM.getUrlParam('sender') || '',
	            'TypeId': '',
	        };
	    }
	    that.ajax({
	        url: '/ApplicationInterface/Gateway.ashx',
	        //'async': false,
	        data: params,
	        success: function (data) {
	            if (data.IsSuccess) {
	            } else {
	                that.alert(data.Message);
	            }
	        }
	    });
	},
    //分享日记
    CTWEventShareLog: function () {
        if (Jit.AM.getUrlParam('CTWEventId') != null) {
            var that = this,
                params = {
                    'action': 'CreativityWarehouse.Log.CTWEventShareLog',
                    'CTWEventId': Jit.AM.getUrlParam('CTWEventId'),
                    'Sender': Jit.AM.getUrlParam('sender'),
                    'OpenId': Jit.AM.getUrlParam('openId'),
                    'BeSharedOpenId': Jit.AM.getUrlParam('BeSharedOpenId'),
                    'BEsharedUserId': Jit.AM.getUrlParam('userId'),
                    'ShareURL': location.pathname.replace(Jit.AM.getUrlParam('oldurl').replace(/_Slash_/g, "\\"), Jit.AM.getUrlParam('newurl').replace(/_Slash_/g, "\\"))
                    //'RecommandId': Jit.AM.getUrlParam('sender') || ''
                };
            that.ajax({
                url: '/ApplicationInterface/Gateway.ashx',
                data: params,
                success: function (data) {
                    if (data.IsSuccess && data.Data) {
                        var result = data.Data;

                        if (result) {

                        }

                    } else {
                        alert(data.Message);
                    }
                    Jit.UI.Loading(false);
                }
            });
        }

    }
}); 