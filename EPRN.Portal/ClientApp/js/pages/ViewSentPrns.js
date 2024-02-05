export default (() => {
    $(document).ready(function () {
        $('#filterBy, #sortBy').change(function () {
            $(this).closest('form').submit();
        });
    });

})();