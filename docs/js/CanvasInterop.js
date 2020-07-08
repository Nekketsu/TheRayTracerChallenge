window.canvas = {
	render: (canvas, width, height, colors) => {
		canvas.width = width;
		canvas.height = height;

		let context = canvas.getContext("2d");
		let imageData = context.getImageData(0, 0, canvas.width, canvas.height);
		let data = imageData.data;

		let length = width * height;
		for (let i = 0; i < length; i++) {
			let dataIndex = i * 4;
			data[dataIndex] = colors[i].red;
			data[dataIndex + 1] = colors[i].green;
			data[dataIndex + 2] = colors[i].blue;
			data[dataIndex + 3] = 255;
		}

		context.putImageData(imageData, 0, 0);
	}
	//,
	//setSize: (canvas, width, height) => {
	//	canvas.width = width;
	//	canvas.height = height;
	//},
	//startDrawing: canvas => {
	//	window.canvas.context = canvas.getContext("2d");
	//	window.canvas.imageData = window.canvas.context.getImageData(0, 0, canvas.width, canvas.height);
	//},
	//setColor: (i, color) => {
	//	let dataIndex = i * 4;
	//	window.canvas.imageData.data[dataIndex] = color.red;
	//	window.canvas.imageData.data[dataIndex + 1] = color.green;
	//	window.canvas.imageData.data[dataIndex + 2] = color.blue;
	//	window.canvas.imageData.data[dataIndex + 3] = 255;
	//	if (i % 100 === 0) { console.log(`Color: ${i}`); }
	//},
	//endDrawing: () => {
	//	window.canvas.context.putImageData(imageData, 0, 0);

	//	window.canvas.imageData = undefined;
	//	window.canavs.context = undefined;
	//}
}