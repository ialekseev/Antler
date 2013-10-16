namespace SmartElk.Antler.Domain
{
    public interface ISessionScopeFactoryEx
    {
        void SetSession(object session);
        void ResetSession();
    }
}
