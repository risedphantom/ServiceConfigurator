$('document').ready(function() {
    $("#activeRestrictionsLink").trigger('click');
});

$('#sidebarUl a').on('click', function (e) {
    var $a = $(this);
    var $li = $a.parent();
    $li.addClass('active').siblings().removeClass('active');
    var href = $a.attr('href');
    $.get(href, function (data) {
        $("#content").html(data);
    })
    .fail(function(data) {
        $("#content").html(
        '<div class="col-sm-9 col-sm-offset-3 col-md-10 col-md-offset-2 main">' + 
            data.statusText + 
        '</div>');
    });
    e.preventDefault();
});

function xmlFormatter(value) {
    return value.replace(/</g, '&lt').replace(/>/g, '&gt');
}