var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++) {
	acc[i].addEventListener("click", function () {
		this.classList.toggle("active");
		var panel = this.nextElementSibling;
		if (panel.style.maxHeight) {
			panel.style.maxHeight = null;
			panel.style.padding = "0px";
		} else {
			panel.style.maxHeight = (panel.scrollHeight + 80) + "px";
			panel.style.padding = "10px 18px 18px 10px";
		}
	});
}