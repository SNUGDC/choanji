public class MapMeta
{
	/// <summary>
	/// enable day/night tinting, hidden move Fly.
	/// </summary>
	public bool outdoor;
	/// <summary>
	/// </summary>
	private bool? mBicycle;
	/// <summary>
	/// The position on the region map where this map is located.
	/// </summary>
	public Coor? regionMapPosition;
	/// <summary>
	/// If this is TRUE, a location signpost stating the map's name will be displayed at the top left of the screen when it is entered.
	/// </summary>
	public bool showMilestone = false;
	public DiscreteSampler<WeatherType> weather;
	public bool cameraBounded = false;
	public BGMList wildBattleBGM;
	public BGMList trainerBattleBGM;
	public BGMList wildVictoryBGM;
	public BGMList trainerVictoryBGM;

	/// <summary>
	/// the bicycle can be used on this map.
	/// if null, only in outdoor map bicycle can be used.
	/// </summary>
	public bool bicycle
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