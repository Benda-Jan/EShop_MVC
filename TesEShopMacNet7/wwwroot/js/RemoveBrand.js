'use strict';

function RemoveBrand(brandId) {
    if (confirm(`Opravdu chcete vymazat tuto značku?`)) {
        $.post("Brand/Delete",
            { Id: brandId },
            function (response) {
                if (response) {
                    let row = document.getElementById(brandId);
                    row.remove();
                }
            });
    }
}
