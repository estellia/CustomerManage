<%@ Page Title="" Language="C#" MasterPageFile="~/Framework/MasterPage/optimize.Master"
    AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>配送方式</title>
    <link href="<%=StaticUrl+"/module/dispatching/css/style.css?v=0.7"%>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



<div class="dispatchingListArea" id="section" data-js="js/dispatchingList.js?v1.0">
    <div class="tableWrap cursorDef" >
        <div id="dispatchingList">
        	<div class="loading" style="padding-top:40px;">
              <span><img src="../static/images/loading.gif"></span>
            </div>
        </div>
        <p class="tip-payment">至少配置一种配送方式，否则无法完成配送环节。</p>
    </div>
</div>
<div id="kkpager" style="padding-right:35px;text-align:right;"></div>

<!-- 遮罩层 -->
<div class="jui-mask"></div>
<!-- 弹出，送货到家 -->
<div id="jui-dialog-1" class="jui-dialog jui-dialog-dispatching">
	<div class="jui-dialog-tit">
    	<h2>送货到家</h2>
        <span class="jui-dialog-close"></span>
    </div>
    <div class="dispatchingContent">
    	<div class="commonSelectWrap">
            <em class="tit">是否启用:</em>
            <div class="radioWrap">
               <p class="radioBox startUs">启用</p>
               <p class="radioBox unstartUs">停用</p>
            </div>
        </div>
        
        <div class="commonSelectWrap" style="height:100px">
            <em class="tit"><span class="fontRed">*</span> 配送费描述:</em>
            <p class="searchInput" style="height:100px"><textarea class="formInputBox" id="dispatching_describe" placeholder="例如：每笔订单消费满**元，本店包邮，除港澳台地区、内蒙古、新疆、西藏地区除外。每笔订单**元以下，快递费**元；**元以上，免快递费。" style="width:100%;height:100%;"></textarea></p>
        </div>
        <div class="commonSelectWrap">
            <em class="tit"><span class="fontRed">*</span>默认配送费:</em>
            <p class="searchInput"><input class="formInputBox" id="dispatching_cost" type="text" value="" /></p><span style="display:inline-block;line-height:34px;padding-left:10px;font-size:16px;color:#ccc;">元</span>
        </div>
		<div class="commonSelectWrap">
            <em class="tit">免配送费最低订单金额:</em>
            <p class="searchInput"><input class="formInputBox" id="dispatching_mincost" type="text" value="" /></p><span style="display:inline-block;line-height:34px;padding-left:10px;font-size:16px;color:#ccc;">元</span>
        </div>
        
        <div class="btnWrap">
        	<a href="javascript:;" class="commonBtn saveBtn">保存</a>
            <a href="javascript:;" class="commonBtn cancelBtn">取消</a>
        </div>
    </div>
</div>



<!-- 弹出，到店自提 -->
<div id="jui-dialog-2" class="jui-dialog jui-dialog-dispatching" style="width:580px;min-height:200px;margin-left:-290px;">
	<div class="jui-dialog-tit">
    	<h2>到店自提</h2>
        <span class="jui-dialog-close"></span>
    </div>
    <div class="dispatchingContent">
    	<div class="commonSelectWrap">
            <em class="tit">是否启用：</em>
            <div class="radioWrap">
                <p class="radioBox startUs" date-type="1">启用</p>
                <p class="radioBox unstartUs" date-type="2">停用</p>
            </div>
        </div>
        
        <div class="commonSelectWrap">
            <em class="tit">备货期：</em>
            <p class="searchInput" style="width:120px;"><input class="formInputBox" id="dispatching_stockup" type="text" value="0"></p>
            <span class="tip-text">小时</span>
        </div>
        
        <div class="commonSelectWrap deliveryNumBox">
            <em class="tit">提货设置：</em>
            <span class="checkBox" date-type="1"></span>
            <span class="tip-text">可提货天数</span>
            <p class="searchInput" style="width:100px;">
            <input class="formInputBox" id="dispatching_pickup" type="text" value="" readonly="readonly" /></p>
            <span class="tip-text">天  用户可选择X天中任意一天提货</span>
        </div>
        
        <div class="commonSelectWrap" style="height:auto">
            <em class="tit"></em>
            <div class="timePassageArea">
            	<span class="checkBox" date-type="2"></span>
            	<p class="">提货时间段</p>
                <div class="setTimeBox">
                    <div class="timePassage">
                    	<!--
                        <div class="timeItem">
                            <p><span class="startTime">10:00</span> 至 <span class="endTime">14:00</span></p>
                            <span class="editBtn">修改</span>
                            <span class="removeBtn">删除</span>
                        </div>
                        <div class="timeItem">
                            <p><span class="startTime">10:00</span> 至 <span class="endTime">14:00</span></p>
                            <span class="editBtn">修改</span>
                            <span class="removeBtn">删除</span>
                        </div>
                        -->
                    </div>
                    
                    <div class="addTimePassageBox" style="display:none">
                        <p class="searchInput" style="width:100px;"><input class="formInputBox" id="dispatching_startTime"  type="text" /></p>
                        <span class="tip-text">至</span>
                        <p class="searchInput" style="width:100px;"><input class="formInputBox" id="dispatching_endTime" type="text" /></p>
                        <span class="timeSaveBtn">保存</span>
                        <span class="timeCancelBtn">取消</span>
                    </div>
                    
                    <a href="javascript:;" id="addTimeBtn" class="commonBtn w80">新增</a>
                </div>
            </div>
        </div>

        <div class="btnWrap">
        	<a href="javascript:;" class="commonBtn saveBtn">保存</a>
            <a href="javascript:;" class="commonBtn cancelBtn">取消</a>
        </div>
    </div>
