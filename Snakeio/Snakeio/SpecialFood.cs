/*
 * Created by SharpDevelop.
 * User: User
 * Date: 02/07/2019
 * Time: 19:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;

namespace Snakeio
{
	/// <summary>
	/// Description of SpecialFood.
	/// </summary>
	public class SpecialFood
	{
		public string effectName { get; set; }
		public int foodRadius { get; set; }
		public Snake affectedSnake { get; set; }
		public Point position { get; set; }
		public bool eaten { get; set; }
		public int duration { get; set; }
		
		
		
		public SpecialFood(Point position, String effectName)
		{
			this.position = position;
			this.effectName = effectName;
			foodRadius = 25;
			affectedSnake = null;
			eaten = false;
			
			switch (effectName) {
				case "coffee":
					duration = 80;
					break;
				case "shield":
					duration = 100;
					break;
			}
		}
		
		
		
		
	}
}
