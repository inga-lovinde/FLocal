function showSpoiler(obj) {
	var inner = obj.parentNode.getElementsByTagName("div")[0];
	if(inner.style.display == "none" || inner.style.display == null || inner.style.display == "") {
		inner.style.display = "block";
	} else {
		inner.style.display = "none";
	}
}