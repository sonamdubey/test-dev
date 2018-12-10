//Owner - Akansha Srivastava
//Created on - 22.09.2016
//Warning: Before modifying please consult 

//Include all the necessary plugins for the build process
var gulp = require('gulp'),
	plugins = require('gulp-load-plugins')(),
	runSequence = require('run-sequence'),
	htmlreplace = require('gulp-html-replace');
var replaceStyle = require('gulp-replace');
var fs = require('fs');
var pump = require('pump');
var rev = require('gulp-rev');
var fsCache = require('gulp-fs-cache');
var saveLicense = require('uglify-save-license');
var exec = require('child_process').exec;
var path = require('path');
var fs = require('fs');
var mkdirp = require('mkdirp');
var readline = require('readline');
var sass = require('gulp-sass');
var CleanCSS = require('clean-css');
var versionMapping = {};
var configuration = "Debug";
var cdnURL = "https://stc.aeplcdn.com";
var stagingCdnUrl = "https://stc-staging.aeplcdn.com";
var isDevelopment = false;
var fileType = {
	css: 'css',
	js: 'js'
};
var get_branch_cmd = "git rev-parse --abbrev-ref HEAD";
var curr_branch, release_folder_path;

function checkVersionedFilenameValidity(versionedName, filename) {
    if (versionedName == undefined) {
        console.log("No versioned mapping found for file: " + filename)
        process.exit(1)
    }
}

function getFilePath(filename) {
	var filepath = "";
	switch (configuration) {
		case "Debug":
			filepath = "/static/" + filename;
			break;
		case "Staging":
			filepath = stagingCdnUrl + "/staticminv2/" + filename;
			break;
		case "Release":
			filepath = cdnURL + "/staticminv2/" + filename;
			break;
		default:
			console.log("Polluted Environment! Go, plant some trees.")
	}
	return filepath;
}

gulp.task('clean', function () {
	return gulp.src(['./Static/staticminv2/', './build/','./Static/staticminv1/'], { read: false })
		.pipe(plugins.clean());
});

gulp.task('minifyJS', function (cb) {
	var jsCache = fsCache('.gulp-cache/js');
	var options = {
	    preserveComments: 'license',
	    compress: {
	        drop_console: true
	    }
		//output: {
		//	comments : saveLicense
		//}
	};
	pump([
	 gulp.src(['./Static/**/*.js', '!./Static/staticminv2/**/*.js', '!./Static/Test/**/*.js'], { base: './Static' }),
	 jsCache,
	 plugins.uglify(options),
	 jsCache.restore,
	 rev(),
	 gulp.dest('./Static/staticminv2/'),
	 plugins.rename(function (path) { path.dirname = path.dirname.toLowerCase(); path.basename = path.basename.toLowerCase(); }),
	 rev.manifest({
	 	base: './Static/staticminv2/',
	 	merge: true // merge with the existing manifest if one exists
	 }),
	 plugins.header('\ufeff'),
	 gulp.dest('./Static/staticminv2/')
	],
   cb
 );
});

gulp.task('minifyCSS', function (cb) {
	var cssCache = fsCache('.gulp-cache/js');
	pump([
	 gulp.src(['./Static/**/*.css', '!./Static/staticminv2/**/*.css'], { base: './Static' }),
	 cssCache,
	 plugins.cssmin(),
	 cssCache.restore,
	 rev(),
	 gulp.dest('./Static/staticminv2/'),
	 plugins.rename(function (path) { path.dirname = path.dirname.toLowerCase(); path.basename = path.basename.toLowerCase(); }),
	 rev.manifest({
	 	base: './Static/staticminv2/',
	 	merge: true // merge with the existing manifest if one exists
	 }),
	 plugins.header('\ufeff'),
	 gulp.dest('./Static/staticminv2/')
	],
   cb
 );
});

