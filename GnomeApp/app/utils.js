define(["require", "exports"], function (require, exports) {
    var Utils = (function () {
        function Utils() {
        }
        Utils.isFunction = function (obj) {
            return !!(obj && obj.constructor && obj.call && obj.apply);
        };
        return Utils;
    })();
    return Utils;
});
//# sourceMappingURL=utils.js.map