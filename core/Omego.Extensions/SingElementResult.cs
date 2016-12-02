namespace Omego.Extensions
{
    public struct SingElementResult<T>
    {
        public SingElementResult(Matches matches, T value)
        {
            Matches = matches;
            Value = value;
        }

        public SingElementResult(Matches matches)
        {
            Matches = matches;
            Value = default(T);
        }

        public Matches Matches { get; private set; }

        public T Value { get; private set; }
    }
}