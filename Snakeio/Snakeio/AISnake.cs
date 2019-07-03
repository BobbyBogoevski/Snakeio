/*
 * Created by SharpDevelop.
 * User: User
 * Date: 28/06/2019
 * Time: 23:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Snakeio
{
	/// <summary>
	/// Description of AISnake.
	/// </summary>
	public class AISnake:Snake
	{
		int movement_action = 4;
		static Random randomizer { get; set; }
		public int count { get; set; }
		static Color[] ColorChoice = {
			Color.Red,
			Color.Blue,
			Color.Yellow,
			Color.Magenta,
			Color.Cyan,
			Color.Brown,
			Color.White
		};
		
		
		public AISnake(int worldWidth, int worldHeight)
			: base()
		{
			if (randomizer == null)
				randomizer = new Random();
			
			int X = randomizer.Next((int)(worldWidth * 0.5) - 3000, (int)(worldWidth * 0.5) + 3000);
			int Y = randomizer.Next((int)(worldHeight * 0.5) - 3000, (int)(worldHeight * 0.5) + 3000);
			body = new List<Point>();
			int length = randomizer.Next(20, 81);
			for (int i = 0; i < length; i++) {
				body.Add(new Point(X - i * 10, Y));
			}
			force = 10;
			newBodyParts = 0;
			count = randomizer.Next(0, 200);
			color = ColorChoice[randomizer.Next(0, ColorChoice.Length)];
		}
		
		public AISnake(Point p)
			: base()
		{
			if (randomizer == null)
				randomizer = new Random();
			
			body = new List<Point>();
			int length = randomizer.Next(20, 50);
			for (int i = 0; i < length; i++) {
				body.Add(new Point(p.X - i * 10, p.Y));
			}
			force = 10;
			newBodyParts = 0;
			count = randomizer.Next(0, 200);
			defColor = color = ColorChoice[randomizer.Next(0, ColorChoice.Length)];
		}
		
		public void changeAngle(Point pos)
		{
			
			angle = -Math.Atan2(1d * (body[0].Y - pos.Y), 1d * (body[0].X - pos.X));
		}
		
		
		public void Move(List<Point> food, Snake player, List<AISnake> snakes)
		{
			if (count == 0) {
				if (movement_action > 4)
					movement_action = 4;
				double min_dist = double.MaxValue;
				int md_index = -1;
				
				for (int i = 0; i < food.Count; i++) {
					double dist = MainForm.distance(body[0], food[i]);
					if (dist < min_dist) {
						min_dist = dist;
						md_index = i;
					}
				}
				
				changeAngle(food[md_index]);
				
				
				for (int i = 0; i < snakes.Count; i++) {
					if (this == snakes[i])
						continue;
					for (int j = 0; j < snakes[i].body.Count; j++) {
						if (MainForm.distance(body[0], snakes[i].body[j]) < 150) {
							changeAngle(snakes[i].body[j]);
							angle = -angle;
							movement_action = 10;
							break;
						}
					}
				}
				
				for (int i = 0; i < player.body.Count; i++) {
					if (MainForm.distance(body[0], player.body[i]) < 150) {
						changeAngle(player.body[i]);
						angle = -angle;
						movement_action = 10;
						break;
					}
				}
				
				
				
			}
			
			
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
			
			count = (count + 1) % movement_action;
		}
		
		public void Draw(Graphics g, int viewStartX, int viewEndX, int viewStartY, int viewEndY)
		{
			double angleDeg = (angle * 180 / Math.PI) + 180;
			
			g.FillEllipse(new SolidBrush(color), body[0].X - viewStartX, body[0].Y - viewStartY, SNAKEHEAD_RADIUS * 2, SNAKEHEAD_RADIUS * 2);
			if (color != Color.White) {
				g.FillPie(new SolidBrush(Color.White), body[0].X - viewStartX, body[0].Y - viewStartY, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, -(float)(angleDeg + 50), 30f);
				g.FillPie(new SolidBrush(Color.White), body[0].X - viewStartX, body[0].Y - viewStartY, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, -(float)(angleDeg - 30), 30f);
			} else {
				g.FillPie(new SolidBrush(Color.Black), body[0].X - viewStartX, body[0].Y - viewStartY, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, -(float)(angleDeg + 50), 30f);
				g.FillPie(new SolidBrush(Color.Black), body[0].X - viewStartX, body[0].Y - viewStartY, 2 * SNAKEHEAD_RADIUS, 2 * SNAKEHEAD_RADIUS, -(float)(angleDeg - 30), 30f);
			
			}
			
			for (int i = 1; i < body.Count; i++) {
				g.FillEllipse(new SolidBrush(color), body[i].X - viewStartX, body[i].Y - viewStartY, SNAKEHEAD_RADIUS * 2, SNAKEHEAD_RADIUS * 2);
				
			}
		}
		
	}
	
	
}