namespace Omego.Extensions
{
    public struct FirstElementResults<T>
    {
        public FirstElementResults(T value)
        {
            Value = value;
            HasValue = true;
        }

        public bool HasValue { get; private set; }

        public T Value { get; private set; }
    }
}