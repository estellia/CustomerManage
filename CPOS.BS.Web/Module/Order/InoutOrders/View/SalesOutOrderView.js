﻿function InitView() {
    //    //debugger;
    Ext.create('Ext.form.Panel', {
        id: 'searchPanel',
        layout: {
            type: 'table',
            columns: 4
        },
        renderTo: 'span_panel',
        padding: '10 0 0 0',
        bodyStyle: 'background:#F1F2F5;',
        border: 0,
        items: [

        {
            xtype: "jittextfield",
            fieldLabel: "单据号码",
            id: "txtOrderNo",
            name: "order_no",
            jitSize: 'small'
        },

        {
            xtype: "jitbizcustomerunit",
            fieldLabel: "客户",
            id: "txtPurchaseUnit",
            name: "purchase_unit_id",
            jitSize: 'small'
        },
        {
            xtype: "jitbizunitselecttree",
            fieldLabel: "销售单位",
            id: "txtSalesUnit",
            name: "sales_unit_id",
            jitSize: 'small'
//            ,isDefault: false
//            ,allowBlank: false
        },

        {
            xtype: "jitbizwarehouse",
            fieldLabel: "仓库",
            id: "txtSalesWarehuouse",
            name: "warehouse_id",
            jitSize: 'small'
            , parent_id: "txtSalesUnit"
        },
        {
            xtype: 'panel',
            colspan: 2,
            layout: 'hbox',
            border: 0,
            bodyBorder: false,
            bodyStyle: 'background:#F1F2F5;',
            id: 'txtOrderDate',
            hidden: true,
            items: [
                {
                    xtype: "jitdatefield",
                    fieldLabel: "单据日期",
                    id: "txtOrderDateBegin",
                    name: "order_date_begin",
                    jitSize: 'small'
                },
                {
                    xtype: "label",
                    text: "至"
                },
                {
                    xtype: "jitdatefield",
                    fieldLabel: "",
                    id: "txtOrderDateEnd",
                    name: "order_date_end",
                    jitSize: 'small',
                    width: 100
                }
            ]
        },
        {
            xtype: 'panel',
            colspan: 2,
            layout: 'hbox',
            border: 0,
            bodyBorder: false,
            bodyStyle: 'background:#F1F2F5;',
            id: 'txtCompleteDate',
            hidden: true,
            items: [
                {
                    xtype: "jitdatefield",
                    fieldLabel: "出库日期",
                    id: "txtCompleteDateBegin",
                    name: "complete_date_begin",
                    jitSize: 'small'
                },
                {
                    xtype: "label",
                    text: "至"
                },
                {
                    xtype: "jitdatefield",
                    fieldLabel: "",
                    id: "txtCompleteDateEnd",
                    name: "complete_date_end",
                    jitSize: 'small',
                    width: 100
                }
            ]
            },
        {
            xtype: "jitbizorderstatus",
            fieldLabel: "单据状态",
            id: "txtOrderStatus",
            name: "status",
            jitSize: 'small',
            orderType: 'order_status_ro'
            ,hidden:true
        }]

    });

    Ext.create('Ext.form.Panel', {
        id: 'btn_panel',
        layout: {
            type: 'table',
            columns: 4
        },
        renderTo: 'btn_panel',
        padding: '10 0 0 0',
        bodyStyle: 'background:#F1F2F5;',
        border: 0,
        //width: 200,
        height: 42,
        items: [
        {
            xtype: "jitbutton",
            text: "查询",
            //hidden: __getHidden("search"),
            margin: '0 0 10 14',
            handler: fnSearch
        }
        , {
            xtype: "jitbutton",
            id: "btnMoreSearchView",
            text: "高级查询",
            margin: '0 0 10 14',
            handler: fnMoreSearchView
        }
        ]

    });

    //operator area
    Ext.create('Jit.button.Button', {
        text: "添加",
        renderTo: "span_create",
        //hidden: __getHidden("create"),
        handler: fnCreate
    });
    //list area

    Ext.create('Ext.grid.Panel', {
        store: Ext.getStore("salesOutOrderStore"),
        id: "gridView",
        renderTo: "DivGridView",
        columnLines: true,
        height: 450,
        stripeRows: true,
        width: "100%",
        selModel: Ext.create('Ext.selection.CheckboxModel', {
            mode: 'MULTI'
        }),
        bbar: new Ext.PagingToolbar({
            displayInfo: true,
            id: "pageBar",
            defaultType: 'button',
            store: Ext.getStore("salesOutOrderStore"),
            pageSize: JITPage.PageSize.getValue()
        }),
        listeners: {
            render: function (p) {
                p.setLoading({
                    store: p.getStore()
                }).hide();
            }
        },
        columns: [{
            text: '操作',
            width: 110,
            sortable: true,
            dataIndex: 'order_id',
            align: 'left',
            //hidden: __getHidden("delete"),
            renderer: function (value, p, record) {
                var str = "";
                var d = record.data;
                if (d.status == "1") {
                    str += "<a class=\"z_op_link\" href=\"#\" onclick=\"fnDelete('" + value + "')\">删除</a>";
                    //                str += "<a class=\"z_op_link2\" href=\"#\" onclick=\"fnView('" + value + "','edit')\">修改</a>";
                    str += "<a class=\"z_op_link2\" href=\"#\" onclick=\"fnPass('" + value + "')\">审核</a>";
                }
                return str;
            }
        }, {
            text: '单据号码',
            width: 110,
            sortable: true,
            dataIndex: 'order_no',
            align: 'left'
            , renderer: function (value, p, record) {
                var str = "";
                var d = record.data;
                str += "<a class=\"pointer z_col_light_text\" onclick=\"fnView('" + d.order_id + "','" + d.status + "')\">" + value + "</a>";
                return str;
            }
        }, {
            text: '客户',
            width: 110,
            sortable: true,
            dataIndex: 'purchase_unit_name',
            align: 'left'
        }, {
            text: '销售单位',
            width: 110,
            sortable: true,
            dataIndex: 'sales_unit_name',
            align: 'left'
        }
    , {
        text: '仓库',
        width: 110,
        sortable: true,
        dataIndex: 'warehouse_name',
        align: 'left'
    }
    , {
        text: '单据日期',
        width: 110,
        sortable: true,
        dataIndex: 'order_date',
        align: 'left'
    }, {
        text: '出库日期',
        width: 110,
        sortable: true,
        dataIndex: 'complete_date',
        align: 'left'
    }, {
        text: '金额',
        width: 110,
        sortable: true,
        dataIndex: 'total_amount',
        align: 'left'
    }, {
        text: '数量',
        width: 110,
        sortable: true,
        dataIndex: 'total_qty',
        align: 'left'
    }, {
        text: '状态',
        width: 110,
        sortable: true,
        dataIndex: 'status_desc',
        align: 'left'
    }]
    });
}