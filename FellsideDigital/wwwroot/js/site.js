window.fellsideScroll = (function () {
    let cards = [];
    let isMobile = window.matchMedia("(max-width: 768px)").matches;

    function initCards() {
        cards = Array.from(document.querySelectorAll(".scroll-card"));

        cards.forEach((card) => {
            const ripple = card.querySelector(".ripple");

            // Click ripple + navigate
            card.addEventListener("click", () => {
                if (ripple) {
                    ripple.style.transform = "scale(1.8)";
                    ripple.style.opacity = "1";

                    setTimeout(() => {
                        ripple.style.transform = "scale(0)";
                        ripple.style.opacity = "0";
                    }, 600);
                }

                const link = card.dataset.link || card.getAttribute("data-link");
                if (link) {
                    setTimeout(() => {
                        window.location.href = link;
                    }, 750);
                }
            });

            // Mouse hover coordinates
            card.addEventListener("mousemove", (e) => {
                const rect = card.getBoundingClientRect();
                card.style.setProperty("--x", `${e.clientX - rect.left}px`);
                card.style.setProperty("--y", `${e.clientY - rect.top}px`);
            });
        });

        // Trigger animation once initially
        animateOnScroll();
    }

    function animateOnScroll() {
        if (isMobile) return;
        const vh = window.innerHeight;

        cards.forEach((card) => {
            const rect = card.getBoundingClientRect();
            const start = vh * 0.9;
            const end = vh * 0.25;

            let progress = (start - rect.top) / (start - end);
            progress = Math.min(Math.max(progress, 0), 1);

            const translateY = 80 * (1 - progress);
            const scale = 0.97 + 0.03 * progress;
            card.style.transform = `translateY(${translateY}px) scale(${scale})`;

            card.style.opacity = rect.top <= vh * 0.5 ? 1 : progress;
        });
    }

    // Smooth scroll to offerings
    function toOfferings() {
        const el = document.getElementById("offerings");
        if (el) el.scrollIntoView({ behavior: "smooth" });
    }

    // Initialize scroll listener and cards
    function init() {
        document.documentElement.style.scrollBehavior = "smooth";
        initCards();

        window.addEventListener("scroll", animateOnScroll);
        window.addEventListener("resize", () => {
            isMobile = window.matchMedia("(max-width: 768px)").matches;
            animateOnScroll();
        });
        window.addEventListener("load", animateOnScroll);
    }

    return {
        init,
        toOfferings,
        animateOnScroll
    };
})();
