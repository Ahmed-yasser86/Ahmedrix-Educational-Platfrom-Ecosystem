// Ahmedrix Platform Enhancements
(function () {
    'use strict';

    function init() {
        console.log('Ahmedrix AI Platform - Initializing enhanced UI');

        // Initialize AI Assistant
        initAIAssistant();

        // Add Bootstrap 5 tooltips if needed
        if (typeof bootstrap !== 'undefined') {
            initTooltips();
        }
    }

    function initAIAssistant() {
        const aiBadge = document.getElementById('aiAssistant');
        if (!aiBadge) return;

        aiBadge.addEventListener('click', function (e) {
            e.preventDefault();
            showAIMessage();
        });
    }

    function showAIMessage() {
        const messages = [
            "🤖 AI Assistant: \"Ready to optimize your learning journey?\"",
            "🤖 AI Assistant: \"I can help explain concepts, debug code, or suggest learning paths!\"",
            "🤖 AI Assistant: \"What would you like to learn today?\""
        ];

        const randomMessage = messages[Math.floor(Math.random() * messages.length)];
        alert(randomMessage);
    }

    function initTooltips() {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();