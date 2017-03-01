
            $('.share-btn').click(function () {
                var str = $(this).attr('data-attr');
                var cururl = window.location.href;
                switch (str) {
                    case 'fb':
                        url = 'https://www.facebook.com/sharer/sharer.php?u=';
                        break;
                    case 'tw':
                        url = 'https://twitter.com/home?status=';
                        break;
                    case 'gp':
                        url = 'https://plus.google.com/share?url=';
                        break;
                    case 'wp':
                        var text = document.getElementsByTagName("title")[0].innerHTML;
                        var message = encodeURIComponent(text) + " - " + encodeURIComponent(cururl);
                        var whatsapp_url = "whatsapp://send?text=" + message;
                        url = whatsapp_url;
                        window.open(url, '_blank');
                        return;
                }
                url += cururl;
                window.open(url, '_blank');
            });
$('.comma').each(function (i, obj) {
    var y = formatPrice($(this).html());
    if (y != null)
        $(this).html(y);
});
function formatPrice(x) { try { x = x.toString(); var lastThree = x.substring(x.length - 3); var otherNumbers = x.substring(0, x.length - 3); if (otherNumbers != '') lastThree = ',' + lastThree; var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree; return res; } catch (err) { } }
