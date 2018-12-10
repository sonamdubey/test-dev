var Editorial = {
    utils: {
        getImageUrl: function (imageName, imageId) {
            var imgUrl = '';
            if (imageName != null) {
                imageName = imageName.toLowerCase();
                var ind = imageName.indexOf(".jpg");
                if (ind > 0) {
                    imageName = imageName.substring(0, ind);
                    var hyphenIndex = imageName.indexOf('-');
                    if (hyphenIndex < 0)
                        imageName = "-" + imageId;
                    imgUrl = imageName + "/";
                }
                else
                    imgUrl = imageName + "-" + imageId + "/";
            }
            return imgUrl;
        },

        getDisplayDate: function (date) {
            if (Object.prototype.toString.call(date) === "[object Date]")
            {
                return date.format("MMMM dd, yyyy, hh:mm tt");
            }
            else
            {
                var newDate = new Date(date);
                return newDate.format("MMMM dd, yyyy, hh:mm tt");
            }
        }
    }
}