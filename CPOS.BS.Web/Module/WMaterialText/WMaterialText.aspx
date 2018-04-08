<%@ Page Title="" Language="C#" MasterPageFile="~/Framework/MasterPage/CPOS.Master"
    AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<meta charset="UTF-8" />
    <title>图文素材管理</title>
    <%--    <script type="text/javascript" src="/framework/javascript/Other/jquery-1.9.0.min.js"></script>--%>
    <script src="<%=StaticUrl+"/Framework/javascript/Biz/WApplicationInterface.js"%>" type="text/javascript"></script>


    <script type="text/javascript" src="<%=StaticUrl+"/Framework/javascript/other/fancybox-v2/lib/jquery.mousewheel-3.0.6.pack.js"%>"></script>
    <script type="text/javascript" src="<%=StaticUrl+"/Framework/javascript/other/fancybox-v2/source/jquery.fancybox.js?v=2.1.4"%>"
        charset="gb2312"></script>
    <link rel="stylesheet" type="text/css" href="<%=StaticUrl+"/Framework/javascript/other/fancybox-v2/source/jquery.fancybox.css?v=2.1.4"%>"
        media="screen" />
    <link rel="stylesheet" type="text/css" href="<%=StaticUrl+"/Framework/javascript/other/fancybox-v2/source/helpers/jquery.fancybox-buttons.css?v=1.0.5"%>" />
    <script type="text/javascript" src="<%=StaticUrl+"/Framework/javascript/other/fancybox-v2/source/helpers/jquery.fancybox-buttons.js?v=1.0.5"%>"></script>
    <link rel="stylesheet" type="text/css" href="<%=StaticUrl+"/Framework/javascript/other/fancybox-v2/source/helpers/jquery.fancybox-thumbs.css?v=1.0.7"%>" />
    <script type="text/javascript" src="<%=StaticUrl+"/Framework/javascript/other/fancybox-v2/source/helpers/jquery.fancybox-thumbs.js?v=1.0.7"%>"></script>
    <script type="text/javascript" src="<%=StaticUrl+"/Framework/javascript/other/fancybox-v2/source/helpers/jquery.fancybox-media.js?v=1.0.5"%>"></script>
    
    <script src="<%=StaticUrl+"/Module/WMaterialText/Controller/WMaterialTextCtl.js?v=0.2"%>" type="text/javascript"></script>
    <script src="<%=StaticUrl+"/Module/WMaterialText/Model/WMaterialTextVM.js"%>" type="text/javascript"></script>
    <script src="<%=StaticUrl+"/Module/WMaterialText/Store/WMaterialTextVMStore.js"%>" type="text/javascript"></script>
    <script src="<%=StaticUrl+"/Module/WMaterialText/View/WMaterialTextView.js"%>" type="text/javascript"></script>
      <link rel="stylesheet" type="text/css" href="<%=StaticUrl+"/Module/static/jkb/css/v.css?v=1.0"%>"
        media="screen" />
     <script src="<%=StaticUrl+"/Module/static/jkb/js/v.js?v=1.0"%>" type="text/javascript"></script>
    <style type="text/css">
        td {
            vertical-align: middle;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="section">
        <div class="m10 article">
            <div class="art-tit">
                <div class="view_Search">
                    <span id='span_panel'></span>
                </div>
                <div class="view_Search2">
                    <span id='span_panel2'></span>
                </div>
            </div>
            <div class="art-titbutton">
                <div class="view_Button">
                    <span id='span_create'></span>
                </div>
            </div>
            <div class="DivGridView" id="DivGridView">
            </div>
            <div class="cb">
            </div>
        </div>
    </div>

    <script type="text/javascript">
        
        //获取新手引导步数
         function GetModuleNextStep(datas, callback) {

            var json ={
                    "Locale": null,
                    "CustomerID": null,
                    "UserID": null,
                    "OpenID": null,
                    "Token": null,
                    "ChannelID": null,
                    "Parameters": datas
                }
            
            //console.log(JSON.stringify(json));

            $.ajax({
                url: "/ApplicationInterface/Gateway.ashx?type=Product&action=Basic.Guide.GetModuleNextStep",
                data: {
                    req: JSON.stringify(json)
                },
                success: function (data) {
                    var data = JSON.parse(data);
                    if (data.IsSuccess && data.ResultCode == 0) {
                        //  $.util.isLoading(true);
                        if (callback) {
                            callback(data.Data);
                        }
                    } else {
                        //$.util.isLoading(true);
                       alert("提示", data.Message);
                    }
                }
            });
        };
        //记录模块的最后一步
       function SetModuleLastStep(datas, callback) {
            var that = this;
            var json = {
                "Locale": null,
                "CustomerID": null,
                "UserID": null,
                "OpenID": null,
                "Token": null,
                "ChannelID": null,
                "Parameters": datas
            }
            $.ajax({
                url: "/ApplicationInterface/Gateway.ashx?type=Product&action=Basic.Guide.SetModuleLastStep",
                data: {
                    req: JSON.stringify(json)
                },
                success: function (data) {
                    var data = JSON.parse(data);
                    if (data.IsSuccess && data.ResultCode == 0) {
                        //  $.util.isLoading(true);
                        if (callback) {
                            callback(data.Data);
                        }
                    } else {
                        //$.util.isLoading(true);
                        alert("提示", data.Message);
                    }
                }
            });
        };

        var url = window.location.href;

        if (url.indexOf("stepIndex") != -1 && url.indexOf("GuId") != -1) {
            var stepIndex = parseInt(url.substr(url.indexOf("stepIndex=") + 10, 1));
            var GuId = url.substr(url.indexOf("GuId=") + 5);
            GetModuleNextStep({
                "action": 'Basic.Guide.GetModuleNextStep',
                "ModuleCode": "Operation",
                "ParentModule": GuId,// "9DDC3C3C-8C22-43A9-A64F-CA17DD4DD2C3",
                "Step": stepIndex,

            }, function (data) {
                console.log(data);
                if (data.NowModule) {
                    var step1 = new noviceGuide("step1", {
                        top: 170,
                        left: 300,
                        isShow: true,
                        title: "编辑图文咨询",
                        content: "图文编辑是资讯建立的基础，</br> 图文并茂让您的资讯更富有吸引力",
                        step: data.NowModule.ModuleStep,
                        sumStep: 8,
                        video: {
                            top: 200,
                            left: 700,
                            src: "http://guide.chainclouds.cn" + data.NowModule.VideoUrl// "../static/jkb/movie/sczx.mp4",
                        },
                        direction: {
                            angle: -160,
                            top: 20,
                            left: "0"
                        }
                    }, function () {
                        window.location.href = data.PreModule.Url + "#stepIndex=" + data.PreModule.ModuleStep + "&GuId=" + data.PreModule.ParentModule;
                    }, function () {

                        SetModuleLastStep({
                            "action": 'Basic.Guide.SetModuleLastStep',
                            "ModuleCode": "Operation",
                            "ParentModule": GuId,//"BAB72601-4801-4403-B97F-F97CC0B9A564",
                            "Step": stepIndex,
                            "Url": data.NowModule.Url,
                            "FinishedStatus": 0
                        }, function () {
                            window.location.href = data.NextModule.Url + "#stepIndex=" + data.NextModule.ModuleStep + "&GuId=" + data.NextModule.ParentModule;
                        });
                    });
                }

            });
        };
    </script>
</asp:Content>
