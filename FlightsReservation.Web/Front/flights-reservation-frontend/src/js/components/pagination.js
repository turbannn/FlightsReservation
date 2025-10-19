const pagination = (function() {
    let currentPage = 1;
    let totalPages = 1;

    const init = (total, perPage) => {
        totalPages = Math.ceil(total / perPage);
        render();
    };

    const render = () => {
        const paginationContainer = document.getElementById('pagination');
        paginationContainer.innerHTML = '';

        const prevButton = document.createElement('button');
        prevButton.innerText = 'Previous';
        prevButton.disabled = currentPage === 1;
        prevButton.onclick = () => changePage(currentPage - 1);
        paginationContainer.appendChild(prevButton);

        for (let i = 1; i <= totalPages; i++) {
            const pageButton = document.createElement('button');
            pageButton.innerText = i;
            pageButton.classList.toggle('active', i === currentPage);
            pageButton.onclick = () => changePage(i);
            paginationContainer.appendChild(pageButton);
        }

        const nextButton = document.createElement('button');
        nextButton.innerText = 'Next';
        nextButton.disabled = currentPage === totalPages;
        nextButton.onclick = () => changePage(currentPage + 1);
        paginationContainer.appendChild(nextButton);
    };

    const changePage = (page) => {
        if (page < 1 || page > totalPages) return;
        currentPage = page;
        render();
        // Trigger an event or callback to load new data based on the current page
    };

    return {
        init,
        changePage,
    };
})();

export default pagination;