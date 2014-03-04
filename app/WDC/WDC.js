
(function(w, $) {
    if (!w.WDC) {
        alert("WDC not defined!");
        return;
    }

    $.ajaxSetup({
        type: "post",
        cache: false,
        crossDomain:true,
        dataType:"json",
    });
    $.extend(w.WDC, {
        
        connect: function (command, data, cb) {

           return $.ajax({

                url: WDC.ServiceUrl + command,
                data: { data: data ,identity:WDC.Identity},
                complete: function () {
                    if ($.isFunction(cb))
                        cb();
                }



            });
            
        },
        store:function(data,callback) {
            return connect("store", data, callback);
        },
        next:function(data,callback) {
        return connect("next", data, callback);
    }


    });


})(window,jQuery);
