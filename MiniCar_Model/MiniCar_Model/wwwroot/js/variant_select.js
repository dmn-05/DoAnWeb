//document.addEventListener("DOMContentLoaded", function () {

//  const select = document.getElementById("variantSelect");
//  if (!select) return;

//  const priceEl = document.getElementById("price");
//  const qtyEl = document.getElementById("stock-count");
//  const statusEl = document.getElementById("stock-status");

//  function render() {
//    const opt = select.options[select.selectedIndex];
//    if (!opt) return;

//    const price = Number(opt.dataset.price);
//    const qty = Number(opt.dataset.qty);

//    priceEl.textContent = price.toLocaleString('vi-VN') + " đ";
//    qtyEl.textContent = qty;

//    if (qty > 0) {
//      statusEl.textContent = "Còn hàng";
//      statusEl.className = "text-success";
//      qtyEl.className = "ml-2 text-success";
//    } else {
//      statusEl.textContent = "Hết hàng";
//      statusEl.className = "text-danger";
//      qtyEl.className = "ml-2 text-danger";
//    }
//  }

//  // 👉 Bắt click CHỈ trong nice-select
//  const observer = new MutationObserver(() => {
//    const nice = document.querySelector(".nice-select");
//    if (!nice) return;

//    nice.addEventListener("click", function (e) {
//      const option = e.target.closest(".option");
//      if (!option) return;

//      select.value = option.dataset.value;
//      render();
//    });

//    observer.disconnect(); // 🔥 chỉ init 1 lần
//  });

//  observer.observe(document.body, { childList: true, subtree: true });

//  render();
//});
document.addEventListener("DOMContentLoaded", function () {

  const select = document.getElementById("variantSelect");
  if (!select) return;

  const priceEl = document.getElementById("price");
  const qtyEl = document.getElementById("stock-count");
  const statusEl = document.getElementById("stock-status");

  async function loadRealtime() {
    const variantId = select.value;
    if (!variantId) return;

    const res = await fetch(`/Product/GetVariantInfo?variantId=${variantId}`);
    if (!res.ok) return;

    const data = await res.json();

    priceEl.textContent =
      Number(data.price).toLocaleString("vi-VN") + " đ";

    qtyEl.textContent = data.quantity;

    if (data.quantity > 0) {
      statusEl.textContent = "Còn hàng";
      statusEl.className = "text-success";
      qtyEl.className = "ml-2 text-success";
    } else {
      statusEl.textContent = "Hết hàng";
      statusEl.className = "text-danger";
      qtyEl.className = "ml-2 text-danger";
    }
  }

  // 👉 Hook với nice-select
  const observer = new MutationObserver(() => {
    const nice = document.querySelector(".nice-select");
    if (!nice) return;

    nice.addEventListener("click", function (e) {
      const option = e.target.closest(".option");
      if (!option) return;

      select.value = option.dataset.value;
      loadRealtime(); // 🔥 GỌI SERVER
    });

    observer.disconnect();
  });

  observer.observe(document.body, { childList: true, subtree: true });

  loadRealtime(); // load lần đầu
});

