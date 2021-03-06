﻿define(['jquery', 'bdTemplate', 'tools', 'pagination'], function () {

    var page = {
        ele: {
            section: $("#section"),
            sceneList: $("#sceneList"),
            selectList: $("#selectList"),
            allList: $("#allList"),

            menuList: $("#menuList"),
            titleContainer: $("#titleContainer")
        },
        init: function () {
            this.formName = "";
            this.UsedFieldListLength = 0;
            this.selectList = [];
            this.allList = [];
            this.sceneList = [];

            this.selectObj = null;
            this.allObj = null;
            this.sceneObj = null;


            this.typeId = 1;
            this.loadData();
            this.initEvent();
        },
        loadData: function () {
            this.classMap = {
                "1": "iconName",
                "2": "iconNumber",
                "3": "iconName",
                "30": "iconName",
                "4": "iconDate",
                "5": "iconDate",
                "6": "iconMenu",
                "7": "iconMenu",
                //                    "1":"iconSex01",
                //                    "1":"iconSex02",
                //                    "1":"iconMobile",
                //                    "1":"iconEmail",
                "201": "iconOther",
                "202": "iconOther",
                "203": "iconOther",
                "204": "iconOther"
            };
            this.loadPageList(function () {

                self.dataLoadedProcess();

            });
        },
        dataLoadedProcess: function () {
            
            self.ele.titleContainer.find(".inputBox span").html(self.formName).siblings().val(self.formName);
            if (!!!self.selectList) {
                self.selectList = [];
            }
            self.ele.selectList.html(self.render("selectItemTemp", { list: self.selectList, typeId: self.typeId }));
            if (!!!self.allList) {
                self.allList = [];
            }
            self.ele.allList.html(self.render("allItemTemp", { list: self.allList, classMap: self.classMap, typeId: self.typeId }));

            if (self.sceneList && self.sceneList.length) {
                self.ele.sceneList.html(self.render("sceneItemTemp", { list: self.sceneList, typeId: self.typeId }));
            }
            self.selectObj = $.util.list2obj(self.selectList, "FormControlID");
            self.allObj = $.util.list2obj(self.allList, "FormControlID");
            //self.sceneObj = $.util.list2obj(self.sceneList, "SceneValue");

            // 把all中已选的置为select的对象
            for (var i = 0; i < self.selectList.length; i++) {
                var pid = self.selectList[i].FormControlID;
                self.allObj[pid] = self.selectObj[pid];
            }
        },
        initEvent: function () {
            this.ele.section.delegate(".checkbox", "click", function () {
                var $this = $(this), id = $this.attr("data-id"), isSelected;
                $this.toggleClass("on");
                isSelected = $this.hasClass("on") ? 1 : 0;
                if (id) {
                    self.sceneObj[id].IsSelected = isSelected;
                }
            }).delegate("#sceneCommitBtn", "click", function () {
                if (self.UsedFieldListLength == 0) {
                    alert("表单组件为空，若正在编辑，请先保存表单！");
                    return;
                }
                self.sceneList = $.util.obj2list(self.sceneObj);
                self.applyToAction();
            });
            // title container
            this.ele.titleContainer.delegate(".modifyBtn", "click", function () {
                var $this = $(this);
                $this.hide().siblings(".btnWrap").show();
                $this.siblings(".inputBox").find("span").hide().siblings().show();
            }).delegate(".cancelBtn", "click", function () {
                var $this = $(this);
                $this.parents(".btnWrap").hide().siblings(".modifyBtn").show().siblings(".inputBox").find("span").show().siblings().hide();
                $this.siblings(".inputBox").find("span").hide().siblings().show();
            }).delegate(".saveBtn", "click", function () {
                var $this = $(this);
                var value = $this.parent().siblings(".inputBox").find("input").val();
                if (value.length == 0 || value.length > 100) {
                    alert("你输入的字符串过长，请减少再试试");
                    return false;
                }
                self.renameAction(value, function () {
                    $this.parents(".btnWrap").hide().siblings(".modifyBtn").show().siblings(".inputBox").find("span").html(value).show().siblings().hide();
                    $this.siblings(".inputBox").find("span").hide().siblings().show();
                });

            });

            //已选组件层 事件
            this.ele.selectList.delegate(".closeBtn", "click", function () {
                var item = $(this).parent();
                var id = item.attr("data-id");
                $(this).addClass("exist");
                delete self.selectObj[id];
                self.selectList = $.util.obj2list(self.selectObj);
                self.ele.selectList.html(self.render("selectItemTemp", { list: self.selectList, typeId: self.typeId }));

                self.allObj[id].EditOrder = 0;
                self.allList = $.util.obj2list(self.allObj);
                self.ele.allList.html(self.render("allItemTemp", { list: self.allList, classMap: self.classMap, typeId: self.typeId }));

            }).delegate(".selectBox .dropList span", "click", function () {
                var $this = $(this);
                var text = $this.text();
                var id = $this.parents(".formShowArea-item").attr("data-id");
                $this.parent().siblings().html(text).parent().attr("data-val", $this.attr("data-val"));
                self.selectObj[id].IsMustDo = 0;
                self.selectObj[id].IsRead = parseInt($this.attr("data-val"));
            }).delegate("#selectCommitBtn", "click", function () {
                for (var i in self.selectObj) {
                    if (self.typeId == 2) {
                        self.selectObj[i].EditOrder = $("#" + i).index() + 22;
                    } else {
                        self.selectObj[i].EditOrder = $("#" + i).index();
                    }
                }
                self.selectList = $.util.obj2list(self.selectObj);
                self.saveFormAction();
            });


            //全部组件层 事件
            this.ele.allList.delegate("li", "click", function () {
                var id = $(this).attr("data-id");
                $(this).addClass("exist");
                //设置editOrder
                var length = self.ele.selectList.find(".formShowArea-item").length;
                if (self.typeId == 2) {
                    self.allObj[id].EditOrder = length + 1 + 20;
                } else {
                    self.allObj[id].EditOrder = length + 1;
                }
                self.selectObj[id] = self.allObj[id];
                self.selectList = $.util.obj2list(self.selectObj);
                self.ele.selectList.html(self.render("selectItemTemp", { list: self.selectList, typeId: self.typeId }));
            });



            // tab事件
            this.ele.menuList.delegate("li", "click", function () {
                
                $(this).addClass("on").siblings().removeClass("on");
                self.typeId = $(this).data("type");
                $("#" + $(this).data("layer")).show().siblings(".tabContainer").hide();
                self.loadData();
            });

        },
        loadPageList: function (callback) {
            $.util.ajax({
                url: "/ApplicationInterface/DynamicVipForm/DynamicVipFormEntry.ashx",
                type: "get",
                data: {
                    action: "DynamicControlDisplayList",
                    Type: this.typeId,
                    TableName: "vip"
                },
                success: function (data) {


                    if (data.IsSuccess) {
                        
                        self.formName = data.Data.FormName;
                        self.allList = data.Data.AllFieldList;
                        self.selectList = data.Data.UsedFieldList;
                        self.sceneList = data.Data.SceneList;
                        self.UsedFieldListLength = (data.Data.UsedFieldList && data.Data.UsedFieldList.length) || 0;
                        if (callback) {
                            callback(data.Data);
                        }
                    } else {
                        alert(data.Message);
                    }
                }
            });
        },
        renameAction: function (name, callback) {
            $.util.ajax({
                url: "/ApplicationInterface/DynamicVipForm/DynamicVipFormEntry.ashx",
                type: "get",
                data: {
                    action: "DynamicVipFormRename",
                    FormID: self.formId,
                    Name: name
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        if (callback) {
                            callback();
                        }
                    } else {
                        alert(data.Message);
                    }
                }
            });
        },
        applyToAction: function (callback) {
            $.util.ajax({
                url: "/ApplicationInterface/DynamicVipForm/DynamicVipFormEntry.ashx",
                type: "get",
                data: {
                    action: "DynamicVipFormSceneSave",
                    FormID: self.formId,
                    SceneList: self.sceneList
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        if (callback) {
                            callback();
                        } else {
                            alert("提交成功！");
                        }
                    } else {
                        alert(data.Message);
                    }
                }
            });
        },
        saveFormAction: function (callback) {
            $.util.ajax({
                url: "/ApplicationInterface/DynamicVipForm/DynamicVipFormEntry.ashx",
                data: {
                    action: "DynamicControlDisplaySave",
                    TableName: "Vip",
                    FieldList: self.selectList,
                    Type: this.typeId
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        if (callback) {
                            callback();
                        } else {
                            alert("保存成功！");
                            self.loadPageList(function () {
                                self.dataLoadedProcess();
                            });
                        }
                    } else {
                        alert(data.Message);
                    }
                }
            });
        },
        render: function (tempId, data) {
            var render = bd.template(tempId, data);
            return render || {};
        }
    };

    var self = page;
    page.init();
});