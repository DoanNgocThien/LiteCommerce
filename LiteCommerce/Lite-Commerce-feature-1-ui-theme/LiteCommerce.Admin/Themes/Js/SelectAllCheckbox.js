$(function () {
    $('#select-shipper').click(function (event) {
        if (this.checked) {
            // Iterate each checkbox
            $(':ShipperIDs').each(function () {
                this.checked = true;
            });
        } else {
            $(':ShipperIDs').each(function () {
                this.checked = false;
            });
        }
    });
});