'use strict';

function RemoveRole(roleName) {
    if (confirm(`Opravdu chcete vymazat roli ${roleName}?`)) {
        $.post("Role/Delete",
            { Name: roleName },
            function (response) {
                if (response) {
                    let row = document.getElementById(roleName);
                    row.remove();
                }
            });
    }
}
