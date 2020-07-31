$(".btn,.navbar a").click(function(e){
    var ripple = $(this);
    if(ripple.find(".efekt").length == 0) {
        ripple.append("<span class='efekt'></span>");
    }

    var efekt = ripple.find(".efekt");

    efekt.removeClass("oynat");

    if(!efekt.height() && !efekt.width())
    {
        var d = Math.max(ripple.outerWidth(), ripple.outerHeight());
        efekt.css({height: 150, width: 150});
    }

    var x = e.pageX - ripple.offset().left - efekt.width()/2;
    var y = e.pageY - ripple.offset().top - efekt.height()/2;

    efekt.css({
        top: y+'px',
        left:x+'px'
    }).addClass("oynat");
})