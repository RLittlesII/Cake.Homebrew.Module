using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.Composition;
using Cake.Core.Packaging;

namespace Cake.Homebrew.Module
{
    public sealed class HomebrewModule : ICakeModule
    {
        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

            registrar.RegisterType<HomebrewPackageInstaller>().As<IPackageInstaller>().Singleton();
            registrar.RegisterType<HomebrewContentResolver>().As<IHomebrewContentResolver>().Singleton();
        }
    }
}
