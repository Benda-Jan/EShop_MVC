'use strict';

function RemoveUser(roleName) {
    if (confirm(`Opravdu chcete vymazat účet ${roleName}?`)) {
        $.post("/Account/Delete",
            { Name: roleName },
            function (response) {
                window.location.replace("/Home/Index");
            });
    }
}
