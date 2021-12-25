using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ScaffoldMod
{
	using PartType = class_139;
	using Permissions = enum_149;
	using PartTypes = class_191;
	using Texture = class_256;

	public class ScaffoldMod : QuintessentialMod {

		private static IDetour hook_Sim_method_1828;

		public static string[] ScaffoldNames = new string[] {
			"repeat","salt",
			"air","water","fire","earth",
			"vitae","mors",
			"quintessence","quicksilver",
			"lead","tin","iron","copper","silver","gold"
		};
		public static Dictionary<PartType, string> ScaffoldParts = new Dictionary<PartType, string>();

		public PartType scaffoldPart(string name) {
			string Description = "The glyph of scaffolding creates a single atom that can be used for initialization, wanding, or other purposes. ";
			if (name == "repeat") {Description += "This variant spawns an atom sheathed in [REDACTED].";}
			else {Description += "This variant spawns an elemental " + name + " atom.";};

			int cost;
			switch (name)
			{
				case "salt"			: cost = 5;		break;
				case "air"			: cost = 10;	break;
				case "water"		: cost = 10;	break;
				case "fire"			: cost = 10;	break;
				case "earth"		: cost = 10;	break;
				case "vitae"		: cost = 10;	break;
				case "mors"			: cost = 10;	break;
				case "quintessence"	: cost = 40;	break;
				case "quicksilver"	: cost = 5;		break;
				case "lead"			: cost = 5;		break;
				case "tin"			: cost = 10;	break;
				case "iron"			: cost = 20;	break;
				case "copper"		: cost = 40;	break;
				case "silver"		: cost = 80;	break;
				case "gold"			: cost = 160;	break;
				default/*repeat*/	: cost = 1;		break;
			}
			string Name = name[0].ToString().ToUpper() + name.Substring(1) + " ";
			if (name == "repeat") { Name = ""; };

			PartType scaffold = new PartType()
			{
				/*ID*/field_1528 = "glyph-scaffolding-" + name,
				/*Name*/field_1529 = class_134.method_253("Glyph of " + Name + "Scaffolding", string.Empty),
				/*Desc*/field_1530 = class_134.method_253(Description, string.Empty),
				/*Cost*/field_1531 = cost,
				/*Is a Glyph?*/field_1539 = true,
				/*Hex Footprint*/field_1540 = new HexIndex[1] { new HexIndex(0, 0) },
				/*Icon*/field_1547 = class_235.method_615("icwass/textures/parts/icons/scaffolds/" + name),
				/*Hover Icon*/field_1548 = class_235.method_615("icwass/textures/parts/icons/scaffolds/" + name + "_hover"),
				/*Glow (Shadow)*/field_1549 = class_238.field_1989.field_97.field_382,
				/*Stroke (Outline)*/field_1550 = class_238.field_1989.field_97.field_383,
				/*Permissions*/field_1551 = Permissions.None,
			};
			return scaffold;
		}

		public Quintessential.PartRenderer scaffoldRenderer(Texture Base, Texture Lighting, Texture Details)
		{
			return (part, pos, editor, renderer) =>
			{
				Vector2 vector2 = (Base.field_2056.ToVector2() / 2).Rounded() + new Vector2(0.0f, 1f);
				renderer.method_521(Base, vector2);
				renderer./*marker_lighting*/method_528(Lighting, new HexIndex(0, 0), Vector2.Zero);
				renderer./*marker_details*/method_521(Details, vector2);
			};
		}
		public override void Load() { }

		public override void LoadPuzzleContent() {
			Texture Base = class_235.method_615("icwass/textures/parts/scaffolds/glyph_base");
			Texture Lighting = class_235.method_615("icwass/textures/parts/scaffolds/glyph_lighting");

			foreach (string name in ScaffoldNames)
			{
				Texture Details = class_235.method_615("icwass/textures/parts/scaffolds/" + name);
				PartType scaffold = scaffoldPart(name);
				ScaffoldParts.Add(scaffold, name);
				QApi.AddPartType(scaffold, scaffoldRenderer(Base, Lighting, Details));
				QApi.AddPartTypeToPanel(scaffold, PartTypes.field_1782);
			}
			hook_Sim_method_1828 = new Hook(
				typeof(Sim).GetMethod("method_1828", BindingFlags.Instance | BindingFlags.NonPublic),
				typeof(ScaffoldMod).GetMethod("OnSimMethod1828", BindingFlags.Static | BindingFlags.NonPublic)
			);
		}

		private delegate void orig_Sim_method_1828(Sim sim); //code that runs every cycle but before parts are processed
		private static void OnSimMethod1828(orig_Sim_method_1828 orig, Sim sim)
		{
			orig(sim);
			if (sim.method_1818() == 0)//run once at the start of simulation, before arms execute grabs
			{
				var dict = new DynamicData(sim).Get<Dictionary<Part, PartSimState>>("field_3821");
				var parts = new List<Part>();

				foreach (var kvp in dict)
				{
					Part part = kvp.Key;
					PartType partType = part.method_1159();

					if (ScaffoldParts.ContainsKey(partType))
					{
						string name = ScaffoldParts[partType];
						AtomType atomType;
						switch (name)
						{
							case "salt": atomType = class_175.field_1675; break;
							case "air": atomType = class_175.field_1676; break;
							case "water": atomType = class_175.field_1679; break;
							case "fire": atomType = class_175.field_1678; break;
							case "earth": atomType = class_175.field_1677; break;
							case "vitae": atomType = class_175.field_1687; break;
							case "mors": atomType = class_175.field_1688; break;
							case "quintessence": atomType = class_175.field_1690; break;
							case "quicksilver": atomType = class_175.field_1680; break;
							case "lead": atomType = class_175.field_1681; break;
							case "tin": atomType = class_175.field_1683; break;
							case "iron": atomType = class_175.field_1684; break;
							case "copper": atomType = class_175.field_1682; break;
							case "silver": atomType = class_175.field_1685; break;
							case "gold": atomType = class_175.field_1686; break;
							default/*repeat*/	: atomType = class_175.field_1689; break;
						}
						var allMolecules = new DynamicData(sim).Get<List<Molecule>>("field_3823");
						Molecule molecule = new Molecule();
						molecule.method_1105(new Atom(atomType), part.method_1184(new HexIndex(0, 0)));
						allMolecules.Add(molecule);
					}
				}
			}
		}

		public override void Unload()
		{
			hook_Sim_method_1828.Dispose();
		}

		public override void PostLoad() {
		
		}
	}
}
