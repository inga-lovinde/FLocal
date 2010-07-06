function showSpoiler(obj) {
	var inner = obj.parentNode.getElementsByTagName("div")[0];
	if(inner.style.display == "none" || inner.style.display == null || inner.style.display == "") {
		inner.style.display = "block";
	} else {
		inner.style.display = "none";
	}
}

var isNetscape = (document.layers) ? true : false;
var isOpera = (navigator.userAgent.toLowerCase ().indexOf ("opera") != -1);
var isIe = (document.all && !isOpera) ? true : false;
var isWebkit = (navigator.userAgent.toLowerCase ().indexOf ("webkit") != -1);
var isMozilla = (document.getElementById && !isIe && !isOpera && !isWebkit) ? true : false;
var isMac = (navigator.userAgent.toLowerCase ().indexOf ("macintosh") != -1);

function assignArrowsHandlers() {
	document.onkeydown = function(event) {
		if(window.event && isIe) {
			keyCode = window.event.keyCode;
		} else if (event.keyCode) {
			keyCode = event.keyCode;
		} else {
			keyCode = event.which;
		}

		if (keyCode == 37) {
			location.href = document.getElementById("actionLink_left").href;
		} else if (keyCode == 39) {
			location.href = document.getElementById("actionLink_right").href;
		}
	}
}
