using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Homebrew.Module
{
    public class HomebrewContentResolver : IHomebrewContentResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;

        public HomebrewContentResolver(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
        }

        public IReadOnlyCollection<IFile> GetFiles(PackageReference package, PackageType packageType)
        {
            if (packageType == PackageType.Addin)
            {
                throw new InvalidOperationException("Homebrew Module does not support Addins'");
            }

            if (packageType == PackageType.Tool)
            {
                return GetTools(package);
            }
            
            throw new InvalidOperationException("Unkown resource type.");
        }

        private IReadOnlyCollection<IFile> GetTools(PackageReference package)
        {
            var result = new List<IFile>();
            var homebrewPath = _environment.GetEnvironmentVariable("HomebrewInstall");

            if (string.IsNullOrWhiteSpace(homebrewPath))
            {
                throw new InvalidOperationException("Homebrew is not installed.");
            }

            var toolDirectory = _fileSystem.GetDirectory("");

            if (toolDirectory.Exists)
            {
                result.AddRange(GetFiles(toolDirectory.Path.FullPath, package));
            }

            return result;
        }

        private IEnumerable<IFile> GetFiles(DirectoryPath path, PackageReference packageReference)
        {
            var collection = new FilePathCollection(new PathComparer(_environment));

            var patterns = new[] {path.FullPath + "/**/*.sh", path.FullPath + "/**/*.nuspec"};

            foreach (var pattern in patterns)
            {
                collection.Add(_globber.GetFiles(pattern));
            }
            
            // Include files.
            if (packageReference.Parameters.ContainsKey("include"))
            {
                foreach (var include in packageReference.Parameters["include"])
                {
                    var includePath = string.Concat(path.FullPath, "/", include.TrimStart('/'));
                    collection.Add(_globber.GetFiles(includePath));
                }
            }

            // Exclude files.
            if (packageReference.Parameters.ContainsKey("exclude"))
            {
                foreach (var exclude in packageReference.Parameters["exclude"])
                {
                    var excludePath = string.Concat(path.FullPath, "/", exclude.TrimStart('/'));
                    collection.Remove(_globber.GetFiles(excludePath));
                }
            }

            // Return the files.
            return collection.Select(p => _fileSystem.GetFile(p)).ToArray();
        }
    }
}
