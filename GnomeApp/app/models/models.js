define(["require", "exports", 'knockout'], function (require, exports, ko) {
    var GnomeSummary = (function () {
        function GnomeSummary(obj) {
            this.Population = ko.observable(obj.Population);
            this.Gnomes = ko.observableArray(obj.Gnomes.map(function (item) {
                return new Gnome(item);
            }));
        }
        return GnomeSummary;
    })();
    exports.GnomeSummary = GnomeSummary;
    var GnomeProfession = (function () {
        function GnomeProfession(obj) {
            this.Name = ko.observable(obj.Name);
            this.Skills = ko.observableArray(obj.Skills);
        }
        return GnomeProfession;
    })();
    var Gnome = (function () {
        function Gnome(obj) {
            var _this = this;
            // An array of skills that are applicable to the Gnome's current profession.
            this.ProfessionSkills = ko.pureComputed({
                owner: this,
                read: function () {
                    var professionSkills = _this.Skills().filter(function (value) {
                        return _this.Profession().Skills().indexOf(value.Name()) >= 0;
                    });
                    return GnomeSkill.SortBySkill(professionSkills, true);
                },
                deferEvaluation: true
            });
            // An array of skills that are not used by the Gnome's current profession.
            this.NonProfessionSkills = ko.pureComputed({
                owner: this,
                read: function () {
                    var nonprofessionSkills = _this.Skills().filter(function (value) {
                        return _this.Profession().Skills().indexOf(value.Name()) < 0;
                    });
                    return GnomeSkill.SortBySkill(nonprofessionSkills, true);
                },
                deferEvaluation: true
            });
            this.ID = ko.observable(obj.ID);
            this.Name = ko.observable(obj.Name);
            this.Stats = ko.observable(new GnomeStats(obj.Stats));
            this.Location = ko.observable(new Location(obj.Location));
            this.BodyParts = ko.observableArray(obj.BodyParts.map(function (item) {
                return new GnomeBodyPartStatus(item);
            }));
            this.Skills = ko.observableArray(GnomeSkill.SortBySkill(obj.Skills.map(function (item) {
                return new GnomeSkill(item);
            }), true));
            this.Profession = ko.observable(new GnomeProfession(obj.Profession));
        }
        return Gnome;
    })();
    exports.Gnome = Gnome;
    var GnomeStats = (function () {
        function GnomeStats(obj) {
            this.Happiness = ko.observable(obj.Happiness);
            this.BloodLevel = ko.observable(obj.BloodLevel);
            this.Rest = ko.observable(obj.Rest);
            this.Hunger = ko.observable(obj.Hunger);
            this.Thirst = ko.observable(obj.Thirst);
        }
        return GnomeStats;
    })();
    exports.GnomeStats = GnomeStats;
    var Location = (function () {
        function Location(obj) {
            var _this = this;
            this.Text = ko.pureComputed({
                owner: this,
                read: function () {
                    return "(Layer " + _this.Y() + " @ " + _this.X() + ", " + _this.Z() + ")";
                }
            });
            this.X = ko.observable(obj.X);
            this.Y = ko.observable(obj.Y);
            this.Z = ko.observable(obj.Z);
        }
        return Location;
    })();
    exports.Location = Location;
    var GnomeBodyPartStatus = (function () {
        function GnomeBodyPartStatus(obj) {
            var _this = this;
            this.StatusText = ko.pureComputed({
                owner: this,
                read: function () {
                    return _this.Statuses().join(", ");
                },
                deferEvaluation: true
            });
            this.BodyPart = ko.observable(obj.BodyPart);
            this.Statuses = ko.observableArray(obj.Statuses);
        }
        return GnomeBodyPartStatus;
    })();
    exports.GnomeBodyPartStatus = GnomeBodyPartStatus;
    var GnomeSkill = (function () {
        function GnomeSkill(obj) {
            this.Name = ko.observable(obj.Name);
            this.Skill = ko.observable(obj.Skill);
        }
        // Returns the input array, sorted in ascending/descending order.
        GnomeSkill.SortBySkill = function (arr, descending) {
            var sorted = arr.sort(this.CompareByskill);
            if (descending) {
                sorted.reverse();
            }
            return sorted;
        };
        // Used to determine the sequence of IGnomeSkill objects.
        GnomeSkill.CompareByskill = function (a, b) {
            if (a.Skill() < b.Skill()) {
                return -1;
            }
            if (a.Skill() > b.Skill()) {
                return 1;
            }
            return 0;
        };
        return GnomeSkill;
    })();
    exports.GnomeSkill = GnomeSkill;
});
//# sourceMappingURL=models.js.map