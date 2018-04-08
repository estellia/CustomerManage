Jit.AM.defindPage({
    name: 'Fanslist',
    questionnaireID: "",
    eventId: "",
    iserror: false,
    ismiyuezhuan: false,
    IsShare: false,
    ModelType: "1",
    back: window.history.back,
    element: {
    },
    onPageLoad: function () {
        this.initPage();
    },
    initPage: function () {
        var that = this;
        that.initEvent();

        that.GetVipFansList("","",function (data) {
            var htmltext = "";
            htmltext += bd.template('Tpl_item', data);
            $(".listcontent").html(htmltext);
        });
    },
    initEvent: function () {
        var that = this;
        debugger;
        //绑定是否同意协议事件
        $('.listhead').on('click', ".tit", function () {
            $(".listhead .tit").removeClass("select")
            $(this).addClass("select");
            that.GetVipFansList($(this).data("code"),"", function (data) {
                debugger;
                var htmltext="";
                htmltext += bd.template('Tpl_item', data);
                $(".listcontent").html(htmltext);
            });
        });

        $(".listcontent").on('click', ".phonebtn", function () {
            var phone = $(this).data("phone");
            if (phone != null && phone != "") {
                $(".l_content span").html(phone);
                $(".dial").attr("href", "tel:" + phone);
                $(".mask").show();
                $(".layer").show();
                $(".closelayer").show();
            }
        });

        $(".colsebtn").on("click", function () {
            $(".mask").hide();
            $(".layer").hide();
            $(".closelayer").hide();
        });

        $(".dial").on("click", function () {
            $(".mask").hide();
            $(".layer").hide();
            $(".closelayer").hide();
        });

        $(".searchbtn").keydown(function (e) {
            var curKey = e.which; 
            if(curKey == 13){ 
                that.GetVipFansList("",$(this).val(), function (data) {
                    debugger;
                    var htmltext = "";
                    htmltext += bd.template('Tpl_item', data);
                    $(".listcontent").html(htmltext);
                });

                return false; 
            } 

        });

    },

    GetVipFansList: function (code,vipname, callback) {
        var that = this;
        if (code == "")
        {
            code = null;
        }
        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
            data: {
                action: 'VIP.Dealer.GetVipFansList',
                Code: code,
                VipName: vipname
            },
            success: function (data) {
                debugger;
                if (data.IsSuccess && data.ResultCode == 0) {
                    var data = data.Data;
                    if (callback) {
                        callback(data);
                    }

                } else {
                    alert(data.Message);
                }
            }
        });

    }
});