var gulp = require('gulp'),
	cleanCss = require('gulp-clean-css'),
	uglify = require('gulp-uglify'),
	del = require('del'),
	sass = require('gulp-sass'),
	watch = require('gulp-watch'),
	gulpSequence = require('gulp-sequence'),
	fs = require('fs'),
	replace = require('gulp-replace'),
	fsCache = require('gulp-fs-cache')

var app = 'BikeWale.UI/',
	buildFolder = app + 'build/',
	minifiedAssetsFolder = buildFolder + 'min/';

var paths = {
	CSS: 'css/',
	JS: 'src/',
	SASS: 'sass/'
};

var pattern = {
	CSS_ATF: /<link(?:[^>]*)href=(?:"|')([^,"']*)(?:"|')(?:[^>]*)atf-css(?:[^>]*)\/{0,1}>/ig,
	INLINE_CSS: /<link(?:[^>]*)href=(?:"|')([^,"']*)(?:"|')(?:[^>]*)inline(?:[^>]*)\/{0,1}>/ig
};

gulp.task('clean', function () {
	return del([buildFolder]);
});

// minify css and js
gulp.task('minify-css', function () {
	var cssCache = fsCache(app + '.gulp-cache/' + paths.CSS);

	return gulp.src([app + paths.CSS + '**', app + 'm/' + paths.CSS + '**'], { base: app })
		.pipe(cssCache)
		.pipe(cleanCss())
		.pipe(cssCache.restore)
		.pipe(gulp.dest(minifiedAssetsFolder));
});

gulp.task('minify-js', function () {
	var jsCache = fsCache(app + '.gulp-cache/' + paths.JS);

	return gulp.src([app + paths.JS + '**', app + 'm/' + paths.JS + '**'], { base: app })
		.pipe(jsCache)
		.pipe(uglify().on('error', function (e) {
			console.log(e);
		}))
		.pipe(jsCache.restore)
		.pipe(gulp.dest(minifiedAssetsFolder));
});

gulp.task('minify-sass-css', function () {
	var sassCache = fsCache(app + '.gulp-cache/' + paths.SASS);

	return gulp.src(['BikeWale.UI/sass/**/*.sass', 'BikeWale.UI/m/sass/**/*.sass'], { base: 'BikeWale.UI/' })
		.pipe(sassCache)
		.pipe(sass().on('error', sass.logError))
		.pipe(cleanCss())
		.pipe(sassCache.restore)
		.pipe(gulp.dest('BikeWale.UI/build/min/'));
});


