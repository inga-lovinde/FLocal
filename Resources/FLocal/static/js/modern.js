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