namespace Bonbibi
{
    public static class GameState
    {
        public static int ConscienceScore { get; private set; } = 0;

        public static void AddConscience(int value) => ConscienceScore += value;

        public static MoralLean GetLean()
        {
            switch (ConscienceScore)
            {
                case >= 3:
                    return MoralLean.HighConscience;
                case 2:
                    return MoralLean.Neutral;
                default:
                    return MoralLean.HighCompliance;
            }
        }

        public static bool PromisedMother { get; private set; } = false;
        public static int Scene5ChoiceIndex { get; private set; } = -1;

        public static void SetPromisedMother(bool value) => PromisedMother = value;
        public static void SetScene5Choice(int index) => Scene5ChoiceIndex = index;

        public static void Reset()
        {
            ConscienceScore = 0;
            PromisedMother = false;
            Scene5ChoiceIndex = -1;
        }
    }

    public enum MoralLean
    {
        HighConscience,
        Neutral,
        HighCompliance
    }
}