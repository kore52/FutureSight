using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureSight.lib
{
    [Flags]
    public enum Color
    {
        Colorless = 0x00,
        White = 0x01,
        Blue = 0x02,
        Black = 0x04,
        Red = 0x08,
        Green = 0x10,
    }

    public enum ManaSymbol
    {
        // basic mana
        White,
        Blue,
        Black,
        Red,
        Green,

        // Oath of Gatewatch
        Colorless,

        // color hybrid mana
        WhiteBlue,
        WhiteBlack,
        WhiteRed,
        WhiteGreen,
        BlueBlack,
        BlueRed,
        BlueGreen,
        BlackRed,
        BlackGreen,
        RedGreen,

        // color generic hybrid mana
        Generic2White,
        Generic2Black,
        Generic2Blue,
        Generic2Red,
        Generic2Green,

        Generic1WhiteBlue,
        Generic1WhiteBlack,
        Generic1WhiteRed,
        Generic1WhiteGreen,
        Generic1BlueBlack,
        Generic1BlueRed,
        Generic1BlueGreen,
        Generic1BlackRed,
        Generic1BlackGreen,
        Generic1RedGreen,

        // Phyrexian mana
        PhyrexianWhite,
        PhyrexianBlue,
        PhyrexianBlack,
        PhyrexianRed,
        PhyrexianGreen,

        // Snow-Covered land mana
        Snow,

        Generic1,
        Generic2,
        Generic3,
        Generic4,
        Generic5,
        Generic6,
        Generic7,
        Generic8,
        Generic9,
        Generic10,
        Generic11,
        Generic12,
        Generic13,
        Generic14,
        Generic15,
        GenericX,

        Null
    }

    public enum LocationType
    {
        Command,
        Hand,
        Exile,
        Battlefield,
        Graveyard,
        Library,
    }

    [Flags]
    public enum PermanentType
    {
        Unknown = 0x00,
        Artifact = 0x01,
        Creature = 0x02,
        Enchantment = 0x04,
        Land = 0x08,
        Planeswalker = 0x10,
    }

    [Flags]
    public enum CardType
    {
        Unknown = 0,
        Artifact = 0x01,
        Creature = 0x02,
        Enchantment = 0x04,
        Instant = 0x08,
        Land = 0x10,
        Planeswalker = 0x11,
        Sorcery = 0x12,
        Tribal = 0x14,

        // 以下は多分使わない
        Conspiracy = 0x18,
        Phenomenon = 0x20,
        Plane = 0x21,
        Scheme = 0x22,
        Vanguard = 0x24,
    }

    public enum MTGSubType
    {
        None,

        // sub type of artifact
        Contraption,
        Equipment,
        Fortification,

        // sub type of enchantment
        Aura,
        Curse,
        Shrine,

        // sub type of land
        Plains,
        Island,
        Swamp,
        Mountain,
        Forest,

        Desert,
        Gate,
        Lair,
        Locus,
        Mine,
        PowerPlant,
        Tower,
        Urzas,

        // sub type of planeswalker
        Ajani,
        Ashiok,
        Bolas,
        Chandra,
        Dack,
        Daretti,
        Domri,
        Elspeth,
        Garruk,
        Gideon,
        Jace,
        Karn,
        Kiora,
        Koth,
        Liliana,
        Nahiri,
        Narset,
        Nissa,
        Nixilis,
        Ral,
        Sarkhan,
        Sorin,
        Tamiyo,
        Tefei,
        Tezzeret,
        Tibalt,
        Ugin,
        Venser,
        Vraska,
        Xenagos,

        // sub type of instant and sorcery
        Arcane,
        Trap,

        // sub type of creature
        Advisor,
        Ally,
        Angel,
        Antelope,
        Ape,
        Archer,
        Archon,
        Artificer,
        Assassin,
        Assembly,
        Atog,
        Aurochs,
        Avatar,
        Badger,
        Barbarian,
        Basilisk,
        Bat,
        Bear,
        Beast,
        Beeble,
        Berserker,
        Bird,
        Blinkmoth,
        Boar,
        Bringer,
        Brushwagg,
        Camarid,
        Camel,
        Caribou,
        Carrier,
        Cat,
        Centaur,
        Cephalid,
        Chimera,
        Citizen,
        Cleric,
        Cockatrice,
        Construct,
        Coward,
        Crab,
        Crocodile,
        Cyclops,
        Dauthi,
        Demon,
        Deserter,
        Devil,
        Djinn,
        Dragon,
        Drake,
        Dreadnought,
        Drone,
        Druid,
        Dryad,
        Dwarf,
        Efreet,
        Elder,
        Eldrazi,
        Elemental,
        Elephant,
        Elf,
        Elk,
        Eye,
        Faerie,
        Ferret,
        Fish,
        Flagbearer,
        Fox,
        Frog,
        Fungus,
        Gargoyle,
        Germ,
        Giant,
        Gnome,
        Goat,
        Goblin,
        God,
        Golem,
        Gorgon,
        Graveborn,
        Gremlin,
        Griffin,
        Hag,
        Harpy,
        Hellion,
        Hippo,
        Hippogriff,
        Homarid,
        Homunculus,
        Horror,
        Horse,
        Hound,
        Human,
        Hydra,
        Hyena,
        Illusion,
        Imp,
        Incarnation,
        Insect,
        Jellyfish,
        Juggernaut,
        Kavu,
        Kirin,
        Kithkin,
        Knight,
        Kobold,
        Kor,
        Kraken,
        Lamia,
        Lammasu,
        Leech,
        Leviathan,
        Lhurgoyf,
        Licid,
        Lizard,
        Manticore,
        Masticore,
        Mercenary,
        Merfolk,
        Metathran,
        Minion,
        Minotaur,
        Monger,
        Mongoose,
        Monk,
        Moonfolk,
        Mutant,
        Myr,
        Mystic,
        Naga,
        Nautilus,
        Nephilim,
        Nightmare,
        Nightstalker,
        Ninja,
        Noggle,
        Nomad,
        Nymph,
        Octopus,
        Ogre,
        Ooze,
        Orb,
        Orc,
        Orgg,
        Ouphe,
        Ox,
        Oyster,
        Pegasus,
        Pentavite,
        Pest,
        Phelddagrif,
        Phoenix,
        Pincher,
        Pirate,
        Plant,
        Praetor,
        Prism,
        Processor,
        Rabbit,
        Rat,
        Rebel,
        Reflection,
        Rhino,
        Rigger,
        Rogue,
        Sable,
        Salamander,
        Samurai,
        Sand,
        Saproling,
        Satyr,
        Scarecrow,
        Scion,
        Scorpion,
        Scout,
        Serf,
        Serpent,
        Shade,
        Shaman,
        Shapeshifter,
        Sheep,
        Siren,
        Skeleton,
        Slith,
        Sliver,
        Slug,
        Snake,
        Soldier,
        Soltari,
        Spawn,
        Specter,
        Spellshaper,
        Sphinx,
        Spider,
        Spike,
        Spirit,
        Splinter,
        Sponge,
        Squid,
        Squirrel,
        Starfish,
        Surrakar,
        Survivor,
        Tetravite,
        Thalakos,
        Thopter,
        Thrull,
        Treefolk,
        Triskelavite,
        Troll,
        Turtle,
        Unicorn,
        Vampire,
        Vedalken,
        Viashino,
        Volver,
        Wall,
        Warrior,
        Weird,
        Werewolf,
        Whale,
        Wizard,
        Wolf,
        Wolverine,
        Wombat,
        Worm,
        Wraith,
        Wurm,
        Yeti,
        Zombie,
        Zubera,
    }

    public enum MTGSpecialType
    {
        // 特殊タイプなし
        None,

        // 基本（土地）
        Basic,

        // 伝説の
        Legendary,

        // 持続(アーチエネミーのみ)
        Ongoing,

        // 氷雪の
        Snow,

        // ワールド・エンチャント
        World,
    }

    public enum MTGCounterType
    {
        Plus1_Plus1,
        Plus1_Plus0,
        Plus1_Plus2,
        Plus0_Plus1,
        Plus0_Plus2,
        Plus2_Plus0,
        Plus2_Plus2,

        Minus1_Minus1,
        Minus1_Minus0,
        Minus0_Minus1,
        Minus0_Minus2,
        Minus2_Minus1,

        Age,
        Aim,
        Arrow,
        Arrowhead,
        Awakening,
        Blaze,
        Blood,
        Bounty,
        Bribery,
        Carrion,
        Charge,
        Chip,
        Corpse,
        Credit,
        Crystal,
        Cube,
        Currency,
        Death,
        Delay,
        Depletion,
        Despair,
        Devotion,
        Divinity,
        Doom,
        Dream,
        Echo,
        Elixir,
        Energy,
        Eon,
        Experience,
        Eyeball,
        Fade,
        Fate,
        Feather,
        Filibuster,
        Flame,
        Flood,
        Fungus,
        Funk,
        Fuse,
        Gem,
        Glyph,
        Gold,
        Growth,
        Hatchling,
        Healing,
        Hoofprint,
        Hourglass,
        Hunger,
        Husk,
        Ice,
        Infection,
        Intervention,
        Isolation,
        Javelin,
        Ki,
        Level,
        Lore,
        Loyalty,
        Luck,
        Magnet,
        Manifestation,
        Mannequin,
        Matrix,
        Mine,
        Mining,
        Mire,
        Muster,
        Net,
        Omen,
        Ore,
        Page,
        Pain,
        Paralyzation,
        Petal,
        Petrification,
        Phylactery,
        Pin,
        Plague,
        Poison,
        Polyp,
        Pressure,
        Pupa,
        Quest,
        Rust,
        Scream,
        Scroll,
        Shell,
        Shield,
        Shred,
        Sleep,
        Sleight,
        Slime,
        Soot,
        Spore,
        Storage,
        Strife,
        Study,
        Theft,
        Tide,
        Time,
        Tower,
        Training,
        Trap,
        Treasure,
        Velocity,
        Verse,
        Vitality,
        Wage,
        Winch,
        Wind,
        Wish,

        MaxValue
    }

    public enum MTGPlayerState
    {
        None,
        WinGame,
        LoseGame,
        Hexproof,
        CannotWinGame,
        CannotLoseGame,
    }

    public enum MTGPermanentState
    {
        None,
        Tapped,
        Attacking,
        Blocking,
        Blocked,
        Destroyed,
        Summoned,
    }

    public static class EnumTypeHelper
    {
        public static PermanentType GetPermanentType(this CardType ct)
        {
            PermanentType result = PermanentType.Unknown;
            switch (ct)
            {
                case CardType.Artifact:
                case CardType.Creature:
                case CardType.Enchantment:
                    result = (PermanentType)ct; break;
                case CardType.Land:
                    result = PermanentType.Land; break;
                case CardType.Planeswalker:
                    result = PermanentType.Planeswalker; break;
                default: break;
            }
            return result;
        }

        public static List<MTGSubType> GetSubTypeList(this string stringTypeList)
        {
            List<MTGSubType> result = new List<MTGSubType>();
            try
            {
                foreach (var type in stringTypeList.Split(' '))
                {
                    result.Add((MTGSubType)Enum.Parse(typeof(MTGSubType), type));
                }
            }
            catch (Exception e)
            {
                result.Add(MTGSubType.None);
#if DEBUG
                System.Diagnostics.Debug.Print(e.Message);
#endif
            }

            return result;
        }

        public static List<MTGSpecialType> GetSpecialTypeList(this string stringTypeList)
        {
            List<MTGSpecialType> result = new List<MTGSpecialType>();
            try
            {
                foreach (var type in stringTypeList.Split(' '))
                {
                    result.Add((MTGSpecialType)Enum.Parse(typeof(MTGSpecialType), type));
                }
            }
            catch (Exception e)
            {
                result.Add(MTGSpecialType.None);
#if DEBUG
                System.Diagnostics.Debug.Print(e.Message);
#endif
            }

            return result;
        }
    }

    [Serializable()]
    public class MTGSubTypeSet
    {
        private List<MTGSubType> subTypeList;

        public MTGSubTypeSet() { subTypeList = new List<MTGSubType>(); }
        public MTGSubTypeSet(string type) { subTypeList = type.GetSubTypeList(); }
        public MTGSubTypeSet(List<MTGSubType> list) { subTypeList = new List<MTGSubType>(list); }
        public void Add(MTGSubType type) { subTypeList.Add(type); }
        public void Remove(MTGSubType type) { subTypeList.Remove(type); }
        public void Clear() { subTypeList.Clear(); }
        public List<MTGSubType> Get { get { return subTypeList; } }
    }

    [Serializable()]
    public class MTGSpecialTypeSet
    {
        private List<MTGSpecialType> specialTypeList;

        public MTGSpecialTypeSet() { specialTypeList = new List<MTGSpecialType>(); }
        public MTGSpecialTypeSet(string type) { specialTypeList = type.GetSpecialTypeList(); }        
        public MTGSpecialTypeSet(List<MTGSpecialType> list) { specialTypeList = list; }
        public void Add(MTGSpecialType type) { specialTypeList.Add(type); }
        public void Remove(MTGSpecialType type) { specialTypeList.Remove(type); }
        public void Clear() { specialTypeList.Clear(); }
        public List<MTGSpecialType> Get { get { return specialTypeList; } }
    }
}
