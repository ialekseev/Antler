namespace SmartElk.Antler.Abstractions.Configuration
{
    public class BasicConfiguration: IBasicConfiguration
    {
        public IContainer Container { get; set; }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
