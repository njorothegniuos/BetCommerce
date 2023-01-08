$(document).ready(function () {

    function BankLookupGrid() {

        var BankLookupGrid;

        if (BankLookupGrid != undefined) {
            BankLookupGrid.fnDestroy();
        };

        BankLookupGrid = $('#BankLookupGrid').DataTable({
            "bretrieve": true,
            "sPaginationType": "full_numbers",
            "bProcessing": true,
            "bServerSide": true,
            "info": false,
            "sAjaxSource": '/base/Banks',
            "sServerMethod": "POST",
            // Callback settings
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                return nRow;
            },
            // Display settings
            "aoColumns": [
                { "mDataProp": "Name", "sTitle": "Name", "sWidth": "40%" },
                { "mDataProp": "Code", "sTitle": "Code", "sWidth": "60%" }],
            "aaSorting": [[0, "desc"]],
            "responsive": true,
            "dom": '<"html5buttons"B>lTfgitp',
            "buttons": []
        });

        $('#BankLookupGrid tbody').on('click', 'tr', function () {

            var data = BankLookupGrid.row(this).data();

            $('input[name=BankCode]').val(data.Code);
            $('input[name=BankName]').val(data.Code);

            $('#BankLookupModal').modal('hide');
        });
    }

    BankLookupGrid();
});