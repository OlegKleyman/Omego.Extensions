namespace Omego.Extensions
{
    using System;

    public struct SingleElementResult<T>
    {
        private readonly Element<T> value;

        public SingleElementResult(T value)
        {
            Elements = Elements.One;
            this.value = new Element<T>(value);
        }

        public SingleElementResult(Elements elements)
        {
            Elements = elements;
            value = default(Element<T>);
        }

        public Elements Elements { get; }

        public T Value
        {
            get
            {
                if (Elements == Elements.Multiple)
                {
                    throw new InvalidOperationException("Multiple elements found.");
                }

                return value.Value;
            }
        }

        public static SingleElementResult<T> MultipleElements => new SingleElementResult<T>(Elements.Multiple);

        public static SingleElementResult<T> NoElements => new SingleElementResult<T>(Elements.None);
    }
}