define(['jquery', 'template', 'tools', 'easyui'], function ($) {
    var page = {
        elems: {
            sectionPage: $("#section"),
            simpleQueryDiv: $("#simpleQuery"),     //简单查询条件的层dom
            allQueryDiv: $("#allQuery"),             //所有的查询条件层dom
            uiMask: $("#ui-mask"),
            tabel: $("#gridTable"),                   //表格body部分
            tabelWrap: $('#tableWrap'),
            thead: $("#thead"),                    //表格head部分
            showDetail: $('#showDetail'),         //弹出框查看详情部分
            operation: $('#operation'),              //弹出框操作部分
            dataMessage: $(".dataMessage"),
            click: true,
            panlH: 116,                        // 下来框统一高度
            domain: "",
            isshow: false                      //是否显示选择类型弹出框
        },
        select: {
            isSelectAllPage: false,                 //是否是选择所有页面
            tagType: [],                             //标签类型
            tagList: []                              //标签列表
        },
        init: function () {

            var that = this,
				UA = navigator.userAgent.toLowerCase();
            //if (!(UA.indexOf("chrome") > 0 || UA.indexOf("safari") > 0)) {
            //    location.href = 'browserDown.html';
            //} else {
                this.loadPageData();
				this.initEvent();
            //}
			
        },
        initEvent: function () {
            var that = this,
				$uiMask = $('.ui-mask'),
				$closeBtn = $('.closeBtn'),
                $WeixinServicecloseBtn = $(".WeixinServicecloseBtn"),
				$qbQuick = $('.qb_quick'),
				$WeixinServiceqbQuick = $('.qb_WeixinService'),
				$notShow = $('.nextNotShow span'),
				stepIndex = $.util.getUrlParam('stepIndex'),
				guId = $.util.getUrlParam('GuId');
				
			$notShow.on('click',function(){
				if($notShow.hasClass('on')){
					$notShow.removeClass('on');
				}else{
					$notShow.addClass('on');
				}
			});
		/*	  新手引导相关注释
			$('#win2').window({
                modal: true,
                shadow: false,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                closed: true,
                closable: true,
				onClose:function(){
					$('.guideStepIndexBox').show();
					$('.window-guide-mask').show();
				}
            });

			if(stepIndex==0 && guId){
				$('.window-guide-mask').show();
				$('.guideStepFinishBox').show();
				if(guId=="46f16bcd-76a3-4737-b8ce-38a213e188ba"){//营销
					$('.guideStepFinishBox').css({"background":"url(images/guide/finish_4.gif) no-repeat center bottom"});
					$('.guideFinishBtn').css({"top":"366px"});
				}else if(guId=="514a37d0-bc41-4af8-aa63-9655eb943a2e"){//会员
					$('.guideStepFinishBox').css({"background":"url(images/guide/finish_3.gif) no-repeat center center"});
					$('.guideFinishBtn').css({"top":"304px"});
				}else if(guId=="9ddc3c3c-8c22-43a9-a64f-ca17dd4dd2c3"){//运营
					$('.guideStepFinishBox').css({"background":"url(images/guide/finish_2.gif) no-repeat center center"});
					$('.guideFinishBtn').css({"top":"304px"});
				}else if(guId=="16407ba6-4b77-4a68-9779-f05c689803e2"){//集客
					$('.guideStepFinishBox').css({"background":"url(images/guide/finish_5.gif) no-repeat center center"});
					$('.guideFinishBtn').css({"top":"366px"});
				}else if(guId=="bab72601-4801-4403-b97f-f97cc0b9a564"){//云店
					$('.guideStepFinishBox').css({"background":"url(images/guide/finish_1.gif) no-repeat center center"});
					$('.guideFinishBtn').css({"top":"304px"});
				}
				that.GetGuideModuleList(1);
			}else{
				//绑定微信服务号授权start
				/!*window.onload = function(){
					that.GetGuideModuleList();
					$('#win2').window('open');
				};*!/
				
				$(document).ready(function(){ 
					that.GetGuideModuleList(0);
					//$('#win2').window('open');
				});
			}
			*/

			$('#win2').delegate('.registerBtn','mousemove',function(){
				$('#win2 .tipBox').show();
			}).delegate('.registerBtn','mouseout',function(){
				$('#win2 .tipBox').hide();
			});
			
		/*	//集客宝-新手引导
			$('body').delegate('.guideStepClose','click',function(){
				var $this = $(this);
				$('.window-guide-mask').hide();
				$this.parent().hide();
			}).delegate('.guideFinishBtn','click',function(){
				var $this = $(this);
				//$('.window-guide-mask').hide();
				$this.parent().hide();
				$('.guideStepIndexBox').show();
			}).delegate('.guideStartClose','click',function(){
				var $this = $(this);
				$('.window-guide-mask').hide();
				$this.parent().hide();
			})*/
			
			
	/*		$('.guideStepBox a').on('click',function(){
				var $this = $(this),
					ind = $this.index(),
					guid = $this.data('guid'),
					step = $this.data('step'),
					url = $this.data('url'),
					url2 = $this.data('url2');
				/!*if($('span',$this).hasClass('active')){
					return;
				}*!/
				if(url==0){
					$('.guideStepIndexBox').hide();
					$('.guideStepStartBox').show();
					url2 = url2+"#stepIndex=1"+'&GuId='+guid ;
					if(ind==0){
						$('.guideStepStartBox').css({"background":"url(images/guide/start_1.png) no-repeat center center"});
						$('.guideStartClose').css({"top":"6px","right":"20px"});
						$('.guideStartBtn').css({"top":"196px","right":"103px"}).attr('href',url2);
					}else if(ind==1){
						$('.guideStepStartBox').css({"background":"url(images/guide/start_2.png) no-repeat center center"});
						$('.guideStartClose').css({"top":"6px","right":"6px"});
						$('.guideStartBtn').css({"top":"203px","right":"102px"}).attr('href',url2);
					}else if(ind==2){
						$('.guideStepStartBox').css({"background":"url(images/guide/start_3.png) no-repeat center center"});
						$('.guideStartClose').css({"top":"21px","right":"20px"});
						$('.guideStartBtn').css({"top":"189px","right":"93px"}).attr('href',url2);
					}else if(ind==3){
						$('.guideStepStartBox').css({"background":"url(images/guide/start_4.png) no-repeat center center"});
						$('.guideStartClose').css({"top":"9px","right":"54px"});
						$('.guideStartBtn').css({"top":"199px","right":"145px"}).attr('href',url2);
					}else if(ind==4){
						$('.guideStepStartBox').css({"background":"url(images/guide/start_5.png) no-repeat center center"});
						$('.guideStartClose').css({"top":"13px","right":"26px"});
                        $('.guideStartBtn').css({"top":"200px","right":"118px"}).attr('id',"guideStartBtn").attr('data-guid',guid);
                        //$('.guideStepStartBox').hide();

					}
				}else{
                    if(ind!=4){
						if(url.indexOf('GroupList.aspx')==-1){
							window.location.href = url+"#stepIndex="+step+'&GuId='+guid ;
						}else{
							window.location.href = url+"&stepIndex="+step+'&GuId='+guid ;
						}
                    }else{
                        //$('.guideStepStartBox').hide();
                        that.getStepGuId(step,guid);
                    }

				}	
			});



            $('body').delegate('#guideStartBtn','click',function(){
                var $this = $(this),
                    guId= $this.data('guid');
                $('.guideStepIndexBox').hide();
                $('.guideStepStartBox').hide();
                that.getStepGuId(1,guId);
            })
*/
			
        },

		quicklyDialog: function(){
			var that=this,
				$notShow = $('.nextNotShow span'),
				cooksName = '';
			$('#win').window({title:"快速上手",width:922,height:422,top:($(window).height() - 422) * 0.5,left:($(window).width() - 922) * 0.5,
			onClose:function(){
				if($notShow.hasClass('on')){
					$.util.setCookie('chainclouds_management_system_index', 'zmind');
				}
				//var mid = JITMethod.getUrlParam("mid"),PMenuID = JITMethod.getUrlParam("PMenuID");
				//location.href = "/module/newVipManage/querylist.aspx?mid=" +mid+"&PMenuID="+PMenuID;
			}
			});
			cooksName = $.util.getCookie('chainclouds_management_system_index');
			if(!cooksName){
				$(document).ready(function() {
					setTimeout(function(){
						$('#win').window('open');
					},1000);
				});
			}else{
				$(document).ready(function() {
					$('#win').window('close');
				});
			}
			//改变弹框内容，调用百度模板显示不同内容
			/*$('#panlconent').layout('remove','center');
			var html=bd.template('tpl_addProm');
			var options = {
				region: 'center',
				content:html
			};
			$('#panlconent').layout('add',options);*/
		},


       
        //加载页面的数据请求
        loadPageData: function (e) {
            //debugger;
            var that = this;
            that.loadData.GetHomePageStats(function (result) {
                var data;
                if (result)
                {
                    data = result.Data;
                }
                if (data)
                {
                   // 
                    $(".UnitCount").html((data.UnitCount?data.UnitCount:"0")+"家");//门店数
                   
                    that.NumberFormat(data.UnitCurrentDayOrderAmount, function (result, isyuan) {
                        //debugger;
                        $(".UnitCurrentDayOrderAmount").html("<span style='font-size: 14px;'>￥</span>" + result);//门店当日总业绩
                        if (isyuan)
                            $(".UnitCurrentDayOrderAmount").parents(".achievementnum").find(".CurrentMonthunit").html("");
                        else
                            $(".UnitCurrentDayOrderAmount").parents(".achievementnum").find(".CurrentMonthunit").html("万");

                    });



                    var UnitCurrentDayOrderAmountDToD = data.UnitCurrentDayOrderAmountDToD;
                    if (UnitCurrentDayOrderAmountDToD < 0) {
                        $(".UnitCurrentDayOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/drop1.png");
                        UnitCurrentDayOrderAmountDToD = -UnitCurrentDayOrderAmountDToD;
                    } else {
                        if (UnitCurrentDayOrderAmountDToD == null || UnitCurrentDayOrderAmountDToD == 0)
                            $(".UnitCurrentDayOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/defaultdrop.png");
                        else
                            $(".UnitCurrentDayOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/rise1.png");
                    }
                    $(".UnitCurrentDayOrderAmountDToD").html((UnitCurrentDayOrderAmountDToD ? UnitCurrentDayOrderAmountDToD.toFixed(1) : "0") + "%");//门店当日总业绩环比
                    $(".UnitMangerCount").html((data.UnitMangerCount ? data.UnitMangerCount : "0") + "人");//门店店长数
                    that.NumberFormat(data.UnitCurrentDayAvgOrderAmount, function (result, isyuan) {
                        $(".UnitCurrentDayAvgOrderAmount").html("<span style='font-size: 14px;'>￥</span>" + result);//当日店均业绩
                        if (isyuan)
                            $(".UnitCurrentDayAvgOrderAmount").parents(".achievementnum").find(".CurrentMonthunit").html("");
                        else
                            $(".UnitCurrentDayAvgOrderAmount").parents(".achievementnum").find(".CurrentMonthunit").html("万");

                    });


                    var UnitCurrentDayAvgOrderAmountDToD = data.UnitCurrentDayAvgOrderAmountDToD;
                    if (UnitCurrentDayAvgOrderAmountDToD < 0) {
                        $(".UnitCurrentDayAvgOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/drop1.png");
                        UnitCurrentDayAvgOrderAmountDToD = -UnitCurrentDayAvgOrderAmountDToD;
                    } else {
                        if (UnitCurrentDayAvgOrderAmountDToD == null || UnitCurrentDayAvgOrderAmountDToD == 0)
                            $(".UnitCurrentDayAvgOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/defaultdrop.png");
                        else
                            $(".UnitCurrentDayAvgOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/rise1.png");
                    }
                    $(".UnitCurrentDayAvgOrderAmountDToD").html((UnitCurrentDayAvgOrderAmountDToD ? UnitCurrentDayAvgOrderAmountDToD.toFixed(1) : "0") + "%");//当日店均业绩环比
                    $(".UnitUserCount").html((data.UnitUserCount ? data.UnitUserCount : "0") + "人");//门店员工数
                    that.NumberFormat(data.UserCurrentDayAvgOrderAmount, function (result, isyuan) {
                        $(".UserCurrentDayAvgOrderAmount").html("<span style='font-size: 14px;'>￥</span>" + result);//当日人均业绩
                        if (isyuan)
                            $(".UserCurrentDayAvgOrderAmount").parents(".achievementnum").find(".CurrentMonthunit").html("");
                        else
                            $(".UserCurrentDayAvgOrderAmount").parents(".achievementnum").find(".CurrentMonthunit").html("万");

                    });


                    var UserCurrentDayAvgOrderAmountDToD = data.UserCurrentDayAvgOrderAmountDToD;
                    if (UserCurrentDayAvgOrderAmountDToD < 0) {
                        $(".UserCurrentDayAvgOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/drop1.png");
                        UserCurrentDayAvgOrderAmountDToD = -UserCurrentDayAvgOrderAmountDToD;
                    } else {
                        if (UserCurrentDayAvgOrderAmountDToD == null || UserCurrentDayAvgOrderAmountDToD == 0)
                            $(".UserCurrentDayAvgOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/defaultdrop.png");
                        else
                        $(".UserCurrentDayAvgOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/rise1.png");
                    }
                    $(".UserCurrentDayAvgOrderAmountDToD").html((UserCurrentDayAvgOrderAmountDToD ? UserCurrentDayAvgOrderAmountDToD.toFixed(1) : "0") + "%");//当日人均业绩环比
                    $(".RetailTraderCount").html((data.SuperRetailTraderCount ? data.SuperRetailTraderCount : "0") + "人");//分销商数                                 
                    that.NumberFormat(data.CurrentDayRetailTraderOrderAmount, function (result, isyuan) {
                        $(".CurrentDayRetailTraderOrderAmount").html("<span style='font-size: 14px;'>￥</span>" + result);//当日分销业绩
                        if (isyuan)
                            $(".CurrentDayRetailTraderOrderAmount").parents(".achievementnum").find(".CurrentMonthunit").html("");
                        else
                            $(".CurrentDayRetailTraderOrderAmount").parents(".achievementnum").find(".CurrentMonthunit").html("万");

                    });
					
					
					var CurrentDaySuperRetailTraderOrderAmount = data.CurrentDaySuperRetailTraderOrderAmount;
					if(!!CurrentDaySuperRetailTraderOrderAmount){
						$(".CurrentDayRetailTraderOrderAmount").html("<span style='font-size: 14px;'>￥</span>" + CurrentDaySuperRetailTraderOrderAmount);
					}
					

                    var CurrentDayRetailTraderOrderAmountDToD = data.CurrentDaySuperRetailTraderOrderAmountDToD;
                    if (CurrentDayRetailTraderOrderAmountDToD < 0) {
                        $(".CurrentDayRetailTraderOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/drop1.png");
                        CurrentDayRetailTraderOrderAmountDToD = -CurrentDayRetailTraderOrderAmountDToD;
                    } else {
                        if (CurrentDayRetailTraderOrderAmountDToD == null||CurrentDayRetailTraderOrderAmountDToD == 0)
                            $(".CurrentDayRetailTraderOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/defaultdrop.png");
                        else
                        $(".CurrentDayRetailTraderOrderAmountDToD").parents(".achievementnum").find(".rise").attr("src", "images/rise1.png");
                    }
                    $(".CurrentDayRetailTraderOrderAmountDToD").html((CurrentDayRetailTraderOrderAmountDToD ? CurrentDayRetailTraderOrderAmountDToD.toFixed(1) : "0") + "%");//当日分销业绩环比
                    $(".VipCount").html((data.VipCount ? data.VipCount : "0") + "人");//会员数   
                    $(".NewVipCount").html((data.NewVipCount ? data.NewVipCount : "0") + "人");//当日新增会员数

                    var NewVipDToD = data.NewVipDToD;
                    if (NewVipDToD < 0) {
                        $(".NewVipDToD").parents(".achievementnum").find(".rise").attr("src", "images/drop1.png");
                        NewVipDToD = -NewVipDToD;
                    } else {
                        if (NewVipDToD == null || NewVipDToD == 0)
                            $(".NewVipDToD").parents(".achievementnum").find(".rise").attr("src", "images/defaultdrop.png");
                        else
                            $(".NewVipDToD").parents(".achievementnum").find(".rise").attr("src", "images/rise1.png");
                    }
                    $(".NewVipDToD").html((NewVipDToD ? NewVipDToD.toFixed(1) : "0") + "%");//新增会员环比


                    $(".EventsCount").html((data.EventsCount ? data.EventsCount : "0") + "个");//活动数
                    $(".EventJoinCount").html((data.EventJoinCount ? data.EventJoinCount : "0") + "人");//活动参与人次

                    //业绩排名前5名
                    if (data.PerformanceTop) {
                        $(".PerformanceTop").html("");
                        for (i = 0; i < data.PerformanceTop.length; i++) {
                            $(".PerformanceTop").append("<li>" + (i + 1) + " " + data.PerformanceTop[i].UnitName + "<span class='red'>￥" + data.PerformanceTop[i].OrderPeopleTranAmount + "</span></li>");
                        }
                    }

                    //业绩排名后5名
                    if (data.PerformanceLower)
                    {
                        $(".PerformanceLower").html("");
                        for (i = 0; i < data.PerformanceLower.length; i++)
                        {
                            $(".PerformanceLower").append("<li>" + (i + 1) + " " + data.PerformanceLower[i].UnitName + "<span class='green'>￥" + data.PerformanceLower[i].OrderPeopleTranAmount + "</span></li>");
                        }
                    }

                    //debugger;
                    var degbase = 3.6;//角度转换百分比基数
                    //会员贡献率
                    var VipContributePect = data.VipContributePect;
                    if (VipContributePect < 50) {
                        $(".VipContributePectring").css("-webkit-transform", "rotate(" + (VipContributePect * degbase) + "deg)");
                        $(".VipContributePectmovering").hide();
                    } else {
                        $(".VipContributePectmovering").show();
                        $(".VipContributePectring").css("-webkit-transform", "rotate(180deg)");
                        $(".VipContributePectmovering").css("-webkit-transform", "rotate(" + ((VipContributePect - 50) * degbase) + "deg)");
                    }
                    $(".VipContributePect").html((VipContributePect ? VipContributePect : "0") + "%");

                    //月度月均达成
                    var MonthArchivePect = data.MonthArchivePect;
                    if (MonthArchivePect < 50) {
                        $(".MonthArchivePectring").css("-webkit-transform", "rotate(" + (MonthArchivePect * degbase) + "deg)");
                        $(".MonthArchivePectmovering").hide();
                    } else {
                        $(".MonthArchivePectmovering").show();
                        $(".MonthArchivePectring").css("-webkit-transform", "rotate(180deg)");
                        $(".MonthArchivePectmovering").css("-webkit-transform", "rotate(" + ((MonthArchivePect - 50) * degbase) + "deg)");
                    }
                    $(".MonthArchivePect").html((MonthArchivePect ? MonthArchivePect : "0") + "%");


                    $(".CurrentMonthSingleUnitAvgTranCount").html(data.CurrentMonthSingleUnitAvgTranCount ? data.CurrentMonthSingleUnitAvgTranCount : 0);//单店月均消费人次
                    $(".CurrentMonthUnitAvgCustPrice").html(data.CurrentMonthUnitAvgCustPrice ? data.CurrentMonthUnitAvgCustPrice : 0);//门店月均单价
                    

                    that.NumberFormat(data.CurrentMonthSingleUnitAvgTranAmount, function (result, isyuan) {
                        $(".CurrentMonthSingleUnitAvgTranAmount").html(result);//单店月均业绩
                        if (isyuan)
                            $(".CurrentMonthSingleUnitAvgTranAmount").parents(".optiondesc").find(".CurrentMonthunit").html("元");
                        else
                            $(".CurrentMonthSingleUnitAvgTranAmount").parents(".optiondesc").find(".CurrentMonthunit").html("万元");

                    });

                    
                    that.NumberFormat(data.CurrentMonthTranAmount, function (result, isyuan) {
                        $(".CurrentMonthTranAmount").html(result);//门店月均总业绩
                        if (isyuan)
                            $(".CurrentMonthTranAmount").parents(".optiondesc").find(".CurrentMonthunit").html("元");
                        else
                            $(".CurrentMonthTranAmount").parents(".optiondesc").find(".CurrentMonthunit").html("万元");

                    });

                    $(".PreAuditOrder").html(data.PreAuditOrder?data.PreAuditOrder:0);//待审核订单
                    $(".PreSendOrder").html(data.PreSendOrder ? data.PreSendOrder : 0);//待发货订单
                    $(".PreTakeOrder").html(data.PreTakeOrder ? data.PreTakeOrder : 0);//门店待提货订单
                    $(".PreRefund").html(data.PreRefund ? data.PreRefund : 0);//待退货
                    $(".PreReturnCash").html(data.PreReturnCash ? data.PreReturnCash : 0);//待退款

					//老的新手引导弹层
					//that.quicklyDialog();
                }

            });
			/*
            that.loadData.GetMenuList(function (result) {
                var data;
                if (result) {
                    data = result.Data;
                    if (data)
                    {
                        for (i = 0; i<data.MenuList.length; i++) {
                            var menu = data.MenuList[i];
                            $("[data-menucode]").each(function () {
                                var menucode = $(this).data("menucode");
								//if(menu.Menu_Code=='yxlq'){console.log(menu.IsCanAccess);}
                                if (menu.Menu_Code == menucode){
                                    if (menu.IsCanAccess==1 && menu.SubMenuList.length > 0&&menu.SubMenuList[0].SubMenuList&&menu.SubMenuList[0].SubMenuList.length>0) {
                                        $(this).find(".menusrc").attr("href", menu.SubMenuList[0].SubMenuList[0].Url_Path);
                                       // $(this).find(".menusrc").attr("href", menu.SubMenuList[0].SubMenuList[0].Url_Path + "?mid=" + menu.SubMenuList[0].SubMenuList[0].Menu_Id+ "&PMenuID=" +menu.SubMenuList[0].Menu_Id+ "&MMenuID=" + menu.Menu_Id);
                                          // console.log(menu.Menu_Code+"url____"+menu.SubMenuList[0].SubMenuList[0].Url_Path);
                                    } else {
                                        $(this).find(".menusrc").attr("href", "JavaScript:void(0)");
                                    }
                                }
                            });

                        }

                    }
                }

            });
			*/

            //发布集客第五部分新手引导
            $('#guid_App').delegate('.prevBtn,.nextBtn','click',function(){
                var ii = $(this).attr('data-index'),
                    isFinished = $(this).hasClass('finish');
                if(isFinished){
                    that.loadData.args.finishedStatus =1;
                }else{
                    that.loadData.getModuleNextStep(function(data){
                        var list = {};
                        if(ii=='1'){
                            list = data.PreModule;
                        }else{
                            list = data.NextModule;
                        }
                        if(list.ModuleStep>0){
                            that.guidData(list);
                        }

                    })
                }
            })

            $('.leadNewerMoudle').delegate('.closeGuid','click',function(){
                $('.leadNewerMoudle').hide();
                $('.window-guide-mask').hide();
            })
            $('#guid_App').delegate('.guid_video .close','click',function(){
                $(this).parents('.guid_video').hide();
            })
            $('#guid_App').delegate('.prompt_close','click',function(){
                $(this).parents('.guid_prompt').hide();
            })
            $('#guid_App').delegate('.prompt_btnVideo a','click',function(){
                var videos = $(this).parents('.Module').children('.guid_video')[0];
                //console.log(videos.display);
                videos.currentTime = 0;
                if(videos.style.display == 'none'){
                    videos.style.display ='block';
                }
            })
			
            $.util.stopBubble(e)
        },
        // 启动集客新手引导
        guidData:function(data){
            var that =this,
                step = data.ModuleStep;
            that.loadData.args.Step = step;
            //$('#guid_App').find(' img').attr('src','');
            $('#guid_App').find('video').attr('src','');
            $('#guid_App').find('.prevStep').html(step);
            $('#guid_App').find('.prompt_step a:last-child').removeClass('finish');
            $('#guid_App').find('.prompt_step a:last-child').removeAttr('href');
            $('#guid_App').find('.guid_video video').attr('src','http://guide.chainClouds.cn'+data.VideoUrl);
            if(step==3){
                $('#guid_App').find('.guid_Img2 img').attr('src','images/03.png');
                $('#guid_App').find('.leadNewer1').hide();
                $('#guid_App').find('.leadNewer2').show();
                $('#guid_App').find('.leadNewer2 .guid_prompt').css({'top':'240px','bottom':'40px'});
                $('#guid_App').find('.leadNewer2 .guid_video').css({'top':'40px','bottom':''});
                $('#guid_App').find('.leadNewer2 .prompt_Arrow').css('transform','rotateX(180deg)');
                $('#guid_App').find('.leadNewer2 .prompt_title').html('员工管理');
                $('#guid_App').find('.leadNewer2 .prompt_content').html('店长创建在职门店员账号并进行管理');
            }else if(step==5){
                $('#guid_App').find('.guid_Img2 img').attr('src','images/05.png');
                $('#guid_App').find('.leadNewer1').hide();
                $('#guid_App').find('.leadNewer2').show();
                $('#guid_App').find('.leadNewer2 .guid_prompt').css({'top':'0px','bottom':''});
                $('#guid_App').find('.leadNewer2 .guid_video').css({'top':'','bottom':'40px'});
                $('#guid_App').find('.leadNewer2 .prompt_Arrow').css('transform','rotateY(0deg)');
                $('#guid_App').find('.leadNewer2 .prompt_title').html('卡劵核销');
                $('#guid_App').find('.leadNewer2 .prompt_content').html('门店员工通过APP实现多种方式的卡劵核销');
            }else{
                $('#guid_App').find('.leadNewer1').show();
                $('#guid_App').find('.leadNewer2').hide();
                if(step==7){
                    $('#guid_App').find('.guid_Img1 img').attr('src','images/07.png');
                    //that.loadData.args.finishedStatus =1;
                    $('#guid_App').find('.prompt_step a:last-child').addClass('finish');
                    $('#guid_App').find('.leadNewer1 .guid_prompt').css({'top':'240px','bottom':'40px'});
                    $('#guid_App').find('.leadNewer1 .guid_video').css({'top':'40px','bottom':''});
                    $('#guid_App').find('.leadNewer1 .prompt_Arrow').css('transform','rotate(180deg)');
                    $('#guid_App').find('.leadNewer1 .prompt_title').html('在线客服');
                    $('#guid_App').find('.leadNewer1 .prompt_content').html('门店员工使用APP与会员实现会员互动');
                    var guid = that.loadData.args.guid,
                        url = "/module/Index/IndexPage.aspx"+"?stepIndex=0&GuId="+guid;
                    $('#guid_App').find('.prompt_step a:last-child').attr('href',url);
                }else{
                    $('#guid_App').find('.leadNewer1 .guid_prompt').css({'top':'0px','bottom':''});
                    $('#guid_App').find('.leadNewer1 .guid_video').css({'top':'','bottom':'40px'});
                    $('#guid_App').find('.leadNewer1 .prompt_Arrow').css('transform','rotateY(180deg)');
                    if(step ==6){
                        $('#guid_App').find('.guid_Img1 img').attr('src','images/06.png');
                        $('#guid_App').find('.leadNewer1 .prompt_title').html("会员服务");
                        $('#guid_App').find('.leadNewer1 .prompt_content').html('门店员工使用APP进行提货'+'</br>'+'打标签等会员服务');
                    }else if(step ==4){
                        $('#guid_App').find('.guid_Img1 img').attr('src','images/04.png');
                        $('#guid_App').find('.leadNewer1 .prompt_title').html('集客吸粉');
                        $('#guid_App').find('.leadNewer1 .prompt_content').html('门店员工使用APP的相应集客工具'+'</br>'+'进行集客吸粉');
                    }else if(step ==2){
                        $('#guid_App').find('.guid_Img1 img').attr('src','images/02.png');
                        $('#guid_App').find('.leadNewer1 .prompt_title').html('用户登入');
                        $('#guid_App').find('.leadNewer1 .prompt_content').html('店长或店员根据总部提供的账号'+'</br>'+'登录连锁掌柜App');
                    }else if(step ==1){
                        $('#guid_App').find('.guid_Img1 img').attr('src','images/01.png');
                        $('#guid_App').find('.leadNewer1 .prompt_title').html('APP安装');
                        $('#guid_App').find('.leadNewer1 .prompt_content').html('扫描二维码下载，安装连锁掌柜');
                    }
                }
            }
            that.loadData.setModuleLastStep();
        },
        //数据格式转换
        NumberFormat: function (oldformatdata, callback)
        {
            var isyuan = true;
            var result = 0;
            if (oldformatdata) {
                if (oldformatdata > 9999) {
                    
                    result = (oldformatdata / 10000).toFixed(2);
                    isyuan = false;
                } else {
                    result= oldformatdata.toFixed(0);
                }
            }
            if (callback) {
                callback(result, isyuan);
            }
        }
        ,
        //加载更多的资讯或者活动
        getStepGuId: function (steps,guid) {
            // this.loadData.args.PageIndex = currentPage;
            var that = this;
            that.loadData.args.Step =steps;
            that.loadData.args.guid =guid;
            if(steps==0){
                steps =7;
            }
            if(steps==7){
                that.loadData.args.finishedStatus = 1;
            }
            if(steps&&guid){
                $('.window-guide-mask').show();
                $('.leadNewerMoudle').show();
                //显示新手引导
                that.loadData.getModuleNextStep(function(data){
                    that.guidData(data.NowModule);
                })
            }
            
        },
		
		
		GetGuideModuleList:function(flag){
			var that = this;
			
			function htmlTag(ind,param){
				var $guideStepBox = $('.guideStepBox a'),
					$item = $guideStepBox.eq(ind);
				$item.attr('data-guid',param.UserGuideModulesId);
				$item.attr('data-step',param.LastStep);
				$item.attr('data-url',param.LastUrl || 0);
				$item.attr('data-url2',param.Url || 0);
				if(param.FinishedStatus){
					$('span',$item).addClass('active');
				}
			}
			
			$.util.ajax({
				url: "/ApplicationInterface/Gateway.ashx",
				async : false,
				data: {
					action: 'Basic.Guide.GetModuleList'
				},
				success: function (data) {
					if (data.IsSuccess && data.ResultCode == 0) {
						var result = data.Data,
							moduleInfoList = result.ModuleInfoList,
							$guideStepBox = $('.guideStepBox a');
						//debugger;
						for(var i=0;i<moduleInfoList.length;i++){
							var obj = moduleInfoList[i],
								name = obj.ModuleName;
							switch(name){
								case '云店':
									 htmlTag(0,obj);
									 break;
								case '运营':
									 htmlTag(1,obj);
									 break;
								case '会员':
									 htmlTag(2,obj);
									 break;
								case '营销':
									 htmlTag(3,obj);
									 break;
								case '集客':
									 htmlTag(4,obj);
									 break;
							}
						}
		
						
						if(!result.IsBindWeChat && flag==0){
							$('#win2').window({
								title:"绑定微信服务号",
								width:522,
								height:320,
								top:($(window).height() - 320) * 0.5,
								left:($(window).width() - 522) * 0.5
							});
							$('#receive').attr("src",window.weixinUrl);
                            setTimeout(function(){
                                $('#win2').window('open');
                            },1000);
						}else{
							if(flag==0){
								$('.guideStepIndexBox').show();
								$('.window-guide-mask').show();
							}
						}
						
						/*if (callback) {
							callback(data);
						}*/

					} else {
						//debugger;
						alert(data.Message);
					}
				}
			});
		},

        loadData: {
            args: {
                PageIndex: 1,
                PageSize: 10,
                SearchColumns: {},    //查询的动态表单配置
                OrderBy: "",           //排序字段
                SortType: 'DESC',    //如果有提供OrderBy，SortType默认为'ASC'
                Status: -1,
                Step:1,
                finishedStatus:0,
                guid:""
            },
            tag: {
                VipId: "",
                orderID: ''
            },
            seach: {
                QuestionnaireName: "",
                QuestionnaireType: 0
            },
            //获取首页模块链接地址
            GetMenuList: function (callback) {
                //debugger;
                var that = this;
                $.util.ajax({
                    url: "/ApplicationInterface/Gateway.ashx",
                    data: {
                        action: 'Basic.Menu.GetMenuList'
                    },
                    success: function (data) {
                        if (data.IsSuccess && data.ResultCode == 0) {
                            var result = data.Data;
                            if (callback) {
                                callback(data);
                            }

                        } else {
                            //debugger;
                            alert(data.Message);
                        }
                    }
                });
            },
            //获取首页统计数据
            GetHomePageStats: function (callback) {
                //debugger;
                var that = this;
                $.util.ajax({
                    url: "/ApplicationInterface/Gateway.ashx",
					async : false,
                    data: {
                        action: 'Basic.HomePageStats.GetHomePageStats'
                    },
                    success: function (data) {
                        if (data.IsSuccess && data.ResultCode == 0) {
                            var result = data.Data;
                            if (callback) {
                                callback(data);
                            }

                        } else {
                            //debugger;
                            alert(data.Message);
                        }
                    }
                });
            },
            //获取新手引导操作
            getModuleNextStep: function (callback) {
                //debugger;
                var that = this;
                $.util.ajax({
                    url: "/ApplicationInterface/Gateway.ashx",
                    data: {
                        action: 'Basic.Guide.GetModuleNextStep',
                        ParentModule:this.args.guid,
                        Step:this.args.Step
                    },
                    success: function (data) {
                        if (data.IsSuccess && data.ResultCode == 0) {
                            var result = data.Data;
                            if (callback) {
                                callback(data.Data);
                            }

                        } else {
                            //debugger;
                            alert(data.Message);
                        }
                    }
                });
            },

            //记录当前步
            setModuleLastStep: function (callback) {
                //debugger;
                var that = this;
                $.util.ajax({
                    url: "/ApplicationInterface/Gateway.ashx",
                    data: {
                        action: 'Basic.Guide.SetModuleLastStep',
                        ParentModule:this.args.guid,
                        Step:this.args.Step,
                        Url:window.location.pathname,
                        FinishedStatus:this.args.finishedStatus
                    },
                    success: function (data) {
                        if (data.IsSuccess && data.ResultCode == 0) {
                            var result = data.Data;
                            if (callback) {
                                callback(data);
                            }

                        } else {
                            //debugger;
                            alert(data.Message);
                        }
                    }
                });
            }
        }

    };
    page.init();
});

