var gulp = require('gulp'),
	cleanCss = require('gulp-clean-css'),
	cssmin = require('gulp-cssmin'),
	uglify = require('gulp-uglify'),
	del = require('del'),
	sass = require('gulp-sass'),
	watch = require('gulp-watch'),
	gulpSequence = require('gulp-sequence'),
	fs = require('fs'),
	replace = require('gulp-replace'),
	fsCache = require('gulp-fs-cache'),
	rev = require('gulp-rev'),
	htmlmin = require('gulp-htmlmin'),
	workbox = require('workbox-build');


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
	INLINE_CSS: /<link(?:[^>]*)href=(?:"|')([^,"']*)(?:"|')(?:[^>]*)inline(?:[^>]*)\/{0,1}>/ig,
	AMP_CSS: /<link(?:[^>]*)href=(?:"|')([^,"']*)(?:"|')(?:[^>]*)amp(?:[^>]*)\/{0,1}>/ig
};

var Configuration = process.argv[3] || 'Debug';
var webpackAssetsJson = require('./BikeWale.UI/pwa/webpack-assets.json');
if(!webpackAssetsJson) {
	console.log('Webpack assets json not found');
	return;
}
var cssChunksJson = {};
for (const key of Object.keys(webpackAssetsJson)) {
	if(webpackAssetsJson[key].css)
		cssChunksJson[key] = webpackAssetsJson[key].css;    
}
var jsChunksJson = {};
for(const key of Object.keys(webpackAssetsJson)) {
	if(webpackAssetsJson[key].js) {
		jsChunksJson['/pwa/js/'+key+'.bundle.js'] = webpackAssetsJson[key].js;
	}
}

gulp.task('clean', function () {
	return del([buildFolder]);
});

