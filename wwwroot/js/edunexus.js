document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.getElementById("appSidebar");
    const toggleButton = document.getElementById("sidebarToggle");
    const overlay = document.getElementById("sidebarOverlay");

    function closeSidebar() {
        sidebar?.classList.remove("open");
        overlay?.classList.remove("show");
    }

    toggleButton?.addEventListener("click", function () {
        sidebar?.classList.toggle("open");
        overlay?.classList.toggle("show");
    });

    overlay?.addEventListener("click", closeSidebar);

    document
        .querySelectorAll("[data-alert-close]")
        .forEach(function (button) {
            button.addEventListener("click", function () {
                button.closest(".app-alert")?.remove();
            });
        });

    document
        .querySelectorAll("[data-auto-dismiss]")
        .forEach(function (alertElement) {
            window.setTimeout(function () {
                alertElement.style.transition =
                    "opacity 0.25s ease, transform 0.25s ease";

                alertElement.style.opacity = "0";
                alertElement.style.transform =
                    "translateY(-6px)";

                window.setTimeout(function () {
                    alertElement.remove();
                }, 250);
            }, 5000);
        });
});