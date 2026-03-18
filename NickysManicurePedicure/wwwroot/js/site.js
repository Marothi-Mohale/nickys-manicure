document.addEventListener("DOMContentLoaded", () => {
    const notices = document.querySelectorAll(".toast-stack .alert");
    notices.forEach((notice) => {
        window.setTimeout(() => {
            notice.classList.add("d-none");
        }, 5000);
    });
});
