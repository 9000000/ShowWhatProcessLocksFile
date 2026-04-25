using NUnit.Framework;
using ShowWhatProcessLocksFile.LockFinding.Utils;

namespace Test.LockFinding.Utils;

[TestFixture]
[Parallelizable(scope: ParallelScope.All)]
internal class PathUtilsTest
{
    [Test]
    public void AddTrailingSeparatorIfItIsAFolderTest()
    {
        Assert.That(PathUtils.AddTrailingSeparatorIfItIsAFolder(@"C:\Windows\System32"), Is.EqualTo(@"C:\Windows\System32\"));
        Assert.That(PathUtils.AddTrailingSeparatorIfItIsAFolder(@"C:\Windows\System32\"), Is.EqualTo(@"C:\Windows\System32\"));
        Assert.That(new CanonicalPath(@"C:\Windows\System32\ntdll.dll").Path, Is.EqualTo(@"C:\Windows\System32\ntdll.dll"));
    }

    [Test]
    public void CanonicalPathTest()
    {
        Assert.That(new CanonicalPath(@"C:\Windows\System32").Path, Is.EqualTo(@"C:\Windows\System32\"));
        Assert.That(new CanonicalPath(@"C:\Windows\System32\").Path, Is.EqualTo(@"C:\Windows\System32\"));
        Assert.That(new CanonicalPath(@"C:\Windows\System32\\").Path, Is.EqualTo(@"C:\Windows\System32\"));
        Assert.That(new CanonicalPath(@"C:\Windows\\System32/").Path, Is.EqualTo(@"C:\Windows\System32\"));
        Assert.That(new CanonicalPath(@"C:/Windows/System32").Path, Is.EqualTo(@"C:\Windows\System32\"));

        Assert.That(new CanonicalPath(@"C:/Windows/System32/ntdll.dll").Path, Is.EqualTo(@"C:\Windows\System32\ntdll.dll"));
        Assert.That(new CanonicalPath(@"C:/Windows/System32//ntdll.dll").Path, Is.EqualTo(@"C:\Windows\System32\ntdll.dll"));
        Assert.That(new CanonicalPath(@"C:\Windows\\System32\ntdll.dll").Path, Is.EqualTo(@"C:\Windows\System32\ntdll.dll"));
    }

    // This test requires a network share drive
    [Test]
    [Explicit]
    public void CanonicalPath_converts_network_mounts_to_UNC_path()
    {
        Assert.That(new CanonicalPath(@"Z:\var\tmp\").Path, Is.EqualTo(@"\\wsl.localhost\Ubuntu-24.04\var\tmp\"));
        Assert.That(new CanonicalPath(@"Z:\var\tmp").Path, Is.EqualTo(@"\\wsl.localhost\Ubuntu-24.04\var\tmp\"));
        Assert.That(new CanonicalPath(@"Z:\init").Path, Is.EqualTo(@"\\wsl.localhost\Ubuntu-24.04\init"));
    }

    // This test requires a mounted drive. I used Cryptomator with mounting type "WinFsp (Local Drive)"
    [Test]
    [Explicit]
    public void CanonicalPath_converts_path_on_mounted_drives()
    {
        Assert.That(new CanonicalPath(@"E:\test\test_doc.docx").Path, Is.EqualTo(@"E:\test\test_doc.docx"));
        Assert.That(new CanonicalPath(@"E:\test").Path, Is.EqualTo(@"E:\test\"));
        Assert.That(new CanonicalPath(@"E:\test\").Path, Is.EqualTo(@"E:\test\"));
    }
}
