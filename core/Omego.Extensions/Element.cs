namespace Omego.Extensions
{
    public struct Element<T>
    {
        public Element(T value)
        {
            Value = value;
            Present = true;
        }

        public bool Present { get; private set; }

        public T Value { get; private set; }
    }
}