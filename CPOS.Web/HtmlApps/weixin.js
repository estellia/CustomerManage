// JavaScript Document
define(['/HtmlApps/lib/jweixin-1.0.0.js'], function (wx) {
    this.shareData = {
        title: document.title,
        desc: document.title,
        link: window.location.href,
        imgUrl: "",
        type: "",
        dataUrl: "",
        success: function (res) {

        },
        cancel: function (res) {
        }
    };
    this.config = Jit.AM.ajax({
        url: "/ApplicationInterface/Customer/CustomerGateway.ashx",
        async: false,
        data: {
            action: "getJsApiConfig",
            debug: false,
            url: window.location.href
        },
        success: function (result) {
            debugger;
            if (!result.ResultCode) {
                result.Data.jsApiList = ["onMenuShareTimeline", "onMenuShareAppMessage", "onMenuShareQQ", "onMenuShareWeibo"];
                wx.error(function (res) {
                    //alert(JSON.stringify(res));
                });
                wx.config(result.Data);
            }
        },
        fail: function (result) {
            alert(JSON.stringify(result));
        }
    });
    this.isWeiXin = false;
    wx.checkJsApi({
        jsApiList: ["onMenuShareTimeline"],
        success: function (res) {
            debugger;
            setWeiXin();
        }
    });
    function setWeiXin() {
        wx.isWeiXin = true;
        this.isWeiXin = true;
    }
    this.init = function (shareData) {
        shareData = shareData || this.shareData;
        wx.ready(function () {
            wx.onMenuShareTimeline({
                title: shareData.title,
                link: shareData.link,
                imgUrl: shareData.imgUrl,
                success: shareData.success,
                cancel: shareData.cancel
            });
            wx.onMenuShareAppMessage({
                title: shareData.title,
                desc: shareData.desc,
                link: shareData.link,
                imgUrl: shareData.imgUrl,
                type: shareData.type,
                dataUrl: shareData.dataUrl,
                success: shareData.success,
                cancel: shareData.cancel
            });
            wx.onMenuShareQQ({
                title: shareData.title,
                desc: shareData.desc,
                link: shareData.link,
                imgUrl: shareData.imgUrl,
                success: shareData.success,
                cancel: shareData.cancel
            });
            wx.onMenuShareWeibo({
                title: shareData.title,
                desc: shareData.desc,
                link: shareData.link,
                imgUrl: shareData.imgUrl,
                success: shareData.success,
                cancel: shareData.cancel
            });

        });
		wx.error(function (res) {
		  alert(res.errMsg);
		});
    };
    return { obj: wx  };
});