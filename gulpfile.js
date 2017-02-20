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
    CSS: 'css/',
    JS: 'src/',
    SASS: 'sass/'
};

gulp.task('clean', function () {
    return del([buildFolder]);
});

// minify css and js
gulp.task('minify-bw-css', function () {
    return gulp.src(app + paths.CSS + '**', { base: app + paths.CSS })
        .pipe(cleanCss())
        .pipe(gulp.dest(minifiedAssetsFolder + paths.CSS));
});

gulp.task('minify-bw-js', function () {
    return gulp.src(app + paths.JS + '**', { base: app + paths.JS })
        .pipe(uglify().on('error', function (e) {
            console.log(e);
        }))
        .pipe(gulp.dest(minifiedAssetsFolder + paths.JS));
});

gulp.task('minify-bwm-css', function () {
    return gulp.src(app + 'm/' + paths.CSS + '**', { base: app + 'm/' + paths.CSS })
        .pipe(cleanCss())
        .pipe(gulp.dest(minifiedAssetsFolder + 'm/' + paths.CSS));
});

gulp.task('minify-bwm-js', function () {
    return gulp.src(app + 'm/' + paths.JS + '**', { base: app + 'm/' + paths.JS })
        .pipe(uglify().on('error', function (e) {
            console.log(e);
        }))
        .pipe(gulp.dest(minifiedAssetsFolder + 'm/' + paths.JS));
});

var desktopSASSFolder = ['service/', 'sell-bike/', 'generic/', 'new-launch/'],
    mobileSASSFolder = ['service/', 'sell-bike/', 'generic/', 'new-launch/'];

// convert desktop sass to css
gulp.task('bw-sass-to-css', function () {
    var fileLength = desktopSASSFolder.length;

    for (var i = 0; i < fileLength; i++) {
        gulp.src(app + 'sass/' + desktopSASSFolder[i] + '**', { base: app + 'sass/' + desktopSASSFolder[i] })
            .pipe(sass().on('error', sass.logError))
            .pipe(gulp.dest(app + 'css/' + desktopSASSFolder[i]))
    }

    console.log('Desktop SASS files converted to CSS files');
});

// convert mobile sass to css
gulp.task('bwm-sass-to-css', function () {
    var fileLength = mobileSASSFolder.length;

    for (var i = 0; i < fileLength; i++) {
        gulp.src(app + 'm/sass/' + mobileSASSFolder[i] + '**', { base: app + 'm/sass/' + mobileSASSFolder[i] })
            .pipe(sass().on('error', sass.logError))
            .pipe(gulp.dest(app + 'm/css/' + mobileSASSFolder[i]))
    }

    console.log('Mobile SASS files converted to CSS files');
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
    },
    {
        folderName: 'm/new/compare/',
        fileName: 'CompareBike.aspx',
        stylesheet: 'm/css/compare/landing.css'
    },
    {
        folderName: 'm/new/compare/',
        fileName: 'CompareBikeDetails.aspx',
        stylesheet: 'm/css/compare/details.css'
    }
	
];

var mvcPageArray =[
	{
        folderName: 'Views/m/NewLaunches/',
        fileName: 'Index.cshtml',
        stylesheet: 'm/css/new-launch/new-launch.css'
    },
	{
        folderName: 'Views/m/NewLaunches/',
        fileName: 'BikesByMake.cshtml',
        stylesheet: 'm/css/new-launch/new-launch.css'
    },
	{
        folderName: 'Views/m/NewLaunches/',
        fileName: 'BikesByYear.cshtml',
        stylesheet: 'm/css/new-launch/new-launch.css'
    },
	{
        folderName: 'Views/NewLaunches/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/new-launch/new-launch.css'
    },
	{
        folderName: 'Views/NewLaunches/',
        fileName: 'BikesByYear.cshtml',
        stylesheet: 'css/new-launch/new-launch.css'
    },
	{
        folderName: 'Views/NewLaunches/',
        fileName: 'BikesByMake.cshtml',
        stylesheet: 'css/new-launch/new-launch.css'
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

// replace css reference with internal css for MVC views
gulp.task('replace-mvc-css-reference', function () {
    var pageLength = mvcPageArray.length;

    for (var i = 0; i < pageLength; i++) {
        var element = mvcPageArray[i],
            style = fs.readFileSync(minifiedAssetsFolder + element.stylesheet, 'utf-8'),
			styleTag = "<style type='text/css'>@charset 'utf-8';" + style.replace(/\"/g,"'").replace(/\\/g,"\\") + "</style>",
            styleLink = "<link rel='stylesheet' type='text/css' href='/" + element.stylesheet + "' />";

        gulp.src(app + element.folderName + element.fileName, { base: app + element.folderName })
            .pipe(replace(styleLink, styleTag))
            .pipe(gulp.dest(buildFolder + element.folderName));
    }

    console.log('internal css reference replaced');
});

// replace desktop frameworks js, ie8 fix
gulp.task('bw-framework-js', function () {
    return gulp.src(app + paths.JS + 'frameworks.js', { base: app + paths.JS })
        .pipe(gulp.dest(minifiedAssetsFolder + paths.JS));
});

//Watch task
gulp.task('watch-sass', function () {
    gulp.watch(app + paths.SASS + '**', ['bw-sass-to-css']);
    gulp.watch(app + 'm/' + paths.SASS + '**', ['bwm-sass-to-css']);
});

gulp.task('default', gulpSequence(
    'clean',
    'bw-sass-to-css', 'bwm-sass-to-css',
    'minify-bw-css', 'minify-bw-js',
    'minify-bwm-css', 'minify-bwm-js',
    'bw-framework-js',
    'replace-css-reference',
	'replace-mvc-css-reference'
    )
);