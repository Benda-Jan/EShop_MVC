'use strict';

function RemoveFromCart(productId) {
    $.post("/Cart/RemoveFromCart",
        { ProductId: productId },
        function (response) {
            if (response === true) {
                let tdEdit = document.getElementById(productId);
                alert(tdEdit.childNodes.item(5).innerText);
                tdEdit.childNodes.item(5).innerText--;
                if (tdEdit.childNodes.item(5).innerText === "0")
                    document.getElementById(productId).remove();
            }
        });
    
}
