$(document).ready(function () {
    var $el = $('body');
    $(document).on("submit", "#myForm", function (event) {
        $el.trigger('loading-overlay:show');
        var dataString;
        event.preventDefault();
        event.stopImmediatePropagation();
        var action = $("#myForm").attr("action");
        dataString = new FormData($("#myForm").get(0));
        contentType = false;
        processData = false;

        $.ajax({ type: "POST", url: action, data: dataString, dataType: "json", contentType: contentType, processData: processData, success: (result) => Success(result), error: (result) => Success(result) });
    });

    $(document).on("submit", "#myForm1", function (event) {
        $el.trigger('loading-overlay:show');
        var dataString;
        event.preventDefault();
        event.stopImmediatePropagation();
        var action = $("#myForm1").attr("action");
        dataString = new FormData($("#myForm1").get(0));
        contentType = false;
        processData = false;

        $.ajax({ type: "POST", url: action, data: dataString, dataType: "json", contentType: contentType, processData: processData, success: (result) => Success(result), error: (result) => Success(result) });
    });
});

var Success = function (result) {
    var $el = $('body');
    if (result.Title == "Error") {
        toastr.error(result.Message);
    }
    if (result.Title == "Success") {
        toastr.success(result.Message);
        setTimeout(function () {
            window.location.href = result.ReturnUrl;
        }, 2000);
    }
    $el.trigger('loading-overlay:hide');
}