gulp.task('htmlVersioning', function (cb) {
	pump([
	 gulp.src(['./build/Static/**/*.html'], { base: './build/Static/' }),
	 replaceStyle(/>[\s]*\<(?!(\/pre))/gi, '><'),
	 rev(),
	 gulp.dest('./Static/staticminv2/'),
	 plugins.rename(function (path) { path.dirname = path.dirname.toLowerCase(); path.basename = path.basename.toLowerCase(); }),
	 rev.manifest({
	 	base: './Static/staticminv2/',
	 	merge: true // merge with the existing manifest if one exists
	 }),
	 plugins.header('\ufeff'),
	 gulp.dest('./Static/staticminv2/')
	],cb)

});

gulp.task('copyFonts', function (cb) {
	pump([
	 gulp.src(['./Static/fonts/*.*', './Static/m/fonts/*.*'], { base: './Static' }),
	 gulp.dest('./Static/staticminv2/')
	],
   cb
 );
});

gulp.task('replaceVersionedLinks', function (cb) {
	versionMapping = require('./rev-manifest.json');
	var key, keys = Object.keys(versionMapping);
	var n = keys.length;
	var versionMappingLower = {}
	while (n--) {
		key = keys[n];
		versionMappingLower[key.toLowerCase()] = versionMapping[key];
	}
	pump([
	 gulp.src(['./Views/**/*.cshtml', './**/*.{aspx,ascx}', '!./obj/**/*.*', '!./build/**/*.{aspx,ascx}'], { base: '.' }),
	 replaceStyle(/<link(?:[^>]*)href=(?:"|')~{0,1}\/static\/([^,"']*)(?:"|')(?:[^>]*)inline>/ig, function (s, filename) {

         var filepath = versionMappingLower[filename.toLowerCase()];
         checkVersionedFilenameValidity(filepath,filename)
	 	var style = fs.readFileSync("./Static/staticminv2/" + filepath, 'utf8');
	 	var styleToInject = new CleanCSS({}).minify(style).styles;
	 	return '<style>\n' + styleToInject.replace(/@{1}/g, "@@") + '\n</style>';
	 }),
	 replaceStyle(/<link([^>]*)href=(?:"|')~{0,1}\/static\/([^,"']*)(?:"|')([^>]*)>/ig, function (s, before, filename, after) {
         var versionedFileName = versionMappingLower[filename.toLowerCase()]
         checkVersionedFilenameValidity(versionedFileName, filename)
	     var filepath = getFilePath(versionedFileName);
		 
	     if (before.indexOf("preload") >= 0 || after.indexOf("preload") >= 0) {
	         return '<link ' + before + ' href="' + filepath + '" ' + after + '>';
	     }
	 	return '<link rel="stylesheet" href="' + filepath + '" type="text\/css" >';
	 }),
     replaceStyle(/<script([^>]*)src=(?:"|')~{0,1}\/static\/([^,"']*)(?:"|')([^>]*)>/ig, function (s, before, filename, after) {
        var versionedFileName = versionMappingLower[filename.toLowerCase()]
         checkVersionedFilenameValidity(versionedFileName, filename)
         var filepath = getFilePath(versionedFileName);
	 	if (filename.indexOf("tiny_mce") >= 0) { filepath = "/static/" + filename; }
	 	return '<script ' + before + ' src="' + filepath + '" ' + after + '>';
	 }),
	 plugins.tabify(),
	 plugins.header('\ufeff'),
	 gulp.dest('./build/')
	],
   cb
 );
   
});

