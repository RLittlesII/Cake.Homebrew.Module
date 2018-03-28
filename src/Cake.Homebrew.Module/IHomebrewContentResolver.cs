using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Homebrew.Module
{

    /// <summary>
    /// Interface that represents a file locator for Homebrew packages.
    /// </summary>
    public interface IHomebrewContentResolver
    {
        /// <summary>
        /// Gets the files for a Homebrew package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="packageType">Type of the package.</param>
        /// <returns></returns>
        IReadOnlyCollection<IFile> GetFiles(PackageReference package, PackageType packageType);
    }
}
