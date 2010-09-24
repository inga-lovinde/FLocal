function showChildren(elem) {
	for(var i=0; i<elem.childNodes.length; i++) {
		if(elem.childNodes[i].nodeName == "DIV") {
			elem.childNodes[i].style.display="block";
		}
	}
}

function hideChildren(elem) {
	for(var i=0; i<elem.childNodes.length; i++) {
		if(elem.childNodes[i].nodeName == "DIV" && elem.childNodes[i].getAttribute("default") != "default") {
			elem.childNodes[i].style.display="none";
		}
	}
}