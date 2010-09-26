
function simpleMachicharaUpdater(element) {
	return function() {
		simpleMachicharaUpdate(element);
	}
}

function simpleMachicharaUpdate(element) {
	clearTimeout(element.interval);
	element.style.top = Math.floor(Math.random()*90) + "%";
	element.style.left = Math.floor(Math.random()*90) + "%";
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