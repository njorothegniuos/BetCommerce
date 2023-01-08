$(document).ready(function () {

    function CustomerLookupGrid() {

        var customerLookupGrid;

        if (customerLookupGrid != undefined) {
            customerLookupGrid.fnDestroy();
        };

        customerLookupGrid = $('#customerLookupGrid').DataTable({
            "bretrieve": true,
            "sPaginationType": "full_numbers",
            "bProcessing": true,
            "bServerSide": true,
            "info": false,
            "sAjaxSource": '/LookUp/EmployeesList',
            "sServerMethod": "POST",
            // Callback settings
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                var createdDate = new Date(parseInt(aData.CreatedDate.replace("/Date(", "").replace(")/", ""), 10));
                var h_createdDate = ("0" + createdDate.getHours()).slice(-2);
                var m_createdDate = ("0" + createdDate.getMinutes()).slice(-2);
                var s_createdDate = ("0" + createdDate.getSeconds()).slice(-2);

                var dt_Created = $.datepicker.formatDate('dd/mm/yy', createdDate) + " " + h_createdDate + ":" + m_createdDate + ":" + s_createdDate;
                aData.CreatedDate = dt_Created;

                $('td:eq(4)', nRow).html(dt_Created);

                return nRow;
            },
            // Display settings
            "aoColumns": [
                { "mDataProp": "CustomerName", "sTitle": "Employer", "bSortable": false },
                { "mDataProp": "FullName", "sTitle": "Employee Names", "bSortable": false },
                { "mDataProp": "RecordStatusDesc", "sTitle": "Record Status", "bSortable": false },
                { "mDataProp": "IsEnabled", "sTitle": "IsEnabled", "bSortable": false },
                { "mDataProp": "CreatedDate", "sTitle": "Created Date" },
                { "mDataProp": "CreatedDate", "sTitle": "Action", "bSortable": false }
            ],
            "aaSorting": [[5, "desc"]],
            "responsive": true,
            "dom": '<"html5buttons"B>lTfgitp',
            "buttons": []
        });

        $('#customerLookupGrid tbody').on('click', 'tr', function () {

            var data = customerLookupGrid.row(this).data();

            $('input[name=EmployeeId]').val(data.Id);
            $('input[name=EmployeeEmployeeNames]').val(data.FullName); 

            $('#customerLookupModal').modal('hide');
        });
    }

    CustomerLookupGrid();
});