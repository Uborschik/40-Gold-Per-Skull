namespace Game.Core.Utils
{
    public readonly struct Result<TValue, TError>
    {
        public readonly bool IsSuccess;
        public readonly TValue Value;
        public readonly TError Error;

        private Result(TValue value, TError error, bool isSuccess)
        {
            Value = value;
            Error = error;
            IsSuccess = isSuccess;
        }

        public static Result<TValue, TError> Success(TValue value) =>
            new(value, default, true);

        public static Result<TValue, TError> Failure(TError error) =>
            new(default, error, false);

        public void Deconstruct(out bool success, out TValue value, out TError error)
        {
            success = IsSuccess;
            value = Value;
            error = Error;
        }
    }

    public enum PlacementError
    {
        OutOfBounds,
        CellBlocked,
        CellOccupied,
        InvalidData
    }
}