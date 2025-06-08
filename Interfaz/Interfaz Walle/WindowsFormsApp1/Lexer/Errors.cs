using System;

namespace WindowsFormsApp1
{
    public class CompilingError : Exception
    {
        public ErrorCode Code { get; private set; }

        public string Argument { get; private set; }

        public CodeLocation Location { get; private set; }

        public CompilingError(CodeLocation location, ErrorCode code, string argument)
        {
            this.Code = code;
            this.Argument = argument;
            Location = location;
        }
        public override string ToString()
        {
            return Argument + " Error Type: " + Code + " in line " + Location.Line;
        }

    }
    public enum ErrorCode
    {
        None,
        Expected,
        Invalid,
        Unknown,
        UndefinedLabel
    }
}