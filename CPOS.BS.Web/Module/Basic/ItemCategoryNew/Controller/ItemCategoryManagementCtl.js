﻿Ext.Loader.setConfig({
    enabled: true
});
var K;
var htmlEditor;
var disabledString = "(已停用)";
var uploadImageUrl = "";
var isAld; //判断该客户信息是否同步到阿拉丁

//alert(isAld);
Ext.onReady(function () {
    //初始化
    InitVE();
    InitStore();
    InitView();
    //判断客户是否同步信息到阿拉丁
    fnIsAld();
    //绑定事件
    fnBindEvents();

    //初始化图片上传控件
    KE = KindEditor;
    var uploadbutton = KE.uploadbutton({
        button: KE('#btnUploadImage')[0],
        //上传的文件类型
        fieldName: 'imgFile',
        //注意后面的参数，dir表示文件类型，width表示缩略图的宽，height表示高
        url: '/Framework/Javascript/Other/kindeditor/asp.net/upload_thumbnails_json.ashx?dir=image&width=65&height=61',
        afterUpload: function (data) {
            if (data.error === 0) {
                Ext.Msg.show({
                    title: '提示',
                    msg: '图片上传成功',
                    buttons: Ext.Msg.OK,
                    icon: Ext.Msg.INFO
                });
                uploadImageUrl = getStr(data.url);
                document.getElementById("txtImageUrl").src = data.url;
                // Ext.getCmp("txtImageUrl").setValue(getStr(data.url));
            } else {
                Ext.Msg.show({
                    title: '提示',
                    msg: data.message,
                    buttons: Ext.Msg.OK,
                    icon: Ext.Msg.INFO
                });
            }
        },
        afterError: function (str) {
            Ext.Msg.show({
                title: '自定义错误信息' + str,
                msg: jdata.msg,
                buttons: Ext.Msg.OK,
                icon: Ext.Msg.ERROR
            });
        }
    });
    uploadbutton.fileBox.change(function (e) {
        uploadbutton.submit();
    });

    var itemCategoryStore = Ext.getStore('itemCategoryStore');//用于获取父类树节点
    itemCategoryStore.proxy.url = "Handler/ItemCategoryHandler.ashx?method=getAll";//获取了全部的节点。
    itemCategoryStore.load();
});

function fnBindEvents() {
    //树的上下文菜单
    var treePanel = Ext.getCmp('trpItemCategoryTree');
    treePanel.on('itemcontextmenu', this.fnTrpItemCategoryTree_ItemContextMenu_Click, this);
    //树某一项选中事件
    treePanel.on('itemclick', this.fnTrpItemCategoryTree_ItemClick, this);
    //树上下文菜单项的点击事件
    var ctnMenuItemAdd = Ext.getCmp('ctnMenuItemAdd');
    ctnMenuItemAdd.on('click', this.fnCtnMenuItemAdd_Click);
    //var ctnMenuItemDelete = Ext.getCmp('ctnMenuItemDelete');
    //ctnMenuItemDelete.on('click', this.fnCtnMenuItemDelete_Click);
    //停用事件处理
    var btnDelete = Ext.getCmp('btnDelete');
    btnDelete.on('click', this.fnBtnDelete_Click);
    //保存事件处理
    var btnSave = Ext.getCmp('btnSave');
    btnSave.on('click', this.fnBtnSave_Click);
    //新增事件处理
    var btnAdd = Ext.getCmp('btnAdd');
    btnAdd.on('click', this.fnBtnAdd_Click);
}

/*
@private
停用事件处理
*/
function fnBtnDelete_Click() {
    var ctl_hddID = Ext.getCmp('hddID');
    var id = ctl_hddID.getValue();
    if (id == null || id == '') {
        Ext.Msg.alert('提示信息', '没有数据需要停用.');
        return;
    }

    var record = Ext.getCmp('trpItemCategoryTree').getStore().getNodeById(id);
    if (record == null) {
        Ext.Msg.alert('提示信息', '没有数据需要停用.');
        return;
    } else {
        if (this.text == "启用")
            fnToggleItemCategoryStatus(record, 1);
        else if (this.text == "停用")
            fnToggleItemCategoryStatus(record, -1);

        fnClearEditArea();
    }
}

