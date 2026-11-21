using System.Text;
using static Yafc.Parser.FactorioDataSource;

namespace Yafc.Parser.Tests;

public class ModSelectionTests {
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void DistinguishesIncompatibleModFromMissingMod(bool installed) {
        var root = Directory.CreateTempSubdirectory();
        try {
            File.WriteAllText(Path.Combine(root.FullName, "mod-list.json"), """{"mods":[{"name":"test-mod","enabled":true}]}""");
            File.WriteAllText(Path.Combine(root.CreateSubdirectory("base").FullName, "info.json"), """{"name":"base","version":"2.1.24"}""");
            File.WriteAllText(Path.Combine(root.CreateSubdirectory("core").FullName, "info.json"), """{"name":"core","version":"2.1.24"}""");
            if (installed) {
                File.WriteAllText(Path.Combine(root.CreateSubdirectory("mod").FullName, "info.json"), """{"name":"test-mod","version":"0.1.0","factorio_version":"2.0"}""");
            }

            var error = Assert.Throws<NotSupportedException>(() => Parse(root.FullName, root.FullName, "", false, false,
                new Progress<(string, string)>(), new(), "en", false, false));
            string expected = installed ? I18n.LSs.ModFactorioVersionMismatch.L("test-mod", "2.0", new Version("2.1.24"))
                : I18n.LSs.ModNotFoundTryInFactorio.L("test-mod");
            Assert.Equal(expected, error.Message);
            Assert.Equal("test-mod", CurrentLoadingMod);
        }
        finally {
            CurrentLoadingMod = null;
            root.Delete(true);
        }
    }

    [Theory]
    [InlineData("2.1.1", "2.0.5", "2.0.5")]
    [InlineData("2.0.5", "2.1.1", "2.0.5")]
    [InlineData("2.1.1", "2.0.5", null)]
    [InlineData("2.0.5", "2.1.1", null)]
    [InlineData("2.1.1", "2.0.5", "1.0.0")]
    public void SelectsRequestedVersionRegardlessOfDiscoveryOrder(string first, string second, string? requested) {
        using ModInfo firstMod = CreateMod(first);
        using ModInfo secondMod = CreateMod(second);
        Version? requestedVersion = requested == null ? null : new(requested);
        ModInfo? selected = null;
        foreach (ModInfo candidate in new[] { firstMod, secondMod }) {
            if (ShouldReplaceMod(selected, candidate, requestedVersion)) {
                selected = candidate;
            }
        }
        Assert.Equal(requested == "2.0.5" ? "2.0.5" : "2.1.1", selected?.version);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void PrefersFolderForSameRequestedVersion(bool archiveFirst) {
        using ModInfo zipped = CreateMod("2.0.5", new EmptyZipArchive());
        using ModInfo folder = CreateMod("2.0.5");
        ModInfo first = archiveFirst ? zipped : folder;
        ModInfo second = archiveFirst ? folder : zipped;
        ModInfo selected = ShouldReplaceMod(first, second, new Version("2.0.5")) ? second : first;
        Assert.Same(folder, selected);
    }

    private static ModInfo CreateMod(string version, IZipArchive? archive = null) =>
        new("test-mod/", Encoding.UTF8.GetBytes($$"""{"name":"test-mod","version":"{{version}}","factorio_version":"2.0"}"""), 0, 0, archive);

    private sealed class EmptyZipArchive : IZipArchive {
        public IReadOnlyCollection<IZipEntry> Entries => [];
        public IZipEntry? GetEntry(string path) => null;
        public void Dispose() { }
    }
}
