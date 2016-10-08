using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Service.Contracts.Data.Helper
{
    /// <summary>
    /// Nullable Date Wrapper
    /// </summary>
    internal class NullableDateFieldHelper
    {
        internal NullableDateFieldHelper()
        {
            this.date = null;
            this.dateText = null;
            this.IsValid = true;
        }

        private DateTime? date;
        private string dateText;

        internal DateTime? Date
        {
            get
            {
                if (!this.IsValid)
                {
                    throw new InvalidOperationException("Attempt to access Invalid NullableDate Value");
                }
                return this.date;
            }
            set
            {
                this.date = value;
                this.dateText = this.date.HasValue ? this.date.Value.ToString(ServiceConstants.DateFormat, CultureInfo.InvariantCulture) : null;
                this.IsValid = true;
            }
        }


        internal string DateText
        {
            get
            {
                return this.dateText;
            }
            set
            {
                this.dateText = value;
                if (this.dateText == null)
                {
                    this.date = null;
                    this.IsValid = true;
                }
                else
                {
                    DateTime parsedDate;
                    if (DateTime.TryParseExact(this.dateText, ServiceConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                    {
                        this.date = parsedDate;
                        this.IsValid = true;
                    }
                    else
                    {
                        this.date = null;
                        this.IsValid = false;
                    }
                }
            }
        }

        internal bool IsValid
        {
            get;
            private set;
        }
    }

}
