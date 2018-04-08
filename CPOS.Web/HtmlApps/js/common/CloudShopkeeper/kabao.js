/**
 * Created by Administrator on 2014/12/15.
 */

Jit.AM.defindPage({

    onPageLoad: function () {

        //当页面加载完成时触
        this.initEvent();
        this.loadData();
    },
    initEvent:function(){

    },
    loadData: function () {
        $('#menu').delegate(".panl","click",function(){
            location.href= $(this).find("a").attr("href")
        });

        var self = this;

            self.ajax({
                url: 'http://wxapi.xgxshop.com/applicationinterface/customer/CloudGateway.ashx',
                //url: '~/applicationinterface/customer/CloudGateway.ashx',
                type:'post',
                data: {
                    'action': 'GetCardBag'
                },
                beforeSend:function(){
                    Jit.UI.Loading(true);
                    self.isSending = true;
                },
                success: function (data) {
                    debugger;
                    self.renderPage(data.Data);
                },
                complete:function(){
                    Jit.UI.Loading(false);
                    self.isSending =false;
                }
            });



    },
    renderPage: function (data) {
        debugger;
        var itemlists = data.CardBagList;
        if(itemlists) {
            var tpl = $('#panlList').html(), html = '';

            for (var i = 0; i < itemlists.length; i++) {
                var itemdata = itemlists[i];

                itemdata.index = i % 3 + 1;


                html += Mustache.render(tpl, itemdata);
            }
            //console.log("index:"+this.page.index)


            if (itemlists.length == 0) {
                $('#menu').after('<div style="width:320px;height:100px;text-align: center;position: absolute;left:50%;top:50%;margin:-50px 0 0 -160px;;">你没有关注任何商家!</div>');
            }
            else {
                $('#menu').append(html).siblings().remove();
            }
        }else{
            $('#menu').after('<div style="width:320px;height:100px;text-align: center;position: absolute;left:50%;top:50%;margin:-50px 0 0 -160px;;">你没有关注任何商家!</div>');
        }
    }
});