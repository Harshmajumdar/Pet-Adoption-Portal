// ==========================================================================
// PAWS HOME - DYNAMIC CLIENT-SIDE INTERACTIONS
// ==========================================================================

document.addEventListener("DOMContentLoaded", function () {
    // 1. Auto-dismiss alerts after 5 seconds
    setTimeout(function () {
        document.querySelectorAll(".alert:not(.alert-permanent)").forEach(function (el) {
            el.classList.remove("show");
            setTimeout(function () { el.remove(); }, 300);
        });
    }, 5000);

    // 2. Real-time instant search & filter for Find a Pet page
    const searchInput = document.querySelector("#petLiveSearchInput");
    const petCards = document.querySelectorAll(".pet-grid-item");
    const countBadge = document.querySelector("#petResultCount");
    const emptyState = document.querySelector("#petNoResultsState");

    function filterPets() {
        if (!petCards.length) return;

        const query = (searchInput ? searchInput.value : "").trim().toLowerCase();
        const activeChip = document.querySelector(".category-chip.active");
        const selectedSpecies = activeChip ? activeChip.getAttribute("data-species") || "" : "";

        let visibleCount = 0;

        petCards.forEach(function (item) {
            const petName = (item.getAttribute("data-name") || "").toLowerCase();
            const petBreed = (item.getAttribute("data-breed") || "").toLowerCase();
            const petSpecies = (item.getAttribute("data-species") || "").toLowerCase();

            const matchesQuery = !query || petName.includes(query) || petBreed.includes(query);
            const matchesSpecies = !selectedSpecies || petSpecies === selectedSpecies.toLowerCase();

            if (matchesQuery && matchesSpecies) {
                item.style.display = "";
                item.classList.add("fade-in-up");
                visibleCount++;
            } else {
                item.style.display = "none";
            }
        });

        if (countBadge) {
            countBadge.textContent = visibleCount + (visibleCount === 1 ? " pet found" : " pets found");
        }

        if (emptyState) {
            emptyState.style.display = visibleCount === 0 ? "block" : "none";
        }
    }

    if (searchInput) {
        searchInput.addEventListener("input", filterPets);
    }

    document.querySelectorAll(".category-chip[data-species]").forEach(function (chip) {
        chip.addEventListener("click", function (e) {
            // If it's a client filter trigger
            if (this.hasAttribute("data-filter-client")) {
                e.preventDefault();
                document.querySelectorAll(".category-chip[data-species]").forEach(c => c.classList.remove("active"));
                this.classList.add("active");
                filterPets();
            }
        });
    });

    // 3. Heart Favorite Button micro-interaction
    document.querySelectorAll(".pet-heart-btn").forEach(function (btn) {
        btn.addEventListener("click", function () {
            this.style.transform = "scale(1.35)";
            setTimeout(() => {
                this.style.transform = "";
            }, 250);
        });
    });

    // 4. Quick Demo Login autofill helper
    const fillCredsBtn = document.querySelector("#fillDemoCredsBtn");
    if (fillCredsBtn) {
        fillCredsBtn.addEventListener("click", function () {
            const userField = document.querySelector("#Username");
            const passField = document.querySelector("#Password");
            if (userField && passField) {
                userField.value = "admin";
                passField.value = "admin123";
                userField.classList.add("is-valid");
                passField.classList.add("is-valid");
                this.innerHTML = '<i class="bi bi-check2-circle"></i> Credentials Filled!';
                setTimeout(() => {
                    this.innerHTML = '<i class="bi bi-lightning-fill"></i> Fill Demo Credentials (admin / admin123)';
                }, 2000);
            }
        });
    }

    // 5. Image Fallback Handling
    document.querySelectorAll("img.pet-img-element").forEach(function (img) {
        img.addEventListener("error", function () {
            this.style.display = "none";
            const fallback = this.nextElementSibling;
            if (fallback && fallback.classList.contains("pet-fallback-box")) {
                fallback.style.display = "flex";
            }
        });
    });
});