/*
@private
保存事件处理
*/
function fnBtnSave_Click() {
    var ctl_hddID = Ext.getCmp('hddID');
    var ctl_txtCode = Ext.getCmp('txtCode');
    var ctl_txtName = Ext.getCmp('txtName');
    var ctl_txtZJM = Ext.getCmp('txtZJM');
    var ctl_cmbStatus = Ext.getCmp('cmbStatus');
    var ctl_cmbParent = Ext.getCmp('cmbParent');
    var ctl_nmbNO = Ext.getCmp('nmbNO');
    //var ctl_txtImageUrl =  Ext.getCmp('txtImageUrl');
    var ctl_aldCategory = Ext.getCmp('aldCategory'); //阿拉丁分类,要判断，只有是

    var method = 'update';
    //检查数据
    var id = ctl_hddID.getValue();
    var code = ctl_txtCode.getValue();
    var name = ctl_txtName.getValue();
    var zjm = ctl_txtZJM.getValue();
    var status = ctl_cmbStatus.getValue();
    var parentID = ctl_cmbParent.jitGetValue();//父类获取值
    var no = ctl_nmbNO.getValue();
    var imageUrl = uploadImageUrl;  //ctl_txtImageUrl.getValue();
    var aldCategoryID = ctl_aldCategory.jitGetValue(); //阿拉丁分类获取值，只有同步到阿拉丁的，才是必填，才需要同步
//    alert(aldCategoryID);
//    return;

    if (method == "update") {
        if (parentID == id) {
            Ext.Msg.alert('提示信息', '自身不能作为上级分类.');
            return;
        }
    }

    if (code == null || code == '') {
        Ext.Msg.alert('提示信息', '必须输入类型编码.');
        return;
    }
    if (name == null || name == '') {
        Ext.Msg.alert('提示信息', '必须输入类型名称.');
        return;
    }
    if (status == null || status == '') {
        Ext.Msg.alert('提示信息', '必须选择状态.');
        return;
    }
    if (parentID == null || parentID == '') {
        Ext.Msg.alert('提示信息', '必须选择上级分类.');
        return;
    }
    //提交数据
    var data = {};
    data.Item_Category_Id = id;
    data.Item_Category_Code = code;
    data.Item_Category_Name = name;
    data.Pyzjm = zjm;
    data.Status = status;
    data.Parent_Id = parentID;
    data.DisplayIndex = no;
    data.ImageUrl = imageUrl;
    if (isAld == "1") {
        if (aldCategoryID == null ||  aldCategoryID =='' || aldCategoryID == '--请选择--') {
            Ext.Msg.alert('提示信息', '必须选择对应的阿拉丁分类.');
            return;
        }
        data.ALDCategoryID = aldCategoryID; //只有同步到阿拉丁的，才是必填，才需要同步，这里需要判断，如果不需要同步到ALD，就设为空
    } else {data.ALDCategoryID = ""; }
  //遮罩层
    var mask = new Ext.LoadMask(Ext.getBody(), {
        msg: '正在向服务器提交,请稍等...'
    });
    if (id == null || id == '') {
        method = "add";
    }
    mask.show();//遮罩层显示
    Ext.Ajax.request({
        url: 'Handler/ItemCategoryHandler.ashx?method=' + method   //又获取值
        , method: 'POST'
        , params: Ext.JSON.encode(data)
        , callback: function (options, success, response) {
            //
            var d = Ext.JSON.decode(response.responseText);
            if (d.success) {
                Ext.Msg.alert('提示信息', '保存成功！');
            } else {
                Ext.Msg.alert('提示信息', "保存失败," + d.msg);
            }
            //如果变更了上级分类，则重新取数据
            var isNeedReload = false;
            if (method == "add") {
                isNeedReload = true;
            } else {
                var updatedRecord = Ext.getStore('itemCategoryStore').getById(data.Item_Category_Id);//
                if (updatedRecord.get('Parent_Id') != data.Parent_Id) {
                    isNeedReload = true;
                }
                isNeedReload = true;
            }
            if (isNeedReload) {
                Ext.getCmp("trpItemCategoryTree").on("load", function () { Ext.getStore('itemCategoryTreeStore').getNodeById(parentID).expand(); });

                if (Ext.getCmp('cmbParent').store != null)
                    Ext.getCmp('cmbParent').store.load();
                Ext.getStore('itemCategoryTreeStore').load();//加载数据，左边树面板
                Ext.getStore('itemCategoryStore').load();//右边父类
            }
            //
            var ctl_pnlEdit = Ext.getCmp('pnlEdit');
            ctl_pnlEdit.setTitle('编辑');
            //
            mask.hide();
        }
    });
}

