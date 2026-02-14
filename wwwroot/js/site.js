// Simple enhancements
document.addEventListener('DOMContentLoaded', function () {
    // AI Assistant
    const aiAssistant = document.getElementById('aiAssistant');
    if (aiAssistant) {
        aiAssistant.addEventListener('click', function () {
            // Your AI assistant functionality
        });
    }

    // Card hover effects
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-4px)';
        });

        card.addEventListener('mouseleave', function () {
            this.style.transform = 'translateY(0)';
        });
    });
});