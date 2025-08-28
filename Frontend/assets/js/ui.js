/**
 * Displays a toast notification at the top of the screen.
 * @param {string} message - The text to display.
 * @param {boolean} [isError=false] - If true, the toast will be styled as an error.
 */
export const showMessage = (message, isError = false) => {
    const container = document.getElementById('toast-container');
    if (!container) return;

    // Create the toast element
    const toast = document.createElement('div');
    toast.className = `toast ${isError ? 'error' : 'success'}`;
    toast.textContent = message;

    // Add it to the container
    container.appendChild(toast);

    // Automatically remove the toast after 3 seconds
    setTimeout(() => {
        toast.remove();
    }, 3000);
};

/**
 * Hides any on-screen messages (no longer needed for toasts, but kept for compatibility).
 */
export const hideMessage = () => {
    // This function is no longer necessary as toasts disappear automatically.
};

/**
 * Initializes event listeners for all modal trigger buttons and close buttons.
 */
export const initializeModals = () => {
    document.querySelectorAll('[data-modal-target]').forEach(button => {
        button.addEventListener('click', () => {
            const modal = document.querySelector(button.dataset.modalTarget);
            modal.classList.remove('hidden');
        });
    });

    document.querySelectorAll('.close-btn').forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal');
            modal.classList.add('hidden');
        });
    });
};

/**
 * Hides a specific modal.
 * @param {string} modalId - The ID of the modal to hide.
 */
export const hideModal = (modalId) => {
    const modal = document.getElementById(modalId);
    if (modal) modal.classList.add('hidden');
};