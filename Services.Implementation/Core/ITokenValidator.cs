using System;

namespace Services.Implementation.Core
{
    public interface ITokenValidator
    {
        bool Validate(string token);
        bool Validate(Guid guid);
    }

    public class Validator : ITokenValidator
    {
        private static readonly Validator instance = new Validator();

        private Validator()
        {
        }

        public static Validator Instance()
        {
            return instance;
        }


        #region ITokenValidator Members

        public bool Validate(string token)
        {
            throw new NotImplementedException();
        }

        public bool Validate(Guid guid)
        {
            if (guid == Guid.Empty)
                return true;
            return false;
        }

        #endregion
    }
}