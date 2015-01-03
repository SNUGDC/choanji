using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Flags()]
public enum ItemAction
{
	USE,
	READ,
	GIVE,
	TOSS,
	REG,
	UNREG,
}
