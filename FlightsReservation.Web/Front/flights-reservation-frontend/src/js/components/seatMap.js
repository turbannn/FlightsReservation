// This file contains the logic for displaying the seat map.

const seatMapContainer = document.getElementById('seat-map');
const seatSelection = document.getElementById('selected-seats');
const totalPrice = document.getElementById('total-price');

let selectedSeats = [];
let seatPrice = 100; // Example price per seat

// Function to create the seat map
function createSeatMap(rows, cols) {
    for (let row = 1; row <= rows; row++) {
        const rowDiv = document.createElement('div');
        rowDiv.className = 'seat-row';
        
        for (let col = 1; col <= cols; col++) {
            const seat = document.createElement('div');
            seat.className = 'seat';
            seat.dataset.seatId = `${row}-${col}`;
            seat.innerText = `${row}-${col}`;
            seat.addEventListener('click', toggleSeatSelection);
            rowDiv.appendChild(seat);
        }
        
        seatMapContainer.appendChild(rowDiv);
    }
}

// Function to toggle seat selection
function toggleSeatSelection(event) {
    const seatId = event.target.dataset.seatId;
    
    if (selectedSeats.includes(seatId)) {
        selectedSeats = selectedSeats.filter(seat => seat !== seatId);
        event.target.classList.remove('selected');
    } else {
        selectedSeats.push(seatId);
        event.target.classList.add('selected');
    }
    
    updateTotalPrice();
}

// Function to update total price
function updateTotalPrice() {
    totalPrice.innerText = `Total Price: $${selectedSeats.length * seatPrice}`;
    seatSelection.innerText = `Selected Seats: ${selectedSeats.join(', ')}`;
}

// Initialize the seat map with 5 rows and 6 columns
createSeatMap(5, 6);