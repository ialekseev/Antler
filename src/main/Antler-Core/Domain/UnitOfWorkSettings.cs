namespace SmartElk.Antler.Core.Domain
{
    public class UnitOfWorkSettings
    {
        public bool EnableCommit { get; set; }
        public bool RollbackOnDispose { get; set; }                

        public UnitOfWorkSettings()
        {
            EnableCommit = true;
            RollbackOnDispose = false;
        }        
                
        private static UnitOfWorkSettings _default = new UnitOfWorkSettings();
        public static UnitOfWorkSettings Default
        {
            get { return _default; }
            set { _default = value; }
        }
    }
}
