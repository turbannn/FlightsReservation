const navbar = () => {
    const nav = document.createElement('nav');
    nav.classList.add('navbar');

    const logo = document.createElement('div');
    logo.classList.add('navbar-logo');
    logo.innerText = 'Flight Reservation';

    const navLinks = document.createElement('ul');
    navLinks.classList.add('navbar-links');

    const pages = [
        { name: 'Home', path: 'index.html' },
        { name: 'Flights', path: 'flights.html' },
        { name: 'Reservations', path: 'reservation.html' },
        { name: 'Admin', path: 'admin.html' },
        { name: 'Login', path: 'login.html' }
    ];

    pages.forEach(page => {
        const link = document.createElement('li');
        link.innerHTML = `<a href="${page.path}">${page.name}</a>`;
        navLinks.appendChild(link);
    });

    nav.appendChild(logo);
    nav.appendChild(navLinks);

    document.body.insertBefore(nav, document.body.firstChild);
};

export default navbar;