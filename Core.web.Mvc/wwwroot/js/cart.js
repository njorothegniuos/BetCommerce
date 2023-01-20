"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/cartHub").build();

$(function () {
	connection.start().then(function () {
		/*alert('Connected to dashboardHub');*/

		InvokeProducts();


	}).catch(function (err) {
		return console.error(err.toString());
	});
});

// Product
function InvokeProducts() {
	connection.invoke("SendCartCountAndSum").catch(function (err) {
		return console.error(err.toString());
	});
}

connection.on("ReceivedCartCountAndSum", function (cartItem) {
	BindCartCountAndSum(cartItem);
});

function BindCartCountAndSum(cartItem) {
	$('#cartCount').empty();
	$('#total').empty();
	console.log("cartCount => " + cartItem.itemCount);
	console.log("cartCount => " + cartItem.total);
	var count;
	var total;
	count = cartItem.itemCount
	$('#cartCount').append(count);
	total = cartItem.total
	$('#total').append(total);
}

