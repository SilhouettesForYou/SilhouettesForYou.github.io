$(function () {
  $('[data-toggle="tooltip"]').tooltip();
  $('[data-toggle="tooltip"]').tooltip().on('inserted.bs.tooltip', function () {
    var width = $(this).attr('data-width');

    if (width == undefined || width == "") {
        $('.tooltip-inner').prop('style', '');
    } else {
        $('.tooltip-inner').prop('style', 'max-width:' + width);
    }

  });

    for (let i = 0; i < 3; i++) {
        var btn = document.getElementById('link-copy-' + (i + 1));
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

})