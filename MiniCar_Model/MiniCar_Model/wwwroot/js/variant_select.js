	document.addEventListener("DOMContentLoaded", function() {
		setTimeout(function () {
			const variantSelect = document.getElementById("variantSelect");
			const priceDisplay = document.getElementById("price");
			const stockCountDisplay = document.getElementById("stock-count");
			const stockStatusDisplay = document.getElementById("stock-status");

			function updateProductInfo() {
				const selectedOption = variantSelect.options[variantSelect.selectedIndex];
				const price = parseFloat(selectedOption.getAttribute("data-price"));
				const qty = parseInt(selectedOption.getAttribute("data-qty"));

				// Cập nhật giá
				priceDisplay.textContent = price.toLocaleString('vi-VN') + " đ";

				// Cập nhật số lượng tồn
				stockCountDisplay.textContent = qty;

				// Cập nhật trạng thái
				if (qty > 0) {
					stockStatusDisplay.textContent = "Còn hàng";
					stockStatusDisplay.className = "text-success";
					stockCountDisplay.className = "ml-2 text-success";
				} else {
					stockStatusDisplay.textContent = "Hết hàng";
					stockStatusDisplay.className = "text-danger";
					stockCountDisplay.className = "ml-2 text-danger";
				}
			}

			// 1. Lắng nghe event change từ select gốc
			variantSelect.addEventListener('change', function () {
				console.log("Change event fired!");
				updateProductInfo();
			});

			// 2. Lắng nghe click trực tiếp vào nice-select options
			document.addEventListener('click', function (e) {
				if (e.target.classList.contains('option') && e.target.closest('.nice-select')) {
					setTimeout(updateProductInfo, 50);
				}
			});

			// 3. Khởi tạo lần đầu
			updateProductInfo();
		}, 500); // Tăng timeout lên 500ms
	});
