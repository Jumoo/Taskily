var tskly = (function () {

    var tk = {};

    var props;

    tk.init = function (opt, d) {

        props = opt || {};
        props.id = opt.id || 'unknown';
        props.title = opt.title || 'Title';
        props.msg = opt.msg || 'Dialog box message';
        props.freq = opt.freq || 10;

        load_styles(d);
        build_modal();
        attach_modal();
        show_modal();
    };

    function load_styles(d) {
        var s = document.createElement('link');
        s.setAttribute('rel', 'stylesheet');
        s.setAttribute('type', 'text/css');
        s.setAttribute('href', d + '/taskily-pop.css');
        document.getElementsByTagName('head')[0].appendChild(s);
    }

    function build_modal() {
        var modal = document.createElement('div');
        modal.setAttribute('id', 'tsk-pop');
        modal.setAttribute('style', 'visibility:hidden;');
        modal.innerHTML = '<div class="tsk-box">'
			+ '<h2>' + props.title + '</h2>'
			+ '<p>' + props.msg + '</p>'
			+ '<div class="tsk-btn-box">'
			+ '<a href="#" id="tsk-py" class="tsk-btn">Yes</a>'
			+ '<a href="#" id="tsk-pc" class="tsk-btn">No Thanks</a>'
			+ '</div>';

        document.getElementsByTagName('body')[0].appendChild(modal);
    }

    function attach_modal() {
        document.getElementById("tsk-pc").onclick = function () {
            var el = document.getElementById("tsk-pop");
            el.style.visibility = (el.style.visibility == "visible") ? "hidden" : "visible";
        };
        document.getElementById("tsk-py").onclick = function () {
            document.location.href = 'https://taskily.azurewebsites.net/public/' + props.id;
        };
    }

    function show_modal() {

        if (getCookie("tskly_popped") != 'true') {
            var n = Math.floor(Math.random() * props.freq) + 1;
            console.log(n);
            if (n == 1) {
                document.cookie = "tskly_popped=true";
                document.getElementById("tsk-pop").style.visibility = "visible";
            }
        }
    }

    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i].trim();
            if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
        }
        return "";
    }

    return tk;
});

