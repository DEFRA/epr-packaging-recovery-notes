// @ts-nocheck 
var esm = require('esm');
var gulp = require('gulp');
var concat = require("gulp-concat");
var sass = require('gulp-sass')(require('sass'));
var rollup = require("rollup");
var cleanCSS = require('gulp-clean-css');
var util = require('gulp-util');

// rollup config
var rollupConfig = require("./rollup.config.js");

// set the NODE_ENV variable node environment variable or set to 'development'
var NODE_ENV = process.env.NODE_ENV || "development";

gulp.task("default", ["js", "sass", "assets"]);

gulp.task("js", function () {
    // run the rollup script to create a bundled and minified js script for the whole site
    return rollup.rollup(rollupConfig.default).then(bundle => {
        return bundle.write(rollupConfig.default.output);
    });
});

gulp.task('sass', function () {
    const isProduction = process.env.NODE_ENV !== "development";

    console.log(`environment is: ${NODE_ENV}`);
    console.log(isProduction);

    // index.scss pulls in all other sass files
    return gulp.src('ClientApp/sass/index.scss')
        .pipe(sass())
        // concat will combine all files declared in your "src"
        .pipe(concat('site.css'))
        // if it's production minify the css, otherwise do nothing
        .pipe(isProduction ? cleanCSS() : util.noop())
        // output to required directory
        .pipe(gulp.dest('wwwroot/css'));
});

// copy images/fonts, etc
gulp.task("assets", function () {
    return gulp.src('node_modules/govuk-frontend/govuk/assets/**/*')
        .pipe(gulp.dest(`wwwroot/assets`));
});

gulp.task("watch-sass", function () {
    return gulp.watch('ClientApp/sass/**/*.scss', ["sass"]);
});

gulp.task("watch-scripts", function () {
    console.warn(`\n*\n* Use \`rollup -c --watch\` for better error messages and faster incremental builds.\n*\n`);

    return rollup.watch(rollupConfig.default)
        .on("event",
            event => {
                switch (event.code) {
                    case "ERROR":
                    case "FATAL":
                        console.error(event);
                        break;
                    default:
                        var args = [event.code];
                        if (event.duration) {
                            args.push(event.duration + "ms");
                        }
                        console.log.apply(console, args);
                }
            });
});