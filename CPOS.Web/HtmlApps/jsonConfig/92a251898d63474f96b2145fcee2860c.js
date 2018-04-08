{
    "pageKey":"H_Record",
    "pageDes":"推荐战绩",
    "htmls":[
        {
            "id":1,
            "path":"special/lj/record.html",
            "css":[
                "base/global","special/lj/style_other"
            ],
            "des":"推荐战绩"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"推荐战绩",
    "plugin":[
		"mustache"
    ],
    "script":[
        "special/lj/record"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_Reserve",
    "pageDes":"客房预订",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/reserve.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"客房预订"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"客房预订",
    "plugin":[
		"mustache"
    ],
    "script":[
        "public/hotel/reserve"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_News",
    "pageDes":"资讯列表页",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/news.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"资讯列表"
        }
    ],
    "defaultHtml":1,
    "isEntry":1,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"资讯列表",
    "plugin":[
        "iscroll"
    ],
    "script":[
        "public/hotel/news"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
} 

{
    "pageKey":"H_NewDetail",
    "pageDes":"详情",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/new_detail.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"详情"
        }
    ],
    "defaultHtml":1,
    "isEntry":1,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"资讯列表",
    "plugin":[
    ],
    "script":[
        "public/hotel/new_detail"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
} 

{
    "pageKey":"H_HomeIndex",
    "pageDes":"微官网",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/homeIndex.html",
            "css":[
                "base/global"
            ],
            "des":"微官网"
        }
    ],
    "defaultHtml":1,
    "isEntry":1,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"微官网",
    "plugin":[
        "alice"
    ],
    "script":[
        "common/home/index"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[
         {
            "Name":"title显示名称",
            "Key":"title",
            "valueType":"string",
            "defaultValue":"微官网"
        },
        {
            "Name":"logo图片",
            "Key":"logo",
            "valueType":"string",
            "defaultValue":"../../../images/public/hotel_default/logo.png"
        },
        {
            "Name":"背景图片",
            "Key":"backgroundImage",
            "valueType":"string",
            "defaultValue":"../../../images/special/bollssom/indexBg.jpg"
        },
       {
            "Name":"小图标大小",
            "Key":"littleImageSize",
            "valueType":"string",
            "defaultValue":"13px"
        },
    {
            "Name":"小图标图片",
            "Key":"littleImageUrl",
            "valueType":"string",
            "defaultValue":"../../../images/public/hotel_default/icon.png"
        },
    {
            "Name":"动画方向",
            "Key":"animateDirection",
            "valueType":"string",
            "defaultValue":"up"
        },
       {
            "Name":"功能项配置",
            "Key":"links",
            "SubKey":"title,english,toUrl",
            "SubName":"显示名称,英文字符,跳转地址",
            "SubValueType":"string,string,string",
            "SubDefaultValue":"输入名称,,",
            "valueType":"Array",
            "arrayLength":5,
            "defaultValue":""
        }
]
} 

{
    "pageKey":"H_StoreList",
    "pageDes":"选择酒店",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/store_list.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"选择酒店"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"客房预订",
    "plugin":[
		"mustache"
    ],
    "script":[
        "public/hotel/store_list"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}


{
    "pageKey":"H_Order",
    "pageDes":"订单详细",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/order.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"订单详细"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"订单详细",
    "plugin":[
		"mustache"
    ],
    "script":[
        "public/hotel/order"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_OrderSubmit",
    "pageDes":"预订房间",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/order_submit.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"预订房间"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"预订房间",
    "plugin":[
		"mustache"
    ],
    "script":[
        "public/hotel/order_submit"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_HousingType",
    "pageDes":"选择房型",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/housing_type.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"选择房型"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"选择房型",
    "plugin":[
		"mustache","iscroll","zepto-fx"
    ],
    "script":[
        "public/hotel/housing_type"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_HouseDetail",
    "pageDes":"房间预订",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/house_detail.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"房间预订"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"房间预订",
    "plugin":[
		"mustache","iscroll","zepto-fx"
    ],
    "script":[
        "public/hotel/house_detail"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_IntroduceDetail",
    "pageDes":"酒店详情 - 介绍",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/detail01.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"酒店详情 - 介绍"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"酒店详情 - 介绍",
    "plugin":[
		"mustache"
    ],
    "script":[
        "public/hotel/introducedetail"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_Mating",
    "pageDes":"酒店详情 - 配套",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/detail02.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"酒店详情 - 配套"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"酒店详情 - 配套",
    "plugin":[
		"mustache"
    ],
    "script":[
        "public/hotel/mating"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_Hot",
    "pageDes":"酒店详情 - 热点",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/detail03.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"酒店详情 - 热点"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"酒店详情 - 热点",
    "plugin":[
		"mustache"
    ],
    "script":[
        "public/hotel/hot"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_ChooseDate",
    "pageDes":"日期选择",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/choose_date.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"日期选择"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"日期选择",
    "plugin":[
		"mustache"
    ],
    "script":[
        "public/hotel/choose_date"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}



{
    "pageKey":"H_OrderList",
    "pageDes":"我的订单",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/order_list.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"我的订单"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"我的订单",
    "plugin":[
		"mustache"
    ],
    "script":[
        "public/hotel/order_list"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_GetPosition",
    "pageDes":"系统定位中...",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/getposition.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"系统定位中..."
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"系统定位中...",
    "plugin":[
		"mustache"
    ],
    "script":[
        "public/hotel/getposition"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}



{
    "pageKey":"H_Map",
    "pageDes":"地图查看",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/map.html",
            "css":[
                "special/bollssom/global","special/bollssom/style"
            ],
            "des":"地图查看"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"地图查看",
    "plugin":[
    ],
    "script":[
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_Photo",
    "pageDes":"查看照片",
    "htmls":[
        {
            "id":1,
            "path":"public/hotel/housephoto.html",
            "css":[
                "special/bollssom/photo","public/hotel/photoswipe"
            ],
            "des":"查看照片"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"查看照片",
    "plugin":[
	"bdTemplate","jquery_plugin/inheritance",
        "jquery_plugin/imagesloaded",
        "jquery_plugin/wookmark",
         "jquery_plugin/klass",
        "jquery_plugin/photoswipe",
       "jquery_plugin/wookmark"
    ],
    "script":[
        "public/hotel/photo"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[]
}

{
    "pageKey":"H_Introduce",
    "pageDes":"前世今生",
    "htmls":[
        {
            "id":1,
            "path":"special/bollssom/introduce.html",
            "css":[
                "base/global"
            ],
            "des":"前世今生"
        }
    ],
    "defaultHtml":1,
    "isEntry":0,
    "pageCode":"default",
    "NeedAuth":1,
    "customerId":"92a251898d63474f96b2145fcee2860c",
    "title":"前世今生",
    "plugin":[
        "Dom3DRoute","bdTemplate"
    ],
    "script":[
        "public/hotel/introduce"
    ],
    "urlTemplete":"/HtmlApps/html/_pageName_",
    "params":[
         {
            "Name":"是否显示底部导航",
            "Key":"hasFooter",
            "valueType":"string",
            "defaultValue":"false"
        },
       {
            "Name":"图片地址",
            "Key":"itemList",
            "SubKey":"imgUrl",
            "SubName":"图片地址",
            "SubValueType":"string",
            "SubDefaultValue":"图片地址",
            "valueType":"Array",
            "arrayLength":6,
            "defaultValue":""
        }
]
} 