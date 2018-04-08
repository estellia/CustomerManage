/**
 * Created by ui on 2016/8/22.
 */



    var noviceGuide = function (id, param, callback1, callback2) {
        var domParam, domId = "";
        var _defalut = {
            top: 0,//提示框 top
            left: 0,//提示框 left
            direction: { // 箭头方向
                top: 170, 
                left: 300,
                angle: 0, //角度
            },
            video: {  //视频播放器
                top: 100,
                left: 600,
                src: "", //视频播放地址
            },
            isShow: "true", //是否显示 提示
            content: "这是一个空的提示信息", //提示信息
            title: "提示", //提示标题
            step: "1",  //当前步数
            sumStep: "1", //总步数
            bottomFn: function () { //打开视屏播放器
                startVideo();
            }

        }

        if (!!!id) {
            return console.log("创建失败")
        }
        var _callback1 = callback1;
        var _callback2 = callback2;
        console.log("id:" + id);

        domParamSet = {
            top: param.top ? param.top : _defalut.top,
            left: param.left ? param.left : _defalut.left,
            step: param.step ? param.step : _defalut.step,
            sumStep: param.sumStep ? param.sumStep : _defalut.sumStep,
            isShow: (param["isShow"] == true || param["isShow"] == false) ? param.isShow : _defalut.isShow,
            src: param.src ? param.src : _defalut.src,
            content: param.content ? param.content : _defalut.content,
            title: param.title ? param.title : _defalut.title,
            bottomFn: param.bottomFn ? param.bottomFn : _defalut.bottomFn,
            direction: {
                top: param.direction ? (param.direction.top ? param.direction.top : _defalut.direction.top) : _defalut.direction.top,
                left: param.direction ? (param.direction.left ? param.direction.left : _defalut.direction.left) : _defalut.direction.left,
                angle: param.direction ? (param.direction.angle ? param.direction.angle : _defalut.direction.angle) : _defalut.direction.angle,
            },
            video: {
                top: param.video ? (param.video.top ? param.video.top : _defalut.video.top) : _defalut.video.top,
                left: param.video ? (param.video.left ? param.video.left : _defalut.video.left) : _defalut.video.left,
                src: param.video ? (param.video.src ? param.video.src : _defalut.video.src) : _defalut.video.src,
            }
        }
        domId = id;
        function createElementNoviceGuide(callback) {
            var _html = "";
            var dom = document.getElementById("guidePrompt_" + domId);
            if (dom) {
                // dom.style.display = "block";
                var isblock = 'block';
            } else {
                var isblock = domParamSet.isShow ? 'block' : 'none';
                var autoPlay = domParamSet.isShow ? "autoPlay" : "";
            }
            var guidePromptDIV = document.createElement("div");
            guidePromptDIV.id = "guidePrompt_" + domId;
            guidePromptDIV.className = "guidePrompt";
            guidePromptDIV.style.top = domParamSet.top + "px";
            guidePromptDIV.style.left = domParamSet.left + "px";
            guidePromptDIV.style.display = isblock;
            
            _html += //' <div id="guidePrompt_' + domId + '" class="guidePrompt" style="top:' + domParamSet.top + 'px;left:' + domParamSet.left + 'px; display:' + isblock + '" >' +
                 ' <div class="prompt_close" onclick="closeGuide(\'' + domId + '\')"></div>' +
                '  <div class="prompt_Arrow" style="left: ' + domParamSet.direction.left + 'px;top: ' + domParamSet.direction.top + 'px;  transform: rotate(' + domParamSet.direction.angle + 'deg);"></div>' +
                '  <div class="guidePrompt_title">' +
                        domParamSet.title +
               '   </div>' +
               '   <div class="guidePrompt_info">' +
                        domParamSet.content +
                '  </div>' +
                '  <div>';//onclick=" domParamSet.prevFn()" href="javascript:domParamSet.prevFn()"onclick="prevCallback()"
                    if (domParamSet.step == domParamSet.sumStep) {
                        _html += '     <a class="guidePrompt_btnPrev" id="btnPrev_' + domId + '"  ></a><label class="prompt_label">第<span>' + domParamSet.step + '</span>步，共<span>' + domParamSet.sumStep + '</span>步 </label> <a href="javascript:void(0)" id="btnNext_' + domId + '" class="guidePrompt_btnFinish"></a>'
                    } else {
                        _html += '      <a class="guidePrompt_btnPrev" id="btnPrev_' + domId + '"  ></a><label class="prompt_label">第<span>' + domParamSet.step + '</span>步，共<span>' + domParamSet.sumStep + '</span>步 </label> <a href="javascript:void(0)" id="btnNext_' + domId + '" class="guidePrompt_btnNext"></a>'
                    }

                    _html += '</div>' +
                   '   <div class="guidePrompt_btmBtn" onclick="domParamSet.bottomFn()">查看视频</div>' +
                   //'  </div>' +
                ' <div class="videoPanel"  style="top:' + domParamSet.video.top + 'px;left:' + domParamSet.video.left + 'px;display:' + isblock + '">' +
                '       <div class="icon_close" onclick="closeVideo(this)"></div>' +//../static/jkb/movie/sczx.mp4
                '       <video id="example_video_1"  controls="controls" onclick="playing(this)" ' + autoPlay + ' style=""  >' +//poster="images/1.jpg"
                '           <source src="' + domParamSet.video.src + '" type="video/mp4" />' +
                '       </video>' +
               '  </div>' //+
               //'  </div>';
               guidePromptDIV.innerHTML=_html;


               document.body.appendChild(guidePromptDIV);



        }

        //创建DOM 元素

        createElementNoviceGuide();
        prevFn(domId);

        this.closeGuide = function () {

            var dom = document.getElementById("guidePrompt_" + domId);

            dom.style.display = "none";
            // dom.remove();

        }

        function prevFn(id) {
            var btnPrev = document.getElementById("btnPrev_" + id);
            var btnNext = document.getElementById("btnNext_" + id);
            btnPrev.addEventListener("click", function () {
                prevCallback();
            }, true);
            btnNext.addEventListener("click", function () {
                nextCallback();
            }, true);
            //addEvent(btnPrev, 'click', prevCallback);
        }

        function prevCallback() {
            if (_callback1) {
                _callback1();
                //if (typeof _callback === "function") {
                //    eval(_callback1());
                //} else {
                //    eval(_callback1);
                //}
            }
        };
        function nextCallback() {
            if (_callback2) {
                _callback2();
            }
        };
    }


    function showGuide(id) {
        var dom = document.getElementById("guidePrompt_" + id);
        dom.style.display = "block";
    }
    function closeGuide(id) {
        var dom = document.getElementById("guidePrompt_" + id);
        dom.style.display = "none";
    }



    //开始播放
    function playing(dom) {

        if (dom.paused) {
            dom.play();
        } else {
            dom.pause();
        }
    };
    //closeGuide=function() {

    //    var dom = document.getElementsByClassName("guidePrompt")[0];

    //    dom.style.display = "none";
    //  dom.remove();
    //}

    function closeVideo() {
        var dom = document.getElementsByClassName("videoPanel")[0];
        var video = document.getElementsByTagName("video")[0];
        video.pause();
        dom.style.display = "none";
        //dom.remove();
    }
    function startVideo() {
        var dom = document.getElementsByClassName("videoPanel")[0];
        var video = document.getElementsByTagName("video")[0];
        video.play();
        dom.style.display = "block";
    }
    function addEvent(el, type, fn) {

        if (document.addEventListener) {
            el.addEventListener(type, fn, true);
        } else if (document.attachEvent) {
            el.attachEvent("on" + type, fn);
        } else {
            el["on" + type] = fn;
        }
    };

 






   
