$(document).ready(function () {

    function UsersByRoleLookupGrid() {

        var UsersByRoleLookupGrid;

        if (UsersByRoleLookupGrid != undefined) {
            UsersByRoleLookupGrid.fnDestroy();
        };

        UsersByRoleLookupGrid = $('#usersByRoleLookupGrid').DataTable({
            "bretrieve": true,
            "sPaginationType": "full_numbers",
            "bProcessing": true,
            "bServerSide": true,
            "info": false,
            "sAjaxSource": '/base/usersinrole/Relationship Manager',
            "sServerMethod": "POST",
            // Callback settings
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                var createdDate = new Date(parseInt(aData.CreatedDate.replace("/Date(", "").replace(")/", ""), 10));

                var h = ("0" + createdDate.getHours()).slice(-2);

                var m = ("0" + createdDate.getMinutes()).slice(-2);

                var s = ("0" + createdDate.getSeconds()).slice(-2);

                var dt_Created = $.datepicker.formatDate("dd/mm/yy", createdDate) + " " + h + ":" + m + ":" + s;

                aData.CreatedDate = dt_Created;

                $("td:eq(5)", nRow).html(dt_Created);

                return nRow;
            },
            // Display settings
            "aoColumns": [
                { "mDataProp": "FullName", "sTitle": "Full Name" },
                { "mDataProp": "Email", "sTitle": "Email" },
                { "mDataProp": "UserName", "sTitle": "UserName" },
                { "mDataProp": "PhoneNumber", "sTitle": "Mobile Number" },
                { "mDataProp": "RoleName", "sTitle": "Roles" },
                { "mDataProp": "CreatedDate", "sTitle": "Created Date" },
                { "mDataProp": "UserName", "sTitle": "Action" }],
            "aaSorting": [[4, "desc"]],
            "responsive": true,
            "dom": '<"html5buttons"B>lTfgitp',
            "buttons": []
        });

        $('#usersByRoleLookupGrid tbody').on('click', 'tr', function () {

            var data = UsersByRoleLookupGrid.row(this).data();

            $('input[name=RelationShipManagersUserId]').val(data.Id);

            $("#RelationShipManagersUsername").tagsinput('add', data.UserName);

            $('#UsersByRoleLookupModal').modal('hide');
        });
    }

    UsersByRoleLookupGrid();
});