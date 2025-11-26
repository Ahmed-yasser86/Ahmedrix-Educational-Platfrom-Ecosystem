// ===== FUTURISTIC ADMIN PANEL - 2026 JS =====

document.addEventListener('DOMContentLoaded', function () {

    // === Navbar Scroll Effect ===
    const navbar = document.querySelector('.futuristic-navbar');
    let lastScroll = 0;

    window.addEventListener('scroll', function () {
        const currentScroll = window.pageYOffset;

        if (currentScroll > 50) {
            navbar.style.background = 'rgba(10, 14, 39, 0.98)';
            navbar.style.boxShadow = '0 4px 30px rgba(0, 0, 0, 0.5)';
        } else {
            navbar.style.background = 'rgba(10, 14, 39, 0.85)';
            navbar.style.boxShadow = '0 4px 30px rgba(0, 0, 0, 0.3)';
        }

        lastScroll = currentScroll;
    });

    // === AI Assistant Badge Interaction ===
    const aiAssistant = document.getElementById('aiAssistant');

    if (aiAssistant) {
        aiAssistant.addEventListener('click', function () {
            // Add your AI assistant logic here
            this.style.transform = 'scale(0.9)';
            setTimeout(() => {
                this.style.transform = 'scale(1)';
            }, 100);

            // Example: Show AI panel or tooltip
            console.log('AI Assistant activated');
        });
    }

    // === Dropdown Animation Enhancement ===
    const dropdownItems = document.querySelectorAll('.futuristic-dropdown-item');

    dropdownItems.forEach((item, index) => {
        item.style.animationDelay = `${index * 0.05}s`;
        item.style.animation = 'fadeInUp 0.3s ease-out forwards';
    });

    // === Smooth Scroll for Anchor Links ===
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            const href = this.getAttribute('href');
            if (href !== '#' && document.querySelector(href)) {
                e.preventDefault();
                document.querySelector(href).scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // === Active Link Highlighting ===
    const navLinks = document.querySelectorAll('.futuristic-link');
    const currentPath = window.location.pathname;

    navLinks.forEach(link => {
        const linkPath = link.getAttribute('href');
        if (linkPath && currentPath.includes(linkPath) && linkPath !== '/') {
            link.style.background = 'var(--glass-bg)';
            link.style.borderBottom = '2px solid var(--accent-cyan)';
        }
    });

    // === Parallax Effect for Background ===
    window.addEventListener('mousemove', function (e) {
        const moveX = (e.clientX - window.innerWidth / 2) * 0.01;
        const moveY = (e.clientY - window.innerHeight / 2) * 0.01;

        document.body.style.backgroundPosition = `${50 + moveX}% ${50 + moveY}%`;
    });

    // === Dropdown Auto-Close on Click Outside ===
    document.addEventListener('click', function (e) {
        const dropdowns = document.querySelectorAll('.futuristic-dropdown');
        dropdowns.forEach(dropdown => {
            if (!dropdown.contains(e.target)) {
                const menu = dropdown.querySelector('.dropdown-menu');
                if (menu && menu.classList.contains('show')) {
                    menu.classList.remove('show');
                }
            }
        });
    });

    // === Loading Animation for Page Transitions ===
    const links = document.querySelectorAll('a:not([href^="#"]):not([target="_blank"])');

    links.forEach(link => {
        link.addEventListener('click', function (e) {
            if (this.hostname === window.location.hostname) {
                const main = document.querySelector('.futuristic-main');
                main.style.animation = 'fadeOut 0.3s ease-out';
            }
        });
    });

    // === AI Badge Tooltip (Optional Enhancement) ===
    if (aiAssistant) {
        const tooltip = document.createElement('div');
        tooltip.className = 'ai-tooltip';
        tooltip.textContent = 'AI Learning Assistant';
        tooltip.style.cssText = `
            position: fixed;
            bottom: 100px;
            right: 30px;
            background: rgba(10, 14, 39, 0.95);
            color: var(--accent-cyan);
            padding: 0.5rem 1rem;
            border-radius: 8px;
            font-size: 0.85rem;
            opacity: 0;
            pointer-events: none;
            transition: opacity 0.3s ease;
            border: 1px solid var(--border-glow);
            z-index: 999;
        `;
        document.body.appendChild(tooltip);

        aiAssistant.addEventListener('mouseenter', () => {
            tooltip.style.opacity = '1';
        });

        aiAssistant.addEventListener('mouseleave', () => {
            tooltip.style.opacity = '0';
        });
    }

    // === Keyboard Shortcuts ===
    document.addEventListener('keydown', function (e) {
        // Ctrl/Cmd + K for AI Assistant
        if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
            e.preventDefault();
            if (aiAssistant) {
                aiAssistant.click();
            }
        }
    });

});

