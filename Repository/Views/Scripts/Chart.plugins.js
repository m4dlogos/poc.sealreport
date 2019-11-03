Chart.plugins.register({
id:'doughnutWithText',
afterDraw: function(chart) {
	if (chart.config.type == 'doughnut')
	{
		var ctx = chart.chart.ctx;
		ctx.save();

		ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontFamily, 'normal', Chart.defaults.global.defaultFontFamily);
		ctx.textAlign = 'center';
		ctx.textBaseline = 'bottom';
		chart.data.datasets.forEach(function (dataset) {

		for (var i = 0; i < dataset.data.length; i++) {
			var model = dataset._meta[Object.keys(dataset._meta)[0]].data[i]._model,
				total = dataset._meta[Object.keys(dataset._meta)[0]].total,
				mid_radius = model.innerRadius + (model.outerRadius - model.innerRadius)/2,
				start_angle = model.startAngle,
				end_angle = model.endAngle,
				mid_angle = start_angle + (end_angle - start_angle)/2;

				var x = mid_radius * Math.cos(mid_angle);
				var y = mid_radius * Math.sin(mid_angle);

				ctx.fillStyle = '#fff';
				if (i == 3){ // Darker text color for lighter background
					ctx.fillStyle = '#444';
				}
				//var percent = String(Math.round(dataset.data[i]/total*100)) + "%";      
				//Don't Display If Legend is hide or value is 0
				if(dataset.data[i] != 0 && dataset._meta[Object.keys(dataset._meta)[0]].data[i].hidden != true) {
					ctx.fillText(dataset.data[i], model.x + x, model.y + y);
					// Display percent in another line, line break doesn't work for fillText
					//ctx.fillText(percent, model.x + x, model.y + y + 15);
				}
			}
		});

		ctx.fillStyle = '#333333';
		ctx.textBaseline = 'middle';
		ctx.textAlign = 'center';
		var model = chart.data.datasets[0]._meta[Object.keys(chart.data.datasets[0]._meta)[0]].data[0]._model;
		var total = chart.data.datasets[0]._meta[Object.keys(chart.data.datasets[0]._meta)[0]].total;
		var firstValue = chart.data.datasets[0].data[0];
		var percent = Number(firstValue/total).toLocaleString(undefined,{style: 'percent', minimumFractionDigits:0});
		
		let fontSize = Math.floor(model.innerRadius * 0.7);
		ctx.font = fontSize + 'px arial';
		let textHeight = parseInt(ctx.font);
		var maxWidth = (2*model.innerRadius)*0.7;
		
		ctx.fillText(percent, model.x, model.y, maxWidth);
		
		fontSize = fontSize*0.6;
		ctx.font = fontSize + 'px ariral';
		ctx.fillText(chart.data.labels[0], model.x, model.y + textHeight*0.7, maxWidth);

		ctx.restore();
	}
}
});