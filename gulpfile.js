var gulp = require('gulp'),
    cleanCss = require('gulp-clean-css'),
    uglify = require('gulp-uglify'),
    del = require('del'),
    sass = require('gulp-sass'),
    watch = require('gulp-watch'),
    gulpSequence = require('gulp-sequence'),
    fs = require('fs'),
    replace = require('gulp-replace');

var app = 'BikeWale.UI/',
    buildFolder = app + 'build/',
    minifiedAssetsFolder = buildFolder + 'min/';

var paths = {
    bwCSS: 'BikeWale.UI/css/**',
    bwJS: 'BikeWale.UI/src/**',

    bwmCSS: 'BikeWale.UI/m/css/**',
    bwmJS: 'BikeWale.UI/m/src/**',

    bwSASS: 'BikeWale.UI/sass/**',
    bwmSASS: 'BikeWale.UI/m/sass/**',

    destinationD_CSS: 'BikeWale.UI/build/min/css',
    destinationD_JS: 'BikeWale.UI/build/min/src',

    destinationM_CSS: 'BikeWale.UI/build/min/m/css',
    destinationM_JS: 'BikeWale.UI/build/min/m/src'
};

var sassPaths = {
    bw: {
        service: {
            source: 'BikeWale.UI/sass/service/**',
            target: 'BikeWale.UI/build/min/css/service'
        },

        sellBike: {
            source: 'BikeWale.UI/sass/sell-bike/**',
            target: 'BikeWale.UI/build/min/css/'
        },

        generic: {
            source: 'BikeWale.UI/sass/generic/**',
            target: 'BikeWale.UI/build/min/css/generic'
        }
    },
    bwm: {
        service: {
            source: 'BikeWale.UI/m/sass/service/**',
            target: 'BikeWale.UI/build/min/m/css/service'
        },

        sellBike: {
            source: 'BikeWale.UI/m/sass/sell-bike/**',
            target: 'BikeWale.UI/build/min/m/css/'
        },

        generic: {
            source: 'BikeWale.UI/m/sass/generic/**',
            target: 'BikeWale.UI/build/min/m/css/generic'
        }
    }
}

var page = {
    desktop: {
        service: {
            baseFolder: 'BikeWale.UI/servicecenter',
            landing: 'BikeWale.UI/servicecenter/Default.aspx',
            city: 'BikeWale.UI/servicecenter/ServiceCenterInCountry.aspx',
            listing: 'BikeWale.UI/servicecenter/ServiceCenterList.aspx',
            details: 'BikeWale.UI/servicecenter/ServiceCenterDetails.aspx',
        },

        model: 'BikeWale.UI/new/versions.aspx',

        genericListing: 'BikeWale.UI/generic/BikeListing.aspx',

        homepage: 'BikeWale.UI/default.aspx'
    },

    mobile: {
        service: {
            baseFolder: 'BikeWale.UI/m/service',
            landing: 'BikeWale.UI/m/service/Default.aspx',
            city: 'BikeWale.UI/m/service/ServiceCenterInCountry.aspx',
            listing: 'BikeWale.UI/m/service/ServiceCenterList.aspx',
            details: 'BikeWale.UI/m/service/ServiceCenterDetails.aspx',
        },

        genericListing: 'BikeWale.UI/m/generic/BikeListing.aspx',

        homepage: 'BikeWale.UI/m/default.aspx'
    }
}

gulp.task('clean', function () {
    return del(['BikeWale.UI/build']);
});

gulp.task('minify-bw-css', function () {
    return gulp.src(paths.bwCSS, { base: 'BikeWale.UI/css/' })
        .pipe(cleanCss())
        .pipe(gulp.dest(paths.destinationD_CSS));
});

gulp.task('minify-bw-js', function () {
    return gulp.src(paths.bwJS, { base: 'BikeWale.UI/src/' })
        .pipe(uglify().on('error', function (e) {
            console.log(e);
        }))
        .pipe(gulp.dest(paths.destinationD_JS));
});

gulp.task('minify-bwm-css', function () {
    return gulp.src(paths.bwmCSS, { base: 'BikeWale.UI/m/css/' })
        .pipe(cleanCss())
        .pipe(gulp.dest(paths.destinationM_CSS));
});

