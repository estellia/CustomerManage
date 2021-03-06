﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="JIT.CPOS.Web.RateLetterInterface.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../Javascript/jquery-1.9.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //绑定事件
            $("#btnTest").click(fnTestClick);
        });
        //登录 Login
        //用户列表 GetUsers
        function fnTestClick() {
            //var req = "{\"CustomerID\":\"e703dbedadd943abacf864531decdac1\",\"Parameters\":{\"Email\":\"1111@132.com\",\"Password\":\"202cb962ac59075b964b07152d234b70\"}}";
            //var req = "{\"UserID\":\"6ca19df26ad749beb7f6244b9a033451\",\"CustomerID\":\"e703dbedadd943abacf864531decdac1\",\"Parameters\":{\"PageIndex\":0,\"PageSize\":15 }}";

            //群组 CreateGroup
            //var req = "{\"UserID\":\"6ca19df26ad749beb7f6244b9a033451\",\"CustomerID\":\"e703dbedadd943abacf864531decdac1\",\"Parameters\":{\"ChatGroupID\":\"B6C63F16-5EAD-48A0-BA24-4D33D8077C11\",\"GroupName\":\"测试组5\",\"Description\":\"描述\",\"Telephone\":\"12345678901\"}}";

            //群组加人  AddUserGroup
            //var req = "{\"UserID\":\"6ca19df26ad749beb7f6244b9a033451\",\"CustomerID\":\"e703dbedadd943abacf864531decdac1\",\"Parameters\":{\"ChatGroupID\":\"B6C63F16-5EAD-48A0-BA24-4D33D8077C11\",\"UserIDList\":[\"076438440D444D0EB8F64E74F9619319\",\"1D2EFD0B19234B3AA1B638536244B1FE\"]}}";

            //群组列表GetGroupList
            //var req = "{\"UserID\":\"6ca19df26ad749beb7f6244b9a033451\",\"CustomerID\":\"e703dbedadd943abacf864531decdac1\",\"Parameters\":{\"GroupName\":\"测试\",\"PageIndex\":0,\"PageSize\":15}}";

            //群组踢人  DelUserGroup
            // var req = "{\"UserID\":\"6ca19df26ad749beb7f6244b9a033451\",\"CustomerID\":\"e703dbedadd943abacf864531decdac1\",\"Parameters\":{\"ChatGroupID\":\"B6C63F16-5EAD-48A0-BA24-4D33D8077C11\",\"UserIDList\":[\"076438440D444D0EB8F64E74F9619319\",\"1D2EFD0B19234B3AA1B638536244B1FE\"]}}";

            //删除群组DeleteGroup
            //var req = "{\"UserID\":\"6ca19df26ad749beb7f6244b9a033451\",\"CustomerID\":\"e703dbedadd943abacf864531decdac1\",\"Parameters\":{\"ChatGroupID\":\"B6C63F16-5EAD-48A0-BA24-4D33D8077C11\"}}";

            var strUrl = $("#txtUrl").val();
            strUrl = strUrl + "?type=" + $("#cmbType").val();
            strUrl = strUrl + "&action=" + $("#cmbAction").val();
            var req = $("#areaReq").val();
            $("#areaUrl").val(strUrl + "&req=" + req);

            $.ajax({
                //url: "User/UserHandler.ashx?action=GetUsers&type=Product",
                url: strUrl, // "Group/GroupHandler.ashx?action=CreateGroup&type=Product",
                data: {
                    "req": req
                },
                type: "POST",
                success: function (data) {
                    if (data) {
                        //alert(data);
                        $("#areaJson").val(data)
                    }
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    例如：http://localhost:23130/RateLetterInterface/Group/GroupHandler.ashx
    <div>
        commUrl:<input type="text" id="txtUrl" style="width: 100%" value="http://localhost:23130/RateLetterInterface/Group/GroupHandler.ashx" />
    </div>
    <div>
        Type：<select id="cmbType">
            <option>Product</option>
            <option>Project</option>
        </select>
        Action:<select id="cmbAction">
            <option>Login</option>
            <option>GetUsers</option>
            <option>CreateGroup</option>
            <option>AddUserGroup</option>
            <option>GetGroupList</option>
            <option>DelUserGroup</option>
            <option>DeleteGroup</option>
            <option>GetGroupUsers</option>
            <option>GetIMGroupsInfo</option>
            <option>UpdateIMGroupInfo</option>
            <option>UserQuitGroup</option>
             <option>UserApplyJoinGroup</option>
        </select></div>
    req：<div>
        <textarea id="areaReq" cols="200" rows="3"></textarea>
    </div>
    url：<div>
        <textarea id="areaUrl" cols="200" rows="3"></textarea>
    </div>
    <div>
        <input type="button" value="接口测试" id="btnTest" />
    </div>
    return JSON:
    <div id="dvJson">
        <textarea id="areaJson" cols="200" rows="20"></textarea>
    </div>
    </form>
</body>
</html>
