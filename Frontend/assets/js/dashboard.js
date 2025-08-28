import { apiFetch } from './api.js';
import { showMessage, hideMessage, initializeModals, hideModal } from './ui.js';

export const initDashboardPage = () => {
    const welcomeMessage = document.getElementById('welcome-message');
    const pointsBalance = document.getElementById('points-balance');
    const logoutBtn = document.getElementById('logout-btn');

    const logout = () => {
        localStorage.removeItem('jwtToken');
        window.location.href = 'index.html';
    };

    const fetchPoints = async () => {
        try {
            const data = await apiFetch('/Points');
            pointsBalance.textContent = data.totalPoints;

            const token = localStorage.getItem('jwtToken');
            const tokenPayload = JSON.parse(atob(token.split('.')[1]));
            welcomeMessage.textContent = `Welcome, ${tokenPayload.name}!`;
        } catch (error) {
            showMessage(error.message, true);
            if (error.message.toLowerCase().includes('unauthorized')) {
                setTimeout(logout, 2000);
            }
        }
    };

    // --- Event Listeners ---
    logoutBtn.addEventListener('click', logout);
    initializeModals();

    // Add Points Form
    document.getElementById('add-points-form').addEventListener('submit', async (e) => {
        e.preventDefault();
        hideMessage();
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

    // Redeem Coupon Form
    document.getElementById('redeem-coupon-form').addEventListener('submit', async (e) => {
        e.preventDefault();
        hideMessage();
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
            hideModal('redeem-coupon-modal');
            e.target.reset();
        } catch (error) {
            showMessage(error.message, true);
        }
    });

    // --- Initial Load ---
    fetchPoints();
};
