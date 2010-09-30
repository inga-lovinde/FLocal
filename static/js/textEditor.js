function x() {
	return;
}
var cancelEnter = false;
function fontlist() {
	var fonts = new Array("Arial", "Arial Black", "Arial Narrow", "Book Antiqua", "Century Gothic", "Comic Sans MS", "Courier New", "Fixedsys", "Garamond", "Georgia", "Impact", "Lucida Console", "Lucida Sans Unicode", "Microsoft Sans Serif", "Palatino Linotype", "System", "Tahoma", "Times New Roman", "Trebuchet MS", "Verdana");
	for(key in fonts) {
		document.writeln('<option value="' + fonts[key] + '">' + fonts[key] + '</option>');
	}
}
//shaller [26.11.2004 1:15]
function setOffsetPosition(elem,offset) {
	if (isIe) {   
		elem.caretPos.moveStart ('character', offset);
		elem.caretPos.moveEnd ('character', offset);
		elem.caretPos.select();
	} else if (isMozilla) {
		cursorPos = elem.selectionStart + offset;
		//alert(elem.selectionStart);
		elem.setSelectionRange(cursorPos,cursorPos);
		elem.focus();
	}   
}
function storeCaret (textEl) {
	if(textEl.createTextRange) textEl.caretPos = document.selection.createRange().duplicate();
} // end fn
function insertAtCaret (textEl, text) {
	if(textEl.createTextRange && textEl.caretPos) {
		var caretPos = textEl.caretPos;
		caretPos.text = (caretPos.text.charAt(caretPos.text.length - 1) == ' ') ? text + ' ' : text;
	} else if(isMozilla || isWebkit) {
		cursorPos = textEl.selectionStart;
		textEl.value = textEl.value.slice (0, cursorPos) + text + textEl.value.slice (cursorPos , textEl.value.length);
		textEl.setSelectionRange(cursorPos+text.length,cursorPos+text.length);
		textEl.focus();
	} else {
		textEl.value  = textEl.value + text; // for non MSIE browsers just append it
	}
	return true;
}// fn
function checkKeyPressed(form, ev, sendWithEnter) {
//shaller 08.07.04
	if(window.event && (isIe || isWebkit) && !isMac) {
		if(window.event.keyCode == 10) {
			form.submit();
		} else if(sendWithEnter && window.event.keyCode == 13) {
			form.submit();
		} else if(window.event.shiftKey && window.event.keyCode == 2) {
			window.event.returnValue = false;
			DoPrompt("bold");
		} else if(window.event.shiftKey && window.event.keyCode == 9) { 
			window.event.returnValue = false; 
			DoPrompt("italics"); 
		} 								
	} else {
		if(ev.ctrlKey && ev.keyCode == 13) {
			form.submit();	  
		} else if(ev.ctrlKey && ev.shiftKey && ev.which == 66) {
			ev.preventDefault();	
			DoPrompt("bold");
		} else if(ev.ctrlKey && ev.shiftKey && ev.which == 73) {
			ev.preventDefault();
			DoPrompt("italics");
		}
	}
}

