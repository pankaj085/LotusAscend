// --- CONFIGURATION ---
const API_BASE_URL = 'http://localhost:5245/api'; // Make sure this matches your backend's port

/**
 * A wrapper for the fetch API to handle common tasks like setting headers and error handling.
 * @param {string} endpoint - The API endpoint to call (e.g., '/Member/register').
 * @param {object} options - The options for the fetch call (method, body, etc.).
 * @returns {Promise<object>} - The JSON response from the API.
 */
export const apiFetch = async (endpoint, options = {}) => {
    const headers = { 'Content-Type': 'application/json', ...options.headers };
    const jwtToken = localStorage.getItem('jwtToken');
    if (jwtToken) {
        headers['Authorization'] = `Bearer ${jwtToken}`;
    }

    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, { ...options, headers });

        const responseData = await response.json().catch(() => ({})); // Handle empty responses

        if (!response.ok) {
            throw new Error(responseData.error || responseData.title || 'An unknown API error occurred.');
        }

        return responseData;
    } catch (error) {
        console.error('API Fetch Error:', error);
        // Re-throw the error so the calling function can handle it and display a UI message.
        throw error;
    }
};