//判断客户信息是否同步到阿拉丁
function fnIsAld() {
    var isALDUrl = "../../CustomerBasicSetting/Handler/CustomerBasicSettingHander.ashx?mid=" + __mid
    Ext.Ajax.request({
        url: isALDUrl + "&method=IsAld",
        async: false,
        method: 'post',
        success: function (response) {
            var isald = response.responseText;//这个参数没太有用
            if (isald == "1") {
                isAld = isald;
              //  Ext.getCmp("aldCategory").setVisible(false);
            }
            else {
                Ext.getCmp("aldCategory").setVisible(false);
                isAld = isald;
            }
        },
        failure: function () {
            Ext.Msg.alert("提示", "获取参数数据失败");
        }
    });
}
//获取分类对应的阿拉丁分类的信息
function fnGetALDCategory(itemCategoryId) {
    var data = {};
    data.Item_Category_Id = itemCategoryId;
    Ext.Ajax.request({
        url: 'Handler/ItemCategoryHandler.ashx?method=' + 'GetALDByCategoryId',  //又获取值,
        params: Ext.JSON.encode(data),
        async: true, //异步处理吧
        method: 'post',
        success: function (response) {
            //
            var jdata = Ext.JSON.decode(response.responseText);
            var ctl_aldCategory = Ext.getCmp('aldCategory'); //父类
            if (jdata && jdata.ResultCode == '200') {//不为空或者undefined

                var id = jdata.Data.CategoryID;
                var name = jdata.Data.CategoryName;
                var item = [{ id: id, text: name}];
                if (id != null) {
                    ctl_aldCategory.setValues(item, false);
                } else {
                    var id = '';
                    var name = '--请选择--';
                    var item = [{ id: id, text: name}];
                    ctl_aldCategory.setValues(item, false);
                }
            }
        },
        failure: function () {
            //
            Ext.Msg.alert("提示", "获取参数数据失败");
        }
    });
}

/*
@private 
新增事件处理
*/
function fnBtnAdd_Click() {
    fnClearEditArea();
    //
    var ctl_pnlEdit = Ext.getCmp('pnlEdit');
    ctl_pnlEdit.setTitle('新增');

    var ctl_cmbStatus = Ext.getCmp('cmbStatus');
    ctl_cmbStatus.setValue('1');
    
    //新增时重置图片
    uploadImageUrl = "";
    document.getElementById("txtImageUrl").src = uploadImageUrl;
}

/*
@private
商品分类树面板的右键上下文菜单事件处理
*/
function fnTrpItemCategoryTree_ItemContextMenu_Click(view, record, item, index, e, options) {
    var menu = Ext.getCmp('ctnMenu');
    menu.showAt(e.getXY());
    menu.__current_record = record; //菜单引用当前选中的记录
    e.stopEvent();
}