gulp.task('replaceCssVersionedLinksInHtml', function (cb) {
	versionMapping = require('./rev-manifest.json');
	var key, keys = Object.keys(versionMapping);
	var n = keys.length;
	var versionMappingLower = {}
	while (n--) {
		key = keys[n];
		versionMappingLower[key.toLowerCase()] = versionMapping[key];
	}
	pump([
	 gulp.src(['./Static/**/*.html'], { base: '.' }),
	 replaceStyle(/<link(?:[^>]*)href=(?:"|')~{0,1}\/static\/([^,"']*)(?:"|')(?:[^>]*)inline>/ig, function (s, filename) {
         var filepath = versionMappingLower[filename.toLowerCase()];
         checkVersionedFilenameValidity(filepath, filename)
	 	var style = fs.readFileSync("./Static/staticminv2/" + filepath, 'utf8');
	 	var styleToInject = new CleanCSS({}).minify(style).styles;
	 	return '<style>\n' + styleToInject.replace(/@{1}/g, "@@") + '\n</style>';
	 }),
	 replaceStyle(/<link([^>]*)href=(?:"|')~{0,1}\/static\/([^,"']*)(?:"|')([^>]*)>/ig, function (s, before, filename, after) {
         var versionedFileName = versionMappingLower[filename.toLowerCase()]
         checkVersionedFilenameValidity(versionedFileName, filename)
         var filepath = getFilePath(versionedFileName);
	     if (before.indexOf("preload") >= 0 || after.indexOf("preload") >= 0) {
	         return '<link ' + before + ' href="' + filepath + '" ' + after + '>';
	     }
	 	return '<link rel="stylesheet" href="' + filepath + '" type="text\/css" >';
	 }),
	 plugins.tabify(),
	 plugins.header('\ufeff'),
	 gulp.dest('./build/')
	],
   cb
 );
 //rev-manifest is updated once more after this use. So clearing from cache.
 delete require.cache[require.resolve('./rev-manifest.json')];
});

gulp.task('minifyCSHTML', function (cb) {
	pump([
	 gulp.src(['./build/Views/**/*.cshtml'], { base: './build' }),
	 replaceStyle(/<!--(?!\s*\/?ko)[\s\S]*?-->/gi, ''),
     replaceStyle(/>[\s]*\<(?!(\/pre))/gi, '><'),
	 replaceStyle(/@model\s.*?>(?=<)/gi, function (s) { return s + "\n"; }),
	 plugins.header('\ufeff'),
	 gulp.dest('./build/')
	],
   cb
 );
});

gulp.task("updateReferences", function (cb) {

	pump([
	 gulp.src(['./Views/**/*.cshtml', './**/*.{aspx,ascx}', '!./obj/**/*.*', '!./build/**/*.{aspx,ascx}'], { base: '.' }),
	replaceStyle(/<link(?:[^>]*)href=(?:"|')<?%?=?(?:[^>]*)GetStaticUrl\((?:"|')\/([^,"']*)(?:[^>]*)\)\s*%?>?(?:"|')(?:[^>]*)>/g, function (s, filename) {
		console.log(s + " : " + filename);
		return '<link rel="stylesheet" href="/static/' + filename.toLowerCase() + '" type="text\/css" >';
	}),
	replaceStyle(/<script([^>]*)src=(?:"|')<?%?=?(?:[^>]*)GetStaticUrl\((?:"|')\/([^,"']*)(?:[^>]*)\)\s*%?>?(?:"|')([^>]*)>/g, function (s, before, filename, after) {
		console.log(s + ': <script ' + before + ' src="/static/' + filename + '" ' + after + '>');
		return '<script ' + before + ' src="/static/' + filename.toLowerCase() + '" ' + after + '>';
	}),
	 plugins.header('\ufeff'),
	 gulp.dest('.')
	],
   cb
 );
});

gulp.task("copyCarwaleDLLsToReleaseFolder", function (cb) {
	pump([
	 gulp.src(['./bin/Carwale.Notifications.dll', './bin/Carwale.dll', './bin/Carwale.BL.dll', './bin/Carwale.Cache.dll', './bin/Carwale.DAL.dll', './bin/Carwale.DTOs.dll', './bin/Carwale.Entity.dll', './bin/Carwale.Interfaces.dll', './bin/Carwale.Lucene.dll', './bin/Carwale.Service.dll', './bin/Carwale.Utility.dll', './bin/AppWebApi.dll', './bin/Carwale.Notifications.dll'], { base: '.' }),
	 gulp.dest(release_folder_path)
	],
   cb
 );
});

gulp.task("copyGeneratedFilesToReleaseFolder", function (cb) {
	pump([
	 gulp.src(['./build/**/*.*'], { base: './build/' }),
	 plugins.header('\ufeff'),
	 gulp.dest(release_folder_path)
	],
   cb
 );
});

