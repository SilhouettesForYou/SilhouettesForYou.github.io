$(function () {
    var title = document.title;
    if (title.indexOf("Latex 公式") < 0 && title.indexOf("Mingzhe Wang") < 0) {
        return;
    }
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="tooltip"]').tooltip().on('inserted.bs.tooltip', function () {
        var width = $(this).attr('data-width');
    
        if (width == undefined || width == "") {
            $('.tooltip-inner').prop('style', '');
        } else {
            $('.tooltip-inner').prop('style', 'max-width:' + width);
        }
  
    });
    if (title.indexOf("Mingzhe Wang") < 0) {
        return;
    }
    for (let i = 0; i < 3; i++) {
        var btn = document.getElementById('link-copy-' + (i + 1));
        if (btn) {
            var clipboard = new ClipboardJS(btn);
            // 复制
            clipboard.on('success', function (e) {
                e.clearSelection()
                const toastLive = document.getElementById("liveToast");
                const toast = new bootstrap.Toast(toastLive, {
                    "delay": 2000
                });
                toast.show();
            });
        }
    }

})