function DoSmilie(addSmilie) {
	var revisedMessage;
	var currentMessage = document.replier.Body.value;
	revisedMessage = currentMessage+addSmilie;
	document.replier.Body.value=revisedMessage;
	document.replier.Body.focus();
	return;
}
function DoColor(color) {
	tmp_str = "[/color]";
	insertAtCaret(document.replier.Body, ' ' + "[color=" +color + "]  [/color]" + ' ' );
	document.replier.Body.focus();
	setOffsetPosition(document.replier.Body,-tmp_str.length-3);
	return;
}
function DoSize(size) {
	tmp_str = "[/size]";
	insertAtCaret(document.replier.Body, ' ' + "[size=" +size + "]  [/size]" + ' ' );
	document.replier.Body.focus();
	setOffsetPosition(document.replier.Body,-tmp_str.length-3);
	return;
}
function DoFont(font) {
	tmp_str = "[/font]";
	insertAtCaret(document.replier.Body, ' ' + '[font="' + font + '"]  [/font]' + ' ' );
	document.replier.Body.focus();
	setOffsetPosition(document.replier.Body,-tmp_str.length-3);
	return;
}
/* Sending  messages with Ctrl+Enter outside the textarea */
if(typeof(document.send_final) != "undefined") {
	document.replier.Body.onfocus = function(event) {
		cancelEnter=true;
	}
	document.replier.Body.onblur = function(event) {
		cancelEnter=false;
	}
	document.onkeypress = function(event) {
		if(!cancelEnter) {
			checkKeyPressed(document.send_final, event, true);
		}
	}
}
function DoPrompt(action) {
	var currentMessage = document.replier.Body.value;
	if(action == "url") {
		var thisURL = prompt("Введите полный URL ссылки, которую вы хотите вставить.", "http://");
		if (thisURL == null) {
			return;
		}
		var thisTitle = prompt("Теперь введите название web-страницы, на которую указывает данная вами ссылка.", "web-страница");
		if(thisTitle == null) {
			return;
		}
		insertAtCaret(document.replier.Body, ' ' + "[url=" + thisURL + "]" + thisTitle + "[/url]" + ' ');
		document.replier.Body.focus();
		return;
	} else if (action == "color") {
		tmp_str = "[/color]";
		var thisColor = prompt("Введите цвет текста. Это может быть название, например blue, или 16-ричное RGB-выражение, например #ff1123","");
		if(thisColor == null) {
			return;
		}
		insertAtCaret(document.replier.Body, ' ' + "[color=" + thisColor + "] [/color] ");
		document.replier.Body.focus();
		setOffsetPosition(document.replier.Body,-tmp_str.length-3);
		return;
	} else if(action == "email") {
		var thisEmail = prompt("Введите полный адресe email, который вы хотите вставить.", "");
		if(thisEmail == null) {
			return;
		}
		insertAtCaret(document.replier.Body, ' ' + "[Email]" + thisEmail + "[/Email]" + ' ');
		document.replier.Body.focus();
		return;
	} else if(action == "bold") {
		var thisBold = prompt("Введите текст, который вы хотите выделить жирным шрифтом.", "");
		if(thisBold == null) {
			return;
		}
		insertAtCaret(document.replier.Body, ' ' + "[b]" + thisBold + "[/b]" + ' ');
		document.replier.Body.focus();
		return;
	} else if(action == "italics") {
		var thisItal = prompt("Введите текст, который вы хотите выделить наклонным шрифтом.", "");
		if(thisItal == null) {
			return;
		}
		insertAtCaret(document.replier.Body, ' ' + "[i]" + thisItal + "[/i]" + ' ');
		document.replier.Body.focus();
		return;
	} else if(action == "furl") {
		var thisImage = prompt("Введите полный URL адреса на старом форуме.", "http://");
		if(thisImage == null) {
			return;
		}
		insertAtCaret(document.replier.Body, ' ' + "[furl]" + thisImage + "[/furl]" + ' ' );
		document.replier.Body.focus();
		return;
	} else if(action == "quote") {
		tmp_str = "[/quote]";
		insertAtCaret(document.replier.Body, ' ' + "[quote]  [/quote]" + ' ');
		document.replier.Body.focus();
		setOffsetPosition(document.replier.Body,-tmp_str.length-3);
		return;
	} else if(action == "code") {
		tmp_str = "[/code]";
		insertAtCaret(document.replier.Body, ' ' + "[code]  [/code]" + ' ');
		document.replier.Body.focus();
		setOffsetPosition(document.replier.Body,-tmp_str.length-3);
		return;
	} else if(action == "ecode") {
		tmp_str = "[/ecode]";
		insertAtCaret(document.replier.Body, ' ' + "[ecode]  [/ecode]" + ' ');
		document.replier.Body.focus();
		setOffsetPosition(document.replier.Body,-tmp_str.length-3);
		return;
	} else if(action == "liststart") {
		insertAtCaret(document.replier.Body, ' ' + "[list]" + ' ');
		document.replier.Body.focus();
		return;
	} else if(action == "listend") {
		insertAtCaret(document.replier.Body, ' ' + "[/list]" + ' ');
		document.replier.Body.focus();
		return;
	} else if (action == "listitem") {
		var thisItem = prompt("Введите элемент списка. Помните что список должен начинаться с 'Начала списка' и завершаться 'Концом списка'.", "");
		if(thisItem == null) {
			return;
		}
		insertAtCaret(document.replier.Body, ' ' + "[*]" + thisItem + ' ' );
		document.replier.Body.focus();
		return;
	} else if(action == "pollstart") {
		var thisPOLL = prompt("Введите название голосования", "");
		if(thisPOLL == null){
			return;
		}
		insertAtCaret(document.replier.Body, "[pollstart]\n[polltitle=" + thisPOLL + "]\n");
		document.replier.Body.focus();
		return;
	} else if (action == "polloption") {
		var thisOption = prompt("Введите вариант голосования. Помните что голосование должно начинаться с 'Начала голосования' и завершаться 'Концом голосования'.", "");
		if(thisOption == null) {
			return;
		}
		insertAtCaret(document.replier.Body, "[polloption=" + thisOption + "]\n");
		document.replier.Body.focus();
		return;
	} else if(action == "pollstop") {
		insertAtCaret(document.replier.Body, "[pollstop]\n");
		document.replier.Body.focus();
		return;
	} else if(action == "user") {
		var thisOption = prompt("Пользователь", "");
		if(thisOption == null) {
			return;
		}
		insertAtCaret(document.replier.Body, "[user=" + thisOption + "]\n");
		document.replier.Body.focus();
		return;
	} else if (action == "table") {
		insertAtCaret(document.replier.Body, "[table width= border= cellspacing= cellpadding=]\n\n[/table]");
		document.replier.Body.focus();
		tmp_str = "[/table]";
		setOffsetPosition(document.replier.Body,-tmp_str.length-1);
		return;
	} else if(action == "video") {
		var thisVideo = prompt("YouTube link", "http://");
		if(thisVideo == null) {
			return;
		}
		insertAtCaret(document.replier.Body, ' ' + "[video]" + thisVideo + "[/video]" + ' ');
		document.replier.Body.focus();
		return;
	} else if(action == "tex") {
		var thisMath = prompt("LaTeX code", "");
		if(thisMath == null) {
			return;
		}
		insertAtCaret(document.replier.Body, ' ' + "[tex]" + thisMath + "[/tex]" + ' ' );
		document.replier.Body.focus();
		return;
	} else if(action == "math") {
		var thisMath = prompt("Math LaTeX code", "");
		if(thisMath == null) {
			return;
		}
		insertAtCaret(document.replier.Body, ' ' + "[math]" + thisMath + "[/math]" + ' ' );
		document.replier.Body.focus();
		return;
	}		
}
