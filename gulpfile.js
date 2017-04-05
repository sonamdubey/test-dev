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

var desktopSASSFolder = ['service/', 'sell-bike/', 'generic/', 'new-launch/', 'scooters/'],
    mobileSASSFolder = ['service/', 'sell-bike/', 'generic/', 'new-launch/', 'scooters/'];

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
        folderName: 'generic/',
        fileName: 'BikeListing.aspx',
        stylesheet: 'css/generic/listing.css'
    },
    {
        folderName: 'm/generic/',
        fileName: 'BikeListing.aspx',
        stylesheet: 'm/css/generic/listing.css'
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
    },
    {
        folderName: 'm/used/',
        fileName: 'Default.aspx',
        stylesheet: 'm/css/used/landing.css'
    },
    {
        folderName: 'm/used/',
        fileName: 'Search.aspx',
        stylesheet: 'm/css/used/search.css'
    },
    {
        folderName: 'm/used/',
        fileName: 'BikeDetails.aspx',
        stylesheet: 'm/css/used/details.css'
    }
];
/*
var mvcPageArray = [
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
	},
	{
	    folderName: 'Views/Shared/',
	    fileName: '_Layout_Desktop.cshtml',
	    stylesheet: 'css/bw-common-atf.css'
	},
    {
        folderName: 'Views/Shared/',
        fileName: '_Layout_Mobile.cshtml',
        stylesheet: 'm/css/bwm-common-atf.css'
    }
];
*/
var mvcLayoutPages = [
    {
        folderName: 'Views/Shared/',
        fileName: '_Layout.cshtml',
        stylesheet: 'css/bw-common-atf.css'
    },
    {
        folderName: 'Views/Shared/',
        fileName: '_Layout_Mobile.cshtml',
        stylesheet: 'm/css/bwm-common-atf.css'
    }
];

