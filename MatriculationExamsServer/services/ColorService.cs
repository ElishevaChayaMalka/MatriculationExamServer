namespace MatriculationExamsServer.services
{
    public class ColorService
    {
        public string GetColorName(float? red, float? green, float? blue)
        {
            float r = red ?? 0;
            float g = green ?? 0;
            float b = blue ?? 0;

            if (IsWhite(r, g, b)) return "white";
            if (IsGray(r, g, b)) return "gray";
            if (IsRed(r, g, b)) return "red";
            if (IsGreen(r, g, b)) return "green";
            if (IsOrange(r, g, b)) return "orange";

            return "unknown";
        }

        private bool IsWhite(float r, float g, float b) => r > 0.9 && g > 0.9 && b > 0.9;
        private bool IsGray(float r, float g, float b) => Math.Abs(r - g) < 0.1 && Math.Abs(r - b) < 0.1 && r < 0.9;
        private bool IsRed(float r, float g, float b) => r > 0.8 && g < 0.4 && b < 0.4;
        private bool IsGreen(float r, float g, float b) => g > 0.8 && r < 0.4 && b < 0.4;
        private bool IsOrange(float r, float g, float b) => r > 0.8 && g > 0.5 && b < 0.3;
    }

}
