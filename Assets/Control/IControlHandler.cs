using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IControlHandler
{
	/// <summary>
	/// only control with return value true will be processed
	/// </summary>
	bool Down();

	void Update();

	void Up();
}
