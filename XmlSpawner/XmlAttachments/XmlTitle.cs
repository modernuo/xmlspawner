using System;

namespace Server.Engines.XmlSpawner2;

public class XmlTitle : XmlAttachment
{
    private string m_Title; // title string

    // a serial constructor is REQUIRED
    public XmlTitle(ASerial serial) : base(serial)
    {
    }

    [Attachable]
    public XmlTitle(string name)
    {
        Name = name;
        Title = String.Empty;
    }

    [Attachable]
    public XmlTitle(string name, string title)
    {
        Name = name;
        Title = title;
    }

    [Attachable]
    public XmlTitle(string name, string title, double expiresin)
    {
        Name = name;
        Title = title;
        Expiration = TimeSpan.FromMinutes(expiresin);
    }

    [CommandProperty(AccessLevel.GameMaster)]
    public string Title
    {
        get => m_Title;
        set
        {
            m_Title = value;
            // change the title
            if (AttachedTo is Mobile mobile)
            {
                ((Mobile)AttachedTo).InvalidateProperties();
            }
            if (AttachedTo is Item item)
            {
                ((Item)AttachedTo).InvalidateProperties();
            }
        }
    }
    public static void AddTitles(object o, IPropertyList list)
    {
        var alist = XmlAttach.FindAttachments(o, typeof(XmlTitle));

        if (alist != null && alist.Count > 0)
        {
            string titlestring = null;
            bool hastitle = false;
            foreach (XmlTitle t in alist)
            {
                if (t == null || t.Deleted)
                {
                    continue;
                }

                if (hastitle)
                {
                    titlestring += '\n';
                }
                titlestring += Utility.FixHtml(t.Title);
                hastitle = true;
            }
            if (hastitle)
            {
                list.Add(1070722, $"<BASEFONT COLOR=#E6CC80>{titlestring}<BASEFONT COLOR=#FFFFFF>");
            }
        }
    }

    // These are the various ways in which the message attachment can be constructed.
    // These can be called via the [addatt interface, via scripts, via the spawner ATTACH keyword.
    // Other overloads could be defined to handle other types of arguments
    public override void Serialize(IGenericWriter writer)
    {
        base.Serialize(writer);

        writer.Write(0);
        // version 0
        writer.Write(m_Title);
    }

    public override void Deserialize(IGenericReader reader)
    {
        base.Deserialize(reader);

        int version = reader.ReadInt();
        // version 0
        m_Title = reader.ReadString();
    }

    public override void OnDelete()
    {
        base.OnDelete();

        // remove the title when deleted
        if (AttachedTo is Mobile mobile)
        {
            ((Mobile)AttachedTo).InvalidateProperties();
        }
        if (AttachedTo is Item item)
        {
            ((Item)AttachedTo).InvalidateProperties();
        }
    }

    public override void OnAttach()
    {
        base.OnAttach();

        // apply the title immediately when attached
        if (AttachedTo is Mobile mobile)
        {
            ((Mobile)AttachedTo).InvalidateProperties();
        }
        if (AttachedTo is Item item)
        {
            ((Item)AttachedTo).InvalidateProperties();
        }
    }

    public override string OnIdentify(Mobile from)
    {
        if (from == null || from.AccessLevel < AccessLevel.Counselor)
        {
            return null;
        }

        if (Expiration > TimeSpan.Zero)
        {
            return String.Format("{2}: Title {0} expires in {1} mins", Title, Expiration.TotalMinutes, Name);
        }

        return String.Format("{1}: Title {0}", Title, Name);
    }
}
