namespace DDF.Character.Abilities {
    public class Abilities {
        private Ability Pyrokinesis;

        public Abilities( Stats.Stats newStats ) { }

        public void Init() {
            Pyrokinesis = new Ability("Пирокинез");
        }
    }
}