// minify css and js
gulp.task('minify-css', function () {
	var cssCache = fsCache(app + '.gulp-cache/' + paths.CSS);

	return gulp.src([app + paths.CSS + '**', app + 'm/' + paths.CSS + '**'], { base: app })
		.pipe(cssCache)
		.pipe(cssmin())
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

	return gulp.src([app + 'sass/**/*.sass', app + 'm/sass/**/*.sass'], { base: app })
		.pipe(sassCache)
		.pipe(sass().on('error', sass.logError))
		.pipe(cssmin())
		.pipe(sassCache.restore)
		.pipe(gulp.dest(buildFolder))
		.pipe(gulp.dest(minifiedAssetsFolder));
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
gulp.task('replace-amp-css-link-reference', function () {
	return gulp.src(app + 'Views/**/*.cshtml', { base: app })
		.pipe(replace(pattern.AMP_CSS, function (s, fileName) {
			var style = fs.readFileSync(minifiedAssetsFolder + fileName, 'utf-8'),
			styleTag = style.replace(/@charset "utf-8";/g, "").replace(/\"/g, "'").replace(/\\[0-9]/g, "");
			return styleTag;
		}))
		.pipe(gulp.dest(buildFolder));
});

// PWA specific gulp tasks
var patternJSBundle = /\/pwa\/js\/(\w)+.bundle.js/g;
var replaceJsVersion = function(match, p1, offset, string) { //replace js urls with hashcode
			if(jsChunksJson[match])
				return jsChunksJson[match];
			else 
				console.log('No match found for js link '+match);
	    }
var replaceInlineCssReferenceLink = function (s, fileName) { // replace inline css
			
			var regex = /\/pwa\/css\/(.*\/)?(.*).css/;
			var matches = regex.exec(fileName);
			if(!matches || !matches[2]) {
				console.log('Could not find filename from '+fileName);
				return;
			}
			var cssNameWithHash = cssChunksJson[matches[2]];
						
			regex = /\/pwa\/css\/.*\.(.*)\.css/;
			matches = regex.exec(cssNameWithHash);

			if(!matches || !matches[1]) {
				console.log('Could not extract chunk hash for '+fileName);
				return;
			}
			var fileName = fileName.replace('.css','.'+matches[1]+'.css');
			var style = fs.readFileSync(app + fileName, 'utf-8'),
				style = style.replace(/@charset "utf-8";/g, "").replace(/\"/g, "'").replace(/\\[0-9]/g, "").replace(/[@]{1}/g, "@@"),
				styleTag = "<style type='text/css'>" + style + "</style>";
			return styleTag;
		}
gulp.task('replace-js-css-reference' , function() {
	if(Configuration == "Debug") {
				return
			}
	return gulp.src([
		app + 'Views/News/Index_Mobile_Pwa.cshtml',
		app + 'Views/News/Detail_Mobile.cshtml',
		app + 'Views/Videos/Index_Mobile_Pwa.cshtml',
		app + 'Views/Videos/Details_Mobile_Pwa.cshtml',
		app + 'Views/Videos/CategoryVideos_Mobile_Pwa.cshtml',
		app + 'Views/ExpertReviews/Index_Mobile_Pwa.cshtml',
		app + 'Views/ExpertReviews/Detail_Mobile_Pwa.cshtml',
		app + 'Views/Shared/Index_Mobile_Pwa.cshtml'
		], { base: app })
		.pipe(replace(patternJSBundle,replaceJsVersion))
	    .pipe(replace(pattern.CSS_ATF, replaceInlineCssReferenceLink ))
		.pipe(gulp.dest(buildFolder));
})

// replace css chunk links
var cssChunksJsonPattern = /<script>(.|\n)*window\.__CSS_CHUNKS__(.|\n)*=(.|\n)*?<\/script>/g;
var cssChunksTag = '<script>window.__CSS_CHUNKS__='+JSON.stringify(cssChunksJson)+';</script>';
gulp.task('replace-css-chunk-json',function() {
	return gulp.src(app + 'Views/Shared/_CssChunksScript.cshtml' , { base: app })
		.pipe(replace(cssChunksJsonPattern, function(match, p1, offset, string){
			return cssChunksTag;
		}))
		.pipe(gulp.dest(buildFolder));
})

function getCdnPath() {
	switch(Configuration) {
		case "Debug":
			return '/';
		case "Staging":
			return '/'; // not placing static files on staging cdn
			// return 'https://stb.aeplcdn.com/staging/bikewale/';
		case "Release":
			return 'https://stb.aeplcdn.com/bikewale/';
		default : 
			console.log('Wrong environment');
			return '';
	}
}


gulp.task('appshell-procesing', function() {
	if(Configuration == "Debug") {
		return
	}
	
	return gulp.src([
		app + 'pwa/appshell.html'
		] , { base : app})
		.pipe(replace(patternJSBundle,replaceJsVersion))
	    .pipe(replace(pattern.CSS_ATF,replaceInlineCssReferenceLink))
	    .pipe(replace(cssChunksJsonPattern,function(match, p1, offset, string){
			return cssChunksTag;
		}))
		.pipe(replace(/@@/g,function(match, p1, offset, string){
			return '@';
		}))
		.pipe(htmlmin({collapseWhitespace: true, minifyJS : true}))
		.pipe(rev())
		.pipe(gulp.dest(buildFolder))
		.pipe(rev.manifest({
			base: process.cwd(),
			merge : true
		}))
		.pipe(gulp.dest(buildFolder))
})



gulp.task("replace-filepath-in-SW" , function() {
	if(Configuration == "Debug") {
		return
	}
	var revManifest = require('./'+buildFolder+'rev-manifest.json');
	var cdnUrlPattern = /var(\s)*baseUrl(\s|\n)*=(\s|\n)*(?:"|')([^,"']*)(?:"|')(\s|\n)*;/
	return gulp.src([app + 'm/sw.js',
    				app + 'm/news/sw.js'] , { base: app })
        .pipe(replace(/pwa\/appshell(-(\w)*)?\.html/g , function(match, p1, offset, string){ 
        	return revManifest["pwa/appshell.html"]
        	
        }))
        .pipe(replace(cdnUrlPattern,function(match, p1, offset, string){
			return 'var baseUrl = \''+getCdnPath()+'\';';
		}))
		.pipe(replace(/workboxSW\.precache\(\[.*\]\);?/g,function(match,p1,offset,string) {
			console.log('found appshell in sw');
			return "workboxSW.precache([]);";
		}))
        .pipe(gulp.dest(buildFolder));
});

gulp.task('generate-service-worker' , function() { // this task has to follow 'replace-filepath-in-SW' as it takes the file created by previous task
	if(Configuration == "Debug") {
		return
	}
	
	var revManifest = require('./'+buildFolder+'rev-manifest.json');
	var regexPatternForAppshell = /.*appshell.*/;
	return workbox.injectManifest({
		swSrc : buildFolder + 'm/sw.js',
		swDest : buildFolder + 'm/sw.js',
		globDirectory : buildFolder,
		staticFileGlobs: [
			revManifest["pwa/appshell.html"]
		],
		dontCacheBustUrlsMatching: regexPatternForAppshell,
		manifestTransforms : [(manifestEntries) => {
			return manifestEntries.map(entry => {
				if (entry.url.match(regexPatternForAppshell)) {
			      return getCdnPath()+revManifest["pwa/appshell.html"];
			    } 
			})
		}]
		

	})
}); 

gulp.task('sass', function () {
	return gulp.src([app + 'sass/**/*.sass', app + 'm/sass/**/*.sass'], { base: app })
		.pipe(sass().on('error', sass.logError))
		.pipe(cssmin())
		.pipe(gulp.dest(app));
});

gulp.task('sass:watch', function () {
    gulp.watch([app + 'sass/**/*.sass', app + 'm/sass/**/*.sass'], ['sass']);
});


gulp.task('default', gulpSequence(
			'clean',
			'sass',
			'minify-css', 'minify-js', 'minify-sass-css',
			'bw-framework-js',
			'replace-css-reference',
			'replace-css-link-reference',
			'replace-amp-css-link-reference',
			'replace-css-chunk-json',
			'replace-js-css-reference',
			'appshell-procesing',
			'replace-filepath-in-SW',
			'generate-service-worker',
			'replace-mvc-layout-css-reference'
			
		)
);
//end
