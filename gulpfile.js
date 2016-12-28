var gulp = require('gulp'),
  cleanCss = require('gulp-clean-css'),
  uglify = require('gulp-uglify'),
  del = require('del'),
  sass = require('gulp-sass'),
  watch = require('gulp-watch'),
  gulpSequence = require('gulp-sequence'),
  fs = require('fs'),
  replace = require('gulp-replace');

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

        genericListing: 'BikeWale.UI/generic/BikeListing.aspx'
    },

    mobile: {
        service: {
            baseFolder: 'BikeWale.UI/m/service',
            landing: 'BikeWale.UI/m/service/Default.aspx',
            city: 'BikeWale.UI/m/service/ServiceCenterInCountry.aspx',
            listing: 'BikeWale.UI/m/service/ServiceCenterList.aspx',
            details: 'BikeWale.UI/m/service/ServiceCenterDetails.aspx',
        },

        genericListing: 'BikeWale.UI/m/generic/BikeListing.aspx'
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

// replace css reference with internal css
gulp.task('replace-css-reference', function (callback) {
    gulpSequence('desktop-service-center', 'mobile-service-center', 'desktop-model')(callback)
});

// desktop service center
gulp.task('desktop-service-center', function (callback) {
    gulpSequence('desktop-service-landing', 'desktop-service-city', 'desktop-service-listing', 'desktop-service-details')(callback)
});

gulp.task('desktop-service-landing', function () {
    return gulp.src(page.desktop.service.landing, { base: page.desktop.service.baseFolder })
        .pipe(replace(/<link rel="stylesheet" type="text\/css" href="\/css\/service\/landing.css"[^>]*>/, function () {
            var style = fs.readFileSync(sassPaths.bw.service.target + '/landing.css', 'utf-8');
            return '<style type="text/css">\n@charset "utf-8";' + style + '</style>';
        }))
        .pipe(gulp.dest('BikeWale.UI/build/servicecenter'));
});

gulp.task('desktop-service-city', function () {
    return gulp.src(page.desktop.service.city, { base: page.desktop.service.baseFolder })
        .pipe(replace(/<link rel="stylesheet" type="text\/css" href="\/css\/service\/location.css"[^>]*>/, function () {
            var style = fs.readFileSync(sassPaths.bw.service.target + '/location.css', 'utf-8');
            return '<style type="text/css">\n@charset "utf-8";' + style + '</style>';
        }))
        .pipe(gulp.dest('BikeWale.UI/build/servicecenter'));
});

gulp.task('desktop-service-listing', function () {
    return gulp.src(page.desktop.service.listing, { base: page.desktop.service.baseFolder })
        .pipe(replace(/<link rel="stylesheet" type="text\/css" href="\/css\/service\/listing.css"[^>]*>/, function () {
            var style = fs.readFileSync(sassPaths.bw.service.target + '/listing.css', 'utf-8');
            return '<style type="text/css">\n@charset "utf-8";' + style + '</style>';
        }))
        .pipe(gulp.dest('BikeWale.UI/build/servicecenter'));
});


gulp.task('desktop-service-details', function () {
    return gulp.src(page.desktop.service.details, { base: page.desktop.service.baseFolder })
        .pipe(replace(/<link rel="stylesheet" type="text\/css" href="\/css\/service\/details.css"[^>]*>/, function () {
            var style = fs.readFileSync(sassPaths.bw.service.target + '/details.css', 'utf-8');
            return '<style type="text/css">\n@charset "utf-8";' + style + '</style>';
        }))
        .pipe(gulp.dest('BikeWale.UI/build/servicecenter'));
});

gulp.task('desktop-model', function () {
    return gulp.src(page.desktop.model, { base: 'BikeWale.UI/new' })
        .pipe(replace(/<link rel="stylesheet" type="text\/css" href="\/css\/model-atf.css"[^>]*>/, function () {
            var style = fs.readFileSync(paths.destinationD_CSS + '/model-atf.css', 'utf-8');
            return '<style type="text/css">\n@charset "utf-8";' + style + '</style>';
        }))
        .pipe(gulp.dest('BikeWale.UI/build/new'));
});

// mobile service center
gulp.task('mobile-service-center', function (callback) {
    gulpSequence('mobile-service-landing', 'mobile-service-city', 'mobile-service-listing', 'mobile-service-details')(callback)
});

gulp.task('mobile-service-landing', function () {
    return gulp.src(page.mobile.service.landing, { base: page.mobile.service.baseFolder })
        .pipe(replace(/<link rel="stylesheet" type="text\/css" href="\/m\/css\/service\/landing.css"[^>]*>/, function () {
            var style = fs.readFileSync(sassPaths.bwm.service.target + '/landing.css', 'utf-8');
            return '<style type="text/css">\n@charset "utf-8";' + style + '</style>';
        }))
        .pipe(gulp.dest('BikeWale.UI/build/m/service'));
});

gulp.task('mobile-service-city', function () {
    return gulp.src(page.mobile.service.city, { base: page.mobile.service.baseFolder })
        .pipe(replace(/<link rel="stylesheet" type="text\/css" href="\/m\/css\/service\/location.css"[^>]*>/, function () {
            var style = fs.readFileSync(sassPaths.bwm.service.target + '/location.css', 'utf-8');
            return '<style type="text/css">\n@charset "utf-8";' + style + '</style>';
        }))
        .pipe(gulp.dest('BikeWale.UI/build/m/service'));
});

gulp.task('mobile-service-listing', function () {
    return gulp.src(page.mobile.service.listing, { base: page.mobile.service.baseFolder })
        .pipe(replace(/<link rel="stylesheet" type="text\/css" href="\/m\/css\/service\/listing.css"[^>]*>/, function () {
            var style = fs.readFileSync(sassPaths.bwm.service.target + '/listing.css', 'utf-8');
            return '<style type="text/css">\n@charset "utf-8";' + style + '</style>';
        }))
        .pipe(gulp.dest('BikeWale.UI/build/m/service'));
});

gulp.task('mobile-service-details', function () {
    return gulp.src(page.mobile.service.details, { base: page.mobile.service.baseFolder })
        .pipe(replace(/<link rel="stylesheet" type="text\/css" href="\/m\/css\/service\/details.css"[^>]*>/, function () {
            var style = fs.readFileSync(sassPaths.bwm.service.target + '/details.css', 'utf-8');
            return '<style type="text/css">\n@charset "utf-8";' + style + '</style>';
        }))
        .pipe(gulp.dest('BikeWale.UI/build/m/service'));
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