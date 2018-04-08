Jit.AM.defindPage({
    name: 'Questionnaire',
    questionnaireID: "",
    eventId: "",
    iserror: false,
    ismiyuezhuan: false,
    IsShare: false,
    ModelType:"1",
    back: window.history.back,
	initWithParam: function(param) {
	    
	},
	element:{
	    questionnaire: $(".questionnaire"),
	    ruleArea: $('.ruleArea'),  //规则区域
	    regular: $('.regular'),  //规则
	    startbtn: $('.startbtn'),  //开始按钮
	    selfControl:null,    //提示控件
	    ctweventId: ""
	},
	onPageLoad : function() {
		this.initPage();
	},
	initPage: function () {
		var that = this;
		
		window.onhashchange = function () {
		    if (location.hash != "") {
		        that.element.ruleArea.show();
		    } else {

		        that.element.ruleArea.hide();
		    }
		}

		var oHeight = $(window).height(); //浏览器当前的高度

		
	    


		this.eventId = this.getUrlParam("eventId");

       
	    //芈月传单独代码段start
		this.ismiyuezhuan = this.getUrlParam("ismiyuezhuan") ? true : false;
	    //芈月传单独代码段end

		that.getRedpackData();

		if (that.IsShare) {
		    that.setShareAH();
		}

		that.setShareAH();

		that.getQuestionnaire();

		debugger;
		if (!that.questionnaireID||that.questionnaireID == "")
		{
		    alert("没有问卷相关的活动！");
		    return;
		}

		$(".ActivityID").val(this.eventId);


		that.initEvent();
		Jit.UI.Loading(true);
		that.GetQuestionList(function (data) {

		    debugger;
		    var htmltext = "";
		    if (data.QuestionnaireList)
		    {
		        for (var i = 0; i < data.QuestionnaireList.length;i++)
		        {
		            var tempQuestion = data.QuestionnaireList[i];

		            tempQuestion.index=(i+1);
		            tempQuestion.length = data.QuestionnaireList.length;

		            if(tempQuestion.Optionlist)
		            {
                        
		                for (var j = 0; j < tempQuestion.Optionlist.length; j++)
		                {
		                    var option = tempQuestion.Optionlist[j];
		                    option.number = that.getnumber((j+1));
		                }
		            }
		            if (that.ModelType == "1") {
		                htmltext += bd.template('Tpl_itemmodel1', tempQuestion);
		                $(".model2").hide();
		            }

		            if (that.ModelType == "2") {

		                $(".questionnaire").on("focus", "input,textarea,select", function () {
		                    $(".quesfootshow").css("position", "relative");
		                    $(".quesfootshow").parents(".qmodel2").css("margin-bottom", "0px");
		                }).on("focusout", "input,textarea,select", function () {
		                    $(".quesfootshow").css("position", "fixed");
		                    $(".quesfootshow").parents(".qmodel2").css("margin-bottom", "65px");
		                });
		                htmltext += bd.template('Tpl_itemmodel2', tempQuestion);
		                $(".model2").show();
		            }
		        }
		    }

		    $(".questionnaire").html('');
		    $(".questionnaire").html(htmltext);

		   
		    that.fnGetProvince();
		});


		debugger;
	},
	initEvent: function(){
		var that = this;
		
        
	
		that.element.questionnaire.on("click", " .radioitem", function () {
		    if ($(this).hasClass("select")) {
		        $(this).removeClass("select");
		    } else {
		        $(this).parents(".quescontentitem").find(".radioitem").removeClass("select");
		        $(this).addClass("select");
		    }
		});

		that.element.questionnaire.on("click", ".checkitem", function () {
		    $(this).toggleClass("select");
		});

		$(".Endbtn").on("click", function () {
		    that.showSharePanel();
		})

		that.element.questionnaire.on("click", ".step", function () {
		    debugger;
		    var step = $(this).data("show");
		    if ($(this).hasClass("nextquestion") || $(this).hasClass("overquestion"))
		    {
		        var question = $(this).parents(".question");
		        if ($(this).hasClass("model2"))
		        {
		            question = $(".question");
		        }

		        if (!that.Validate(question))
		        {
		            return;
		        }

		        
		        
		    }

		    if ($(this).hasClass("overquestion"))
		    {

		        var Questiondatajson = "";
		        $(".Questiondata").each(function () {

		            Questiondatajson += $(this).data("idname") + ":'" + $(this).val() + "',";

		        });


                //选项
		        var optiondatajson = "";
		        $(".question").each(function () {
		            var tempdatajson = "";
		            var _optiondatajson = "";
		            var optionlist = "";
		            $(this).find(".option.select").each(function () {
		                tempdatajson += "" + $(this).data("name") + ",";
		                optionlist += "{OptionID:'" + $(this).data("optionid") + "',OptionName:'" + $(this).data("optionname") + "'},";
		            });
		            optionlist = "optionlist:[" + optionlist + "]";
		            tempdatajson = "AnswerOption:'" + tempdatajson + "',";
		            $(this).find(".optiondata").each(function () {
		                if ($(this).hasClass("addr"))//地址
		                {
		                    _optiondatajson += $(this).data("idname") + ":'" + $(this).find("option:selected").text() + "',";
		                } else {//其它数据
		                    _optiondatajson += $(this).data("idname") + ":'" + $(this).val().replace(/[\r\n]/g, "") + "',";
		                }
		            });

		            optiondatajson += "{" + _optiondatajson + tempdatajson + optionlist + "},";
		        });
		        optiondatajson = "QuestionnaireAnswerRecordlist:[" + optiondatajson + "]";
		        Questiondatajson = "{action:'Questionnaire.Questionnaire.SetQuestionnaireAnswerRecord'," + Questiondatajson + "" + optiondatajson + "}";

		        Jit.UI.Loading(true);
		        that.SetQuestionnaireAnswerRecord(Ext.decode(Questiondatajson), function (data) {
		            $(".questionnaire").hide();
		            $(".bgend").hide();
		            $(".defaultend").hide();
		            if (data.RecoveryType == "1") {
		                $(".endtext").html(data.RecoveryContent);
		                $(".endtext").show();
		            }

		            if (data.RecoveryType == "2")
		            {
		                $(".defaultend").attr("src", data.RecoveryImg);
		                $(".defaultend").show();
		                $(".bgend").show();
		                $(".endtext").hide();
		            }

		            $(".QuestionnaireEnd").show();


		            //芈月传单独代码段start
		            if (that.ismiyuezhuan) {
		                $(".defaultend").css("top", "0px");
		                $(".defaultend").css("width", "100%");
		                $(".defaultend").css("left", "0px");

		                $(".Endbtn").css("bottom", "initial");
		                $(".Endbtn").css("top", (0.75 * (document.body.clientWidth* 1.75)) + "px");
		                $(".Endbtn").css("height", "35px");
		                $(".Endbtn").css("width", "90%");
		                $(".Endbtn").css("line-height", "35px");
		                $(".Endbtn").css("font-size", "24px");
		                $(".Endbtn").css("left", "5%");
		                $(".bgend").hide();
		            }
		            //芈月传单独代码段end

		            
		        });
		    }

		    if ($("." + step).length > 0) {
		        $(".question").hide();
		        $("." + step).show();

		        //设置选项的高度
		        $(".option").not(".itemimg").find("span").each(function () {
		            var height = $(this).parents(".option").css("height");
		            debugger;
		            if (parseInt(height) > 55) {
		                $(this).css("height", height);
		            }
		        });
		        //设置选项的高度
		    }
		});


	    //活动规则
		that.element.regular.on('click', function () {
		    
		    window.location.href += "#a=1";
		});

		that.element.ruleArea.on('click', function () {
		    window.history.back(-1);
		});

		that.element.startbtn.on('click', function () {
		    Jit.UI.Loading(true);
		    that.getDrawLottery();
		    if (!that.iserror) {
		        $(".QuestionnaireStart").hide();
		        $(".questionnaire").show();

                //设置选项的高度
		        $(".option").not(".itemimg").find("span").each(function () {
		            var height = $(this).parents(".option").css("height");
		            debugger;
		            if (parseInt(height) > 55)
		            {
		                $(this).css("height", height);
		            }
		        });
		        //设置选项的高度
		    }
		});


		that.element.questionnaire.on("change", ".selectoption", function () {
		    var _value = $(this).val();
		    $(this).parents(".dropdown").find(".dropdownvalue").val(_value);
		});


	},
    //初始化中奖页面数据
	getDrawLottery: function () {
	    var that = this,
			params = {
			    'action': 'Event.Lottery.RedPacket',
			    'EventId': that.element.ctweventId
			};
	    that.ajax({
	        url: '/ApplicationInterface/Gateway.ashx',
	        'async': false,   //设置为同步请求
	        data: params,
	        success: function (data) {
	            if (data.IsSuccess && data.Data) {
	                var result = data.Data;
	                if (result) {
	                    debugger;
	                    if (result.PrizeId) {
	                        that.iserror = false;
	                    } else {
	                        that.iserror = true;
	                        that.alertDialog(result.PrizeName ? result.PrizeName : result.ResultMsg);
	                    }
	                }

	            } else {
	                me.alert(data.Message);
	            }
	            Jit.UI.Loading(false);
	        }
	    });

	},
	Validate: function (_question)
	{
	    var that = this;
	    var isok = true;
	    _question.each(function () {
	        question = $(this);

	        var questionindex = "";
	        if (question.data("index"))
	        {
	            questionindex ="第"+ question.data("index")+"题： ";
	        }

	        if (!isok)
	        {
	            return false;
	        }

	        var quescontentitem = question.find(".quescontentitem").eq(0);
	        var IsValidateMinChar = quescontentitem.data("isvalidateminchar");
	        var MinChar = quescontentitem.data("minchar");
	        var MaxChar = quescontentitem.data("maxchar");
	        var IsValidateMaxChar = quescontentitem.data("isvalidatemaxchar");
	        var NoRepeat = quescontentitem.data("norepeat");
	        var IsValidateStartDate = quescontentitem.data("isvalidatestartdate");
	        var StartDate = quescontentitem.data("startdate");
	        var IsValidateEndDate = quescontentitem.data("isvalidateenddate");
	        var EndDate = quescontentitem.data("enddate");
	        var _value = quescontentitem.find(".optiondata").val();
	        var type = quescontentitem.data("type");
	        var ScoreStyle = quescontentitem.data("scorestyle");
	        var MaxScore = quescontentitem.data("maxscore");

	        var isrequired = (quescontentitem.data("isrequired") == "1");

	        //验证必填
	        if (isrequired) {

	            //地址
	            if (question.find(".address").length > 0) {
	                var province = question.find(".province");
	                var city = question.find(".city");
	                var district = question.find(".district");
	                var _address = question.find("._address");
	                if (province && province.val() == "") {
	                    that.element.selfControl = province;
	                    that.alertDialog(questionindex+"请选择省份！");
	                    isok = false;
	                    return;
	                }

	                if (city && city.val() == "") {
	                    that.element.selfControl = city;
	                    that.alertDialog(questionindex + "请选择市！");
	                    isok = false;
	                    return;
	                }

	                if (district && district.val() == "") {
	                    that.element.selfControl = district;
	                    that.alertDialog(questionindex + "请选择区！");
	                    isok = false;
	                    return;
	                }

	                if (_address.val() == "") {
	                    that.element.selfControl = _address;
	                    that.alertDialog(questionindex + "请填写详细地址！");
	                    isok = false;
	                    return;
	                }

	            }

	            //选项
	            if (question.find(".option").length > 0) {
	                if (question.find(".quescontentitem .select").length < 1) {
	                    that.element.selfControl = question.find(".quescontentitem").eq(0);
	                    that.alertDialog(questionindex + "至少选择一项！");
	                    isok = false;
	                    return;
	                }

	            } else if (quescontentitem.find(".optiondata").val() == "") {//文本
	                that.element.selfControl = quescontentitem.find(".optiondata");
	                that.alertDialog(questionindex + "此项为必填！");
	                isok = false;
	                return;
	            }


	        }


	        //验证日期
	        if (IsValidateStartDate == "1" && IsValidateEndDate == "1") {
	            var ques = quescontentitem.find(".optiondata").val();
	            if (ques != "" || isrequired) {
	                var reg = /^(\d{4})-(\d{2})-(\d{2})(\S*)$/;
	                var date = new Date(Date.parse((ques.replace(reg, "$1") + '/' + ques.replace(reg, "$2") + '/' + ques.replace(reg, "$3"))));
	                var _StartDate = new Date(Date.parse((StartDate.replace(reg, "$1") + '/' + StartDate.replace(reg, "$2") + '/' + StartDate.replace(reg, "$3"))));
	                var _EndDate = new Date(Date.parse((EndDate.replace(reg, "$1") + '/' + EndDate.replace(reg, "$2") + '/' + EndDate.replace(reg, "$3"))));
	                if (date > _EndDate || date < _StartDate) {
	                    that.element.selfControl = quescontentitem.find(".optiondata");
	                    that.alertDialog(questionindex + "请填写日期在" + StartDate.substr(0, StartDate.indexOf("T")) + "至" + EndDate.substr(0, EndDate.indexOf("T")) + "范围内！");
	                    isok = false;
	                    return;
	                }
	            }
	        }
				
				//验证日期
	            if (IsValidateStartDate == "1") {
	                var ques = quescontentitem.find(".optiondata").val();
	                if (ques != "" || isrequired) {
	                    var reg = /^(\d{4})-(\d{2})-(\d{2})(\S*)$/;
	                    var date = new Date(Date.parse((ques.replace(reg, "$1")+'/'+ ques.replace(reg, "$2")+'/'+  ques.replace(reg, "$3"))));
	                    var _StartDate = new Date(Date.parse((StartDate.replace(reg, "$1")+'/'+  StartDate.replace(reg, "$2")+'/'+  StartDate.replace(reg, "$3"))));
	                    if (date < _StartDate) {
	                        that.element.selfControl = quescontentitem.find(".optiondata");
	                        that.alertDialog(questionindex + "请填写日期必须大于等于" + StartDate.substr(0, StartDate.indexOf("T")) +"！");
	                        isok = false;
	                        return;
	                    }
	                }
	            }
				
				//验证日期
	            if (IsValidateEndDate == "1") {
	                var ques = quescontentitem.find(".optiondata").val();
	                if (ques != "" || isrequired) {
	                    var reg = /^(\d{4})-(\d{2})-(\d{2})(\S*)$/;
	                    var date = new Date(Date.parse((ques.replace(reg, "$1")+'/'+ ques.replace(reg, "$2")+'/'+  ques.replace(reg, "$3"))));
	                    var _EndDate = new Date(Date.parse((EndDate.replace(reg, "$1")+'/'+  EndDate.replace(reg, "$2")+'/'+  EndDate.replace(reg, "$3"))));
	                    if (date > _EndDate ) {
	                        that.element.selfControl = quescontentitem.find(".optiondata");
	                        that.alertDialog(questionindex + "请填写日期必须小于等于" +  EndDate.substr(0, EndDate.indexOf("T")) + "！");
	                        isok = false;
	                        return;
	                    }
	                }
	            }
	        
	           


	            //验证输入字数范围
	            //if (IsValidateMinChar == "1" || IsValidateMaxChar == "1") {
	                if (quescontentitem.find(".optiondata").val() != "" || isrequired) {
	                    if ( IsValidateMinChar==1  && quescontentitem.find(".optiondata").val().length < MinChar) {
	                        that.element.selfControl = quescontentitem.find(".optiondata");
	                        that.alertDialog(questionindex + "最少输入" + MinChar + "个字符！");
	                        isok = false;
	                        return;
	                    }

	                    if ( IsValidateMaxChar==1 && quescontentitem.find(".optiondata").val().length > MaxChar) {
	                        that.element.selfControl = quescontentitem.find(".optiondata");
	                        that.alertDialog(questionindex + "最多输入" + MaxChar + "个字符！");
	                        isok = false;
	                        return;
	                    }
	                }

	            //}

	            



	        //验证手机号码
	        if (type == "6") {

	            var tel = quescontentitem.find(".optiondata").val();
	            if (tel != "" || isrequired) { //不为空时判断
	                    var reg = /^(((13[0-9]{1})|(15[0-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})$/;
	                    if (!reg.test(tel)) {
	                        that.element.selfControl = quescontentitem.find(".optiondata");
	                        if (tel.length == 11) {
	                            that.alertDialog(questionindex + "手机号码一般是以13、15、17、18等开头！");
	                        } else {
	                            that.alertDialog(questionindex + "手机号码输入错误！");
	                        }
	                        isok = false;
	                        return;
	                    }
	                }
	            }



	        //验证重复

	        var count = 1;
	        if (NoRepeat == "1")
	        {
	            count = 0;
	        }
	        if (_value != "") {
	            $(".optiondata").each(function () {

	                if ($(this).val() == _value) {
	                    if ($(this).parents(".quescontentitem").data("norepeat") == "1" || NoRepeat == "1")
	                        count++;
	                }
	            });
	            if (count > 1) {
	                that.alertDialog(questionindex + "信息已重复！");
	                isok = false;
	                return;
	            }
	        }


	        //获取得分值
	        if (type == "3" || type == "9") {
	            var radioitem = question.find(".radioitem.select");
	            var sumscore = question.find(".sumscore");
	            if (radioitem.length > 0)
	                sumscore.val(Number(radioitem.data("yespptionscore")));
	        }

	        if (type == "4" || type == "10") {
	            var checkitem = question.find(".checkitem.select");
	            var sumscore = question.find(".sumscore");
	            if (ScoreStyle == "1") {
	                checkitem.each(function () {
	                    sumscore.val(Number(sumscore.val()) + Number($(this).data("yespptionscore")));
	                });

	            }

	            if (ScoreStyle == "2") {
	                checkitem.each(function () {
	                    if ($(this).data("isrightvalue") == "1")
	                        sumscore.val(Number(sumscore.val()) + Number($(this).data("yespptionscore")));
	                });
	            }

	            if (ScoreStyle == "3") {
	                debugger;
	                var selectcount = checkitem.length;
	                var rightcount = question.find(".checkitem[data-isrightvalue='1']").length;
	                if (selectcount == rightcount)
	                    sumscore.val(MaxScore);

	            }
	        }
	       

	    });

	    return isok;

	}
    ,
	getnumber: function (index) {
	    if (index > 0 && index < 20)
	        return String.fromCharCode(64 + parseInt(index));
	    else
	        return "";
	},
	alertDialog: function (message) {
	    var height = $(window).height();
	    var that = this;
	    Jit.UI.Dialog({
	        "type": "Alert",
	        "content": message,
	        "CallBackOk": function () {
	            Jit.UI.Dialog('CLOSE');
	            if (that.element.selfControl)
	            {
	                that.element.selfControl.focus();
	            }
	        }
	    });
	},
    
	GetQuestionList: function (callback) {//根据问卷ID获取题目及选项列表
	    var that = this;
	    if (!that.questionnaireID) {
	        return;
	    }
	    that.ajax({
	        url: "/ApplicationInterface/Gateway.ashx",
	        data: {
	            action: 'Questionnaire.Questionnaire.GetQuestionList',
	            QuestionnaireID: that.questionnaireID
	        },
	        success: function (data) {
	            if (data.IsSuccess && data.ResultCode == 0) {
	                var data = data.Data;
	                if (callback) {
	                    callback(data);
	                }
	                
	            } else {
	                alert(data.Message);
	            }
	        },
	        complete: function () {
	            Jit.UI.Loading(false);
	        }
	    });

	}
    ,
	fnGetProvince: function () {
	    var self = this;
	    self.getProvince(function (data) {
	        var itemlists = data.provinceList;
	        var $provinceSelect = $(".province");
	        $provinceSelect.empty();
	        var optionStr = "<option value=''>省</option>";
	        $.each(itemlists, function (i, obj) {
	            if (obj.Province == decodeURIComponent(self.getUrlParam('province'))) {
	                optionStr += "<option value='" + obj.Province + "' selected=\"true\">" + obj.Province + "</option>";
	            } else {
	                optionStr += "<option value='" + obj.Province + "'>" + obj.Province + "</option>";
	            }
	        });
	        $provinceSelect.html(optionStr);
	        self.fnGetCityByProvince($provinceSelect.val(), $provinceSelect);
	    });
	},
	fnGetCityByProvince: function (province, provinceself) {
	    var self = this;
	    self.getCityByProvince(province, function (data) {
	        var itemlists = data.cityList;
	        var $citySelect =$(provinceself).parents(".address").find(".city");
	        $citySelect.empty();
	        var optionStr = "<option value=''>市</option>";
	        $.each(itemlists, function (i, obj) {
	            if (obj.CityName == decodeURIComponent(self.getUrlParam('city'))) {
	                optionStr += "<option value='" + obj.CityID + "' selected=\"true\">" + obj.CityName + "</option>";
	            } else {
	                optionStr += "<option value='" + obj.CityID + "'>" + obj.CityName + "</option>";
	            }
	        });
	        $citySelect.html(optionStr);
	        self.fnGetDistrictByCity($citySelect.val(), $citySelect);
	    });
	},
	fnGetDistrictByCity: function (city, cityself) {
	    var self = this;
	    self.getDistrictByCity(city, function (data) {
	        var itemlists = data;
	        var $districtSelect = $(cityself).parents(".address").find(".district");
	        $districtSelect.empty();
	        var optionStr = "<option value=''>区</option>";
	        $.each(itemlists, function (i, obj) {
	            console.log(obj.Name + "%%%" + decodeURIComponent(self.getUrlParam('districtName')));
	            if (obj.Name == decodeURIComponent(self.getUrlParam('districtName'))) {
	                optionStr += "<option value='" + obj.DistrictID + "' selected=\"true\">" + obj.Name + "</option>";
	            } else {
	                optionStr += "<option value='" + obj.DistrictID + "'>" + obj.Name + "</option>";
	            }
	        });
	        $districtSelect.html(optionStr);
	    });
	},
        //获取问卷内容
	    getQuestionnaire: function () {
	        debugger;
	        var that = this;
	        that.ajax({
	            url: "/ApplicationInterface/Gateway.ashx",
	            async : false,
	            data: {
	                action: 'Questionnaire.Questionnaire.GetQuestionnaire',
	                ActivityID: that.element.ctweventId
	            },
	            success: function (data) {
	                if (data.IsSuccess && data.ResultCode == 0) {
	                    var result = data.Data;

	                    if (result) {

	                        that.questionnaireID = result.QuestionnaireID;
	                        //初始化数据
	                        $(".QuestionnaireID").val(result.QuestionnaireID);
	                        $(".QuestionnaireName").val(result.QuestionnaireName);
	                        //document.title = result.QuestionnaireName;

	                        var $body = $('body');
						    document.title = result.QuestionnaireName;
						    var $iframe = $("<iframe style='display:none;' src='../../../images/common/popup-close.png'></iframe>");
							$iframe.on('load',function() {
							    setTimeout(function() {
							    	$iframe.off('load').remove();
							    }, 0);
							}).appendTo($body);

	                        that.sharePage(result.BGImageSrc || result.QResultBGImg);

	                        debugger;
	                        //初始化开始页按钮背景和字体颜色、规则颜色start
	                        $(".startbtn").attr("style", "background-color:" + result.StartPageBtnBGColor + ";color:" + result.StartPageBtnTextColor + ";");
	                        $(".regular").attr("style", "color:" + result.StartPageBtnBGColor + ";");
	                        //初始化开始页按钮背景和字体颜色、规则颜色end


	                        that.ModelType = result.ModelType;

	                        //问卷规则
	                        $(".ruleArea").html(result.QRegular);

	                        //初始化规则start
	                        if (result.IsShowQRegular == 1) {
	                            $(".rulebtn").addClass("on");
	                            $(".regular").show();
	                        } else {
	                            $(".rulebtn").removeClass("on");
	                            $(".regular").hide();
	                        }
	                        //初始化规则end
	                        //开始页背景图片初始化
	                        $("._BGImageSrc").attr("src", result.BGImageSrc);

	                        //结束页背景图片初始化
	                        $(".end_BGImageSrc").attr("src", result.QResultBGImg);

	                        //结束页按钮背景和字体颜色初始化
	                        $(".Endbtn").attr("style", "background-color:" + result.QResultBGColor + ";color:" + result.QResultBtnTextColor + ";");
	                    }
	                } else {
	                    debugger;
	                    alert(data.Message);
	                }
	            },
	            complete: function () {
	                Jit.UI.Loading(false);
	            }
	        });
	    },

	    SetQuestionnaireAnswerRecord: function (para, callback) {//根据问卷ID获取题目及选项列表

	        var that = this;
	        that.ajax({
	            url: "/ApplicationInterface/Gateway.ashx",
	            data: para ,
	            success: function (data) {
	                if (data.IsSuccess && data.ResultCode == 0) {
	                    var data = data.Data;
	                    if (callback) {
	                        callback(data);
	                    }

	                } else {
	                    alert(data.Message);
	                }
	            },
	            complete: function () {
	                Jit.UI.Loading(false);
	            }
	        });

	    },

	    getProvince: function (callback) {//根据问卷ID获取题目及选项列表

	        var that = this;
	        that.ajax({
	            url: "/OnlineShopping/data/Data.aspx",
	            data: {
	                'action': 'getProvince'
	            },
	            success: function (data) {
	                if (data.code == 200) {
	                    if (callback) {
	                        callback(data.content);
	                    }
	                } else {
	                    alert(data.description);
	                }
	            }
	        });

	    },

	    getCityByProvince: function (province, callback) {//根据问卷ID获取题目及选项列表

	        var that = this;
	        that.ajax({
	            url: "/OnlineShopping/data/Data.aspx",
	            data: {
	                'action': 'getCityByProvince',
                    'Province': province
	            },
	            success: function (data) {
	                if (data.code == 200) {
	                    if (callback) {
	                        callback(data.content);
	                    }
	                } else {
	                    alert(data.description);
	                }
	            }
	        });

	    },

	    getDistrictByCity: function (city, callback) {//根据问卷ID获取题目及选项列表

	        var that = this;
	        that.ajax({
	            url: "/OnlineShopping/data/Data.aspx",
	            data: {
	                'action': 'getDistrictsByDistricID',
	                'districtId': city
	            },
	            success: function (data) {
	                if (data.code == 200) {
	                    if (callback) {
	                        callback(data.content);
	                    }
	                } else {
	                    alert(data.description);
	                }
	            }
	        });

	    },
    //获得初始化数据
	    getRedpackData: function () {
	        var that = this;
	        var params = {
	            'action': 'Event.Lottery.GetImage',
	            'eventId': that.eventId,
	            'RecommandId': Jit.AM.getUrlParam('sender') || ''  //推荐人的id
	        };
	        that.ajax({
	            url: '/ApplicationInterface/Gateway.ashx',
	            async: false,   //设置为同步请求
	            data: params,
	            success: function (data) {
	                if (data.IsSuccess && data.Data) {
	                    debugger;
	                    var result = data.Data;
	                    if (result.EventId) {
	                        that.element.ctweventId = result.EventId;
	                    }
	                    that.IsShare = result.IsShare;
	                } else {
	                    alert(data.Message);
	                }
	            }
	        });

	    },
	    showSharePanel: function () {
	        $('#shareMask').show();
	    },
	    hideSharePanel: function () {
	        $('#shareMask').hide();
	    },
    setShareAH:function(){
        var that = this,
			params = {
			    'action': 'Event.Lottery.Share',
			    'EventId': that.eventId,
			    'ShareUserId': Jit.AM.getUrlParam('sender') || '' ,
			    'TypeId':'',
			};
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            //'async': false,
            data: params,
            success: function(data) {
                if (data.IsSuccess) {
                }else {
                    alert(data.Message);
                }
            }
        });
    },
    sharePage: function (img) {//分享设置
        var that = this;
        var desc = "赶紧来看看吧!";
        var info = Jit.AM.getBaseAjaxParam(),
            shareUrl = location.href;
        var title = document.title;

        shareUrl = shareUrl.replace('&applicationId=' + Jit.AM.getUrlParam('applicationId'), '');
        shareUrl = shareUrl.replace('&rid=' + Jit.AM.getUrlParam('rid'), '');
        shareUrl = shareUrl.replace('&sender=' + Jit.AM.getUrlParam('sender'), '');


        //芈月传单独代码段start
        if (that.ismiyuezhuan) {
            shareUrl = location.href;
            shareUrl = Jit.AM.getUrlParam('shareurl');
            img = 'http://' + location.host + '/HtmlApps/images/common/Questionnaire/miyuezhuan.png';
            
            title= "跟芈月回战国",
	        desc='火速围观，玩游戏赢Money！',
            shareUrl = location.href.replace("Questionnaire.html", "Questionnairemiyuezhuan.html");
            
        }
        //芈月传单独代码段end

		


	    var shareInfo = {
	        'title': title,
	        'desc': desc,
	        'link': shareUrl,
	        'isAuth': true ,//需要高级auth认证
	        'imgUrl': img
	    };
	    Jit.AM.initShareEvent(shareInfo);

	}

}); 