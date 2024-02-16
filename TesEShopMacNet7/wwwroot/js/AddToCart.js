'use strict';

function AddToCart(productId) {
    $.post("/Cart/AddToCart",
        { ProductId: productId },
        function (response) {
            if (response === true) {
                let tdEdit = document.getElementById(productId).childNodes;
                tdEdit.item(5).innerText++;
            }
        }
    );

}
