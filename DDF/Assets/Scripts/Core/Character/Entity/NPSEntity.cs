namespace DDF.Character {
    public class NPSEntity : HumanoidEntity {
        public int strength = 5;
        public int agility = 5;
        public float Speed = 2;
        private void Start() {
            InitStartStats();
            for (int i = 0; i < strength; i++)
                IncreaseStrength();
            for (int i = 0; i < agility; i++)
                IncreaseAgility();

            CurrentSpeed = Speed;
            UpdateStats();
            CurrentHealthPoints = MaxHealthPoints;
        }
    }

}