</div>


<!-- 弹出，到店服务 -->
<div id="jui-dialog-4" class="jui-dialog jui-dialog-dispatching" style="width:580px;min-height:200px;margin-left:-290px;">
	<div class="jui-dialog-tit">
    	<h2>到店服务</h2>
        <span class="jui-dialog-close"></span>
    </div>
    <div class="dispatchingContent">
    	<div class="commonSelectWrap">
            <em class="tit">是否启用：</em>
            <div class="radioWrap">
                <p class="radioBox startUs" date-type="1">启用</p>
                <p class="radioBox unstartUs" date-type="2">停用</p>
            </div>
        </div>
        
        <div class="commonSelectWrap">
            <em class="tit">需要提前：</em>
            <p class="searchInput" style="width:120px;"><input class="formInputBox" id="dispatching_stockuper" type="text" value="0"></p>
            <span class="tip-text">天预约</span>
        </div>
        
        <div class="commonSelectWrap deliveryNumBox textColor">
            提货设置输入0天表示可预约当日，输入1天表示从次日开始接受预约
            <%--<span class="checkBox" date-type="1"></span>
            <span class="tip-text">可提货天数</span>
            <p class="searchInput" style="width:100px;">
            <input class="formInputBox" id="dispatching_pickup" type="text" value="" readonly="readonly" /></p>
            <span class="tip-text">天  用户可选择X天中任意一天提货</span>--%>
        </div>

        <div class="commonSelectWrap">
            <em class="tit">营业时间：</em>
            <div class="radioWrap">
                <p class="radioBoxer custom on" date-type="0">自定义</p>
                <p class="radioBoxer oneDay" date-type="1">24小时</p>
            </div>
        </div>
        
        <div class="commonSelectWrap timeWrap" style="height:auto">
            <em class="tit"></em>
            <div class="timePassageArear">
                <div class="setTimeBox">
                    <div class="addTimePassageBoxer" style="display:block">
                        <p class="searchInput" style="width:100px;"><input class="formInputBox" id="dispat_startTime"  type="text" /></p>
                        <span class="tip-text">至</span>
                        <p class="searchInput" style="width:100px;"><input class="formInputBox" id="dispat_endTime" type="text" /></p>
                    </div>
                </div>
            </div>
        </div>

        <div class="btnWrap">
        	<a href="javascript:;" class="commonBtn saveBtn">保存</a>
            <a href="javascript:;" class="commonBtn cancelBtn">取消</a>
        </div>
    </div>
</div>


<script id="tpl_dispatchingList" type="text/html">
     <#for(var i=0;i<list.length;i++){ var item=list[i];#>
        	<tr>
				<td><#=item.deliveryName#></td>
				<#if(item.IsOpen){#>
					<td class="unstart">已启用</td>
				<#}else{#>
					<td class="unstart blue">未启用</td>
				<#}#>
				<td class="operateWrap" title="编辑" data-typeid="<#=item.deliveryId#>" >
					<span class="editIcon"></span>
				</td>
			</tr>
    <#}#>
</script>

</asp:Content>
