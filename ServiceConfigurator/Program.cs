using Topshelf;

namespace ServiceConfigurator
{
    class Program
    {
        static void Main()
        {
            HostFactory.Run(x =>
            {
                x.Service<WebApiService>(s =>
                {
                    s.ConstructUsing(name => new WebApiService());
                    s.WhenStarted(svc => svc.Start());
                    s.WhenStopped(svc => svc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("Служба для тонкого конфигурирования внутренних систем");
                x.SetDisplayName("Service configurator");
                x.SetServiceName("ServiceConfigurator");
            });
        }
    }
}
