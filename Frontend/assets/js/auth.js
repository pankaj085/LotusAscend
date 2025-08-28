import { apiFetch } from './api.js';
import { showMessage, hideMessage } from './ui.js';

export const initAuthPage = () => {
    const form = document.getElementById('auth-form');
    if (!form) return;

    // Get all input elements
    const usernameInput = document.getElementById('username');
    const mobileInput = document.getElementById('mobile');
    const otpInput = document.getElementById('otp');

    const title = document.getElementById('form-title');
    const subtitle = document.getElementById('form-subtitle');
    const submitBtn = document.getElementById('submit-btn');
    const usernameGroup = document.getElementById('username-group');
    const mobileGroup = document.getElementById('mobile-group');
    const otpGroup = document.getElementById('otp-group');
    let mobileNumberForOtp = '';

    const switchToOtpMode = (mobile) => {
        mobileNumberForOtp = mobile;
        title.textContent = 'Verify Your Number';
        subtitle.textContent = `Enter the 4-digit OTP.`;
        
        // Hide registration fields and show OTP field
        usernameGroup.classList.add('hidden');
        mobileGroup.classList.add('hidden');
        otpGroup.classList.remove('hidden');

        // *** FIX: Update which fields are required ***
        usernameInput.required = false;
        mobileInput.required = false;
        otpInput.required = true;

        submitBtn.textContent = 'Verify';
        form.reset();
    };

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        hideMessage();
        submitBtn.disabled = true;

        // Registration Mode
        if (otpGroup.classList.contains('hidden')) {
            const username = usernameInput.value;
            const mobile = mobileInput.value;
            try {
                await apiFetch('/Member/register', {
                    method: 'POST',
                    body: JSON.stringify({ username, mobileNumber: mobile }),
                });
                showMessage('Registration successful! Please enter the dummy OTP.');
                switchToOtpMode(mobile);
            } catch (error) {
                showMessage(error.message, true);
            }
        }
        // Verification Mode
        else {
            const otp = otpInput.value;
            try {
                const data = await apiFetch('/Member/verify', {
                    method: 'POST',
                    body: JSON.stringify({ mobileNumber: mobileNumberForOtp, otp }),
                });
                localStorage.setItem('jwtToken', data.token);
                showMessage('Verification successful! Redirecting...');
                window.location.href = 'dashboard.html';
            } catch (error) {
                showMessage(error.message, true);
            }
        }
        submitBtn.disabled = false;
    });
};