/*
@private
商品分类树上任何一个项被选中时的事件处理
*/
function fnTrpItemCategoryTree_ItemClick(view, record, item, e, options) {
    var id = record.get('id');
    var itemCategoryStore = Ext.getStore('itemCategoryStore');
    var detailData = itemCategoryStore.getById(id);
    if (detailData != null) {
        if (detailData.get('Item_Category_Name').indexOf(disabledString) >= 0)//停用的不能保存
            Ext.getCmp("btnSave").hide();
        else
            Ext.getCmp("btnSave").show();
        if (detailData.get('Parent_Id') == "-99") {//顶级分类不能做修改调整
            //Ext.getCmp("btnAdd").hide();
            Ext.getCmp("btnDelete").hide();
            Ext.getCmp("btnSave").hide();
        }
        else {
            Ext.getCmp("btnAdd").show();
            Ext.getCmp("btnDelete").show();
            Ext.getCmp("btnSave").show();
        }

        var ctl_hddID = Ext.getCmp('hddID');
        var ctl_txtCode = Ext.getCmp('txtCode');
        var ctl_txtName = Ext.getCmp('txtName');
        var ctl_txtZJM = Ext.getCmp('txtZJM');
        var ctl_cmbStatus = Ext.getCmp('cmbStatus');
       
        var ctl_nmbNO = Ext.getCmp('nmbNO');
        //var ctl_txtImageUrl = Ext.getCmp('txtImageUrl');
        var ctl_pnlEdit = Ext.getCmp('pnlEdit');
        var ctl_txtGUID = Ext.getCmp('txtGUID');
        //
        ctl_hddID.setValue(detailData.get('Item_Category_Id'));
        ctl_txtGUID.setValue(detailData.get('Item_Category_Id'));
        ctl_txtCode.setValue(detailData.get('Item_Category_Code'));
        ctl_txtName.setValue(detailData.get('Item_Category_Name'));
        ctl_txtZJM.setValue(detailData.get('Pyzjm'));
        var parentID = detailData.get('Parent_Id');
        var ctl_cmbParent = Ext.getCmp('cmbParent'); //父类
        if (parentID != null && parentID != '') {
            var name = detailData.get('Parent_Name');
            var item = [{ id: parentID, text: name}];
            ctl_cmbParent.setValues(item, false);
        }
        //阿拉分类，需要处理吗？
        //fnGetALDCategory(detailData.get('Item_Category_Id'));


        var status = detailData.get('Status');
        ctl_cmbStatus.setValue(status);
        ToggleButtonText(status);

        ctl_nmbNO.setValue(detailData.get('DisplayIndex'));
        //ctl_txtImageUrl.setValue(detailData.get('ImageUrl'));
        uploadImageUrl = detailData.get('ImageUrl');
        if (detailData.get('ImageUrl') != null && detailData.get('ImageUrl') != "") {
            document.getElementById("txtImageUrl").src = uploadImageUrl;
        }
        else {
            document.getElementById("txtImageUrl").src = "";
        }
        //
        ctl_pnlEdit.setTitle('编辑');
    }
}

/*
@private
[上下文菜单项 - 添加]的点击事件处理
*/
function fnCtnMenuItemAdd_Click(view, record, item, e, options) {
    var menu = Ext.getCmp('ctnMenu');
    var parent = menu.__current_record;
    var name = parent.get('text');

    if (!(name.indexOf(disabledString) >= 0)) {
        //清除数据
        fnClearEditArea();
        //
        var ctl_cmbParent = Ext.getCmp('cmbParent');//设置值
        ctl_cmbParent.setValue(null);

        var ctl_pnlEdit = Ext.getCmp('pnlEdit');
        ctl_pnlEdit.setTitle('新增');

        var ctl_cmbStatus = Ext.getCmp('cmbStatus');
        ctl_cmbStatus.setValue('1');
        //设置上级分类
        var parentID = parent.get('id');
        if (parentID != null && parentID != '') {
            var item = [{ id: parentID, text: name}];
            ctl_cmbParent.setValues(item, false);
        }
    }
    else
        Ext.Msg.alert('提示信息', '不能为已停用的类添加子类！');
}

