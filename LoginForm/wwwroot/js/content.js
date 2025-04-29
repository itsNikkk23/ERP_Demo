document.addEventListener('DOMContentLoaded', function () {
    const toggleBtn = document.getElementById('sidebarToggle');
    const wrapper = document.querySelector('.wrapper');

    toggleBtn.addEventListener('click', function () {
        wrapper.classList.toggle('toggled');
    });
});