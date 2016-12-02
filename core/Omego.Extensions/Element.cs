namespace Omego.Extensions
{
    using System;

    public struct Element<T>
    {
        private readonly T value;

        public Element(T value)
        {
            this.value = value;
            Present = true;
        }

        public bool Present { get; }

        public T Value
        {
            get
            {
                if (!Present)
                {
                    throw new InvalidOperationException("Element does not exist");
                }

                return value;
            }
        }
    }
}