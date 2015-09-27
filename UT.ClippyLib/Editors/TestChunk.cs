using NUnit.Framework;
using System;
using ClippyLib;
using ClippyLib.Editors;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestChunk : AEditorTester
	{
		private const string _chunktext = "aaaaabbbbbcccccddddde";

		[Test]
		public void CanChunkAt10()
		{
			WhenClipboardContains("aaaaabbbbbcccccddddde");
			AndCommandIsRan("chunk 10 \\n");
			ThenTheClipboardShouldContain("aaaaabbbbb\ncccccddddd\ne");
		}

		[Test]
		public void CanChunkAt10WithDefaultSeparator()
		{
			WhenClipboardContains("aaaaabbbbbcccccddddde");
			AndCommandIsRan("chunk 10");
			ThenTheClipboardShouldContain("aaaaabbbbb\ncccccddddd\ne");
		}

		[Test]
		public void CanChunkWithDifferentSeparator()
		{
			WhenClipboardContains("aaaaabbbbbcccccddddde");
			AndCommandIsRan("chunk 10 |");
			ThenTheClipboardShouldContain("aaaaabbbbb|cccccddddd|e");
		}

		[Test]
		public void CanFailAtNonInteger()
		{
			Assert.Throws<InvalidParameterException>(
				() => {TestEditor(new ChunkText(), _chunktext, new []{"ten","\n"});}
				);
		}
	}
}

