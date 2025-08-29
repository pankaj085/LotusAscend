import { apiFetch } from './api.js';
import { showMessage } from './ui.js';

export const initAuthPage = () => {
    const form = document.getElementById('auth-form');
    if (!form) return;

    // --- Get all UI elements ---
    const usernameInput = document.getElementById('username');
    const mobileInput = document.getElementById('mobile');
    const otpInput = document.getElementById('otp');
    const title = document.getElementById('form-title');
    const subtitle = document.getElementById('form-subtitle');
    const submitBtn = document.getElementById('submit-btn');
    const usernameGroup = document.getElementById('username-group');
    const mobileGroup = document.getElementById('mobile-group');
    const otpGroup = document.getElementById('otp-group');
    const toggleLink = document.getElementById('toggle-form');
    
    let mobileNumberForOtp = '';
    let isLoginMode = false;

    // --- Central function to manage UI state ---
    const setFormState = (mode) => {
        form.reset(); // Clear inputs on state change
        
        // State 1: Registration
        if (mode === 'register') {
            isLoginMode = false;
            title.textContent = 'Join Lotus Ascend';
            subtitle.textContent = 'Register to start earning rewards.';
            usernameGroup.classList.remove('hidden');
            mobileGroup.classList.remove('hidden');
            otpGroup.classList.add('hidden');
            usernameInput.required = true;
            mobileInput.required = true;
            otpInput.required = false;
            submitBtn.textContent = 'Register';
            toggleLink.innerHTML = `Already have an account? <a href="#">Login</a>`;
        }
        // State 2: Login
        else if (mode === 'login') {
            isLoginMode = true;
            title.textContent = 'Welcome Back!';
            subtitle.textContent = 'Enter your mobile number to get an OTP.';
            usernameGroup.classList.add('hidden');
            mobileGroup.classList.remove('hidden');
            otpGroup.classList.add('hidden');
            usernameInput.required = false;
            mobileInput.required = true;
            otpInput.required = false;
            submitBtn.textContent = 'Get OTP';
            toggleLink.innerHTML = `Don't have an account? <a href="#">Register</a>`;
        }
        // State 3: OTP Verification
        else if (mode === 'otp') {
            title.textContent = 'Verify Your Number';
            subtitle.textContent = `Enter the 4-digit OTP.`;
            usernameGroup.classList.add('hidden');
            mobileGroup.classList.add('hidden');
            otpGroup.classList.remove('hidden');
            usernameInput.required = false;
            mobileInput.required = false;
            otpInput.required = true;
            submitBtn.textContent = 'Verify & Proceed';
        }
    };

    // --- Event listener for the toggle link ---
    toggleLink.addEventListener('click', (e) => {
        e.preventDefault();
        // Switch between 'login' and 'register' modes
        setFormState(isLoginMode ? 'register' : 'login');
    });

    // --- Main form submission logic ---
    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        submitBtn.disabled = true;

        // --- A: Handle OTP Verification ---
        if (!otpGroup.classList.contains('hidden')) {
            const otp = otpInput.value;
            try {
                const data = await apiFetch('/Member/verify', {
                    method: 'POST',
                    body: JSON.stringify({ mobileNumber: mobileNumberForOtp, otp }),
                });
                localStorage.setItem('jwtToken', data.token);
                showMessage('Success! Redirecting...');
                window.location.href = 'dashboard.html';
            } catch (error) {
                showMessage(error.message, true);
            }
        } 
        // --- B: Handle Initial Login or Registration ---
        else {
            const mobile = mobileInput.value;
            mobileNumberForOtp = mobile; // Store mobile number for OTP step
            const endpoint = isLoginMode ? '/Member/login' : '/Member/register';
            const payload = isLoginMode 
                ? { mobileNumber: mobile } 
                : { username: usernameInput.value, mobileNumber: mobile };

            try {
                const result = await apiFetch(endpoint, {
                    method: 'POST',
                    body: JSON.stringify(payload),
                });
                showMessage(result.message || 'OTP has been sent!');
                setFormState('otp'); // Switch to OTP mode
            } catch (error) {
                showMessage(error.message, true);
            }
        }
        submitBtn.disabled = false;
    });

    // Initialize the form in the default 'register' state
    setFormState('register');
};