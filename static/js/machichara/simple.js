
function simpleMachicharaUpdater(element) {
	return function() {
		simpleMachicharaUpdate(element);
	}
}

function simpleMachicharaUpdate(element) {
	clearTimeout(element.interval);
	element.style.top = 5 + Math.floor(Math.random()*80) + "%";
	element.style.left = 5 + Math.floor(Math.random()*80) + "%";
	element.interval = window.setTimeout(simpleMachicharaUpdater(element), 5000+Math.floor(Math.random()*25000));
}

function simpleMachichara(imageUrl) {
	var machicharaSimpleElement = document.createElement("div");
	machicharaSimpleElement.style.position = isIe ? "absolute" : "fixed";
	machicharaSimpleElement.style.top = "50%";
	machicharaSimpleElement.style.left = "50%";
	machicharaSimpleElement.style.zIndex = 5000;
	document.body.appendChild(machicharaSimpleElement);
	var machicharaSimpleImageElement = document.createElement("img");
	machicharaSimpleImageElement.src = imageUrl;
	machicharaSimpleImageElement.style.border = "none";
	machicharaSimpleElement.appendChild(machicharaSimpleImageElement);
	machicharaSimpleElement.onmouseover = simpleMachicharaUpdater(machicharaSimpleElement);
	simpleMachicharaUpdate(machicharaSimpleElement);
}