// desktop and mobile pages array to replace css link reference with internal css
var pageArray = [
	{
		folderName: 'generic/',
		fileName: 'BikeListing.aspx',
		stylesheet: 'sass/generic/listing.css'
	},
	{
		folderName: 'm/generic/',
		fileName: 'BikeListing.aspx',
		stylesheet: 'm/sass/generic/listing.css'
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

gulp.task('replace-mvc-layout-css-reference', function () {
	return gulp.src([
		app + 'Views/News/Index_Mobile_Pwa.cshtml',
		app + 'Views/News/Detail_Mobile.cshtml',
		app + 'Views/Videos/Index_Mobile_Pwa.cshtml',
		app + 'Views/Videos/Detail_Mobile_Pwa.cshtml',
		app + 'CategoryVideos_Mobile_Pwa.cshtml',
		app + 'Views/Shared/_Layout.cshtml',
		app + 'Views/Shared/_Layout_Mobile.cshtml'], { base: app })
		.pipe(replace(pattern.CSS_ATF, function (s, fileName) {
			var style = fs.readFileSync(minifiedAssetsFolder + fileName, 'utf-8'),
				style = style.replace(/@charset "utf-8";/g, "").replace(/\"/g, "'").replace(/\\[0-9]/g, "").replace(/[@]{1}/g, "@@"),
				styleTag = "<style type='text/css'>" + style + "</style>";

			return styleTag;
		}))
		.pipe(gulp.dest(buildFolder));
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

// replace PWA JS versions
gulp.task('replacePWAJsVersions' , function() {
	var assetsJson = require('./webpack-assets.json');
	if(!assetsJson) {
		console.log('Webpack assets json not found');
		return;
	}
	var replaceJson = {};
	
	replaceJson["/pwa/vendor.bundle.js"] = assetsJson["vendor"].js;
	replaceJson["/pwa/app.bundle.js"] = assetsJson["app"].js;
	replaceJson["/pwa/manifest.bundle.js"] = assetsJson["manifest"].js;
	var regexCssChunks = /<script>(.|\n)*window\.__CSS_CHUNKS__(.|\n)*=(.|\n)*{(.|\n)*}(.|\n)*(;?)(.|\n)*<\/script>/gm;
	var cssChunksJson = {};
	for (const key of Object.keys(assetsJson)) {
		if(assetsJson[key].css)
			cssChunksJson[key] = assetsJson[key].css;    
	}
	var cssChunksTag = '<script>window.__CSS_CHUNKS__='+JSON.stringify(cssChunksJson)+';</script>'
	return gulp.src([
		app + 'Views/News/Index_Mobile_Pwa.cshtml',
		app + 'Views/News/Detail_Mobile.cshtml',
		app + 'pwa/appshell.html',
		app + 'Views/Videos/Index_Mobile_Pwa.cshtml',
		app + 'Views/Videos/Details_Mobile_Pwa.cshtml'], { base: app })
		.pipe(replace(/\/pwa\/.*.bundle.js/g, function(match, p1, offset, string) {
			console.log('replacing '+match+ ' with ' + replaceJson[match]);
			return replaceJson[match];
	    }))
	    .pipe(replace(pattern.CSS_ATF, function (s, fileName) {
			var style = fs.readFileSync(minifiedAssetsFolder + fileName, 'utf-8'),
				style = style.replace(/@charset "utf-8";/g, "").replace(/\"/g, "'").replace(/\\[0-9]/g, "").replace(/[@]{1}/g, "@@"),
				styleTag = "<style type='text/css'>" + style + "</style>";

			return styleTag;
		}))
		.pipe(replace(regexCssChunks,function(match, p1, offset, string) {
			return cssChunksTag;
	    }))
	    .pipe(gulp.dest(buildFolder))
})

// replace desktop frameworks js, ie8 fix
gulp.task('bw-framework-js', function () {
	return gulp.src(app + paths.JS + 'frameworks.js', { base: app + paths.JS })
		.pipe(gulp.dest(minifiedAssetsFolder + paths.JS));
});

gulp.task('replace-css-link-reference', function () {
	return gulp.src(app + 'Views/**/*.cshtml', { base: app })
		.pipe(replace(pattern.INLINE_CSS, function (s, fileName) {
			var style = fs.readFileSync(minifiedAssetsFolder + fileName, 'utf-8'),
				styleTag = "<style type='text/css'>" + style.replace(/@charset "utf-8";/g, "").replace(/\"/g, "'").replace(/\\[0-9]/g, "") + "</style>";

			return styleTag;
		}))
		.pipe(gulp.dest(buildFolder));
});

gulp.task('sass', function () {
	return gulp.src([app + 'sass/**/*.sass', app + 'm/sass/**/*.sass'], { base: app })
		.pipe(sass().on('error', sass.logError))
		.pipe(gulp.dest(app));
});

//Watch task
gulp.task('sass:watch', function () {
	gulp.watch(app + 'sass/**/*.sass', ['sass']);
});

gulp.task('default',
	gulpSequence(
		'clean',
		'sass',
		'minify-css', 'minify-js', 'minify-sass-css',
		'bw-framework-js',
		'replace-css-reference',
		'replace-css-link-reference',
		// 'replace-mvc-layout-css-reference'//,
		'replacePWAJsVersions'
	)
);
//end