var mvcPageViews = [
    {
        folderName: 'Views/BikeCare/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/content/listing.css'
    },
    {
        folderName: 'Views/BikeCare/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/content/listing.css'
    },
    {
        folderName: 'Views/BikeCare/',
        fileName: 'Detail.cshtml',
        stylesheet: 'css/content/details.css'
    },
    {
        folderName: 'Views/BikeCare/',
        fileName: 'Detail_Mobile.cshtml',
        stylesheet: 'm/css/content/details.css'
    },
    {
        folderName: 'Views/ExpertReviews/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/content/listing.css'
    },
    {
        folderName: 'Views/ExpertReviews/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/content/listing.css'
    },
    {
        folderName: 'Views/ExpertReviews/',
        fileName: 'Detail.cshtml',
        stylesheet: 'css/content/details.css'
    },
    {
        folderName: 'Views/ExpertReviews/',
        fileName: 'Detail_Mobile.cshtml',
        stylesheet: 'm/css/content/details.css'
    },
    {
        folderName: 'Views/Features/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/content/listing.css'
    },
    {
        folderName: 'Views/Features/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/content/listing.css'
    },
    {
        folderName: 'Views/Features/',
        fileName: 'Detail.cshtml',
        stylesheet: 'css/content/details.css'
    },
    {
        folderName: 'Views/Features/',
        fileName: 'Detail_Mobile.cshtml',
        stylesheet: 'm/css/content/details.css'
    },
    {
        folderName: 'Views/News/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/content/listing.css'
    },
    {
        folderName: 'Views/News/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/content/listing.css'
    },
    {
        folderName: 'Views/News/',
        fileName: 'Detail.cshtml',
        stylesheet: 'css/content/details.css'
    },
    {
        folderName: 'Views/News/',
        fileName: 'Detail_Mobile.cshtml',
        stylesheet: 'm/css/content/details.css'
    },
    {
        folderName: 'Views/DealerShowroom/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/dealer/landing.css'
    },
    {
        folderName: 'Views/DealerShowroom/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/dealer/landing.css'
    },
    {
        folderName: 'Views/DealerShowroom/',
        fileName: 'DealersInIndia.cshtml',
        stylesheet: 'css/dealer/location.css'
    },
    {
        folderName: 'Views/DealerShowroom/',
        fileName: 'DealersInIndia_Mobile.cshtml',
        stylesheet: 'm/css/dealer/location.css'
    },
    {
        folderName: 'Views/DealerShowroom/',
        fileName: 'DealerInCity.cshtml',
        stylesheet: 'css/dealer/listing.css'
    },
    {
        folderName: 'Views/DealerShowroom/',
        fileName: 'DealerInCity_Mobile.cshtml',
        stylesheet: 'm/css/dealer/listing.css'
    },
    {
        folderName: 'Views/DealerShowroom/',
        fileName: 'DealerDetail.cshtml',
        stylesheet: 'css/dealer/details.css'
    },
    {
        folderName: 'Views/DealerShowroom/',
        fileName: 'DealerDetail_Mobile.cshtml',
        stylesheet: 'm/css/dealer/details.css'
    },
    {
        folderName: 'Views/HomePage/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/home.css'
    },
    {
        folderName: 'Views/HomePage/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/home.css'
    },
    {
        folderName: 'Views/Make/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/brand.css'
    },
    {
        folderName: 'Views/Make/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/bwm-brand.css'
    },
    {
        folderName: 'Views/Model/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/model-atf.css'
    },
    {
        folderName: 'Views/Model/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/bwm-model-atf.css'
    },
    {
        folderName: 'Views/NewLaunches/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/new-launch/new-launch.css'
    },
    {
        folderName: 'Views/NewLaunches/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/new-launch/new-launch.css'
    },
    {
        folderName: 'Views/NewLaunches/',
        fileName: 'BikeByMake.cshtml',
        stylesheet: 'css/new-launch/new-launch.css'
    },
    {
        folderName: 'Views/NewLaunches/',
        fileName: 'BikeByMake_Mobile.cshtml',
        stylesheet: 'm/css/new-launch/new-launch.css'
    },
    {
        folderName: 'Views/NewPage/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/newbikes.css'
    },
    {
        folderName: 'Views/NewPage/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/bwm-newbikes.css'
    },
    {
        folderName: 'Views/Price/',
        fileName: 'Details.cshtml',
        stylesheet: 'css/dealerpricequote.css'
    },
    {
        folderName: 'Views/Price/',
        fileName: 'Details_Mobile.cshtml',
        stylesheet: 'm/css/dealerpricequote.css'
    },
    {
        folderName: 'Views/PriceInCity/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/new/modelprice-in-city.css'
    },
    {
        folderName: 'Views/PriceInCity/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/new/bwm-modelprice-in-city.css'
    },
    {
        folderName: 'Views/Scooters/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/scooters/landing.css'
    },
    {
        folderName: 'Views/Scooters/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/scooters/landing.css'
    },
    {
        folderName: 'Views/Scooters/',
        fileName: 'BikesByMake.cshtml',
        stylesheet: 'css/scooters/listing.css'
    },
    {
        folderName: 'Views/Scooters/',
        fileName: 'BikesByMake_Mobile.cshtml',
        stylesheet: 'm/css/scooters/listing.css'
    },
    {
        folderName: 'Views/ServiceCenters/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/service/landing.css'
    },
    {
        folderName: 'Views/ServiceCenters/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/service/landing.css'
    },
    {
        folderName: 'Views/ServiceCenters/',
        fileName: 'ServiceCentersInIndia.cshtml',
        stylesheet: 'css/service/location.css'
    },
    {
        folderName: 'Views/ServiceCenters/',
        fileName: 'ServiceCentersInIndia_Mobile.cshtml',
        stylesheet: 'm/css/service/location.css'
    },
    {
        folderName: 'Views/ServiceCenters/',
        fileName: 'ServiceCentersInCity.cshtml',
        stylesheet: 'css/service/listing.css'
    },
    {
        folderName: 'Views/ServiceCenters/',
        fileName: 'ServiceCentersInCity_Mobile.cshtml',
        stylesheet: 'm/css/service/listing.css'
    },
    {
        folderName: 'Views/ServiceCenters/',
        fileName: 'ServiceCenterDetail.cshtml',
        stylesheet: 'css/service/details.css'
    },
    {
        folderName: 'Views/ServiceCenters/',
        fileName: 'ServiceCenterDetail_Mobile.cshtml',
        stylesheet: 'm/css/service/details.css'
    },
    {
        folderName: 'Views/Videos/',
        fileName: 'Index.cshtml',
        stylesheet: 'css/video/landing.css'
    },
    {
        folderName: 'Views/Videos/',
        fileName: 'Index_Mobile.cshtml',
        stylesheet: 'm/css/videos/bwm-videos-landing.css'
    },
    {
        folderName: 'Views/Videos/',
        fileName: 'Detail.cshtml',
        stylesheet: 'css/video/detail.css'
    },
    {
        folderName: 'Views/Videos/',
        fileName: 'Detail_Mobile.cshtml',
        stylesheet: 'm/css/videos/video-detail.css'
    },
    {
        folderName: 'Views/Videos/',
        fileName: 'CategoryVideos.cshtml',
        stylesheet: 'css/video/VideosByCategory.css'
    },
    {
        folderName: 'Views/Videos/',
        fileName: 'CategoryVideos_Mobile.cshtml',
        stylesheet: 'm/css/videos/VideosByCategory.css'
    },
    {
        folderName: 'Views/Videos/',
        fileName: 'Makes.cshtml',
        stylesheet: 'css/video/videos-by-make.css'
    },
    {
        folderName: 'Views/Videos/',
        fileName: 'Makes_Mobile.cshtml',
        stylesheet: 'm/css/videos/videos-by-make.css'
    },
    {
        folderName: 'Views/Videos/',
        fileName: 'Models.cshtml',
        stylesheet: 'css/video/videosByModel.css'
    },
    {
        folderName: 'Views/Videos/',
        fileName: 'Models_Mobile.cshtml',
        stylesheet: 'm/css/videos/videosByModel.css'
    }

];

