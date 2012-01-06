function getChildrenShower(elem) {
	return function() {
		for(var i=0; i<elem.childNodes.length; i++) {
			if(elem.childNodes[i].nodeName == "DIV") {
				elem.childNodes[i].style.display="block";
			}
		}
	}
}

function showChildren(elem) {
	clearTimeout(elem.interval);
	elem.interval = setTimeout(getChildrenShower(elem), 200);
}

function getChildrenHider(elem) {
	return function() {
		for(var i=0; i<elem.childNodes.length; i++) {
			if(elem.childNodes[i].nodeName == "DIV" && elem.childNodes[i].getAttribute("default") != "default") {
				elem.childNodes[i].style.display="none";
			}
		}
	}
}

function hideChildren(elem) {
	clearTimeout(elem.interval);
	elem.interval = setTimeout(getChildrenHider(elem), 200);
}

function makeMainboxScrollable() {
	if(window.innerHeight > 0) {
		var height = window.innerHeight;
		height -= document.getElementById("headerContainer").clientHeight;
		height -= 1; //headerContainer border
		var width = window.innerWidth;
		width -= document.getElementById("bjAndSlutsContainer").clientWidth;
		var mainbox = document.getElementById("contentContainer");
		mainbox.style.overflow = "auto";
		mainbox.style.height = mainbox.style.maxHeight = height + "px";
		mainbox.style.width = mainbox.style.maxWidth = width + "px";
	}
}

function onLoad() {
	if(typeof(window.addEventListener) == "function") {
		makeMainboxScrollable();
		window.addEventListener("resize", makeMainboxScrollable, false);
	}
	if(typeof(machicharaInit) == "function") {
		machicharaInit();
	}
}
