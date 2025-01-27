
# **Flower Site**

This project is a simple website that showcases a carousel and cards for different flower types. Below is an explanation of the more complex parts of the code:

## 1. Carousel
The carousel is built using Bootstrap's components and includes:

Indicators: Small navigation dots, implemented using data-slide-to attributes.
Slides: Each slide uses the carousel-item class, and the first slide is marked as "active."
Controls: Buttons to navigate between slides (Previous and Next).
The carousel dynamically handles transitions and responsiveness through Bootstrap’s built-in JavaScript.

## 2. Hover Effect on Cards
A custom CSS hover effect is applied to the cards. When hovered over, the card scales up slightly and rotates using the transform and transition properties. This adds a subtle 3D effect.

## 3. Image Styling
Images in the carousel and cards use the object-fit: cover property to ensure they fit their containers without distortion. This is important for maintaining a clean layout.

# **Temperature converter**

This project is a web-based temperature converter with a C# backend and a simple HTML/JavaScript frontend. Below is an explanation of the more complex sections of the code:

## 1. Backend Logic (C#)
Conversion Logic:
The backend handles conversions between Celsius, Fahrenheit, and Kelvin. The TemperatureConverter class includes methods to convert from one unit to another by applying the appropriate mathematical formulas.

### Validation:
The backend validates the input temperature, source unit (FromUnit), and target unit (ToUnit). Invalid inputs or unsupported units result in appropriate error messages.

### Endpoint:
A POST endpoint /convert receives a JSON object from the frontend with the temperature value, source unit, and target unit. It processes the data, performs the conversion, and returns the result in JSON format.

### CORS Policy:
The backend is configured to allow all origins and methods to support seamless communication between the frontend and backend.

## 2. Frontend Logic (HTML/JavaScript)
Form:
The user provides input (temperature, source unit, and target unit) through a form styled with Bootstrap. The dropdowns for units ensure that only valid options are selected.

### Dynamic Interaction:
The JavaScript function convertTemperature() collects the form data and sends it to the backend using a POST request. It then parses the JSON response and displays the converted value dynamically on the page.

### Error Handling:
This section needs improvement. I was struggling at some parts and went into kind of rabbit hole.
The backend validates the input and returns meaningful error messages if something goes wrong, which helps maintain robust communication between frontend and backend.