// replace css reference with internal css for MVC views
gulp.task('replace-mvc-layout-css-reference', function () {
    var pageLength = mvcLayoutPages.length;

    for (var i = 0; i < pageLength; i++) {
        var element = mvcLayoutPages[i],
            style = fs.readFileSync(minifiedAssetsFolder + element.stylesheet, 'utf-8'),
			styleTag = "<style type='text/css'>" + style.replace(/@charset "utf-8";/g, "").replace(/\"/g, "'").replace(/\\/g, "\\") + "</style>",
            styleLink = "<link rel='stylesheet' type='text/css' href='/" + element.stylesheet + "' />";

        gulp.src(app + element.folderName + element.fileName, { base: app + element.folderName })
            .pipe(replace(styleLink, styleTag))
            .pipe(gulp.dest(buildFolder + element.folderName));
    }

    console.log('internal css reference replaced');
});

gulp.task('replace-mvc-pageview-css-reference', function () {
    var pageLength = mvcPageViews.length;

    for (var i = 0; i < pageLength; i++) {
        var element = mvcPageViews[i],
            style = fs.readFileSync(minifiedAssetsFolder + element.stylesheet, 'utf-8'),
			styleTag = "<style type='text/css'>@charset 'utf-8';" + style.replace(/\"/g, "'").replace(/\\/g, "\\") + "</style>",
            styleLink = "<link rel='stylesheet' type='text/css' href='/" + element.stylesheet + "' />";

        gulp.src(app + element.folderName + element.fileName, { base: app + element.folderName })
            .pipe(replace(styleLink, styleTag))
            .pipe(gulp.dest(buildFolder + element.folderName));
    }

    console.log('internal css reference replaced');
});

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
    'replace-mvc-layout-css-reference',
	'replace-mvc-pageview-css-reference'
    )
);