gulp.task("copyCarwaleToReleaseFolder", function (cb) {
	pump([
     gulp.src(['./**/*.*', '!./**/*.{config,suo,log,user,json,csproj,cs,gif,swf,htc,xml}', '!./{UrlMappings,SMSModule,mobile,lts,build,ErrorLogs,node_modules,obj,.vs,images,mon,Isapi,App_Data,aspnet_client,bin,sitemaps,Properties,loading,JSUnit}/**/*.*', '!./{Gulpfile,Gulpfile_old}.js', '!./Community/Photos/**/*.*', '!./Users/images/**/*.*', '!./m/{images,Win8ServiceApp,fonts}/**/*.*','!./.gulp-cache'], { base: '.',dot:true}),
     plugins.header('\ufeff'),
     gulp.dest(release_folder_path),
	],
   cb
 );
});

gulp.task("createReleaseFolder", function (cb) {
	exec(get_branch_cmd, function (error, stdout, stderr) {
		curr_branch = stdout.toString().split('\n')[0];
		release_folder_path = '../../Carwale_Releases/gulp_releases/' + curr_branch + '/';
		runSequence("copyCarwaleToReleaseFolder", "copyGeneratedFilesToReleaseFolder", "copyCarwaleDLLsToReleaseFolder");
	});
});

gulp.task('sass', function (cb) {
	pump([
		  gulp.src('./Static/sass/**/*.scss', { base: './Static/' }),
		  sass().on('error', sass.logError),
		  gulp.dest('./Static/')],
	  cb);
});

gulp.task('sass:watch', function () {
	gulp.watch('./Static/sass/**/*.scss', ['sass']);
});

gulp.task("copyEmiAppTemplateToCarwaleFromPWA", function (cb) {
	pump([
	 gulp.src(['./PWA/build/emiapp/index.html'], { base: './PWA/build/emiapp/' }),
	 plugins.header('\ufeff'),
	 plugins.rename('_EmiCalculator.cshtml'),
	 gulp.dest('./build/Views/Shared/m/')
	],
   cb
 );
});
gulp.task("copyLeadFormAppTemplateToCarwaleFromPWA", function (cb) {
    pump([
        gulp.src(['./PWA/build/lead-form/index.html'], { base: './PWA/build/lead-form/' }),
        plugins.header('\ufeff'),
        plugins.rename('LeadformBuild.cshtml'),
        gulp.dest('./build/Views/StaticPartials')
    ],
    cb
 );
});

gulp.task("copyFilterPluginTemplateToCarwaleFromPWA", function (cb) {
    pump([
        gulp.src(['./PWA/build/filterplugin/index.html'], { base: './PWA/build/filterplugin/' }),
        plugins.header('\ufeff'),
        plugins.rename('_FilterPlugin.cshtml'),
        gulp.dest('./build/Views/Shared/NewCars')
    ],
    cb);
});
gulp.task("copyLocationAppTemplateToCarwaleFromPWA", function (cb) {
    pump([
	 gulp.src(['./PWA/build/location/index.html'], { base: './PWA/build/location/' }),
	 plugins.header('\ufeff'),
	 plugins.rename('_Location.cshtml'),
	 gulp.dest('./build/Views/Shared/m/')
    ],
   cb
 );
});

gulp.task('build', function () {
	configuration = process.argv[4] || configuration;
	runSequence('clean', 'minifyJS', 'sass', 'minifyCSS', 'replaceCssVersionedLinksInHtml', 'htmlVersioning', 'replaceVersionedLinks', 'minifyCSHTML', 'pwatemplatecopy');
});



gulp.task('pwatemplatecopy', function () {
    runSequence('copyEmiAppTemplateToCarwaleFromPWA', 'copyLeadFormAppTemplateToCarwaleFromPWA', 'copyFilterPluginTemplateToCarwaleFromPWA', 'copyLocationAppTemplateToCarwaleFromPWA');
});

gulp.task('default', function () {
		runSequence('build');
});