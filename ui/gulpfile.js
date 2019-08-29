const { dest, src, series, parallel, watch } = require('gulp');
const browserSync = require('browser-sync');
const scss = require('gulp-sass');
const useref = require('gulp-useref');

function initBrowserSync() {
    browserSync.init({
        server: './dist'
    });
}

function watchForChanges() {
    watch('app/*.html', processHTMLfiles).on('change', function() {
        browserSync.reload();
    });
    watch('app/styles/**/*.scss', processSCSSFiles);
    watch('app/css/**/*.css', processHTMLfiles).on('change', function() {
        browserSync.reload();
    });
}

function processHTMLfiles() {
    return src('app/*.html')
        .pipe(useref())
        .pipe(dest('dist/'));
}

function processSCSSFiles() {
    return src('app/styles/**/*.scss')
        .pipe(scss().on('error', scss.logError))
        .pipe(dest('app/css'));
}

exports.default = series(
    processSCSSFiles,
    processHTMLfiles,
    parallel(watchForChanges, initBrowserSync)
);
