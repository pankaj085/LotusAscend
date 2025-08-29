import { apiFetch } from './api.js';
import { showMessage, initializeModals, hideModal } from './ui.js';

export const initDashboardPage = () => {
    // --- Get all UI elements ---
    const welcomeMessage = document.getElementById('welcome-message');
    const pointsBalance = document.getElementById('points-balance');
    const logoutBtn = document.getElementById('logout-btn');
    const latestCouponContainer = document.getElementById('latest-coupon-container');
    const latestCouponCode = document.getElementById('latest-coupon-code');
    const addPointsForm = document.getElementById('add-points-form');
    const redeemCouponForm = document.getElementById('redeem-coupon-form');

    // --- Function to fetch and display data ---
    const fetchPoints = async () => {
        try {
            const data = await apiFetch('/Points');
            const token = localStorage.getItem('jwtToken');
            if (token) {
                const payload = JSON.parse(atob(token.split('.')[1]));
                welcomeMessage.textContent = `Welcome, ${payload.name}!`;
            }
            pointsBalance.textContent = data.totalPoints;
        } catch (error) {
            if (error.message.includes('401')) {
                window.location.href = 'index.html';
            }
        }
    };

    const logout = () => {
        localStorage.removeItem('jwtToken');
        window.location.href = 'index.html';
    };

    // --- Event Listeners ---
    logoutBtn.addEventListener('click', logout);
    initializeModals();

    addPointsForm.addEventListener('submit', async (e) => {
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
        } catch (error) {
            showMessage(error.message, true);
        }
    });

    redeemCouponForm.addEventListener('submit', async (e) => {
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

        } catch (error) {
            showMessage(error.message, true);
        }
    });

    // --- Initial Load ---
    fetchPoints();
};