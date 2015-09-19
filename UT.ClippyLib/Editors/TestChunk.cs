using NUnit.Framework;
using System;
using ClippyLib;
using ClippyLib.Editors;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestChunk
	{
		private const string _chunktext = "aaaaabbbbbcccccddddde";
		[Test]
		public void CanCreateInstance ()
		{
			ChunkText chnk = new ChunkText();
			Assert.IsInstanceOf<IClipEditor>(chnk);
		}

		[Test]
		public void CanChunkAt10()
		{
			string actual = EditorTester.TestEditor(new ChunkText(), _chunktext, new []{"10","\n"});
			Assert.AreEqual("aaaaabbbbb\ncccccddddd\ne", actual);
		}

		[Ignore]
		[Test]
		public void CanChunkAt10WithDefaultSeparator()
		{
			string actual = EditorTester.TestEditor(new ChunkText(), _chunktext, "10");
			Assert.AreEqual("aaaaabbbbb\ncccccddddd\ne", actual);
		}

		[Test]
		public void CanChunkAtNonInteger()
		{
			Assert.Throws<InvalidParameterException>(
				() => {EditorTester.TestEditor(new ChunkText(), _chunktext, new []{"ten","\n"});}
				);
		}
	}
}

