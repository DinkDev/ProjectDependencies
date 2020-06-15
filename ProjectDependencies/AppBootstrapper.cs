namespace ProjectDependencies
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using ByteDev.DotNet.Project;
    using ByteDev.DotNet.Solution;
    using Caliburn.Micro;
    using Model;
    //using DataAccess;
    using Model.SolutionAndProjectParsing;
    using Ookii.Dialogs.Wpf;
    using Properties;
    using ViewModels;

    /// <remarks>
    /// Autofac wire up borrowed from: <see href="http://grantbyrne.com/post/settingupautofacwithcaliburnmicro/">Grant's Blog</see>
    /// </remarks>>
    public class AppBootstrapper : BootstrapperBase
    {
        private IContainer _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            // resources
            builder.RegisterType<Settings>().As<IFileSettings>() /*.As<IDataAccessSettings>() */
                .SingleInstance();
            builder.RegisterType<Crc32>();
            builder.RegisterType<DotNetSolution>().SingleInstance();
            builder.RegisterType<DotNetProject>().SingleInstance();
            builder.RegisterType<SolutionParser>().SingleInstance();
            builder.RegisterType<SolutionFileHelper>().SingleInstance();
            //builder.RegisterType<ProjectDependencyContext>();

            // VMs
            builder.RegisterType<SolutionsViewModel>().SingleInstance();
            builder.RegisterType<ShellViewModel>().As<IShell>().SingleInstance();

            // Dialogs
            builder.RegisterType<VistaFolderBrowserDialog>();

            _container = builder.Build();

            AppDomain.CurrentDomain.SetData(@"DataDirectory", Environment.CurrentDirectory);
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                if (_container.IsRegistered(service))
                {
                    return _container.Resolve(service);
                }
            }
            else if (_container.IsRegisteredWithKey(key, service))
            {
                return _container.ResolveKeyed(key, service);
            }

            var keyMessage = string.IsNullOrWhiteSpace(key) ? string.Empty : $"Key: {key}, ";
            var msg = $"Unable to find registration for {keyMessage}Service: {service.Name}.";
            throw new Exception(msg);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(service);
            return _container.Resolve(type) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}
