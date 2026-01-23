document.addEventListener("DOMContentLoaded", function () {

  const input = document.getElementById("imageInput");
  const preview = document.getElementById("previewImages");

  if (!input || !preview) return;

  input.addEventListener("change", function () {
    preview.innerHTML = "";

    Array.from(this.files).forEach(file => {
      if (!file.type.startsWith("image/")) return;

      const img = document.createElement("img");
      img.className = "img-thumbnail";
      img.style.width = "120px";
      img.style.height = "120px";
      img.style.objectFit = "cover";

      img.src = URL.createObjectURL(file);

      preview.appendChild(img);
    });
  });

});
