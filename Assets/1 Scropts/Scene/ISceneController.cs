public interface ISceneController
{
    string Name { get; }
    void OnEnter();
    void OnExit();
    void OnPause();
    void  OnResume();
}