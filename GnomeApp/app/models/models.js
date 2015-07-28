define(["require", "exports"], function (require, exports) {
    var GnomeSummary = (function () {
        function GnomeSummary(obj) {
            this.Population = obj.Population;
            this.Gnomes = obj.Gnomes.map(function (item) { return new Gnome(item); });
        }
        return GnomeSummary;
    })();
    exports.GnomeSummary = GnomeSummary;
    var Gnome = (function () {
        function Gnome(obj) {
            this.ID = obj.ID;
            this.Name = obj.Name;
            this.Title = obj.Title;
            this.Stats = new GnomeStats(obj.Stats);
            this.Location = new GnomeLocation(obj.Location);
            this.BodyParts = obj.BodyParts.map(function (item) { return new GnomeBodyPartStatus(item); });
            this.Skills = GnomeSkill.SortBySkill(obj.Skills.map(function (item) { return new GnomeSkill(item); }), true);
        }
        return Gnome;
    })();
    exports.Gnome = Gnome;
    var GnomeStats = (function () {
        function GnomeStats(obj) {
            this.Happiness = obj.Happiness;
            this.BloodLevel = obj.BloodLevel;
            this.Rest = obj.Rest;
            this.Hunger = obj.Hunger;
            this.Thirst = obj.Thirst;
        }
        return GnomeStats;
    })();
    exports.GnomeStats = GnomeStats;
    var GnomeLocation = (function () {
        function GnomeLocation(obj) {
            this.X = obj.X;
            this.Y = obj.Y;
            this.Z = obj.Z;
            this.Text = "(Layer " + this.Y + " @ " + this.X + ", " + this.Z + ")";
        }
        return GnomeLocation;
    })();
    exports.GnomeLocation = GnomeLocation;
    var GnomeBodyPartStatus = (function () {
        function GnomeBodyPartStatus(obj) {
            this.BodyPart = obj.BodyPart;
            this.Statuses = obj.Statuses;
            this.StatusText = this.Statuses.join(', ');
        }
        return GnomeBodyPartStatus;
    })();
    exports.GnomeBodyPartStatus = GnomeBodyPartStatus;
    var GnomeSkill = (function () {
        function GnomeSkill(obj) {
            this.Name = obj.Name;
            this.Skill = obj.Skill;
        }
        GnomeSkill.SortBySkill = function (arr, descending) {
            var sorted = arr.sort(this.CompareByskill);
            if (descending) {
                sorted.reverse();
            }
            return sorted;
        };
        GnomeSkill.CompareByskill = function (a, b) {
            if (a.Skill < b.Skill) {
                return -1;
            }
            if (a.Skill > b.Skill) {
                return 1;
            }
            return 0;
        };
        return GnomeSkill;
    })();
    exports.GnomeSkill = GnomeSkill;
});
//# sourceMappingURL=models.js.map