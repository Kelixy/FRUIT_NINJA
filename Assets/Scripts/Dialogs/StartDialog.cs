using Controllers;
using DG.Tweening;

namespace Dialogs
{
    public class StartDialog : Dialog
    {
        public void PushPlayButton()
        {
            DOTween.Sequence()
                .AppendCallback(() => base.Hide())
                .AppendInterval(1)
                .AppendCallback(() => ControllersManager.Instance.GameController.StartGame());

        }
        
        public new void Show()
        {
            base.Show();
        }
    }
}