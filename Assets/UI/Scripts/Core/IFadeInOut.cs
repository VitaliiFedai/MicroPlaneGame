using System.Threading.Tasks;


namespace CustomUI
{
    public interface IFadeInOut
    {
        public Task FadeIn(float duration);
        public Task FadeOut(float duration);
    } 
}