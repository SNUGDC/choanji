using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Item 
{
	private ItemIdx idx;
	private string name;
	private ItemType type;
	/// <summary>
	/// if 0, it cannot be sold
	/// </summary>
	private int price;
	private string desc;
}
