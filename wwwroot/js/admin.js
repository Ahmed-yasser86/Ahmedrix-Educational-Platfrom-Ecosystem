// Admin Panel JavaScript
document.addEventListener('DOMContentLoaded', function () {
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Table row selection
    document.querySelectorAll('.selectable-row').forEach(row => {
        row.addEventListener('click', function () {
            this.classList.toggle('selected');
        });
    });

    // Status toggles
    document.querySelectorAll('.status-toggle').forEach(toggle => {
        toggle.addEventListener('change', function () {
            const status = this.checked ? 'active' : 'inactive';
            const itemId = this.dataset.id;

            // Here you would typically make an API call
            console.log(`Status changed for ${itemId}: ${status}`);
        });
    });

    // Search functionality
    const searchInput = document.querySelector('.admin-search');
    if (searchInput) {
        searchInput.addEventListener('input', function (e) {
            const searchTerm = e.target.value.toLowerCase();
            // Implement search logic
        });
    }

    // Load statistics
    loadAdminStats();
});

function loadAdminStats() {
    // This would typically be an API call
    console.log('Loading admin statistics...');
}

// Export functions if needed
window.AdminPanel = {
    refreshData: function () {
        console.log('Refreshing admin data...');
        // Implementation here
    },
    showAlert: function (message, type = 'info') {
        // Show alert implementation
    }
};