# Flights Reservation Frontend

This project is a frontend application for managing flight reservations. It allows users to search for flights, make reservations, and manage their accounts. The application is structured to provide a seamless user experience with a clean and responsive design.

## Project Structure

- **index.html**: The main landing page of the application. It includes a header and input fields for searching flights with options for round trips.
- **login.html**: A page dedicated to user login functionality.
- **flights.html**: Displays available flights and includes pagination controls for navigating through flight results.
- **reservation.html**: Manages flight reservations for users.
- **admin.html**: Provides administrative functionalities for managing flights.

### Source Code

- **src/css/**: Contains stylesheets for the application.
  - **base.css**: Base styles for the application.
  - **layout.css**: Layout-specific styles.
  - **components.css**: Styles for individual components.

- **src/js/**: Contains JavaScript files for application logic.
  - **config.js**: Configuration settings, including API endpoints.
  - **main.js**: Initializes the application and handles global functionality.
  - **api/**: Contains files for making API requests.
    - **client.js**: General API request functions.
    - **flights.js**: Functions for flight-related API requests.
    - **reservations.js**: Functions for managing reservations.
    - **seats.js**: Functions for seat-related requests.
    - **users.js**: Functions for user management.
  - **components/**: Contains logic for UI components.
    - **navbar.js**: Navigation bar component logic.
    - **flightCard.js**: Logic for displaying flight cards.
    - **seatMap.js**: Logic for displaying seat maps.
    - **pagination.js**: Logic for pagination controls.
  - **utils/**: Utility functions.
    - **storage.js**: Functions for managing local storage.
    - **validators.js**: Validation functions for user input.

### Assets

- **src/assets/fonts/**: Directory containing font files used in the application.

## Getting Started

1. Clone the repository to your local machine.
2. Open `index.html` in your web browser to view the application.
3. Ensure that the backend API is running on `localhost` to handle requests.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for any suggestions or improvements.

## License

This project is licensed under the MIT License.