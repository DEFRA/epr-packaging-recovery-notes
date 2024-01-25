export default (() => {

    $(document).ready(function () {
        $('#filter-by').on('change', function () {

            var selectedStatus = $(this).val();

            if (selectedStatus == 'All') {
                $('.govuk-table__row').show();
            } else {
                $('.govuk-table__row').hide();
                $('.govuk-table__row[data-status="' + selectedStatus + '"]').show();
            }
        });
    })
})();