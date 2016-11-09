var gulp = require('gulp'),
  cleanCss = require('gulp-clean-css'),
  uglify = require('gulp-uglify'),
  del = require('del'),
  sass = require('gulp-sass'),
  watch = require('gulp-watch'),
  gulpSequence = require('gulp-sequence');

var paths = {
    bwCSS: 'BikeWale.UI/css/**',
    bwJS: 'BikeWale.UI/src/**',
    bwmCSS: 'BikeWale.UI/m/css/**',
    bwmJS: 'BikeWale.UI/m/src/**',
    bwSASS: 'BikeWale.UI/sass/**',
    bwmSASS: 'BikeWale.UI/m/sass/**',
    destinationD_CSS: 'BikeWale.UI/min/css',
    destinationD_JS: 'BikeWale.UI/min/src',
    destinationM_CSS: 'BikeWale.UI/min/m/css',
    destinationM_JS: 'BikeWale.UI/min/m/src'
};

var sassPaths = {
    bwm: {
        service: {
            source: 'BikeWale.UI/m/sass/service/**',
            target: 'BikeWale.UI/min/m/css/service'
        }
    }
}

gulp.task('clean', function () {
    return del(['BikeWale.UI/min']);
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

gulp.task('bw-sass', function () {
    return gulp.src(paths.bwSASS, { base: 'BikeWale.UI/sass/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(paths.destinationD_CSS));
});

gulp.task('bwm-service-sass', function () {
    return gulp.src(sassPaths.bwm.service.source, { base: 'BikeWale.UI/m/sass/service/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(sassPaths.bwm.service.target));
});

gulp.task('bwm-service-css', function () {
    return gulp.src(sassPaths.bwm.service.source, { base: 'BikeWale.UI/m/sass/service/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('BikeWale.UI/m/css/service/'));
});

//Watch task
gulp.task('watch-sass', function () {
    gulp.watch(paths.bwSASS, ['bw-sass']);
    gulp.watch(paths.bwmSASS, ['bwm-sass']);
});

gulp.task('bwm-sass', function (callback) {
    gulpSequence('bwm-service-sass')(callback)
});

gulp.task('default', gulpSequence('clean', 'minify-bw-css', 'minify-bw-js', 'minify-bwm-css', 'minify-bwm-js', 'bw-sass', 'bwm-sass', 'bwm-service-css'));
