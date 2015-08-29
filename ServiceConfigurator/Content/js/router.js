function renderBody(routes, i) {
    var n = routes[i].length;

    routes[i].forEach(function (route) {
        $.get(route, function (data) {
            placeHTML(route, data);
            n--;
            if (n == 0 && i < routes.length - 1) 
                renderBody(routes, i + 1);
        });
    });
};

function placeHTML(id, data) {
    document.getElementById(id).innerHTML = data;
};

function getRoutesFromTree(url) {
    var urlDec = decodeURIComponent(url);
    var routes = [[]];

    var curLevel = 0;
    var curRoute = '';
    var prefix = '/';

    for (var i = 0; i < urlDec.length; i++) {
        if (urlDec[i] != '{' && urlDec[i] != '}') {
            curRoute += urlDec[i];
            continue;
        };
        if (urlDec[i] == '{') {
            prefix += curRoute;
            if (curRoute != '') {
                routes[curLevel].push(prefix);
                prefix += '/';
            };
            curLevel++;
            if (curLevel == routes.length)
                routes.push([]);
        }
        else {
            if (curRoute != '')
                routes[curLevel].push(prefix + curRoute);
            else {
                prefix = prefix.slice(0, -1);
                prefix = prefix.substr(0, prefix.lastIndexOf('/') + 1);
            };
            curLevel--;
        };
        curRoute = '';
    };
    routes.shift();
    return routes;
};

function getUrlParameter(name) {
    return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null;
};

function setUrlParameter(name, value, encode) {
    if (typeof encode !== 'undefined') 
        value = encodeURIComponent(value);

    var s = location.search;
    s = s.replace(new RegExp('(&|\\?)' + name + '=[^\&]*'), '$1' + name + '=' + value);
    if (!RegExp.$1)
        s += (s.length > 0 ? '&' : '?') + name + '=' + value;

    window.history.pushState("", "", s);
};

function addUrlRoute(name, route, encode) {
    if (route[0] == '/')
        route = route.substr(1);

    var nodes = route.split('/');
    var paramValue = getUrlParameter(name);
    if (paramValue == null) {
        for (var i = nodes.length-1; i >= 0; i--) {
            paramValue = paramValue == null ? nodes[i] : nodes[i] + '{' + paramValue + '}';
        };

        setUrlParameter(name, paramValue, encode);
        return;
    };

    for (i = 0; i < nodes.length; i++) {
        if (paramValue.indexOf(nodes[i]) != -1)
            continue;

        paramValue = paramValue.replace(nodes[i - 1], nodes[i - 1] + '{' + nodes[i] + '}');
        setUrlParameter(name, paramValue, encode);
        return;
    };

};