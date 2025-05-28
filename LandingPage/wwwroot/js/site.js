document.getElementById('loginBtn').addEventListener('click', function (e) {
    e.preventDefault();
    const target = document.getElementById('login-section');
    target.scrollIntoView({
        behavior: 'smooth',
        block: 'start'
    });

    // Optional: Add focus to first form field
    setTimeout(() => {
        const firstInput = target.querySelector('input');
        if (firstInput) firstInput.focus();
    }, 1000);

});