/**
 * Created by DELL042501 on 2016/6/15.
 */
Jit.AM.defindPage({

    name:'VipCardQRCode',

    onPageLoad:function(){
        this.canGetCodeDetail = true;
        this.loadPageData();
        this.initEvent();
    },
    loadPageData:function(){
        var me = this;
        me.getCodeDetail();
    },
    initEvent:function(){
        var me = this;
        me.getCodeDetailTimer = setInterval("JitPage.getCodeDetail()",60*1000);
        $(".qrcodeTxt").click(function(){
            me.getCodeDetail();
        });
    },
    getCodeDetail:function(){
        var me = this;
        if(me.canGetCodeDetail){

            me.canGetCodeDetail = false;
            Jit.UI.Loading(true);

            me.ajax({
                interfaceMode: 'V2.0',
                url: '/ApplicationInterface/Module/Coupon/CouponHandler.ashx',
                data: {
                    'action': 'getVipCartDetail',
                    'cuponID': ""
                },
                success: function (data) {
                    Jit.UI.Loading(false);

                    if(data.ResultCode == 200) {
                        $(".barcodeImg").attr("src",data.BarUrl);
                        $(".barcodeTxt").html(me.handleNumTxt(data.vipCardCode));
                        $(".qrcodeImg").attr("src",data.QRUrl);
                    }else{
                        Jit.UI.Dialog({
                            'content':data.Message,
                            'type': 'Dialog',
                            'isDpi':2,
                            'times': 1500
                        });
                    };
                },
                complete:function(){
                    me.canGetCodeDetail = true;
                }
            });
        }
    },
    handleNumTxt:function(oldstr){
        var newstr = oldstr || "";
        newstr = newstr.replace(/\s/g,'').replace(/(\d{4})(?=\d)/g,"$1 ");
        return newstr;
    }
});