using System.Collections.Generic;
using System.Text;
using Gem;

namespace Choanji
{
	public class RichTextStroller 
	{
		private readonly int mMaxLen;

		public bool isSetuped { get { return orgText != null; } }
		public bool isStrollDone { get { return (length == mMaxLen) || isEnded; } }
		public bool isEnded { get { return (orgText == null) || (mIdx == orgText.Length); } }

		public string orgText { get; private set; }
		private int mIdx;

		private int mBase;
		public string text { get; private set; }
		public int length { get; private set; }

		private string mHeader = "";
		private List<string> mTagHeads = new List<string>();
		private List<string> mTagTails = new List<string>();

		public RichTextStroller(int _maxLen)
		{
			mMaxLen = _maxLen;
		}

		public bool Start(string _text)
		{
			if (isSetuped)
			{
				L.E(L.M.CALL_RETRY("Start"));
				return false;
			}

			orgText = _text;
			return true;
		}

		public void StopAndReset()
		{
#if UNITY_EDITOR
			if (!isSetuped)
			{
				L.E(L.M.CALL_RETRY("StopAndReset"));
				return;
			}
#endif

			mIdx = 0;
			orgText = null;

			mTagHeads.Clear();
			mTagTails.Clear();

			Rebase();
		}

		public bool Next()
		{
			if (!isSetuped)
			{
				L.E(L.M.STATE_INVALID);
				return false;
			}

			if (isStrollDone)
				return false;

			if (isEnded)
				return false;

			mIdx += CleanAndPutTags();
			text = BuildText(MakeRaw());
			++length;

			return true;
		}

		public bool Rebase()
		{
			mBase = mIdx;
			text = "";
			length = 0;

			mHeader = !mTagHeads.Empty() ? MakeHead(ref mTagHeads) : "";

			return true;
		}

		private int CleanAndPutTags()
		{
			var _offset = 0;

			do
			{
				var _hasTag = SplitTag(orgText.Substring(mIdx + _offset));
				if (! _hasTag.HasValue)
				{
					++_offset;
					break;
				}

				var _tag = _hasTag.Value;
				if (!_tag.end)
				{
					mTagHeads.Add(orgText.Substring(mIdx + _offset, _tag.offset));
					mTagTails.Add(_tag.tag);
				}
				else
				{
					D.Assert(!mTagHeads.Empty() && !mTagTails.Empty());
					mTagHeads.RemoveBack();
					mTagTails.RemoveBack();
				}

				_offset += _tag.offset;

			} while (true);

			return _offset;
		}

		private string MakeRaw() { return orgText.Substring(mBase, mIdx - mBase); }

		private string BuildText(string _txt)
		{
			if (string.IsNullOrEmpty(mHeader) && mTagTails.Empty())
				return _txt;

			var _builder = new StringBuilder(mMaxLen + mHeader.Length + mTagTails.Count * 5);

			if (!string.IsNullOrEmpty(mHeader))
				_builder.Append(mHeader);

			_builder.Append(_txt);

			if (!mTagTails.Empty())
				_builder.Append(MakeTail(ref mTagTails));

			return _builder.ToString();
		}

		struct Tag
		{
			public string tag;
			public int offset;
			public bool end;
		}

		private static Tag? SplitTag(string _txt)
		{
			D.Assert(!string.IsNullOrEmpty(_txt));

			if (_txt.Length < 3)
				return null;

			if (_txt[0] != '<')
				return null;

			var _isEnd = (_txt[1] == '/');
			var _tagStart = _isEnd ? 2 : 1;
			var _tagEnd = 0;

			for (var i = _tagStart; i != _txt.Length; ++i)
			{
				if (_tagEnd == 0)
				{
					if (!char.IsLower(_txt[i]))
						_tagEnd = i;
				}

				if (_txt[i] == '>')
				{
					D.Assert((_tagEnd != 0) && (_tagEnd != _tagStart));

					return new Tag
					{
						tag = _txt.Substring(_tagStart, _tagEnd - _tagStart),
						offset = i + 1,
						end = _isEnd,
					};
				}
			}

			L.E("> not found.");
			return null;
		}

		private static string MakeHead(ref List<string> _tags)
		{
			D.Assert(!_tags.Empty());
			if (_tags.Count == 1)
				return _tags[0];
			var _txt = new StringBuilder(_tags.Count * 45);
			foreach (var _tag in _tags)
				_txt.Append(_tag);
			return _txt.ToString();
		}

		private static string MakeTail(ref List<string> _tags)
		{
			D.Assert(!_tags.Empty());
			var _txt = new StringBuilder(_tags.Count * 5);
			foreach (var _tag in _tags.GetReverseEnum())
				_txt.Append("</").Append(_tag).Append('>');
			return _txt.ToString();
		}
	}

}
