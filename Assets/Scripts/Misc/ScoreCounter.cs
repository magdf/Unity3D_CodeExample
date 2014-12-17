/// <summary>
/// Calculate score and determines a stars number for level passing.
/// </summary>
public static class ScoreCounter
{
    private const int _minOneStarScore = 1500;
    private const int _minTwoStarsScore = 2000;
    private const int _minThreeStarsScore = 2500;

    const float _baseStartBattleDuration = 180f;

    /// <summary>
    /// Calculate score depending from start battle duration.
    /// </summary>
    public static int CalculateScore(float startTime, float scoreAtCurrentBattle)
    {
        float k = startTime / _baseStartBattleDuration;

        float v = scoreAtCurrentBattle / k;
        return (int)(v);
    }

    public static int CalculateStarsNumber(int score)
    {
        if (score >= _minThreeStarsScore)
            return 3;
        if (score >= _minTwoStarsScore)
            return 2;
        if (score >= _minOneStarScore)
            return 1;
        return 0;
    }
}