namespace JukeBox.Mobile.Common
{
    public interface INavigable
    {
        void PreActivate(bool clearData);

        void Activate(object parameter);

        void Deactivate();
    }
}