/*
@private
清除编辑区域的所有控件的值
*/
function fnClearEditArea() {
    var ctl_hddID = Ext.getCmp('hddID');
    var ctl_txtCode = Ext.getCmp('txtCode');
    var ctl_txtName = Ext.getCmp('txtName');
    var ctl_txtZJM = Ext.getCmp('txtZJM');
    var ctl_cmbStatus = Ext.getCmp('cmbStatus');
    var ctl_cmbParent = Ext.getCmp('cmbParent');
    var ctl_nmbNO = Ext.getCmp('nmbNO');
    var ctl_guid = Ext.getCmp('txtGUID');
    //var ctl_txtImageUrl = Ext.getCmp('txtImageUrl');
    ctl_hddID.setValue(null);
    ctl_txtCode.setValue(null);
    ctl_txtName.setValue(null);
    ctl_txtZJM.setValue(null);
    ctl_cmbStatus.setValue(null);
    ctl_cmbParent.setValue(null);
    ctl_nmbNO.setValue(null);
    //ctl_txtImageUrl.setValue(null);
    ctl_guid.setValue(null);

    Ext.getCmp("btnSave").show();
}

/*
@private
[上下文菜单项 - 停用]的点击事件处理
*/
function fnCtnMenuItemDelete_Click() {
    var menu = Ext.getCmp('ctnMenu');
    var record = menu.__current_record;
    fnToggleItemCategoryStatus(record);
}

/*
@private 
停用商品分类
*/
function fnToggleItemCategoryStatus(record, status) {
    //数据检查
    var parentId = record.get('parentId');
    var children = record.get('children');
    var itemCategoryStore = Ext.getStore('itemCategoryStore');

    if (status == -1) {
        if ((parentId == null || parentId == '' || parentId == 'root') && (record.get('isFirst'))) {
            Ext.Msg.alert('提示信息', '根节点[' + record.get('text') + ']不能被停用.');
            return;
        }

        if (children != null && children.length > 0) {
            for (var i = 0; i < children.length; i++) {
                var id = children[i]['id'];
                var detailData = itemCategoryStore.getById(id);
                if (detailData.get("Status") != "-1") {
                    Ext.Msg.alert('提示信息', '当前商品分类包含子分类,请首先停用所有的子分类.');
                    return;
                }
            }
        }
    }

    //提交给服务器停用
    var mask = new Ext.LoadMask(Ext.getBody(), {
        msg: '正在向服务器提交,请稍等...'
    });
    mask.show();
    Ext.Ajax.request({
        url: 'Handler/ItemCategoryHandler.ashx?method=toggleStatus&id=' + record.get('id') + "&status=" + status
        , method: 'POST'
        , callback: function (options, success, response) {
            //移除树自身的节点
            //record.remove(true);

            Ext.getCmp("trpItemCategoryTree").on("load", function () { Ext.getStore('itemCategoryTreeStore').getNodeById(record.get("id")).parentNode.expand(); });

            Ext.getStore('itemCategoryTreeStore').load();
            Ext.getStore('itemCategoryStore').load();

            //console.log(record.get("id"), Ext.getStore('itemCategoryTreeStore').getNodeById(record.get("id")));

            var jdata = Ext.JSON.decode(response.responseText);
            if (jdata.success) {
                Ext.Msg.show({
                    title: '提示',
                    msg: '操作成功',
                    buttons: Ext.Msg.OK,
                    icon: Ext.Msg.INFO
                    //                    fn: function () {
                    //                        fnSearch();
                    //                    }
                });
            } else {
                Ext.Msg.show({
                    title: '错误',
                    msg: jdata.msg,
                    buttons: Ext.Msg.OK,
                    icon: Ext.Msg.ERROR
                });
            }

            mask.hide();
        }
    });
}

function ToggleButtonText(status) {
    if (status == -1)
        Ext.getCmp("btnDelete").setText("启用");
    else
        Ext.getCmp("btnDelete").setText("停用");
}