
  document.addEventListener("DOMContentLoaded", function () {

  const select = document.getElementById("variantSelectUI");
  if (!select) return;

  const priceEl = document.getElementById("price");
  const qtyEl = document.getElementById("stock-count");
  const statusEl = document.getElementById("stock-status");
  const carousel = $(".s_Product_carousel");

  async function loadVariant(variantId) {
    if (!variantId) return;

  const res = await fetch(`/Product/GetVariantInfo?variantId=${variantId}`);
  if (!res.ok) return;

  const data = await res.json();

  // 🔹 Giá
  priceEl.innerText =
  Number(data.price).toLocaleString("vi-VN") + " đ";

  // 🔹 Số lượng
  qtyEl.innerText = data.quantity;

  // 🔹 Trạng thái
  statusEl.innerText = data.status;
  statusEl.classList.remove("text-success", "text-danger");
    statusEl.classList.add(data.quantity > 0 ? "text-success" : "text-danger");

  // 🔹 Reset OWL
  if (carousel.hasClass("owl-loaded")) {
    carousel.trigger("destroy.owl.carousel");
  carousel.removeClass("owl-loaded");
  carousel.find(".owl-stage-outer").children().unwrap();
  carousel.find(".owl-stage").remove();
    }

  carousel.html("");

    // 🔹 Load ảnh mới
    data.images.forEach(img => {
    carousel.append(`
        <div class="single-prd-item d-flex align-items-center justify-content-center">
          <img src="${img}" class="img-fluid product-img"/>
        </div>
      `);
    });

  // 🔹 Init lại OWL
  carousel.owlCarousel({
    items: 1,
      loop: data.images.length > 1,
      nav: false,
      dots: true
    });
  }

  // ✅ Load variant mặc định khi mở trang
  loadVariant(select.value);

  // ✅ BẮT NICE-SELECT
  const observer = new MutationObserver(() => {
    const nice = document.querySelector(".nice-select");
  if (!nice) return;

  nice.addEventListener("click", function (e) {
      const option = e.target.closest(".option");
  if (!option) return;

  const variantId = option.dataset.value;

  // 🔁 Đồng bộ lại select ẩn
  select.value = variantId;

  // 🔁 Load data
  loadVariant(variantId);
    });

  observer.disconnect();
  });

  observer.observe(document.body, {childList: true, subtree: true });
});
