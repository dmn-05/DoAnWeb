(function () {
  const imagesInput = document.getElementById('imagesInput');
  const previewDiv = document.getElementById('imagePreview');

  if (!imagesInput || !previewDiv) {
    console.warn('imagesInput hoặc imagePreview chưa tồn tại');
    return;
  }

  imagesInput.addEventListener('change', function () {
    previewDiv.innerHTML = '';

    Array.from(this.files).forEach(file => {
      if (!file.type.startsWith('image/')) return;

      const img = document.createElement('img');
      img.src = URL.createObjectURL(file);
      img.style.width = '120px';
      img.style.height = '150px';
      img.style.objectFit = 'cover';
      img.style.border = '1px solid #ccc';
      img.style.borderRadius = '8px';

      previewDiv.appendChild(img);
    });
  });
})();
