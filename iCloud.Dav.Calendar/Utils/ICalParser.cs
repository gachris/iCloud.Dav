using antlr;
using Ical.Net;
using Ical.Net.ExtensionMethods;
using Ical.Net.Interfaces;
using Ical.Net.Interfaces.Components;
using Ical.Net.Interfaces.General;
using Ical.Net.Interfaces.Serialization;
using Ical.Net.Interfaces.Serialization.Factory;
using Ical.Net.Serialization.iCalendar;
using Ical.Net.Utility;

namespace iCloud.Dav.Calendar.Utils
{
    public class ICalParser : iCalParser
    {
        protected ICalParser(TokenBuffer tokenBuf, int k) : base(tokenBuf, k)
        {
            this.initialize();
        }

        public ICalParser(TokenBuffer tokenBuf) : this(tokenBuf, 3)
        {
        }

        protected ICalParser(TokenStream lexer, int k) : base(lexer, k)
        {
            this.initialize();
        }

        public ICalParser(TokenStream lexer) : this(lexer, 3)
        {
        }

        public ICalParser(ParserSharedInputState state) : base(state)
        {
            this.initialize();
        }

        public IICalendarCollection Icalendar(ISerializationContext ctx)
        {
            IICalendarCollection icalendarCollection = new CalendarCollection();
            SerializationUtil.OnDeserializing(icalendarCollection);
            ICalendar cal = null;
            ctx.GetService<ISerializationSettings>();
            while (this.LA(1) == 4 || this.LA(1) == 5)
            {
                while (this.LA(1) == 4)
                    this.match(4);
                this.match(5);
                this.match(6);
                this.match(7);
                while (this.LA(1) == 4)
                    this.match(4);
                ISerializationProcessor<ICalendar> service = ctx.GetService(typeof(ISerializationProcessor<ICalendar>)) as ISerializationProcessor<ICalendar>;
                service?.PreDeserialization(cal);
                cal = new Ical.Net.Calendar();
                SerializationUtil.OnDeserializing(cal);
                ctx.Push(cal);
                this.Icalbody(ctx, cal);
                this.match(8);
                this.match(6);
                this.match(7);
                while (this.LA(1) == 4 && (this.LA(2) == 1 || this.LA(2) == 4 || this.LA(2) == 5) && ICalParser.tokenSet_0_.member(this.LA(3)))
                    this.match(4);
                service?.PostDeserialization(cal);
                cal.OnLoaded();
                icalendarCollection.Add(cal);
                SerializationUtil.OnDeserialized(cal);
                ctx.Pop();
            }
            SerializationUtil.OnDeserialized(icalendarCollection);
            return icalendarCollection;
        }

        public void Icalbody(ISerializationContext ctx, ICalendar cal)
        {
            ISerializerFactory service1 = ctx.GetService(typeof(ISerializerFactory)) as ISerializerFactory;
            ICalendarComponentFactory service2 = ctx.GetService(typeof(ICalendarComponentFactory)) as ICalendarComponentFactory;
            while (true)
            {
                int num = this.LA(1);
                if (num != 5)
                {
                    if ((uint)(num - 9) <= 1U)
                        this.property(ctx, cal);
                    else
                        break;
                }
                else
                    this.Component(ctx, service1, service2, cal);
            }
        }

        public ICalendarComponent Component(ISerializationContext ctx, ISerializerFactory sf, ICalendarComponentFactory cf, ICalendarObject o)
        {
            IToken token1 = null;
            this.match(5);
            this.match(6);
            ICalendarComponent child;
            switch (this.LA(1))
            {
                case 9:
                    token1 = this.LT(1);
                    this.match(9);
                    child = cf.Build(token1.getText());
                    break;
                case 10:
                    IToken token2 = this.LT(1);
                    this.match(10);
                    child = cf.Build(token2.getText());
                    break;
                default:
                    throw new NoViableAltException(this.LT(1), this.getFilename());
            }
            ISerializationProcessor<ICalendarComponent> service = ctx.GetService(typeof(ISerializationProcessor<ICalendarComponent>)) as ISerializationProcessor<ICalendarComponent>;
            service?.PreDeserialization(child);
            SerializationUtil.OnDeserializing(child);
            ctx.Push(child);
            if (o != null)
                o.AddChild(child);
            child.Line = token1.getLine();
            child.Column = token1.getColumn();
            while (this.LA(1) == 4)
                this.match(4);
            while (true)
            {
                int num = this.LA(1);
                if (num != 5)
                {
                    if ((uint)(num - 9) <= 1U)
                        this.property(ctx, child);
                    else
                        break;
                }
                else
                    this.Component(ctx, sf, cf, child);
            }
            this.match(8);
            this.match(6);
            this.match(9);
            while (this.LA(1) == 4)
                this.match(4);
            service?.PostDeserialization(child);
            child.OnLoaded();
            SerializationUtil.OnDeserialized(child);
            ctx.Pop();
            return child;
        }
    }
}
