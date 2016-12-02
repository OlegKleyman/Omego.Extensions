namespace Omego.Extensions
{
    using System;

    public struct SingleElementResult<T>
    {
        private readonly Element<T> value;

        public SingleElementResult(T value)
        {
            Matches = Matches.One;
            this.value = new Element<T>(value);
        }

        public SingleElementResult(Matches matches)
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