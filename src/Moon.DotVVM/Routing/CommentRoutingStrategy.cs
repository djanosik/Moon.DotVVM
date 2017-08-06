using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Routing;

namespace Moon.DotVVM.Routing
{
    /// <summary>
    /// The DotVVM routing strategy based on <c>&lt;-- Route: --&gt;</c> comments.
    /// </summary>
    public class CommentRoutingStrategy : IRoutingStrategy
    {
        private static readonly Regex routeDefinitionRegex = new Regex(@"<!--\s*Route:\s*(.*),\s*""(.*)""\s*-->", RegexOptions.Compiled);
        private readonly string appPath;

        private readonly DotvvmConfiguration config;
        private readonly DirectoryInfo viewsDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentRoutingStrategy" /> class.
        /// </summary>
        /// <param name="config">The DotVVM configuration.</param>
        /// <param name="viewFolderName">The name of the folder containing views.</param>
        public CommentRoutingStrategy(DotvvmConfiguration config, string viewFolderName = "Views")
        {
            this.config = config;

            appPath = Path.GetFullPath(config.ApplicationPhysicalPath);
            viewsDirectory = new DirectoryInfo(Path.Combine(appPath, viewFolderName));
        }

        /// <summary>
        /// Returns routes for all views discovered in the folder specified.
        /// </summary>
        public IEnumerable<RouteBase> GetRoutes()
            => DiscoverMarkupFiles().Select(BuildRoute).Where(r => r != null);

        private IEnumerable<FileInfo> DiscoverMarkupFiles()
        {
            if (viewsDirectory.Exists)
            {
                return viewsDirectory.EnumerateFiles("*.dothtml", SearchOption.AllDirectories);
            }

            throw new DotvvmRouteStrategyException("Cannot auto-discover DotVVM routes." +
                $" The directory '{viewsDirectory.Name}' was not found!");
        }

        private RouteBase BuildRoute(FileInfo markupFile)
        {
            var match = MatchRouteDefinition(markupFile);

            if (match == null)
            {
                return null;
            }

            var appRelativePath = markupFile.FullName
                .Substring(appPath.Length)
                .Replace('\\', '/')
                .TrimStart('/');

            return new DotvvmRoute(
                match.Groups[2].Value,
                appRelativePath,
                match.Groups[1].Value,
                null,
                GetPresenter, config);
        }

        private Match MatchRouteDefinition(FileInfo markupFile)
        {
            using (var input = markupFile.OpenRead())
            using (var reader = new StreamReader(input))
            {
                for (var i = 0; i < 10; i++)
                {
                    var line = reader.ReadLine();
                    var match = routeDefinitionRegex.Match(line);

                    if (match.Success)
                    {
                        return match;
                    }
                }
            }

            return null;
        }

        private DotvvmPresenter GetPresenter()
        {
            var presenter = config.RouteTable.GetDefaultPresenter();
            return (DotvvmPresenter)presenter;
        }
    }
}