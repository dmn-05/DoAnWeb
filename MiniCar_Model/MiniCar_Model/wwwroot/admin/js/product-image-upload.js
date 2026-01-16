const container_ful = document.getElementById('ful_container');
const btnAddFul = document.getElementById('btn_add_ful');
const hf_num_ful = document.getElementById('hf_num_ful');

// Hàm tạo preview khi chọn ảnh
function handlePreview(input) {
    const previewDiv = input.closest('.input-group').querySelector('.preview');
    previewDiv.innerHTML = "";

    if (input.files && input.files.length > 0) {
        Array.from(input.files).forEach(file => {
            if (file.type.startsWith("image/")) {
                const img = document.createElement("img");
                img.src = URL.createObjectURL(file);
                img.style.width = "120px";
                img.style.height = "150px";
                img.style.objectFit = "cover";
                img.style.border = "1px solid #ccc";
                img.style.borderRadius = "8px";
                previewDiv.appendChild(img);
            }
        });
    }
}

// Lắng nghe sự kiện change cho input đầu tiên
document.getElementById('ful_1').addEventListener('change', function () {
    handlePreview(this);
});

// Khi bấm nút "+"
btnAddFul.onclick = function () {
    const newIndex = parseInt(hf_num_ful.value) + 1;
    hf_num_ful.value = newIndex;

    const wrapper = document.createElement('div');
    wrapper.classList.add('mb-3', 'ful-group');

    const input = document.createElement('input');
    input.type = 'file';
    input.classList.add('form-control', 'ful-input');
    input.id = 'ful_' + newIndex;
    input.name = 'Images';
    input.multiple = true;

    const previewDiv = document.createElement('div');
    previewDiv.classList.add('preview', 'mt-2', 'd-flex', 'flex-wrap', 'gap-2');

    // Gắn sự kiện preview cho input mới
    input.addEventListener('change', function () {
        handlePreview(this);
    });

    wrapper.appendChild(input);
    wrapper.appendChild(previewDiv);
    container_ful.appendChild(wrapper);
};
