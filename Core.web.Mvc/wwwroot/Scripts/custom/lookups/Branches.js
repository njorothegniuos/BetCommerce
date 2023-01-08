$(document).ready(function () {

    function BranchLookupGrid() {

        var BranchLookupGrid;

        if (BranchLookupGrid != undefined) {
            BranchLookupGrid.fnDestroy();
        };

        BranchLookupGrid = $('#BranchLookupGrid').DataTable({
            "bretrieve": true,
            "sPaginationType": "full_numbers",
            "bProcessing": true,
            "bServerSide": true,
            "info": false,
            "sAjaxSource": '/base/Branches',
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

        $('#BranchLookupGrid tbody').on('click', 'tr', function () {

            var data = BranchLookupGrid.row(this).data();

            $('input[name=BranchCode]').val(data.Code);
            $('input[name=BranchName]').val(data.Name);

            $('#branchLookupModal').modal('hide');
        });
    }

    BranchLookupGrid();
});