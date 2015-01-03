using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct ElementInfo
{
	public string name;
	public ElementType type;
	public List<ElementType> weak;
	public List<ElementType> resist;
	public List<ElementType> immun;

	public string icon
	{
		get
		{
			throw new System.NotImplementedException();
		}
		set
		{
		}
	}

	public string iconMove
	{
		get
		{
			throw new System.NotImplementedException();
		}
		set
		{
		}
	}

	public string iconDex
	{
		get
		{
			throw new System.NotImplementedException();
		}
		set
		{
		}
	}
}
