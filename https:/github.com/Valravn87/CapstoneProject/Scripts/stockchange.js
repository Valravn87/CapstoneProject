function StockLevelChanged(id) {
	var select = document.getElementById("new-stock" + id);
	var url = "/RestaurantItems/ChangeStock/" + id + "?code=" + select.value;
	$.ajax({ method: "POST", url: url }).done(function (data) {
		$("#item-id-row" + id).attr("class", "item-row" + select.value);
	});
}