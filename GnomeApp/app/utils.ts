class Utils {
    public static isFunction(obj) {
        return !!(obj && obj.constructor && obj.call && obj.apply);
    }

    public static getRootUrl() {
        var currentURL = document.URL;
        var rootPosition = currentURL.indexOf("/", 7);
        var relativeHomeUrl = currentURL.substring(0, rootPosition + 1);
        return relativeHomeUrl;
    }
}

export = Utils;