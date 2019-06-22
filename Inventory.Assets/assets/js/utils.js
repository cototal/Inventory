// Handle expand/collapse of bulma navbar
export function bulmaNavbar() {
    $(".navbar-burger").on("click", () => {
        // Toggle the "is-active" class on both the "navbar-burger" and the "navbar-menu"
        $(".navbar-burger").toggleClass("is-active");
        $(".navbar-menu").toggleClass("is-active");
    });
}
