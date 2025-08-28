import { initAuthPage } from './auth.js';
import { initDashboardPage } from './dashboard.js';

document.addEventListener('DOMContentLoaded', () => {
    // --- STATE & ROUTING ---
    const token = localStorage.getItem('jwtToken');
    const currentPage = window.location.pathname.split('/').pop();

    // Redirect if on the wrong page based on token existence
    if (currentPage === 'dashboard.html' && !token) {
        window.location.href = 'index.html';
        return;
    }
    if ((currentPage === 'index.html' || currentPage === '') && token) {
        window.location.href = 'dashboard.html';
        return;
    }

    // Load the correct module based on the current page
    if (currentPage === 'index.html' || currentPage === '') {
        initAuthPage();
    } else if (currentPage === 'dashboard.html') {
        initDashboardPage();
    }
});