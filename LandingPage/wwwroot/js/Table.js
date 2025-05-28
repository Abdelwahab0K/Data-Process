document.addEventListener("DOMContentLoaded", function () {
    // Make AJAX request to get data from the server
    fetch('/Dashboard/GetProcessData')
        .then(response => response.json()) // Parse the response as JSON
        .then(data => populateTable(data)) // Call the function to populate the table
        .catch(error => console.error('Error fetching data:', error));
});

function populateTable(data) {
    const tableBody = document.getElementById("processTableBody"); // Get the table body by ID
    tableBody.innerHTML = ''; // Clear any existing table rows

    // Loop through the data and create rows dynamically
    data.forEach(item => {
        const row = document.createElement("tr");

        // Create table cells for each piece of data
        const cell1 = document.createElement("td");
        cell1.textContent = item.journee;
        row.appendChild(cell1);

        const cell2 = document.createElement("td");
        cell2.textContent = item.pce_id;
        row.appendChild(cell2);

        const cell3 = document.createElement("td");
        cell3.textContent = item.nmic;
        row.appendChild(cell3);

        const cell4 = document.createElement("td");
        cell4.textContent = item.coil_id;
        row.appendChild(cell4);

        const cell5 = document.createElement("td");
        cell5.textContent = new Date(item.start_time).toLocaleString(); // Convert Date to readable format
        row.appendChild(cell5);

        const cell6 = document.createElement("td");
        cell6.textContent = new Date(item.end_time).toLocaleString(); // Convert Date to readable format
        row.appendChild(cell6);

        const cell7 = document.createElement("td");
        cell7.textContent = item.nombre_pass;
        row.appendChild(cell7);

        const cell8 = document.createElement("td");
        cell8.textContent = item.restart;
        row.appendChild(cell8);

        const cell9 = document.createElement("td");
        cell9.textContent = item.coble;
        row.appendChild(cell9);

        // Append the row to the table body
        tableBody.appendChild(row);
    });
}
