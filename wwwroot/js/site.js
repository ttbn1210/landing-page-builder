// Drag and drop functionality for components
let draggedElement = null;

function setupDragAndDrop() {
    const components = document.querySelectorAll('.component-item');
    
    components.forEach(component => {
        component.addEventListener('dragstart', (e) => {
            draggedElement = component;
            component.style.opacity = '0.5';
        });

        component.addEventListener('dragend', (e) => {
            component.style.opacity = '1';
        });

        component.addEventListener('dragover', (e) => {
            e.preventDefault();
        });

        component.addEventListener('drop', (e) => {
            e.preventDefault();
            if (draggedElement && draggedElement !== component) {
                const container = component.parentNode;
                container.insertBefore(draggedElement, component);
            }
        });
    });
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', () => {
    setupDragAndDrop();
});

// Utility function for API calls
async function apiCall(url, method = 'GET', body = null) {
    const options = {
        method: method,
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest'
        }
    };

    if (body) {
        options.body = JSON.stringify(body);
    }

    try {
        const response = await fetch(url, options);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return method === 'DELETE' ? null : await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}
