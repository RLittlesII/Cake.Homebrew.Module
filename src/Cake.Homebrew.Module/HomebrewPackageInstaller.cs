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
        private readonly IProcessRunner _processRunner;
        private readonly ICakeLog _log;
        private readonly IHomebrewContentResolver _contentResolver;
        private readonly ICakeConfiguration _configuration;
        private readonly ICakeEnvironment _environment;

        public HomebrewPackageInstaller(ICakeEnvironment environment,
            IProcessRunner processRunner,
            ICakeLog log,
            IHomebrewContentResolver contentResolver,
            ICakeConfiguration configuration)
        {
            _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _contentResolver = contentResolver ?? throw new ArgumentNullException(nameof(contentResolver));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public bool CanInstall(PackageReference package, PackageType type)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            return package.Scheme.Equals("brew", StringComparison.OrdinalIgnoreCase);
        }

        public IReadOnlyCollection<IFile> Install(PackageReference package, PackageType type, DirectoryPath path)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            path = path.MakeAbsolute(_environment);

            _log.Debug("Installing Homebrew package {0}...", package.Package);

            var process = _processRunner.Start("brew",
                new ProcessSettings
                {
                    Arguments = GetArguments(package, path, _configuration),
                    RedirectStandardOutput = true,
                    Silent = _log.Verbosity < Verbosity.Diagnostic
                });

            var result = _contentResolver.GetFiles(package, type);
            if (result.Count != 0)
            {
                return result;
            }

            return result;
        }

        private ProcessArgumentBuilder GetArguments(PackageReference package,
            DirectoryPath path,
            ICakeConfiguration configuration)
        {
            var arguments = new ProcessArgumentBuilder();

            arguments.Append("install");
            arguments.AppendQuoted(package.Package);

            return arguments;
        }
    }
}
