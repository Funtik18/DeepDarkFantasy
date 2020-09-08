using DDF.AI;


public class Snake_AI : AI_Entity
{
     
    protected override void myMind(){
        int hevi_dist = 80;

        if(min>hevi_dist)
        {
        }
        else
            if(min<=hevi_dist && min>50)
            {
                myanim.SetBool("Hevi_Attak", true);
            }
            else
                if(min>sparing_distance)
                {
                    myanim.SetBool("around_attack",true);
                }
                else
                {
                if(!heviatack)
                    {
                    if(!attacking)
                    {
                        attacking = true;
                        myanim.SetBool("Attak", true);
                    }
                }
            } 
    }
    public override void endAnim()
    {
        myanim.SetBool("Attak",false);
        myanim.SetBool("Hevi_Attak",false);
        myanim.SetBool("around_attack", false);
        myanim.SetBool("Voise",false);
        attacking = false;
        hiting = false;
        endbattle = false;
    }

}
