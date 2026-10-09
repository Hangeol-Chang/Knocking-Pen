// Pen ids and unlock rewards. Pen id == scene "Pen{id}" == ObjID.PenNumber.
public static class PenCatalog
{
    // Save slot count. Ids above SceneCount are reward-only placeholders (29 = "wait for update").
    public const int SlotCount = 30;
    // Pens that have a scene: Pen0 .. Pen10.
    public const int SceneCount = 11;

    public enum Counter { Knock, Miss }

    public readonly struct Reward
    {
        public readonly int pen;
        public readonly Counter counter;
        public readonly int count;
        public readonly int unlock;
        public readonly string message;

        public Reward(int pen, Counter counter, int count, int unlock, string message)
        {
            this.pen = pen;
            this.counter = counter;
            this.count = count;
            this.unlock = unlock;
            this.message = message;
        }
    }

    // Pen 2 is unlocked by the tutorial (GuideBehaviour).
    public static readonly Reward[] Rewards =
    {
        new Reward(0, Counter.Knock, 10, 1, "제*스트림을 획득하였습니다."),
        new Reward(1, Counter.Knock, 10, 3, "2색펜을 획득하였습니다."),
        new Reward(1, Counter.Miss, 30, 10, "화이트펜을 획득하였습니다."),
        new Reward(3, Counter.Knock, 10, 4, "수능샤프를 획득하였습니다."),
        new Reward(3, Counter.Miss, 20, 6, "개발자가 고딩때 쓰던 볼펜을 획득하였습니다."),
        new Reward(4, Counter.Knock, 30, 5, "토끼펜을 획득했습니다."),
        new Reward(4, Counter.Knock, 77, 7, "그래프1000을 획득했습니다."),
        new Reward(5, Counter.Knock, 100, 8, "동물펜2를 획득하였습니다."),
        new Reward(5, Counter.Miss, 200, 9, "동물펜3을 획득하였습니다."),
        new Reward(8, Counter.Miss, 200, 9, "동물펜3을 획득하였습니다."),
        new Reward(6, Counter.Miss, 200, 29, "업데이트를 기대해주세요ㅎㅎ."),
    };
}
