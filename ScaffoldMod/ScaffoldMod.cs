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
	using BondType = enum_126;
	using BondSite = class_222;
	using AtomTypes = class_175;
	using PartTypes = class_191;

	public class ScaffoldMod : QuintessentialMod {

		private static IDetour hook_Sim_method_1832;

		public static PartType
			ScaffoldSalt,
			ScaffoldAir,
			ScaffoldWater,
			ScaffoldFire,
			ScaffoldEarth,
			ScaffoldVitae,
			ScaffoldMors,
			ScaffoldQuintessence,
			ScaffoldQuicksilver,
			ScaffoldLead,
			ScaffoldTin,
			ScaffoldIron,
			ScaffoldCopper,
			ScaffoldSilver,
			ScaffoldGold,
			ScaffoldRepeat
		;

		public static Dictionary<PartType, string> ScaffoldParts;

		//method for creating a scaffold part:
		public PartType scaffoldPart(string name) {
			//generate the description string
			string Description = "The glyph of scaffolding creates a single atom that can be used for initialization, wanding, or other purposes. ";
			if (name == "repeat") {
				Description += "This variant spawns an atom sheathed in [REDACTED].";
			}
			else {
				Description += "This variant spawns an elemental " + name + " atom.";
			};

			int cost;
			string Name = name[0].ToString().ToUpper() + name.Substring(1);

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

			PartType scaffold = new PartType();
			scaffold./*ID*/field_1528 = "glyph-of-scaffolding-" + name;
			scaffold./*Name*/field_1529 = class_134.method_253("Glyph of " + Name + " Scaffolding", string.Empty);
			scaffold./*Desc*/field_1530 = class_134.method_253(Description, string.Empty);
			scaffold./*Part Icon*/field_1547 = class_235.method_615("icwass/textures/parts/icons/scaffolds/" + name);
			scaffold./*Hovered Part Icon*/field_1548 = class_235.method_615("icwass/textures/parts/icons/scaffolds/" + name + "_hover");
			//scaffold./*Part Icon*/field_1547 = class_235.method_615("icwass/textures/parts/icons/scaffolds/repeat");
			//scaffold./*Hovered Part Icon*/field_1548 = class_235.method_615("icwass/textures/parts/icons/scaffolds/repeat_hover");
			scaffold./*Cost*/field_1531 = cost;
			scaffold./*Is a Glyph*/field_1539 = true;
			scaffold./*Glow (Shadow)*/field_1549 = class_238.field_1989.field_97.field_382;
			scaffold./*Stroke (Outline)*/field_1550 = class_238.field_1989.field_97.field_383;
			scaffold./*Spaces*/field_1540 = new HexIndex[1] { new HexIndex(0, 0) };
			scaffold./*Permissions*/field_1551 = Permissions.None;

			return scaffold;
		}
		public Quintessential.PartRenderer scaffoldRenderer(string name) {
			return (part, pos, editor, renderer) =>
			{
				class_256 glyphtexture = class_235.method_615("icwass/textures/parts/scaffolds/glyph_base");
				Vector2 vector2 = (glyphtexture.field_2056.ToVector2() / 2).Rounded() + new Vector2(0.0f, 1f);
				renderer.method_521(glyphtexture, vector2);
				renderer./*marker_lighting*/method_528(class_235.method_615("icwass/textures/parts/scaffolds/glyph_lighting"), new HexIndex(0, 0), Vector2.Zero);
				renderer./*marker_details*/method_521(class_235.method_615("icwass/textures/parts/scaffolds/" + name), vector2);
			};
		}
		public override void Load() { }

		public override void LoadPuzzleContent() {
			ScaffoldParts = new Dictionary<PartType, string>();
			ScaffoldRepeat			= scaffoldPart("repeat");		ScaffoldParts.Add(ScaffoldRepeat,		"repeat"		);
			ScaffoldSalt			= scaffoldPart("salt");			ScaffoldParts.Add(ScaffoldSalt,			"salt" 			);
			ScaffoldAir				= scaffoldPart("air");			ScaffoldParts.Add(ScaffoldAir,			"air" 			);
			ScaffoldWater			= scaffoldPart("water");		ScaffoldParts.Add(ScaffoldWater,		"water" 		);
			ScaffoldFire			= scaffoldPart("fire");			ScaffoldParts.Add(ScaffoldFire,			"fire" 			);
			ScaffoldEarth			= scaffoldPart("earth");		ScaffoldParts.Add(ScaffoldEarth,		"earth"			);
			ScaffoldVitae			= scaffoldPart("vitae");		ScaffoldParts.Add(ScaffoldVitae,		"vitae"			);
			ScaffoldMors			= scaffoldPart("mors");			ScaffoldParts.Add(ScaffoldMors,			"mors"			);
			ScaffoldQuintessence	= scaffoldPart("quintessence");	ScaffoldParts.Add(ScaffoldQuintessence,	"quintessence"	);
			ScaffoldQuicksilver		= scaffoldPart("quicksilver");	ScaffoldParts.Add(ScaffoldQuicksilver,	"quicksilver"	);
			ScaffoldLead			= scaffoldPart("lead");			ScaffoldParts.Add(ScaffoldLead,			"lead"			);
			ScaffoldTin				= scaffoldPart("tin");			ScaffoldParts.Add(ScaffoldTin,			"tin"			);
			ScaffoldIron			= scaffoldPart("iron");			ScaffoldParts.Add(ScaffoldIron,			"iron"			);
			ScaffoldCopper			= scaffoldPart("copper");		ScaffoldParts.Add(ScaffoldCopper,		"copper"		);
			ScaffoldSilver			= scaffoldPart("silver");		ScaffoldParts.Add(ScaffoldSilver,		"silver"		);
			ScaffoldGold			= scaffoldPart("gold");			ScaffoldParts.Add(ScaffoldGold,			"gold"			);

			foreach (var kvp in ScaffoldParts) {
				QApi.AddPartType(kvp.Key, scaffoldRenderer(kvp.Value));
				QApi.AddPartTypeToPanel(kvp.Key, PartTypes.field_1782);
			};

			hook_Sim_method_1832 = new Hook(
				typeof(Sim).GetMethod("method_1832", BindingFlags.Instance | BindingFlags.NonPublic),
				typeof(ScaffoldMod).GetMethod("OnSimMethod1832", BindingFlags.Static | BindingFlags.NonPublic)
			);
		}

		// quick and dirty, good enough

		private delegate void orig_Sim_method_1832(Sim self, bool param_5369); //per-cycle code
		private static void OnSimMethod1832(orig_Sim_method_1832 orig, Sim self, bool param_5369) {
			orig(self, param_5369);

			if (self.method_1818() == 0 && param_5369) {
				///*play sound effect*/class_238.field_1991./*ui_unlock*/field_1878.method_28(1f);

				var dict = new DynamicData(self).Get<Dictionary<Part, PartSimState>>("field_3821");
				var parts = new List<Part>();

				foreach (var kvp in dict) parts.Add(kvp.Key);
				foreach (Part part in parts)
				{
					Sim.class_402 class402 = new Sim.class_402();
					class402.field_3841 = part;

					PartType partType = class402.field_3841.method_1159();

					if (ScaffoldParts.ContainsKey(partType)) {
						string name = ScaffoldParts[partType];
						AtomType atomType;
						switch (name)
						{
							//case "repeat": atomType = class_175.field_1689; break;
							case "salt"			: atomType = class_175.field_1675; break;
							case "air"			: atomType = class_175.field_1676; break;
							case "water"		: atomType = class_175.field_1679; break;
							case "fire"			: atomType = class_175.field_1678; break;
							case "earth"		: atomType = class_175.field_1677; break;
							case "vitae"		: atomType = class_175.field_1687; break;
							case "mors"			: atomType = class_175.field_1688; break;
							case "quintessence"	: atomType = class_175.field_1690; break;
							case "quicksilver"	: atomType = class_175.field_1680; break;
							case "lead"			: atomType = class_175.field_1681; break;
							case "tin"			: atomType = class_175.field_1683; break;
							case "iron"			: atomType = class_175.field_1684; break;
							case "copper"		: atomType = class_175.field_1682; break;
							case "silver"		: atomType = class_175.field_1685; break;
							case "gold"			: atomType = class_175.field_1686; break;
							default/*repeat*/	: atomType = class_175.field_1689; break;
						}
						Vector2 vector2 = class_187.field_1742.method_491(class402.field_3841.method_1184(new HexIndex(0, 0)), Vector2.Zero);
						var allMolecules = new DynamicData(self).Get<List<Molecule>>("field_3823");

						Molecule molecule = new Molecule();
						molecule.method_1105(new Atom(atomType), class402.field_3841.method_1184(new HexIndex(0, 0)));
						allMolecules.Add(molecule);
					}
				}
			}
		}

		public override void Unload() {
		    hook_Sim_method_1832.Dispose();
		}

		public override void PostLoad() {
		
		}
	}
}
