namespace SG.Unit
{
    public interface IUnit
    {
        float Health { get; set; }
        float Damage { get; set; }

        void Init();
        void Move();
        void Attack();
        void Dead();
    }
}
