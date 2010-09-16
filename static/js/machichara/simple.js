
function simpleMachicharaUpdate(element) {
	element.style.top = Math.floor(Math.random()*110 - 10) + "%";
	element.style.left = Math.floor(Math.random()*110 - 10) + "%";
	window.setTimeout(function() {
		simpleMachicharaUpdate(element);
	}, 5000+Math.floor(Math.random()*25000));
}

function simpleMachichara(imageUrl) {
	var machicharaSimpleElement = document.createElement("div");
	machicharaSimpleElement.zIndex = 1000;
	machicharaSimpleElement.style.position = "absolute";
	machicharaSimpleElement.style.top = "50%";
	machicharaSimpleElement.style.left = "50%";
	document.body.appendChild(machicharaSimpleElement);
	var machicharaSimpleImageElement = document.createElement("img");
	machicharaSimpleImageElement.src = imageUrl;
	machicharaSimpleImageElement.style.border = "none";
	machicharaSimpleElement.appendChild(machicharaSimpleImageElement);
	simpleMachicharaUpdate(machicharaSimpleElement);
}