gulp.task('minify-bwm-js', function () {
    return gulp.src(paths.bwmJS, { base: 'BikeWale.UI/m/src/' })
        .pipe(uglify().on('error', function (e) {
            console.log(e);
        }))
        .pipe(gulp.dest(paths.destinationM_JS));
});

// desktop: sass to min css [build folder]
gulp.task('bw-sass', function (callback) {
    gulpSequence('bw-service-sass', 'bw-sell-bike-sass', 'bw-generic-sass')(callback)
});

gulp.task('bw-service-sass', function () {
    return gulp.src(sassPaths.bw.service.source, { base: 'BikeWale.UI/sass/service/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(cleanCss())
        .pipe(gulp.dest(sassPaths.bw.service.target));
});

gulp.task('bw-sell-bike-sass', function () {
    return gulp.src(sassPaths.bw.sellBike.source, { base: 'BikeWale.UI/sass/sell-bike/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(cleanCss())
        .pipe(gulp.dest(sassPaths.bw.sellBike.target));
});

gulp.task('bw-generic-sass', function () {
    return gulp.src(sassPaths.bw.generic.source, { base: 'BikeWale.UI/sass/generic/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(cleanCss())
        .pipe(gulp.dest(sassPaths.bw.generic.target));
});

// desktop: sass to original css
gulp.task('bw-sass-to-css', function (callback) {
    gulpSequence('bw-service-sass', 'bw-sell-bike-sass', 'bw-generic-css')(callback)
});

gulp.task('bw-service-css', function () {
    return gulp.src(sassPaths.bw.service.source, { base: 'BikeWale.UI/sass/service/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('BikeWale.UI/css/service/'));
});

gulp.task('bw-sell-bike-css', function () {
    return gulp.src(sassPaths.bw.sellBike.source, { base: 'BikeWale.UI/sass/sell-bike/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('BikeWale.UI/css/'));
});

gulp.task('bw-generic-css', function () {
    return gulp.src(sassPaths.bw.generic.source, { base: 'BikeWale.UI/sass/generic/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('BikeWale.UI/css/generic/'));
});

// mobile: sass to min css [build folder]
gulp.task('bwm-sass', function (callback) {
    gulpSequence('bwm-service-sass', 'bwm-sell-bike-sass', 'bwm-generic-sass')(callback)
});

gulp.task('bwm-service-sass', function () {
    return gulp.src(sassPaths.bwm.service.source, { base: 'BikeWale.UI/m/sass/service/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(cleanCss())
        .pipe(gulp.dest(sassPaths.bwm.service.target));
});

gulp.task('bwm-sell-bike-sass', function () {
    return gulp.src(sassPaths.bwm.sellBike.source, { base: 'BikeWale.UI/m/sass/sell-bike/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(cleanCss())
        .pipe(gulp.dest(sassPaths.bwm.sellBike.target));
});

gulp.task('bwm-generic-sass', function () {
    return gulp.src(sassPaths.bwm.generic.source, { base: 'BikeWale.UI/m/sass/generic/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(cleanCss())
        .pipe(gulp.dest(sassPaths.bwm.generic.target));
});

// mobile: sass to original css
gulp.task('bwm-sass-to-css', function (callback) {
    gulpSequence('bwm-service-css', 'bwm-sell-bike-css', 'bwm-generic-css')(callback)
});

gulp.task('bwm-service-css', function () {
    return gulp.src(sassPaths.bwm.service.source, { base: 'BikeWale.UI/m/sass/service/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('BikeWale.UI/m/css/service/'));
});

gulp.task('bwm-sell-bike-css', function () {
    return gulp.src(sassPaths.bwm.sellBike.source, { base: 'BikeWale.UI/m/sass/sell-bike/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('BikeWale.UI/m/css/'));
});

gulp.task('bwm-generic-css', function () {
    return gulp.src(sassPaths.bwm.generic.source, { base: 'BikeWale.UI/m/sass/generic/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('BikeWale.UI/m/css/generic/'));
});

//Watch task
gulp.task('watch-sass', function () {
    gulp.watch(paths.bwSASS, ['bw-sass']);
    gulp.watch(paths.bwmSASS, ['bwm-sass']);
});

// desktop and mobile pages array to replace css link reference with internal css
var pageArray = [
    {
        folderName: 'servicecenter/',
        fileName: 'Default.aspx',
        stylesheet: 'css/service/landing.css'
    },
    {
        folderName: 'servicecenter/',
        fileName: 'ServiceCenterInCountry.aspx',
        stylesheet: 'css/service/location.css'
    },
    {
        folderName: 'servicecenter/',
        fileName: 'ServiceCenterList.aspx',
        stylesheet: 'css/service/listing.css'
    },
    {
        folderName: 'servicecenter/',
        fileName: 'ServiceCenterDetails.aspx',
        stylesheet: 'css/service/details.css'
    },
    {
        folderName: 'new/',
        fileName: 'versions.aspx',
        stylesheet: 'css/model-atf.css'
    },
    {
        folderName: 'generic/',
        fileName: 'BikeListing.aspx',
        stylesheet: 'css/generic/listing.css'
    },
    {
        folderName: 'm/service/',
        fileName: 'Default.aspx',
        stylesheet: 'm/css/service/landing.css'
    },
    {
        folderName: 'm/service/',
        fileName: 'ServiceCenterInCountry.aspx',
        stylesheet: 'm/css/service/location.css'
    },
    {
        folderName: 'm/service/',
        fileName: 'ServiceCenterList.aspx',
        stylesheet: 'm/css/service/listing.css'
    },
    {
        folderName: 'm/service/',
        fileName: 'ServiceCenterDetails.aspx',
        stylesheet: 'm/css/service/details.css'
    },
    {
        folderName: 'm/generic/',
        fileName: 'BikeListing.aspx',
        stylesheet: 'm/css/generic/listing.css'
    },
    {
        folderName: '',
        fileName: 'default.aspx',
        stylesheet: 'css/home.css'
    },
    {
        folderName: 'm/',
        fileName: 'Default.aspx',
        stylesheet: 'm/css/home.css'
    },
    {
        folderName: 'includes/',
        fileName: 'headscript_desktop_min.aspx',
        stylesheet: 'css/bw-common-atf.css'
    },
    {
        folderName: 'includes/',
        fileName: 'headscript_mobile_min.aspx',
        stylesheet: 'm/css/bwm-common-atf.css'
    },
    {
        folderName: 'm/new/',
        fileName: 'BikeModels.aspx',
        stylesheet: 'm/css/bwm-model-atf.css'
    },
    {
        folderName: 'm/new/photos/',
        fileName: 'Default.aspx',
        stylesheet: 'm/css/photos.css'
    }
];

// replace css reference with internal css
gulp.task('replace-css-reference', function () {
    var pageLength = pageArray.length;

    for (var i = 0; i < pageLength; i++) {
        var element = pageArray[i],
            style = fs.readFileSync(minifiedAssetsFolder + element.stylesheet, 'utf-8'),
            styleTag = '<style type="text/css">\n@charset "utf-8";' + style + '\n</style>',
            styleLink = '<link rel="stylesheet" type="text/css" href="/' + element.stylesheet + '" />';

        gulp.src(app + element.folderName + element.fileName, { base: app + element.folderName })
            .pipe(replace(styleLink, styleTag))
            .pipe(gulp.dest(buildFolder + element.folderName));
    }

    console.log('internal css reference replaced');
});

// replace desktop frameworks js, ie8 fix
gulp.task('bw-framework-js', function () {
    return gulp.src('BikeWale.UI/src/frameworks.js', { base: 'BikeWale.UI/src/' })
        .pipe(gulp.dest(paths.destinationD_JS));
});

gulp.task('default', gulpSequence(
    'clean',
    'minify-bw-css', 'minify-bw-js',
    'minify-bwm-css', 'minify-bwm-js',
    'bw-framework-js',
    'bw-sass', 'bwm-sass',
    'bw-sass-to-css', 'bwm-sass-to-css',
    'replace-css-reference'
    )
);