function HideElement(id) { id.style.display = "none"; }
function ShowElement(id) { id.style.display = "block"; }

window.onload = function() {
var tel_kom = document.getElementById('ContentPlaceHolder1_tel_kom');
var tel_stac = document.getElementById('ContentPlaceHolder1_tel_stac');
var RadioButton1 = document.getElementById('ContentPlaceHolder1_RadioButton1');
var RadioButton2 = document.getElementById('ContentPlaceHolder1_RadioButton2');
tel_stac.style.display = "none";
RadioButton1.onclick = function() {
HideElement(tel_stac);
ShowElement(tel_kom);
}
RadioButton2.onclick = function() {
HideElement(tel_kom);
ShowElement(tel_stac);
}
}