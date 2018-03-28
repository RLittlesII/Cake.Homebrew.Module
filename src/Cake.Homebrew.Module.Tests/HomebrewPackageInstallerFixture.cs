using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Testing;
using NSubstitute;

namespace Cake.Homebrew.Module.Tests
{
    internal sealed class HomebrewPackageInstallerFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IFileSystem FileSystem { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IHomebrewContentResolver ContentResolver { get; set; }
        public ICakeLog Log { get; set; }
        public PackageReference Package { get; set; }
        public PackageType PackageType { get; set; }
        public DirectoryPath InstallPath { get; set; }
        public ICakeConfiguration Config { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomebrewPackageInstallerFixture"/> class.
        /// </summary>
        internal HomebrewPackageInstallerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ContentResolver = Substitute.For<IHomebrewContentResolver>();
            Log = new FakeLog();
            Config = Substitute.For<ICakeConfiguration>();
            Package = new PackageReference("brew:?package=fastlane");
            PackageType = PackageType.Addin;
            InstallPath = new DirectoryPath("./homebrew");
        }

        /// <summary>
        /// Creates the installer.
        /// </summary>
        /// <returns></returns>
        internal HomebrewPackageInstaller CreateInstaller()
        {
            return new HomebrewPackageInstaller(Environment, ProcessRunner, Log, ContentResolver, Config);
        }

        /// <summary>
        /// Installs the specified package resource and the given location.
        /// </summary>
        /// <returns></returns>
        internal IReadOnlyCollection<IFile> Install()
        {
            var installer = CreateInstaller();
            return installer.Install(Package, PackageType, InstallPath);
        }

        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can install; otherwise, <c>false</c>.
        /// </returns>
        internal bool CanInstall()
        {
            var installer = CreateInstaller();
            return installer.CanInstall(Package, PackageType);
        }
    }
}
