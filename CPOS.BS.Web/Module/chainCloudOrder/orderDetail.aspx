﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Framework/MasterPage/optimize.Master"
    AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>订单详情</title>

    <link href="<%=StaticUrl+"/module/chainCloudOrder/css/style.css?v=0.6"%>" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <div class="allPage" id="section" data-js="js/orderDetail.js">
            <!-- 内容区域 -->
            <div class="contentArea_vipquery">
                <!--个别信息查询-->
                <div class="queryTermArea" id="simpleQuery" style="display: inline-block; width: 100%;  padding: 0px;  min-height: 60px;  border-bottom: 1px solid #e1e7ea;">
                     <div class="optionBtn " style="display: none">
                    <!-- <div class="commonBtn DeliveryName1" data-status="500"  data-showstaus="100,900" >审核通过</div>
                     <div class="commonBtn DeliveryName2"  data-status="510"  data-showstaus="100,900" data-flag="到店自提" >审核通过</div>
                     <div class="commonBtn" data-status="900" data-showstaus="100" >审核不通过</div>

                   <div class="DeliveryName2" data-flag="到店自提">
                     &lt;!&ndash;<div class="commonBtn" data-status="510" data-showstaus="410" >备货完成</div>&ndash;&gt;
                     <div class="commonBtn" data-status="610"  data-showstaus="'510'">提货</div>
                    </div>
                    <div class="DeliveryName1" data-flag="送货上门">
                     <div class="commonBtn" data-status="600" data-showstaus="'500'">确认发货</div>
                      </div>

                      <div class="commonBtn" data-status="800" data-showstaus="100,500,410,500,510,900"  >取消</div>

                     <div class="commonBtn" data-status="700" data-showstaus="600,610">完成</div>

