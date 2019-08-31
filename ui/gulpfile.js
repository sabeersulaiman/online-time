const { dest, src, series, parallel, watch } = require('gulp');
const browserSync = require('browser-sync');
const scss = require('gulp-sass');
const useref = require('gulp-useref');
const concat = require('gulp-concat');

function initBrowserSync() {
    browserSync.init({
        server: './dist'
    });
}

function processHTMLfiles() {
    return src('app/*.html')
        .pipe(useref())
        .pipe(dest('dist'));
}

function processSCSSFiles() {
    return src('app/styles/**/*.scss')
        .pipe(scss().on('error', scss.logError))
        .pipe(dest('app/css'));
}

function processJsFiles() {
    return src('app/js/**/*.js')
        .pipe(concat('main.js'))
        .pipe(dest('app/packages'));
}

function watchForChanges() {
    watch('app/*.html', processHTMLfiles).on('change', function() {
        browserSync.reload();
    });
    watch('app/styles/**/*.scss', processSCSSFiles);
    watch(['app/css/**/*.css', 'app/packages/**/*.js'], processHTMLfiles).on(
        'change',
        function() {
            browserSync.reload();
        }
    );
    watch('app/js/**/*.js', processJsFiles);
}

exports.default = series(
    processJsFiles,
    processSCSSFiles,
    processHTMLfiles,
    parallel(watchForChanges, initBrowserSync)
);
