namespace Omego.Extensions
{
    using System;

    public struct SingElementResult<T>
    {
        private readonly Element<T> value;

        public SingElementResult(T value)
        {
            Matches = Matches.One;
            this.value = new Element<T>(value);
        }

        public SingElementResult(Matches matches)
        {
            Matches = matches;
            value = default(Element<T>);
        }

        public Matches Matches { get; }

        public T Value
        {
            get
            {
                if (Matches == Matches.Multiple)
                {
                    throw new InvalidOperationException("Multiple elements found.");
                }

                return value.Value;
            }
        }
    }
}