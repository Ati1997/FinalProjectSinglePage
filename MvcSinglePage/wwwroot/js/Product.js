//var form = document.getElementById("formAddProduct");
//var tbody = document.getElementById("tbodyProducts");
//var resultMessage = document.getElementById("resultMessage");

//var titleInput = document.getElementById("title");
//var descriptionInput = document.getElementById("description");
//var priceInput = document.getElementById("price");

//window.onload = LoadData;

//form.addEventListener("submit", Add);

//function LoadData() {
//    tbody.innerHTML = "";

//    fetch("/Product/GetAll")
//        .then(res => res.json())
//        .then(response => {
//            if (!response.isSuccessful) {
//                TriggerResultMessage(response.errorMessage || "Error loading products");
//                return;
//            }

//            var products = response.result.getById_Product_Dtos || [];
//            if (products.length === 0) {
//                tbody.innerHTML = "<tr><td colspan='3'>No products found</td></tr>";
//                return;
//            }
//            console.table(response);
//            console.table(products);
//            var html = "";
//            products.forEach(p => {
//                html += `<tr>
//                    <td>${p.title}</td>
//                    <td>${p.productDescription}</td>
//                    <td>${p.unitPrice}</td>
//                </tr>`;
//            });
//            tbody.innerHTML = html;
//        })

//}

//function Add(e) {
//    e.preventDefault();

//    let dto = {
//        Title: titleInput.value.trim(),
//        ProductDescription: descriptionInput.value.trim(),
//        UnitPrice: parseFloat(priceInput.value)
//    };

//    fetch("/Product/Post", {
//        method: "POST",
//        headers: { "Content-Type": "application/json" },
//        body: JSON.stringify(dto)
//    })
//    .then(res => res.json())
//    .then(response => {
//        if (response.errorMessage) {
//            TriggerResultMessage("Failed to add product: " + response.errorMessage);
//        } else {
//            TriggerResultMessage("Product added successfully");
//            LoadData();
//            form.reset();
//        }
//    })

//}


//function TriggerResultMessage(message) {
//    resultMessage.innerText = message;
//    resultMessage.style.opacity = "1";
//    setTimeout(() => resultMessage.style.opacity = "0", 2000);
//}


var form = document.getElementById("formAddProduct");
var tbody = document.getElementById("tbodyProducts");
var resultMessage = document.getElementById("resultMessage");

var idInput = document.getElementById("id");
var titleInput = document.getElementById("title");
var descriptionInput = document.getElementById("description");
var priceInput = document.getElementById("price");
var btnSave = document.getElementById("btnSave");

window.onload = LoadData;
form.addEventListener("submit", onAddSubmit);

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
                tbody.innerHTML = "<tr><td colspan='4'>No products found</td></tr>";
                return;
            }

            let html = "";
            products.forEach(p => {
                const safeTitle = (p.title || "").replace(/'/g, "\\'");
                const safeDesc = (p.productDescription || "").replace(/'/g, "\\'");
                html += `<tr>
                    <td>${p.title}</td>
                    <td>${p.productDescription}</td>
                    <td>${p.unitPrice}</td>
                    <td>
                        <button class="btn btn-warning btn-sm" onclick="onEdit('${p.id}','${safeTitle}','${safeDesc}',${p.unitPrice})">Edit</button>
                        <button class="btn btn-danger btn-sm" onclick="onDelete('${p.id}')">Delete</button>
                    </td>
                </tr>`;
            });
            tbody.innerHTML = html;
        })
        .catch(err => {
            console.error(err);
            TriggerResultMessage("Network or server error while loading products");
        });
}

async function onAddSubmit(e) {
    e.preventDefault();
    btnSave.disabled = true;

    try {
        let dto = {
            Title: titleInput.value.trim(),
            ProductDescription: descriptionInput.value.trim(),
            UnitPrice: parseFloat(priceInput.value)
        };

        const res = await fetch("/Product/Post", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        const response = await res.json();
        if (!res.ok) {
            TriggerResultMessage(response.errorMessage || "Failed to add product");
        } else {
            TriggerResultMessage("Product added successfully");
            form.reset();
            LoadData();
        }
    } catch (err) {
        console.error(err);
        TriggerResultMessage("Network error during add");
    } finally {
        btnSave.disabled = false;
    }
}

async function onEditSubmit(e) {
    e.preventDefault();
    btnSave.disabled = true;

    try {
        let dto = {
            Id: idInput.value,
            Title: titleInput.value.trim(),
            ProductDescription: descriptionInput.value.trim(),
            UnitPrice: parseFloat(priceInput.value)
        };

        const res = await fetch("/Product/Put", {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        const response = await res.json();
        if (!res.ok) {
            TriggerResultMessage(response.errorMessage || "Failed to update product");
        } else {
            TriggerResultMessage("Product updated successfully");
            form.reset();
            idInput.value = "";
            LoadData();
            form.removeEventListener("submit", onEditSubmit);
            form.addEventListener("submit", onAddSubmit);
        }
    } catch (err) {
        console.error(err);
        TriggerResultMessage("Network error during update");
    } finally {
        btnSave.disabled = false;
    }
}

function onEdit(id, title, description, price) {
    idInput.value = id;
    titleInput.value = title;
    descriptionInput.value = description;
    priceInput.value = price;
    titleInput.focus();

    form.removeEventListener("submit", onAddSubmit);
    form.addEventListener("submit", onEditSubmit);
}

async function onDelete(id) {
    if (!confirm("Are you sure you want to delete this product?")) return;

    try {
        const res = await fetch("/Product/Delete", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(id)
        });

        let result = {};
        try { result = await res.json(); } catch (e) { console.warn("Empty response or not JSON", e); }

        if (!res.ok) {
            TriggerResultMessage(result.errorMessage || "Delete failed");
            return;
        }

        TriggerResultMessage(result.message || "Product deleted successfully");
        LoadData();
    } catch (err) {
        console.error(err);
        TriggerResultMessage("Network error during delete");
    }
}

function TriggerResultMessage(message) {
    resultMessage.innerText = message;
    resultMessage.style.opacity = "1";
    setTimeout(() => resultMessage.style.opacity = "0", 2000);
}
