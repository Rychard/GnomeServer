﻿
 
 




/// <reference path="Enums.ts" />

declare module GnomeServer.Models {
	interface IGnomeSummary {
		Population: number;
		Gnomes: GnomeServer.Models.IGnome[];
	}
	interface IGnome {
		ID: number;
		Name: string;
		Stats: GnomeServer.Models.IGnomeStats;
		Location: GnomeServer.Models.IGnomeLocation;
		BodyParts: GnomeServer.Models.IGnomeBodyPartStatus[];
		Skills: GnomeServer.Models.IGnomeSkill[];
		Profession: GnomeServer.Models.IGnomeProfession;
	}
	interface IGnomeStats {
		Happiness: number;
		BloodLevel: number;
		Rest: number;
		Hunger: number;
		Thirst: number;
	}
	interface IGnomeLocation {
		X: number;
		Y: number;
		Z: number;
	}
	interface IGnomeBodyPartStatus {
		BodyPart: string;
		Statuses: string[];
	}
	interface IGnomeSkill {
		Name: string;
		Skill: number;
	}
	interface IGnomeProfession {
		Name: string;
		Skills: string[];
	}
}
