namespace Bonbibi
{
    public static class GameState
    {
        // ─────────────────────────────────────────
        // Moral Lean
        // ─────────────────────────────────────────

        public static int ConscienceScore { get; private set; } = 0;

        public static void AddConscience(int value)
        {
            ConscienceScore += value;
        }

        public static MoralLean GetLean()
        {
            if (ConscienceScore >= 3) return MoralLean.HighConscience;
            if (ConscienceScore == 2) return MoralLean.Neutral;
            return MoralLean.HighCompliance;
        }

        // ─────────────────────────────────────────
        // Story Flags
        // ─────────────────────────────────────────

        public static bool PromisedMother { get; private set; } = false;
        public static int Scene5ChoiceIndex { get; private set; } = -1;

        public static void SetPromisedMother(bool value)
        {
            PromisedMother = value;
        }

        public static void SetScene5Choice(int index)
        {
            Scene5ChoiceIndex = index;
        }

        // ─────────────────────────────────────────
        // Reset (call on application start)
        // ─────────────────────────────────────────

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