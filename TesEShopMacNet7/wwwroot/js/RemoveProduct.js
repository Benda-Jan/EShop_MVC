'use strict';

function RemoveProduct(productName) {
    if (confirm(`Opravdu chcete vymazat produkt ${productName}?`)) {
        $.post("Product/Delete",
            { Name: productName },
            function (response) {
                if (response) {
                    let row = document.getElementById(productName);
                    row.remove();
                }
            });
    }
}
