using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Homebrew.Module
{
    public sealed class HomebrewPackageInstaller : IPackageInstaller
    {
        public HomebrewPackageInstaller(ICakeEnvironment environment,
            IProcessRunner processRunner,
            ICakeLog log,
            IHomebrewContentResolver contentResolver,
            ICakeConfiguration configuration)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            if (processRunner == null)
            {
                throw new ArgumentNullException(nameof(processRunner));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            if (contentResolver == null)
            {
                throw new ArgumentNullException(nameof(contentResolver));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
        }

        public bool CanInstall(PackageReference package, PackageType type) { throw new NotImplementedException(); }

        public IReadOnlyCollection<IFile> Install(PackageReference package, PackageType type, DirectoryPath path) { throw new NotImplementedException(); }
    }
}
