// Get all navigation buttons with the common class
const navigationButtons = document.querySelectorAll('.navigation-button');
const formSections = document.querySelectorAll('.form-step');

// Ajoutez un bouton "Submit" à la troisième section du formulaire
formSections.forEach((section, index) => {
    if (index === 2) { // Troisième section
        const submitButton = document.createElement('input');
        submitButton.type = 'submit';
        submitButton.value = 'Valider';
        submitButton.className = 'submit-button';
        section.appendChild(submitButton);
    }
});

// Add click event listener to all navigation buttons
navigationButtons.forEach(button => {
    button.addEventListener('click', function(event) {
        event.preventDefault();

        // Determine the action (precedent or suivant) from the data attribute
        const action = button.getAttribute('data-action');

        // Find the current visible section
        const currentSection = Array.from(formSections).find(section => getComputedStyle(section).display !== 'none');

        if (currentSection) {
            // Find the index of the current section
            const currentIndex = Array.from(formSections).indexOf(currentSection);

            // Calculate the index of the next section based on the action
            let nextIndex;
            if (action === 'precedent') {
                nextIndex = currentIndex - 1;
            } else if (action === 'suivant') {
                nextIndex = currentIndex + 1;
            }

            // Display the next section, hide the current one
            if (nextIndex >= 0 && nextIndex < formSections.length) {
                currentSection.style.display = 'none';
                formSections[nextIndex].style.display = 'block';
            }
        }
    });
});
