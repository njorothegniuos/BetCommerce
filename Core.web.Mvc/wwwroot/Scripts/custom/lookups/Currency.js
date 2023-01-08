$(document).ready(function () {

    function CurrencyLookupGrid() {

        var CurrencyLookupGrid;

        if (CurrencyLookupGrid != undefined) {
            CurrencyLookupGrid.fnDestroy();
        };

        CurrencyLookupGrid = $('#currencyLookupGrid').DataTable({
            "bretrieve": true,
            "sPaginationType": "full_numbers",
            "bProcessing": true,
            "bServerSide": true,
            "info": false,
            "sAjaxSource": '/Registry/Currencies/Index',
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
                { "mDataProp": "CountryName", "sTitle": "Country", "bSortable": false },
                { "mDataProp": "Description", "sTitle": "Description", "bSortable": false },
                { "mDataProp": "Code", "sTitle": "Code", "bSortable": false },
                { "mDataProp": "Name", "sTitle": "Name", "bSortable": false },
                { "mDataProp": "CreatedDate", "sTitle": "Action", "sWidth": "10%", "bSortable": false }],
            "aaSorting": [[4, "desc"]],
            "responsive": true,
            "dom": '<"html5buttons"B>lTfgitp',
            "buttons": []
        });

        $('#currencyLookupGrid tbody').on('click', 'tr', function () {

            var data = CurrencyLookupGrid.row(this).data();

            $('input[name=CurrencyId]').val(data.Id);
            $('input[name=CurrencyName]').val(data.Name);
            $('input[name=CurrencyCode]').val(data.Code);

            $('#CurrencyLookupModal').modal('hide');
        });
    }

    CurrencyLookupGrid();
});