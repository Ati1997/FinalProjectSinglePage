var form = document.getElementById("formAddProduct");
var tbody = document.getElementById("tbodyProducts");
var resultMessage = document.getElementById("resultMessage");

var titleInput = document.getElementById("title");
var descriptionInput = document.getElementById("description");
var priceInput = document.getElementById("price");

window.onload = LoadData;

form.addEventListener("submit", Add);

function LoadData() {
    tbody.innerHTML = "";

    fetch("/Product/GetAll")
        .then(res => res.json())
        .then(response => {
            if (!response.isSuccessful) {
                TriggerResultMessage(response.errorMessage || "Error loading products");
                return;
            }

            var products = response.result.getById_Product_Dtos || [];
            if (products.length === 0) {
                tbody.innerHTML = "<tr><td colspan='3'>No products found</td></tr>";
                return;
            }
            console.table(response);
            console.table(products);
            var html = "";
            products.forEach(p => {
                html += `<tr>
                    <td>${p.title}</td>
                    <td>${p.productDescription}</td>
                    <td>${p.unitPrice}</td>
                </tr>`;
            });
            tbody.innerHTML = html;
        })

}

function Add(e) {
    e.preventDefault();

    let dto = {
        Title: titleInput.value.trim(),
        ProductDescription: descriptionInput.value.trim(),
        UnitPrice: parseFloat(priceInput.value)
    };

    fetch("/Product/Post", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dto)
    })
    .then(res => res.json())
    .then(response => {
        if (response.errorMessage) {
            TriggerResultMessage("Failed to add product: " + response.errorMessage);
        } else {
            TriggerResultMessage("Product added successfully");
            LoadData();
            form.reset();
        }
    })

}


function TriggerResultMessage(message) {
    resultMessage.innerText = message;
    resultMessage.style.opacity = "1";
    setTimeout(() => resultMessage.style.opacity = "0", 2000);
}

