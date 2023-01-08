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
            "sAjaxSource": '/LookUp/CustomersList',
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
                { "mDataProp": "TypeDesc", "sTitle": "Type", "bSortable": false },
                { "mDataProp": "Name", "sTitle": "Name", "bSortable": false },
                { "mDataProp": "RecordStatusDesc", "sTitle": "Record Status", "bSortable": false },
                { "mDataProp": "AddressEmail", "sTitle": "Email", "bSortable": false },
                { "mDataProp": "CreatedDate", "sTitle": "Created Date" },
                { "mDataProp": "CreatedDate", "sTitle": "Action", "bSortable": false }],
            "aaSorting": [[5, "desc"]],
            "responsive": true,
            "dom": '<"html5buttons"B>lTfgitp',
            "buttons": []
        });

        $('#customerLookupGrid tbody').on('click', 'tr', function () {

            var data = customerLookupGrid.row(this).data();

            console.log(data);

            $('input[name=CustomerId]').val(data.Id);
            $('input[name=CustomerName]').val(data.Name);

            $('input[name=ParentId]').val(data.Id);
            $('input[name=ParentName]').val(data.Name);

            $('#customerLookupModal').modal('hide');
        });
    }

    CustomerLookupGrid();
});