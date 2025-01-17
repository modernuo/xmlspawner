using Server.Items;

namespace Server.Mobiles;

public class TalkingSeekerOfAdventure : TalkingBaseEscortable
{
    private static string[] m_Dungeons = new string[]
    {
        "Covetous", "Deceit", "Despise",
        "Destard", "Hythloth", "Shame",
        "Wrong"
    };

    public override string[] GetPossibleDestinations() => m_Dungeons;

    [Constructible]
    public TalkingSeekerOfAdventure() => Title = "the seeker of adventure";

    public override bool ClickTitle => false; // Do not display 'the seeker of adventure' when single-clicking

    private static int GetRandomHue()
    {
        switch (Utility.Random(6))
        {
            default:
            case 0:
                {
                    return 0;
                }
            case 1:
                {
                    return Utility.RandomBlueHue();
                }
            case 2:
                {
                    return Utility.RandomGreenHue();
                }
            case 3:
                {
                    return Utility.RandomRedHue();
                }
            case 4:
                {
                    return Utility.RandomYellowHue();
                }
            case 5:
                {
                    return Utility.RandomNeutralHue();
                }
        }
    }

    public override void InitOutfit()
    {
        if (Female)
        {
            AddItem(new FancyDress(GetRandomHue()));
        }
        else
        {
            AddItem(new FancyShirt(GetRandomHue()));
        }

        int lowHue = GetRandomHue();

        AddItem(new ShortPants(lowHue));

        if (Female)
        {
            AddItem(new ThighBoots(lowHue));
        }
        else
        {
            AddItem(new Boots(lowHue));
        }

        if (!Female)
        {
            AddItem(new BodySash(lowHue));
        }

        AddItem(new Cloak(GetRandomHue()));

        AddItem(new Longsword());

        HairHue = Race.RandomHairHue();

        PackGold(100, 150);
    }

    public TalkingSeekerOfAdventure(Serial serial) : base(serial)
    {
    }

    public override void Serialize(IGenericWriter writer)
    {
        base.Serialize(writer);

        writer.Write(0); // version
    }

    public override void Deserialize(IGenericReader reader)
    {
        base.Deserialize(reader);

        int version = reader.ReadInt();
    }
}
