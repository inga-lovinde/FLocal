
function simpleMachicharaUpdate(element) {
	element.style.top = Math.floor(Math.random()*90) + "%";
	element.style.left = Math.floor(Math.random()*90) + "%";
	window.setTimeout(function() {
		simpleMachicharaUpdate(element);
	}, 5000+Math.floor(Math.random()*25000));
}

function simpleMachichara(imageUrl) {
	var machicharaSimpleElement = document.createElement("div");
	machicharaSimpleElement.zIndex = 1000;
	machicharaSimpleElement.style.position = isIe ? "absolute" : "fixed";
	machicharaSimpleElement.style.top = "50%";
	machicharaSimpleElement.style.left = "50%";
	document.body.appendChild(machicharaSimpleElement);
	var machicharaSimpleImageElement = document.createElement("img");
	machicharaSimpleImageElement.src = imageUrl;
	machicharaSimpleImageElement.style.border = "none";
	machicharaSimpleElement.appendChild(machicharaSimpleImageElement);
	simpleMachicharaUpdate(machicharaSimpleElement);
}