-->
                     <div class="commonBtn" data-status="10000" data-optType="payment">收款</div>

                     <div class="commonBtn printBtn" data-optType="print1" >打印拣货单</div>
                      <div class="commonBtn printBtn" data-optType="print2" >打印配送单</div>
                      <!--<div class="commonBtn" data-optType="download" >下载配送单</div>-->
                     </div>
                </div>

            </div>
             <div class="tablewrap">
             <form></form>
             <form id="orderInfo">
                <div class="panlDiv">
                 <div class="title">订单信息</div>
                 <div class="panlText">
                     <div class="panlL">
                      <div class="commonSelectWrap">
                         <em class="tit">订单编号：</em>
                         <div class="searchInput">
                          <input type="text" readonly="readonly" class="easyui-validatebox" data-options="disabled:true" name="order_no"/>
                         </div>
                         
                      </div>
                      <div class="commonSelectWrap">
                         <em class="tit">订单状态：</em>
                         <div class="searchInput rowStatusStyle">
                          <input type="text"    readonly="readonly" class="easyui-validatebox" data-options="disabled:true" name="Field10"/>
                         </div>
                      </div>
                      <div class="commonSelectWrap">
                         <em class="tit">下单时间：</em>
                         <div class="searchInput">
                          <input type="text"   readonly="readonly" class="easyui-validatebox" data-options="disabled:true" name="order_date"/>
                         </div>
                      </div>
                      <div class="commonSelectWrap">
                         <em class="tit">订单渠道：</em>
                         <div class="searchInput">
                          <input type="text"   readonly="readonly" class="easyui-validatebox" data-options="disabled:true" name="data_from_name"/>
                         </div>
                      </div>
                      <div class="commonSelectWrap">
                         <em class="tit">服务门店：</em>
                         <div class="searchInput">
                          <input type="text"   readonly="readonly" class="easyui-validatebox" data-options="disabled:true" name="create_unit_name"/>
                         </div>
                      </div>
                      
                      <div class="commonSelectWrap">
                         <em class="tit">服务人员：</em>
                         <div class="searchInput rowStatusStyle">
                          <input type="text"   readonly="readonly" class="easyui-validatebox" data-options="disabled:true" name="sales_user_name"/>
                         </div>
                      </div>
                      
                      <div class="commonSelectWrap">
                           <div class="tit">备注：</div>
                           <div class="searchInput">
                              <textarea type="text"  readonly="readonly"  class="easyui-validatebox" data-options="disabled:true" name="remark"> </textarea>
                            </div>
                       </div>

                       <%--提货时间：--%>
                       <div class="commonSelectWrap">
                           <div class="tit serviceTime"></div>
                           <div class="searchInput">
                              <input type="text"   readonly="readonly" class="easyui-validatebox" data-options="disabled:true" name="ReserveTime"/>
                           </div>
                       </div>
                      
                     </div> <!--panl   END-->
                     
                 </div><!--clearfix END-->
                 </div><!--panlDiv订单信息  END-->
                <div class="panlDiv DeliveryInformation">
                 <div class="title">提货人信息</div>
                 <div class="panlText">
                     <div class="panlL">
                      <div class="commonSelectWrap">
                         <em class="tit">姓名：</em>
                         <div class="searchInput">
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"   readonly="readonly" name="Field14"/>
                         </div>
                      </div>
                      <div class="commonSelectWrap">
                         <em class="tit">手机号：</em>
                         <div class="searchInput">
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="Field6"/>
                         </div>
                      </div>
                      <%--<div class="commonSelectWrap">
                         <em class="tit">详细地址:</em>
                         <div class="searchInput" style=" width: 459px">
                          <input type="text"  class="easyui-validatebox" data-options="width:459,disabled:true"  readonly="readonly" name="Field4"/>
                         </div>
                      </div>--%>

                     </div><!--panl   END-->

                 </div><!--clearfix END-->
                 </div><!--panlDiv 会员信息 END-->
                  <div class="panlDiv ConsigneeInformation" style="display:none">
                 <div class="title">收件人信息</div>
                 <div class="panlText">
                     <div class="panlL">
                      <div class="commonSelectWrap">
                         <em class="tit">姓名：</em>
                         <div class="searchInput">
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"   readonly="readonly" name="Field14"/>
                         </div>
                      </div>
                      <div class="commonSelectWrap">
                         <em class="tit">手机号：</em>
                         <div class="searchInput">
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="Field6"/>
                         </div>
                      </div>
                      <div class="commonSelectWrap">
                         <em class="tit">详细地址：</em>
                         <div class="searchInput" style=" width: 459px">
                          <input type="text"  class="easyui-validatebox" data-options="width:459,disabled:true"  readonly="readonly" name="Field4"/>
                         </div>
                      </div>

                     </div><!--panl   END-->

                 </div><!--clearfix END-->
                 </div><!--panlDiv 会员信息 END-->
                <div class="panlDiv">
                 <div class="title">支付信息</div>
                 <div class="panlText">
					 <!--
                     <div style="display:none">
                     	<div class="commonSelectWrap">
                             <em class="tit">订单金额(元):</em>
                             <div class="searchInput">
                              <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="total_amount"/>
                             </div>
                          </div>
                          
                          <div class="commonSelectWrap">
                             <em class="tit">积分抵扣:</em>
                             <div class="searchInput wh80">
                              <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="pay_pointsAmount"/>
                             </div>
                          </div>
                          
                          <div class="commonSelectWrap">
                             <em class="tit">优惠券抵扣(元): </em>
                             <div class="searchInput wh80">
                              <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="couponAmount"/>
                             </div>
                          </div>
                          <div class="commonSelectWrap">
                             <em class="tit">返现抵扣(元):</em>
                             <div class="searchInput wh80">
                              <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="ReturnAmount" value=""/>
                             </div>
                          </div>
                         <div class="commonSelectWrap">
                              <em class="tit">会员折扣(元):</em>
                              <div class="searchInput wh80">
                               <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="memberPrice" value=""/>
                              </div>
                         </div>
                         
                         <div class="commonSelectWrap">
                             <em class="tit">余额支付(元):</em>
                             <div class="searchInput wh80">
                              <input type="text"  class="easyui-validatebox"   readonly="readonly" data-options="disabled:true" name="vipEndAmount"/>
                             </div>
                         </div>
                     </div>
                     -->
                      <div class="actuallyDetailBox">	
                          <div class="commonSelectWrap">
                             <em class="tit"><div class="Paystatus" style="display: inline-block;">实付金额：</div><span style="color: #00a0e8;">￥</span></em>
                             <div class="searchInput" style="margin-left:-3px;">
                              <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="actual_amount" style="color:#00a0e8;"/>
                             </div>
                          </div>
                          <div class="actuallyDetail">
                            <a href="javascript:;" class="PaystatusDetail">实付金额明细</a>
                            <span class="arrIcon"></span>
                            <div class="otherInfoBox">
                            	<div class="commonSelectWrap" style="margin:10px 0">
                                     <em class="tit">商品总金额：</em>
                                     <div class="searchInput">
                                     <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="total_retail"/>
                                     </div>
                                  </div>
                                  <div class="infoBox">
                                      <div class="commonSelectWrap">
                                         <em class="tit">优惠券抵扣： </em>
                                         <div class="searchInput wh80">
                                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="couponAmount"/>
                                         </div>
                                      </div>
                                      <div class="commonSelectWrap">
                                         <em class="tit">积分抵扣：</em>
                                         <div class="searchInput wh80">
                                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="pay_pointsAmount"/>
                                         </div>
                                      </div>
                                      <div class="commonSelectWrap" style="display:none">
                                         <em class="tit">返现抵扣：</em>
                                         <div class="searchInput wh80">
                                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="ReturnAmount" value=""/>
                                         </div>
                                      </div>
                                      <div class="commonSelectWrap">
                                         <em class="tit">余额抵扣：</em>
                                         <div class="searchInput wh80">
                                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="vipEndAmount" value=""/>
                                         </div>
                                      </div>
                                      <div class="commonSelectWrap">
                                          <em class="tit">会员折扣：</em>
                                          <div class="searchInput wh80">
                                           <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="memberPrice" value=""/>
                                          </div>
                                     </div>
                                     <div class="commonSelectWrap">
                                          <em class="tit">配送费用：</em>
                                          <div class="searchInput wh80">
                                           <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="DeliveryAmount" value=""/>
                                          </div>
                                     </div>
                                 </div>
                                 <div class="commonSelectWrap" style="margin:10px 0;">
                                      <em class="tit Paystatus" style="font-size:16px">实付金额：</em>
                                      <div class="searchInput wh80">
                                       <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="actual_amount" value=""  style="font-size:16px">
                                      </div>
                                 </div>
                                 
                            </div>
                          </div>
                      </div>
                      
                      <div class="commonSelectWrap">
                         <em class="tit">支付状态：</em>
                         <div class="searchInput wh80 rowStatusStyle">
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="paystatus"/>
                         </div>
                      </div>
                      
                      <div class="commonSelectWrap">
                         <em class="tit">支付方式：</em>
                         <div class="searchInput wh80" >
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="payment_name"/>
                         </div>
                      </div>
                      
                      <div class="commonSelectWrap">
                         <em class="tit">商户订单号：</em>
                         <div class="searchInput wh80" >
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="paymentcenter_id"/>
                         </div>
                      </div>
                      
                 </div><!--clearfix END-->
                 </div><!--panlDiv 支付信息  END-->
                 
                 
                 <div class="panlDiv">
                     <div class="title">发票信息</div>
                     <div class="panlText">
                          <div class="commonSelectWrap" style="width:459px">
                             <em class="tit">发票信息：</em>
                             <div class="searchInput">
                              <input type="text"  class="easyui-validatebox" data-options="disabled:true" readonly="readonly" name="Field19"/>
                             </div>
                          </div>
                     </div> 
                </div>          
                 
                 
                 
                <div class="panlDiv">
                 <div class="title">配送信息
                 <div class="updateBtn" style="display:none;">
                    <div class="commonBtn" data-flag="update" data-type="gropupdate">修改</div>
                    <div class="commonBtn" data-flag="save"   data-type="groupsubmit">保存</div>
                    <div class="commonBtn" data-flag="cancel" data-type="groupsubmit">取消</div>
                 </div>
                </div>
                 <div data-type="gropupdate" class="panlText">
                      <div class="commonSelectWrap">
                         <em class="tit">配送方式：</em>
                         <div class="searchInput">
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true" readonly="readonly" name="DeliveryName"/>
                         </div>
                      </div>
                      <div class="DeliveryName1" data-datainfo="送货到家" style="display: none">
                      <div class="commonSelectWrap" id="deliveryExpressBox">
                         <em class="tit">配送快递：</em>
                         <div class="searchInput">
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="carrier_name"/>
                         </div>
                      </div>
                      <div class="commonSelectWrap" id="expressNumBox">
                         <em class="tit">快递单号：</em>
                         <div class="searchInput">
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"  readonly="readonly" name="Field2"/>
                         </div>
                      </div>
                      <div class="commonSelectWrap">
                         <em class="tit">快递费(元)：</em>
                         <div class="searchInput">
                          <input type="text"  class="easyui-validatebox" data-options="disabled:true"   readonly="readonly"name="DeliveryAmount"/>
                         </div>
                       </div>
                      
                       </div>
                       <div class="DeliveryName2" data-datainfo="到店自提"  style="display: none">
                           <div class="commonSelectWrap">
                              <em class="tit">服务门店：</em>
                              <div class="searchInput">
                               <input type="text"   class="easyui-validatebox"   readonly="readonly" data-options="disabled:true" name="purchase_unit_name"/>
                              </div>
                           </div>

                           <div class="commonSelectWrap">
                              <em class="tit">门店电话：</em>
                              <div class="searchInput">
                               <input type="text"  class="easyui-validatebox"   readonly="readonly"data-options="disabled:true" name="unit_tel"/>
                              </div>
                           </div>
                           <div class="commonSelectWrap">
                              <em class="tit">门店地址：</em>
                              <div class="searchInput">
                               <input type="text"  class="easyui-validatebox"   readonly="readonly"data-options="disabled:true" name="unit_address"/>
                              </div>
                           </div>
                       </div>
                  </div>
                  <form></form>
                 <form  data-type="groupsubmit" id="optionDelivery" class="panlText forminput" style="display: none">
                      <div class="commonSelectWrap">
                         <em class="tit">配送方式：</em>
                         <div class="searchInput bordrnone">
                          <input type="text" id="DeliveryType"  class="easyui-combobox" data-options="width:160,height:32" name="DeliveryType"/>
                         </div>
                      </div>
                      <div  data-flag="送货到家" style="display: none">
                      <div class="commonSelectWrap">
                         <em class="tit">收件人：</em>
                         <div class="searchInput">
                          <input type="text"  class="easyui-validatebox" placeholder="请输入真实姓名" data-options="width:160,height:32" name="ReceiveMan"/>
                         </div>
                      </div>
                      <div class="commonSelectWrap">
                         <em class="tit">手机：</em>
                         <div class="searchInput">
                          <input type="text"    class="easyui-validatebox" placeholder="请输入手机号码" data-options="width:160,height:32" name="Phone"/>
                         </div>
                      </div>

                      <div class="commonSelectWrap"  id="deliveryExpressBox2">
                         <em class="tit">配送快递：</em>
                         <div class="searchInput bordrnone">
                          <input type="text" id="carrier"  class="easyui-combobox" data-options="width:160,height:32"  name="Carrier_id"/>
                         </div>
                      </div>
                      <div class="commonSelectWrap" id="expressNumBox2">
                         <em class="tit">快递单号：</em>
                         <div class="searchInput">
                          <input type="text"  class="easyui-validatebox"   name="DeliveryCode"/>
                         </div>
                      </div>
						
                      <div class="commonSelectWrap">
                         <em class="tit">地址：</em>
                         <div class="searchInput" style="width: 459px;">
                          <input type="text"  class="easyui-validatebox"  placeholder="请输入有效地址" data-options="width:459" name="Addr"/>
                         </div>
                      </div>
                      </div>
                       <div data-flag="到店自提"  style="display: none">
                           <div class="commonSelectWrap">
                              <em class="tit">服务门店：</em>
                              <div class="selectBox">
                               <input type="text" id="setUnitId"  class="easyui-combotree"  data-options="width:160,height:32" name="purchase_unit_id"/>
                              </div>
                           </div>

                           <div class="commonSelectWrap">
                              <em class="tit">门店电话：</em>
                              <div class="searchInput">
                               <input type="text" id="unitphone"  class="easyui-validatebox"   readonly="readonly" data-options="disabled:true" name="Phone"/>
                              </div>
                           </div>
                           <div class="commonSelectWrap">
                              <em class="tit">门店地址：</em>
                              <div class="searchInput">
                               <input type="text" id="UnitAddr"   class="easyui-validatebox"   readonly="readonly" data-options="disabled:true" name="Addr"/>
                              </div>
                           </div>
                       </div>
                 </form>


                </div>  <!--panlDiv 配送信息  END-->
            </form>

                <div class="panlDiv cursorDef">
                 <div class="title">商品信息</div>
                 <div class="panlText" id="tableWrap" style="margin-left: 0px;">
                  <div class="table" id="commodity"></div>

                 </div>
                </div>  <!--panlDiv 商品信息  END-->
                 <div class="panlDiv cursorDef" >
                                 <div class="title">订单操作记录
                                 </div>
                                 <div class="panlText" style="margin-left: 0px;">
                                  <div class="table" id="StatusList"></div>

                                 </div>
                                </div>  <!--panlDiv 商品信息  END-->
             </div>

        </div>
       <div style="display: none">
      <div id="win" class="easyui-window" data-options="modal:true,shadow:false,collapsible:false,minimizable:false,maximizable:false,closed:true,closable:true" >
      		<div class="easyui-layout" data-options="fit:true" id="panlconent">

      			<div data-options="region:'center'" style="padding:10px;">
      				指定的模板添加内容
      			</div>
      			<div class="btnWrap" id="btnWrap" data-options="region:'south',border:false" style="height:80px;text-align:center;padding:5px 0 0;">
      				<a class="easyui-linkbutton commonBtn saveBtn" >确定</a>
      				<a class="easyui-linkbutton commonBtn cancelBtn"  href="javascript:void(0)" onclick="javascript:$('#win').window('close')" >取消</a>
      			</div>
      		</div>

      	</div>
      </div>

        <!--收款-->
         <script id="tpl_OrderPayMent" type="text/html">
            <form id="orderOption">


            <div class="optionclass">
               <div class="commonSelectWrap amount">
                             <em class="tit w120">订单金额：</em>
                                <div class=" selectBox borderNone" >
                                 <input id="Amount" class=" easyui-numberbox " data-options="width:160,height:32, min:0,precision:2,disabled:true" name="actual_amount" readonly="readonly" style="border:none"/>
                               </div>
                </div>

                <div class="commonSelectWrap PayMethod">
                                        <em class="tit w120">付款方式：</em>
                                        <div class="selectBox bodernone" >
                                               <input id="PayMethod"  class="easyui-combobox" data-options="width:160,height:32,required:true"  name="PayMethod" />
                                       </div>
                </div>
                <div class="commonSelectWrap CheckResult">
                                        <em class="tit w120">审核不通过理由：</em>
                                        <div class="selectBox bodernone" >
                                               <input id="CheckResult"  class="easyui-combobox" data-options="width:160,height:32,required:true"  name="CheckResult" />
                                       </div>
                </div>

                <div class="commonSelectWrap DeliverCompany">
                                        <em class="tit w120">配送商：</em>
                                        <div class="selectBox bodernone" >
                                               <input id="DeliverCompany"  class="easyui-combobox" data-options="width:160,height:32,required:true"  name="DeliverCompany" />
                                       </div>
                </div>
                  <div class="commonSelectWrap DeliverOrder">
                                             <em class="tit w120">配送单号：</em>
                                                <div class="searchInput" >
                                                 <input class="easyui-validatebox" data-options="width:160,height:32" name="DeliverOrder" style="border:none"/>
                                               </div>
                                </div>

               </div>
                      <div class="commonSelectWrap">
                       <div class="tit w120">备注：</div>
                       <div class="searchInput" style="width:457px; height: 100px;">

                          <textarea type="text" id="Remark" style="width:457px; height: 100px;line-height:14px;"  class="easyui-validatebox" data-options="required:true"  name="Remark"></textarea>
                         </div>

                      </div>
                </form>
                </script>

</asp:Content>
