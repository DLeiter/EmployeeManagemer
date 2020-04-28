//bar
//var ctxB = document.getElementById("barChart").getContext('2d');
//var myBarChart = new Chart(ctxB, {
//	type: 'bar',
//	data: {
//		labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
//		datasets: [{
//			label: '# of Votes',
//			data: [12, 19, 3, 5, 2, 3],
//			backgroundColor: [
//				'rgba(255, 99, 132, 0.2)',
//				'rgba(54, 162, 235, 0.2)',
//				'rgba(255, 206, 86, 0.2)',
//				'rgba(75, 192, 192, 0.2)',
//				'rgba(153, 102, 255, 0.2)',
//				'rgba(255, 159, 64, 0.2)'
//			],
//			borderColor: [
//				'rgba(255,99,132,1)',
//				'rgba(54, 162, 235, 1)',
//				'rgba(255, 206, 86, 1)',
//				'rgba(75, 192, 192, 1)',
//				'rgba(153, 102, 255, 1)',
//				'rgba(255, 159, 64, 1)'
//			],
//			borderWidth: 1
//		}]
//	},
//	options: {
//		scales: {
//			yAxes: [{
//				ticks: {
//					beginAtZero: true
//				}
//			}]
//		}
//	}
//});



//$.ajax({
//	type: "POST",
//	url: '@ Url.Action("ActionMethod", "Home")',
//	contentType: 'application/json; charset=utf-8',
//	dataType: "json",
//	data: JSON.stringify({ data: Value }),
//	success: function (data) {
//		alert('success');
//	},
//	error: function(result) {
//		alert('error');
//	}
//});

//function GetTransactionBuyingRate() {
//	//var eventIdB = document.getElementById("CurrencyId").value;
//	console.log("TEST1");
//	$.ajax({
//		url: '@Url.Action("GetEmployees", "Employees")',
//		type: 'GET',
//		cache: false,
//		success: function (result) {
//			alert(result);
//			//$('#Transaction_Rate').val(result);
//		}
//	});
//	console.log("TEST2");
//}