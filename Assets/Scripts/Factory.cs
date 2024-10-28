using UnityEngine;

public class Factory : Structure
{
    [SerializeField] private Recipe Recipe;
    public void craft()
    {
        foreach (var item in Recipe.InputItem)
        {

        }
        foreach (var item in Recipe.OutputItem)
        {

        }
    }

    public bool TryToCraft()
    {
        foreach (var item in Recipe.InputItem)
        {
            /*if ()
            {
                return false;
            }*/
        }
        craft();
        return true;
    }
}
