document.addEventListener("DOMContentLoaded", function () {

  const select = document.getElementById("variantSelect");
  if (!select) return;

  const priceEl = document.getElementById("price");

  async function loadRealtime() {
    const variantId = select.value;
    if (!variantId) return;

    const res = await fetch(`/Product/GetVariantInfo?variantId=${variantId}`);
    if (!res.ok) return;

    const data = await res.json();

    priceEl.textContent =
      Number(data.price).toLocaleString("vi-VN") + " đ";

    const slider = document.querySelector(".s_Product_carousel");
    if (!slider || !data.images || data.images.length === 0) return;

    let html = "";
    data.images.forEach(img => {
      html += `
        <div class="item single-prd-item">
          <img class="img-fluid" src="${img}" />
        </div>`;
    });

    if (window.$ && $.fn.owlCarousel) {
      const $slider = $(slider);

      if ($slider.hasClass("owl-loaded")) {
        $slider.trigger("destroy.owl.carousel");
        $slider.removeClass("owl-loaded");
        $slider.find(".owl-stage-outer").children().unwrap();
        $slider.find(".owl-stage").remove();
      }

      $slider.html(html);

      $slider.owlCarousel({
        items: 1,
        loop: data.images.length > 1,
        nav: false,
        dots: true
      });
    }
  }

  const observer = new MutationObserver(() => {
    const nice = document.querySelector(".nice-select");
    if (!nice) return;

    nice.addEventListener("click", function (e) {
      const option = e.target.closest(".option");
      if (!option) return;

      select.value = option.dataset.value;
      loadRealtime();
    });

    observer.disconnect();
  });

  observer.observe(document.body, { childList: true, subtree: true });

  loadRealtime();
});
