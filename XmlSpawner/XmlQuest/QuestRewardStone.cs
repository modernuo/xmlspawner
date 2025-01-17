using Server.Gumps;

/*
** QuestRewardStone
** used to open the QuestPointsRewardGump that allows players to purchase rewards with their XmlQuestPoints Credits.
*/

namespace Server.Items;

public class QuestRewardStone : Item
{
    [Constructible]
    public QuestRewardStone() : base(0xED4)
    {
        Movable = false;
        Name = "a Quest Points Reward Stone";
    }

    public QuestRewardStone(Serial serial) : base(serial)
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

    public override void OnDoubleClick(Mobile from)
    {
        if (from.InRange(GetWorldLocation(), 2))
        {
            from.SendGump(new QuestRewardGump(from, 0));
        }
        else
        {
            from.SendLocalizedMessage(500446); // That is too far away.
        }
    }
}
