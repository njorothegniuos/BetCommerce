$(document).ready(function () {

    function Users() {

        var UsersDataTable;

        if (UsersDataTable != undefined) {
            UsersDataTable.fnDestroy();
        };

        UsersDataTable = $('#UsersDataTable').DataTable({
            "bretrieve": true,
            "sPaginationType": "full_numbers",
            "bProcessing": true,
            "bServerSide": true,
            "info": false,
            "sAjaxSource": '/LookUp/UsersList',
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
                { "mDataProp": "UserName", "sTitle": "UserName" },
                { "mDataProp": "PhoneNumber", "sTitle": "Mobile Number" },
                { "mDataProp": "RoleName", "sTitle": "Roles" },
                { "mDataProp": "RecordStatusDescription", "sTitle": "Record Status" },
                { "mDataProp": "IsEnabled", "sTitle": "Is Enabled" },
                { "mDataProp": "CreatedDate", "sTitle": "Created Date" },
                { "mDataProp": "UserName", "sTitle": "Action" }],
            "aaSorting": [[4, "desc"]],
            "responsive": true,
            "dom": '<"html5buttons"B>lTfgitp',
            "buttons": []
        });

        $('#UsersDataTable tbody').on('click', 'tr', function () {

            var data = UsersDataTable.row(this).data();

            $('input[name=UserName]').val(data.UserName);
            $('input[name=UserId]').val(data.Id);
            $('input[name=EmailAddress]').val(data.EmailAddress);

            $('#UsersLookupModal').modal('hide');
        });
    }

    Users();
});