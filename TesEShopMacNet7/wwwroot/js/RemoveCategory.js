'use strict';

function RemoveCategory(categoryId) {
    if (confirm(`Opravdu chcete vymazat tuto kategorii?`)) {
        $.post("Category/Delete",
            { Id: categoryId },
            function (response) {
                if (response) {
                    let row = document.getElementById(categoryId);
                    row.remove();
                }
            });
    }
}
