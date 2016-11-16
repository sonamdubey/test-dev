var gulp = require('gulp'),
  cleanCss = require('gulp-clean-css'),
  uglify = require('gulp-uglify'),
  del = require('del'),
  sass = require('gulp-sass'),
  watch = require('gulp-watch');

var paths = {
    bwCSS: 'BikeWale.UI/css/**',
    bwJS: 'BikeWale.UI/src/**',
    bwmCSS: 'BikeWale.UI/m/css/**',
    bwmJS: 'BikeWale.UI/m/src/**',
    bwSASS: 'BikeWale.UI/sass/**',
    destinationD_CSS: 'BikeWale.UI/min/css',
    destinationD_JS: 'BikeWale.UI/min/src',
    destinationM_CSS: 'BikeWale.UI/min/m/css',
    destinationM_JS: 'BikeWale.UI/min/m/src'
};

gulp.task('clean', function() {
    return del(['BikeWale.UI/min']);
});

gulp.task('minify-bw-css', ['clean'], function () {
    return gulp.src(paths.bwCSS, { base: 'BikeWale.UI/css/' })
        .pipe(cleanCss())
        .pipe(gulp.dest(paths.destinationD_CSS));
});

gulp.task('minify-bw-js', ['clean'], function () {
    return gulp.src(paths.bwJS, { base: 'BikeWale.UI/src/' })
        .pipe(uglify().on('error', function(e){
            console.log(e);
         }))
        .pipe(gulp.dest(paths.destinationD_JS));
});

gulp.task('minify-bwm-css', ['clean'], function() {
    return gulp.src(paths.bwmCSS, { base: 'BikeWale.UI/m/css/' })
        .pipe(cleanCss())
        .pipe(gulp.dest(paths.destinationM_CSS));
});

gulp.task('minify-bwm-js', ['clean'], function() {
    return gulp.src(paths.bwmJS, { base: 'BikeWale.UI/m/src/' })
        .pipe(uglify().on('error', function(e){
            console.log(e);
         }))
        .pipe(gulp.dest(paths.destinationM_JS));
});

gulp.task('sass', function () {
    return gulp.src(paths.bwSASS, { base: 'BikeWale.UI/sass/' })
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(paths.destinationD_CSS));
});

//Watch task
gulp.task('watch-sass', function () {
    gulp.watch(paths.bwSASS, ['sass']);
});

gulp.task('default', ['clean', 'minify-bw-css', 'minify-bw-js', 'minify-bwm-css', 'minify-bwm-js']);


gulp.task('sell-form', ['sass']);