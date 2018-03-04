﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Org.Interledger.Encoding.Asn.Codecs
{
    public class AsnTimestampCodec : AsnPrintableStringBasedObjectCodecBase<DateTime>
    {
        private string DateTimeFormatter { get { return "yyyyMMddHHmmssfff"; } }

        public AsnTimestampCodec() : base(new AsnSizeConstraint(17))
        {
            Regex regex = new Regex(this.GetRegex());
            this.Validator = (string charString) => regex.IsMatch(charString);
        }
            
        public override DateTime Decode()
        {
            DateTime dateTime;
            DateTime.TryParseExact(this.CharString, this.DateTimeFormatter, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out dateTime);
            if (dateTime != null)
            {
                return dateTime;
            }
            else
            {
                throw new FormatException(string.Format(
                    "Interledger timestamps only support values in the format 'YYYYMMDDHHMMSSfff', value {0} is invalid.", this.CharString
                ));
            }
        }

        public override void Encode(DateTime value)
        {
            this.CharString = value.ToString(this.DateTimeFormatter);
        }

        public override void Accept(IAsnObjectCodecVisitor visitor)
        {
            throw new NotImplementedException();
        }

        private string GetRegex()
        {
            return "[0-9]{17}";
        }
    }
}
