class Utils {
    public static isFunction(obj) {
        return !!(obj && obj.constructor && obj.call && obj.apply);
    }
}

export = Utils;