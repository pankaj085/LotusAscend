import { apiFetch } from './api.js';
import { showMessage, initializeModals, hideModal } from './ui.js';

export const initDashboardPage = () => {
    // --- Get all UI elements ---
    const welcomeMessage = document.getElementById('welcome-message');
    const pointsBalance = document.getElementById('points-balance');
    const logoutBtn = document.getElementById('logout-btn');
    const historyList = document.getElementById('history-list');
    const latestCouponContainer = document.getElementById('latest-coupon-container');
    const latestCouponCode = document.getElementById('latest-coupon-code');
    const addPointsForm = document.getElementById('add-points-form');
    const redeemCouponForm = document.getElementById('redeem-coupon-form');
    const prevPageBtn = document.getElementById('prev-page-btn');
    const nextPageBtn = document.getElementById('next-page-btn');
    const pageInfo = document.getElementById('page-info');

    // --- State for Pagination ---
    let currentPage = 1;
    let totalPages = 1;

    /**
     * Fetches the member's points and username and updates the UI.
     */
    const fetchMemberData = async () => {
        try {
            const data = await apiFetch('/Points');
            const token = localStorage.getItem('jwtToken');
            if (token) {
                // Decode the JWT to get the username
                const payload = JSON.parse(atob(token.split('.')[1]));
                welcomeMessage.textContent = `Welcome, ${payload.name}!`;
            }
            pointsBalance.textContent = data.totalPoints;
        } catch (error) {
            // If the token is invalid or expired, redirect to login
            if (error.message.includes('401')) {
                logout();
            } else {
                showMessage('Could not load your data. Please try again.', true);
            }
        }
    };

    /**
     * Fetches a specific page of the transaction history and renders it.
     */
    const fetchAndDisplayHistory = async () => {
        try {
            const data = await apiFetch(`/Points/history?page=${currentPage}&pageSize=5`);
            historyList.innerHTML = ''; // Clear previous list
            totalPages = data.totalPages;

            if (data.items.length === 0) {
                historyList.innerHTML = '<li><p>No transactions yet.</p></li>';
                pageInfo.textContent = 'Page 1 of 1';
                prevPageBtn.disabled = true;
                nextPageBtn.disabled = true;
                return;
            }

            data.items.forEach(item => {
                const li = document.createElement('li');
                const transactionDate = new Date(item.date).toLocaleString();
                const amountClass = item.type === 'Points Earned' ? 'earned' : 'redeemed';
                
                // --- THIS IS THE CORRECTED HTML TEMPLATE ---
                li.innerHTML = `
                    <div class="history-details">
                        <strong>${item.type}</strong>
                        <p>${item.description}</p>
                        <span>${transactionDate}</span>
                    </div>
                    <div class="history-amount ${amountClass}">${item.amount}</div>`;
                historyList.appendChild(li);
            });
            
            // Update pagination controls
            pageInfo.textContent = `Page ${currentPage} of ${totalPages}`;
            prevPageBtn.disabled = currentPage <= 1;
            nextPageBtn.disabled = currentPage >= totalPages;

        } catch (error) {
            // Intentionally silent to not bother user if history fails to load
        }
    };

    /**
     * Logs the user out by clearing the token and redirecting.
     */
    const logout = () => {
        localStorage.removeItem('jwtToken');
        window.location.href = 'index.html';
    };

    // --- Event Handlers ---

    const handleAddPoints = async (e) => {
        e.preventDefault();
        const purchaseAmount = document.getElementById('purchase-amount').value;
        try {
            const data = await apiFetch('/Points/add', {
                method: 'POST',
                body: JSON.stringify({ purchaseAmount: parseFloat(purchaseAmount) })
            });
            pointsBalance.textContent = data.totalPoints;
            showMessage('Points added successfully!');
            hideModal('add-points-modal');
            e.target.reset();
            currentPage = 1; // Go to first page to see new transaction
            fetchAndDisplayHistory();
        } catch (error) {
            showMessage(error.message, true);
        }
    };

    const handleRedeemCoupon = async (e) => {
        e.preventDefault();
        const pointsToRedeem = document.getElementById('points-to-redeem').value;
        if (!pointsToRedeem) {
            showMessage('Please select an amount to redeem.', true);
            return;
        }
        try {
            const data = await apiFetch('/Coupon/redeem', {
                method: 'POST',
                body: JSON.stringify({ pointsToRedeem: parseInt(pointsToRedeem) })
            });
            pointsBalance.textContent = data.remainingPoints;
            showMessage(data.message);
            latestCouponCode.textContent = data.newCouponCode;
            latestCouponContainer.classList.remove('hidden');
            hideModal('redeem-coupon-modal');
            e.target.reset();
            currentPage = 1; // Go to first page to see new transaction
            fetchAndDisplayHistory();
        } catch (error) {
            showMessage(error.message, true);
        }
    };

    const goToPrevPage = () => {
        if (currentPage > 1) {
            currentPage--;
            fetchAndDisplayHistory();
        }
    };

    const goToNextPage = () => {
        if (currentPage < totalPages) {
            currentPage++;
            fetchAndDisplayHistory();
        }
    };

    // --- Initial Setup ---

    // 1. Attach all event listeners
    logoutBtn.addEventListener('click', logout);
    addPointsForm.addEventListener('submit', handleAddPoints);
    redeemCouponForm.addEventListener('submit', handleRedeemCoupon);
    prevPageBtn.addEventListener('click', goToPrevPage);
    nextPageBtn.addEventListener('click', goToNextPage);
    initializeModals();

    // 2. Load initial data when the page loads
    fetchMemberData();
    fetchAndDisplayHistory();
};