// === Fade Out Animation ===
const style = document.createElement('style');
style.textContent = `
    @keyframes fadeOut {
        to {
            opacity: 0;
            transform: translateY(-20px);
        }
    }
`;
document.head.appendChild(style);

// ===== REGISTER MODAL FUNCTIONALITY =====

// Password Toggle Function
function togglePassword(fieldId) {
    const field = document.getElementById(fieldId);
    const button = event.currentTarget;
    const icon = button.querySelector('i');

    if (field.type === 'password') {
        field.type = 'text';
        icon.classList.remove('fa-eye');
        icon.classList.add('fa-eye-slash');
    } else {
        field.type = 'password';
        icon.classList.remove('fa-eye-slash');
        icon.classList.add('fa-eye');
    }
}

// Input Animation on Focus
document.addEventListener('DOMContentLoaded', function () {
    const inputs = document.querySelectorAll('.futuristic-input');

    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.parentElement.classList.add('input-focused');
        });

        input.addEventListener('blur', function () {
            this.parentElement.classList.remove('input-focused');
        });
    });

});

// ===================================
// CATEGORIES MANAGEMENT - 2026 JS
// ===================================

document.addEventListener('DOMContentLoaded', function () {
    initializeCategoriesPage();
});

function initializeCategoriesPage() {
    // Search functionality
    const searchInput = document.getElementById('categorySearch');
    if (searchInput) {
        searchInput.addEventListener('input', debounce(handleSearch, 300));
    }

    // Card animations on scroll
    observeCardAnimations();

    // Delete confirmation
    attachDeleteConfirmations();

    // AI insights animation
    animateAIInsights();
}

// Search handler with AI simulation
function handleSearch(e) {
    const searchTerm = e.target.value.toLowerCase();
    const cards = document.querySelectorAll('.category-card');

    cards.forEach(card => {
        const title = card.querySelector('.category-title').textContent.toLowerCase();
        const description = card.querySelector('.category-description').textContent.toLowerCase();

        if (title.includes(searchTerm) || description.includes(searchTerm)) {
            card.style.display = 'block';
            card.style.animation = 'fadeIn 0.4s ease-out';
        } else {
            card.style.display = 'none';
        }
    });

    // Show "no results" message if needed
    const visibleCards = document.querySelectorAll('.category-card[style*="display: block"]');
    if (visibleCards.length === 0 && searchTerm !== '') {
        showNoResultsMessage();
    } else {
        hideNoResultsMessage();
    }
}

// Debounce utility
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Intersection Observer for card animations
function observeCardAnimations() {
    const cards = document.querySelectorAll('.category-card');

    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }, index * 100);
                observer.unobserve(entry.target);
            }
        });
    }, {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    });

    cards.forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(30px)';
        card.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(card);
    });
}

// Delete confirmation
function attachDeleteConfirmations() {
    const deleteButtons = document.querySelectorAll('.btn-delete');

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (e) {
            const confirmed = confirm('Are you sure you want to delete this category? This action cannot be undone.');
            if (!confirmed) {
                e.preventDefault();
            }
        });
    });
}

// Animate AI insights with counting effect
function animateAIInsights() {
    const insightValues = document.querySelectorAll('.insight-value');

    insightValues.forEach(element => {
        const text = element.textContent.trim();
        const number = parseInt(text);

        if (!isNaN(number)) {
            animateCounter(element, 0, number, 1500);
        }
    });
}

// ===================================
// ENHANCED CATEGORIES MANAGEMENT - 2026 JS
// ===================================

document.addEventListener('DOMContentLoaded', function () {
    initializeEnhancedCategories();
});

function initializeEnhancedCategories() {
    // Enhanced search with debouncing
    const searchInput = document.getElementById('categorySearch');
    if (searchInput) {
        let searchTimeout;
        searchInput.addEventListener('input', function (e) {
            clearTimeout(searchTimeout);

            // Add loading state
            const aiIcon = searchInput.parentElement.querySelector('.ai-badge');
            if (aiIcon) {
                aiIcon.textContent = '...';
                aiIcon.style.animation = 'spin 1s linear infinite';
            }

            searchTimeout = setTimeout(() => {
                handleEnhancedSearch(e);
                if (aiIcon) {
                    aiIcon.textContent = 'AI';
                    aiIcon.style.animation = 'badgePulse 3s ease-in-out infinite';
                }
            }, 400);
        });

        // Search on Enter key
        searchInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') {
                clearTimeout(searchTimeout);
                handleEnhancedSearch(e);
            }
        });
    }

    // Enhanced card animations with stagger effect
    observeEnhancedCardAnimations();

// Delete