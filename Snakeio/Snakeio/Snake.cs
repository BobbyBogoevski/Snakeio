/*
 * Created by SharpDevelop.
 * User: User
 * Date: 19/06/2019
 * Time: 20:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Snakeio
{
	/// <summary>
	/// Description of Snake.
	/// </summary>
	public class Snake
	{
		public List<Point> body { get; set; }
		public int force { get; set; }
		public readonly int SNAKEHEAD_RADIUS = 20;
		public double angle { get; set; }
		public int forceX { get; set; }
		public int forceY { get; set; }
		public int newBodyParts { get; set; }
		public int score { get; set; }
		public bool invincible { get; set; }
		public Color defColor { get; set; }
		public Color color { get; set; }
		public bool hasEffect{ get; set; }
		
		
		public Snake()
		{
			
		}
		
		public Snake(int X, int Y)
		{
			body = new List<Point>();
			for (int i = 0; i < 20; i++) {
				body.Add(new Point(X - i * 10, Y));
			}
			force = 10;
			newBodyParts = 0;
			score = 0;
			invincible = false;
			defColor = color = Color.Green;
		}
		
		public void changeAngle(Point mousePos, int worldWidth, int worldHeight)
		{
			int centerX = (int)(worldWidth / 2);
			int centerY = (int)(worldHeight / 2);
			angle = -Math.Atan2((1d * centerY - mousePos.Y), (1d * centerX - mousePos.X));
		}

		public void Eat(String foodType)
		{
			
			switch (foodType) {
				case "normal":
					newBodyParts++;
					score++;
					break;
				case "coffee":
					hasEffect = true;
					force = 15;
					break;
				case "shield":
					hasEffect = true;
					invincible = true;
					break;
			}
		}
		
		
		public void Move()
		{
			
			
			Point tail = new Point(body[body.Count - 1].X, body[body.Count - 1].Y);
			
			for (int i = body.Count - 1; i >= 1; i--) {
				body[i] = body[i - 1];
			}
			if (newBodyParts > 0) {
				body.Add(tail);
				newBodyParts--;
			}
			
			forceX = -(int)(force * Math.Cos(angle));
			forceY = (int)(force * Math.Sin(angle));
			int posX, posY;
			if (forceX > 0 && body[0].X >= int.MaxValue - forceX) {
				posX = forceX - (int.MaxValue - body[0].X);
			} else {
				posX = body[0].X + forceX;
			}
			
			if (forceY > 0 && body[0].Y >= int.MaxValue - forceY) {
				posY = forceY - (int.MaxValue - body[0].Y);
			} else {
				posY = body[0].Y + forceY;
			}
			
			if (posX < 0)
				posX = int.MaxValue + forceX + body[0].X;
			if (posY < 0)
				posY = int.MaxValue + forceY + body[0].Y;
			
			body[0] = new Point(posX, posY);
		}
		
		public void Draw(Graphics g, int worldWidth, int worldHeight)
		{
			
			SolidBrush br = new SolidBrush(color);
			
			int centerX = (int)(worldWidth / 2);
			int centerY = (int)(worldHeight / 2);
			g.FillEllipse(br, centerX - SNAKEHEAD_RADIUS, centerY - SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS);
			double angleDeg = (angle * 180 / Math.PI) + 180;
			g.FillPie(new SolidBrush(Color.White), centerX - SNAKEHEAD_RADIUS, centerY - SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, -(float)(angleDeg + 50), 30f);
			g.FillPie(new SolidBrush(Color.White), centerX - SNAKEHEAD_RADIUS, centerY - SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, -(float)(angleDeg - 30), 30f);
			
			for (int i = 1; i < body.Count; i++) {
				int distanceX = body[i].X - body[0].X;
				int distanceY = body[i].Y - body[0].Y;
				if (distanceX > (20 * i))
					distanceX = -(int.MaxValue - body[i].X + body[0].X);
				if (distanceY > (20 * i))
					distanceY = -(int.MaxValue - body[i].Y + body[0].Y);
				if (distanceX < (-20 * i))
					distanceX = (int.MaxValue - body[0].X + body[i].X);
				if (distanceY < (-20 * i))
					distanceY = (int.MaxValue - body[0].Y + body[i].Y);
				
				Point bodypos = new Point(centerX + distanceX, centerY + distanceY);
				g.FillEllipse(br, bodypos.X - SNAKEHEAD_RADIUS, bodypos.Y - SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS);
			}
			
			
			br.Dispose();
			
		}
		
		
		public int decision_score(Point p){
			double distance=int.MaxValue;
			for(int i=0;i<body.Count;i++){
				double d=MainForm.distance(body[i],p);
				if(distance>d) distance=d;
			}
			
			return (int)(-distance)+5000;
		}
		
	}
}
