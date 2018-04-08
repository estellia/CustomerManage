<%@ Page Title="" Language="C#" MasterPageFile="~/Framework/MasterPage/optimize.Master"
    AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>连锁掌柜-首页</title>
    <link href="css/IndexPage.css?ver=4.2" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="allPage" id="section" data-js="js/IndexPage.js?ver=0.94">
        <div class="indexcontent">
            <div class="contentmodule">
                <div class="modules">
                    <div class="module" data-menucode="wx">
                        <a class="menusrc" href="JavaScript:void(0)">
                            <div class="modulehead">

                                <div class="title">
                                    <img src="images/weixin.png" /><span>微信运营</span>
                                </div>
                                <div class="gomore">
                                    <img src="images/gomore.png" />
                                </div>

                            </div>
                        </a>
                        <div class="modulecontent">
                            <a class="menusrc color1" href="JavaScript:void(0)">点击进入 》         </a>
                        </div>
                    </div>
                    <div class="module " data-menucode="yd">
                        <a class="menusrc" href="JavaScript:void(0)">
                            <div class="modulehead">

                                <div class="title">
                                    <img src="images/shop.png" /><span>微商城</span>
                                </div>
                                <div class="gomore">
                                    <img src="images/gomore.png" />
                                </div>
                            </div>
                        </a>
                        <div class="modulecontent">
                            <div class="count">
                                <div class="numtitle">门店数量</div>
                                <div class="num UnitCount"></div>

                            </div>
                            <div class="achievement">
                                <div class="achievementtitle">当日总业绩</div>
                                <div class="achievementnum"><span class="num UnitCurrentDayOrderAmount">0</span><span class="CurrentMonthunit">万</span><img src="images/defaultdrop.png" class="rise" /><span class="UnitCurrentDayOrderAmountDToD">0%</span></div>

                            </div>
                        </div>
                    </div>
                    <div class="module" data-menucode="hyjk">
                        <a class="menusrc" href="JavaScript:void(0)">
                            <div class="modulehead">

                                <div class="title">
                                    <img src="images/member.png" /><span>会员金矿</span>
                                </div>
                                <div class="gomore">
                                    <img src="images/gomore.png" />

                                </div>
                            </div>
                        </a>
                        <div class="modulecontent">
                            <div class="count">
                                <div class="numtitle">会员总数</div>
                                <div class="num VipCount"></div>

                            </div>
                            <div class="achievement">
                                <div class="achievementtitle">当日新增会员</div>
                                <div class="achievementnum">
                                    <span class="num NewVipCount"></span>
                                    <img src="images/defaultdrop.png" class="rise" /><span class="NewVipDToD">0%</span>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="module" data-menucode="yxlq">
                        <a class="menusrc" href="JavaScript:void(0)">
                            <div class="modulehead">

                                <div class="title">
                                    <img src="images/arms.png" /><span>营销利器</span>
                                </div>
                                <div class="gomore">
                                    <img src="images/gomore.png" />
                                </div>

                            </div>
                        </a>
                        <div class="modulecontent">
                            <div class="count">
                                <div class="numtitle">当前活动数</div>
                                <div class="num EventsCount"></div>

                            </div>
                            <div class="achievement">
                                <div class="achievementtitle">活动参与人数</div>
                                <div class="achievementnum"><span class="num EventJoinCount"></span></div>

                            </div>
                        </div>
                    </div>



                    <div class="module" data-menucode="jysj">
                        <a class="menusrc" href="JavaScript:void(0)">
                            <div class="modulehead">

                                <div class="title">
                                    <img src="images/jyshuju.png" /><span>经营数据</span>
                                </div>
                                <div class="gomore">
                                    <img src="images/gomore.png" />
                                </div>

                            </div>
                        </a>
                        <div class="modulecontent">
                            <a class="menusrc color2" href="JavaScript:void(0)">点击进入 》         </a>
                        </div>
                    </div>

                    <div class="module" data-menucode="txgl">
                        <a class="menusrc" href="JavaScript:void(0)">
                            <div class="modulehead">

                                <div class="title">
                                    <img src="images/tixiangl.png" /><span>提现管理</span>
                                </div>
                                <div class="gomore">
                                    <img src="images/gomore.png" />
                                </div>

                            </div>
                        </a>
                        <div class="modulecontent">
                            <a class="menusrc color3" href="JavaScript:void(0)">点击进入 》         </a>
                        </div>
                    </div>
                    <div class="module" data-menucode="ygjl" style="display: none">
                        <a class="menusrc" href="JavaScript:void(0)">
                            <div class="modulehead" data-menucode="ygjl">

                                <div class="title">
                                    <img src="images/reward.png" /><span>员工激励</span>
                                </div>
                                <div class="gomore">
                                    <img src="images/gomore.png" />
                                </div>


                            </div>
                        </a>
                        <div class="modulecontent">
                            <div class="count">
                                <div class="numtitle">店员人数</div>
                                <div class="num UnitUserCount">0人</div>

                            </div>
                            <div class="achievement">
                                <div class="achievementtitle">当日人均业绩</div>
                                <div class="achievementnum"><span class="num UserCurrentDayAvgOrderAmount">0</span><span class="CurrentMonthunit">万</span><img src="images/defaultdrop.png" class="rise" /><span class="UserCurrentDayAvgOrderAmountDToD">0%</span></div>

                            </div>
                        </div>
                    </div>
                    <div class="module" data-menucode="cjfx" style="display: none">
                        <a class="menusrc" href="JavaScript:void(0)">
                            <div class="modulehead">

                                <div class="title">
                                    <img src="images/fenxiao.png" /><span>超级分销</span>
                                </div>
                                <div class="gomore">
                                    <img src="images/gomore.png" />
                                </div>

                            </div>
                        </a>
                        <div class="modulecontent">
                            <div class="count">
                                <div class="numtitle">分销商数量</div>
                                <div class="num RetailTraderCount"></div>

                            </div>
                            <div class="achievement">
                                <div class="achievementtitle">当日分销业绩</div>
                                <div class="achievementnum"><span class="num CurrentDayRetailTraderOrderAmount">0</span><span class="CurrentMonthunit">万</span><img src="images/defaultdrop.png" class="rise" /><span class="CurrentDayRetailTraderOrderAmountDToD">0%</span></div>

                            </div>
                        </div>
                    </div>
                    <div class="module" data-menucode="dzhh" style="display: none">
                        <a class="menusrc" href="JavaScript:void(0)">
                            <div class="modulehead">

                                <div class="title">
                                    <img src="images/partner.png" /><span>店长合伙</span>
                                </div>
                                <div class="gomore">
                                    <img src="images/gomore.png" />
                                </div>

                            </div>
                        </a>
                        <div class="modulecontent">
                            <div class="count">
                                <div class="numtitle">店长人数</div>
                                <div class="num UnitMangerCount"></div>

                            </div>
                            <div class="achievement">
                                <div class="achievementtitle">当日店均业绩</div>
                                <div class="achievementnum"><span class="num UnitCurrentDayAvgOrderAmount">0</span><span class="CurrentMonthunit">万</span><img src="images/defaultdrop.png" class="rise" /><span class="UnitCurrentDayAvgOrderAmountDToD">0%</span></div>

                            </div>
                        </div>
                    </div>


                </div>
                <div class="Ranking">
                    <div class="RankingList">
                        <div class="Rankinghead">
                            <div class="title">
                                <img src="images/riselist.png" /><span>业绩月排名</span>
                            </div>
                            <div class="desc">前五名<img src="images/rise.png" /></div>
                        </div>
                        <div class="Rankingcontent">
                            <ul class="PerformanceTop">
                            </ul>
                        </div>
                    </div>
                    <div class="RankingList">
                        <div class="Rankinghead">
                            <div class="title">
                                <img src="images/droplist.png" /><span>业绩月排名</span>
                            </div>
                            <div class="desc">后五名<img src="images/drop.png" /></div>
                        </div>
                        <div class="Rankingcontent">
                            <ul class="PerformanceLower">
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="Statistic">
                <div class="Statisticlist">
                    <div class="achievementarea">
                        <div class="achievementmodule">
                            <div class="achievementmodulehead">
                                <div class="title">
                                    <img src="images/list.png" /><span>业绩</span>
                                </div>
                            </div>
                            <div class="achievementmodulecontent">
                                <div class="Charts">
                                    <div class="Chart">
                                        <div class="ring MonthArchivePectring">
                                            <div class="bigcircle"></div>
                                            <div class="Smallcircle"></div>
                                        </div>
                                        <div class="movering MonthArchivePectmovering">
                                            <div class="bigcircle"></div>
                                            <div class="Smallcircle"></div>
                                        </div>
                                        <div class="leftring">
                                            <div class="bigcircle"></div>
                                            <div class="Smallcircle"></div>
                                        </div>
                                        <div class="numvalue "><span class="Pectvalue MonthArchivePect">0%</span><span>月度业绩达成</span></div>
                                        <div class="leftcover"></div>
                                    </div>

                                    <div class="Chart">
                                        <div class="ring VipContributePectring">
                                            <div class="bigcircle"></div>
                                            <div class="Smallcircle"></div>
                                        </div>
                                        <div class="movering VipContributePectmovering">
                                            <div class="bigcircle"></div>
                                            <div class="Smallcircle"></div>
                                        </div>
                                        <div class="leftring">
                                            <div class="bigcircle"></div>
                                            <div class="Smallcircle"></div>
                                        </div>
                                        <div class="numvalue"><span class="Pectvalue VipContributePect">0%</span><span>会员贡献率</span></div>
                                        <div class="leftcover"></div>
                                    </div>
                                </div>
                                <div class="Chartdesc">
                                    <div class="optiondesc"><em>单店月均消费人次：</em><span class="CurrentMonthSingleUnitAvgTranCount">0</span>人次</div>
                                    <div class="optiondesc"><em>门店月均客单价：</em><span class="CurrentMonthUnitAvgCustPrice">0</span>元/人次</div>
                                    <div class="optiondesc"><em>单店月均业绩：</em><span class="CurrentMonthSingleUnitAvgTranAmount">0</span><span class="CurrentMonthunit">万元</span></div>
                                    <div class="optiondesc"><em>门店月均总业绩：</em><span class="CurrentMonthTranAmount">0</span><span class="CurrentMonthunit">万元</span></div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="ordersarea">
                        <div class="orders">
                            <div class="orderhead">
                                <div class="title">
                                    <img src="images/order.png" /><span>订单</span>
                                </div>
                            </div>
                            <div class="ordercontent">
                                <div class="order">
                                    <div class="ordervalue PreAuditOrder">0</div>
                                    <div class="orderdesc">待审核订单</div>
                                </div>
                                <div class="order">
                                    <div class="ordervalue PreSendOrder">0</div>
                                    <div class="orderdesc">待发货订单</div>
                                </div>
                                <div class="order">
                                    <div class="ordervalue PreTakeOrder">0</div>
                                    <div class="orderdesc">门店待提货订单</div>
                                </div>
                                <div class="order">
                                    <div class="ordervalue PreRefund">0</div>
                                    <div class="orderdesc ">待退货</div>
                                </div>
                                <div class="order">
                                    <div class="ordervalue PreReturnCash">0</div>
                                    <div class="orderdesc">待退款</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <!-- 新手引导 -->
    <div class="window-guide-mask"></div>
    <div class="guideStepIndexBox">
        <em class="guideStepClose"></em>
        <div class="guideStepBox">
            <a href="javascript:;" class="stepItem" data-guid="" data-step="" data-url="">
                <em class="arrTip"></em>
                <span class="mod1"></span>
                <p class="text">启动云店</p>
                <h3 class="modTit modTit1" style="display: none">商品发布 云店装修</h3>
            </a>
            <a href="javascript:;" class="stepItem" data-guid="" data-step="" data-url="">
                <em class="arrTip"></em>
                <span class="mod2"></span>
                <p class="text">启动运营</p>
                <h3 class="modTit modTit2" style="display: none">微信 门店管理</h3>
            </a>
            <a href="javascript:;" class="stepItem" data-guid="" data-step="" data-url="">
                <em class="arrTip"></em>
                <span class="mod3"></span>
                <p class="text">启动会员</p>
                <h3 class="modTit modTit3" style="display: none">集客行动 设置会员</h3>
            </a>
            <a href="javascript:;" class="stepItem" data-guid="" data-step="" data-url="">
                <em class="arrTip"></em>
                <span class="mod4"></span>
                <p class="text">启动营销</p>
                <h3 class="modTit modTit4" style="display: none">优惠券 商城活动</h3>
            </a>
            <a href="javascript:;" class="stepItem" data-guid="" data-step="" data-url="">
                <em class="arrTip"></em>
                <span class="mod5"></span>
                <p class="text">启动集客</p>
                <h3 class="modTit modTit5" style="display: none">软件安装 软件使用</h3>
            </a>
        </div>
    </div>

    <!-- 引导完成提示 -->
    <div class="guideStepFinishBox">
        <a href="javascript:;" class="guideFinishBtn">完成</a>
    </div>

    <!-- 引导初始化提示 -->
    <div class="guideStepStartBox">
        <em class="guideStartClose"></em>
        <a href="javascript:;" class="guideStartBtn"></a>
    </div>

    <!--第五步新手引导-->

    <!-- 新手引导 -->
    <div class="leadNewerMoudle">
        <a class="closeGuid"></a>
        <div class="contentMoudle" id="guid_App">
            <div class="leadNewer1">
                <div class="Module guid_Img1">
                    <img src="images/01.png">
                </div>
                <div class="Module">
                    <div class="guid_prompt">
                        <div class="prompt_close"></div>
                        <div class="prompt_Arrow"></div>
                        <div class="prompt_title">APP 安装</div>
                        <div class="prompt_content">
                            扫描二维码下载，安装连锁掌柜
                        </div>
                        <div class="prompt_step">
                            <a class="prevBtn" data-index="1"></a>
                            <span class="stepInfo">第<b class="prevStep">1</b>步,
                                   共<b class="sumStep">7</b>步
                            </span>
                            <a class="nextBtn" data-index="2"></a>
                        </div>
                        <div class="prompt_btnVideo">
                            <p><a>查看视频</a></p>
                        </div>
                    </div>
                    <div class="guid_video">
                        <a class="close"></a>
                        <video width="320" height="200" controls="controls" autoplay="autoplay">

                          </video>
                    </div>

                </div>
            </div>
            <div class="leadNewer2">
                <div class="Module">
                    <div class="guid_prompt">
                        <div class="prompt_close"></div>
                        <div class="prompt_Arrow"></div>
                        <div class="prompt_title">卡劵核销</div>
                        <div class="prompt_content">
                            门店员工通过APP实现多种方式的卡劵核销
                        </div>
                        <div class="prompt_step">
                            <a class="prevBtn" data-index="1"></a>
                            <span class="stepInfo">第<b class="prevStep">5</b>步,
                                   共<b class="sumStep">7</b>步
                            </span>
                            <a class="nextBtn" data-index="2"></a>
                        </div>
                        <div class="prompt_btnVideo">
                            <p><a>查看视频</a></p>
                        </div>
                    </div>
                    <div class="guid_video">
                        <a class="close"></a>
                        <video width="320" height="200" controls="controls" autoplay="autoplay">

                          </video>
                    </div>

                </div>
                <div class="Module guid_Img2">
                    <img src="images/01.png">
                </div>
            </div>
        </div>
    </div>








    <div style="display: none;">
        <div id="win" class="easyui-window" data-options="modal:true,shadow:false,collapsible:false,minimizable:false,maximizable:false,closed:true,closable:true">
            <div class="easyui-layout" data-options="fit:true" id="panlconent">
                <div data-options="region:'center'" style="padding: 10px;">
                    <div class="quicklyBox">
                        <div>
                            <img src="images/index-quickly.png" alt="" />
                        </div>
                    </div>
                    <div class="quicklyBtnBox">
                        <a href="javascript:;" style="text-indent: 5px;" target="_blank">
                            <span class="wxAuthBox">操作指引</span>
                        </a>
                        <a href="http://help.chainclouds.cn/?p=763" style="text-indent: 13px;" target="_blank"><span>操作指引</span></a>
                        <a href="http://help.chainclouds.cn/?p=772" style="text-indent: 25px;" target="_blank"><span>操作指引</span></a>
                        <a href="http://help.chainclouds.cn/?p=755" style="text-indent: 35px;" target="_blank"><span>操作指引</span></a>
                    </div>
                    <p class="nextNotShow"><span>下次不再显示</span></p>
                </div>
            </div>
        </div>
    </div>

    <!-- 微信服务号授权 -->
    <div style="display: none;">
        <div id="win2" class="easyui-window" data-options="modal:true,shadow:false,collapsible:false,minimizable:false,maximizable:false,closed:true,closable:true">
            <div class="easyui-layout" data-options="fit:true" id="panlconent">
                <div data-options="region:'center'" style="padding: 10px;">
                    <div class="qb_WeixinService">
                        <div class="quick">
                            <div class="quick_center">
                                <div class="quick_centernr ">
                                    <div class="found">
                                        <br />
                                        <p>您是否已有<font>微信公众服务号</font>并已通过实名认证?</p>
                                        <!--<iframe id="receive" scrolling="no" src="<%=System.Web.Configuration.WebConfigurationManager.AppSettings["openOAuthUrl"] %>/receive" ></iframe><img src="images/weixinset.png" />
                                            
                                            如果你还没有微信公众服务号，可以<a target="_blank"  href="https://mp.weixin.qq.com/cgi-bin/readtemplate?t=register/step1_tmpl&lang=zh_CN">点击注册</a><br />
                                            -->
                                        <div class="explainText">请注意是微信公众服务号，不是订阅号哦，<a target="_blank" href="http://kf.qq.com/faq/140806zARbmm140826M36RJF.html">有何区别？</a></div>
                                        <div class="explainText">绑定微信服务号，将可使用连锁掌柜所有功能，<a target="_blank" href="\Module\helpCenter\helpCenterClass.aspx">了解所有功能。</a></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="btnWrap" id="btnWrap" data-options="region:'south'" style="height: 80px; text-align: center; padding: 22px 0 0;">
                    <div class="tipBox"></div>
                    <a class="easyui-linkbutton commonBtn registerBtn" style="display: inline-block;" target="_blank" href="https://mp.weixin.qq.com/cgi-bin/readtemplate?t=register/step1_tmpl&lang=zh_CN">还没有，先去注册</a>
                    <a class="easyui-linkbutton commonBtn saveBtn" href="javascript:;">已有，立即绑定</a>
                    <iframe id="receive" scrolling="no" src="<%=System.Web.Configuration.WebConfigurationManager.AppSettings["openOAuthUrl"] %>/receive"></iframe>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
