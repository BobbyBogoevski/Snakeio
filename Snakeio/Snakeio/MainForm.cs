/*
 * Created by SharpDevelop.
 * User: User
 * Date: 19/06/2019
 * Time: 20:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Snakeio
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Snake player{ get; set; }
		Timer mainTimer{ get; set; }
		Timer effectTimer {get;set;}
		Random randomizer { get; set; }
		List<Point> food { get; set; }
		readonly int NORMAL_FOOD_RADIUS = 5;
		List<AISnake> snakebots { get; set; }
		List<Point> dead_snake_food { get; set; }
		ResourceManager rm {get;set;}
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			DoubleBuffered = true;
			randomizer = new Random();
			rm = new ResourceManager("Snakeio.foodResources",Assembly.GetExecutingAssembly());
			
			initTimers();
			startGame();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		void EffectTick(object sender, EventArgs e)
		{
			
		}
		
		
		void initTimers()
		{
			mainTimer = new Timer();
			mainTimer.Interval = 40;
			mainTimer.Enabled = true;
			mainTimer.Tick += MoveSnakes;
			effectTimer = new Timer();
			effectTimer.Interval = 1000;
			effectTimer.Enabled = true;
			effectTimer.Tick += EffectTick;
		}
		
		void generateFood()
		{
			
			int viewStartX = player.body[0].X - (Width / 2) - 20;
			int startY = player.body[0].Y - (Height / 2) - 20;
				
			int viewEndX = player.body[0].X + (Width / 2) + 20;
			int endY = player.body[0].Y + (Height / 2) + 20;
				
				
			for (int i = 0; i < 20; i++) {	
				Point f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(startY, endY));
				if (!food.Contains(f))
					food.Add(f);
			}
		}
		
		void startGame()
		{
			player = new Snake(int.MaxValue / 2, int.MaxValue / 2);
			food = new List<Point>();
			snakebots = new List<AISnake>();
			dead_snake_food = new List<Point>();
			generateAISnakes();
			generateFood();
			
			mainTimer.Start();
		}
		
		void generateAISnakes()
		{
			for (int i = 0; i < 5; i++) {
				snakebots.Add(new AISnake(int.MaxValue, int.MaxValue));
			}
		}
		
		void stopGame()
		{
			mainTimer.Stop();
			MessageBox.Show(String.Format("You died. Your score: {0}", player.score), "GAME OVER");
			Close();
		}

		bool isInCameraView(Point point, int viewStartX, int viewEndX, int viewStartY, int viewEndY, int offset)
		{
			if ((point.X > (viewStartX - offset) && point.X < (viewEndX + offset)) && (point.Y > (viewStartY - offset) && point.Y < (viewEndY + offset)))
				return true;
			return false;
		}
		
		void MainFormPaint(object sender, PaintEventArgs e)
		{
			
			Graphics g = e.Graphics;
			g.Clear(Color.Black);
			player.Draw(g, Width, Height);
			Point cameraCenter = new Point(Width / 2, Height / 2);
			int viewStartX = player.body[0].X - cameraCenter.X;
			int viewStartY = player.body[0].Y - cameraCenter.Y;
			int viewEndX = player.body[0].X + cameraCenter.X;
			int viewEndY = player.body[0].Y + cameraCenter.Y;
			
			
			//Draw food
			for (int i = 0; i < food.Count; i++) {
				if (isInCameraView(food[i], viewStartX, viewEndX, viewStartY, viewEndY, 40)) {
					g.FillEllipse(new SolidBrush(Color.Pink), food[i].X - viewStartX, food[i].Y - viewStartY, NORMAL_FOOD_RADIUS * 2, NORMAL_FOOD_RADIUS * 2);
				} else {
				
					food.RemoveAt(i--);
					generateNewFood(viewStartX, viewEndX, viewStartY, viewEndY);
				
				}
			}
				
			//Draw dead snake food
			for (int i = 0; i < dead_snake_food.Count; i++) {
				if (isInCameraView(dead_snake_food[i], viewStartX, viewEndX, viewStartY, viewEndY, 80)) {
					g.FillEllipse(new SolidBrush(Color.Pink), dead_snake_food[i].X - viewStartX, dead_snake_food[i].Y - viewStartY, NORMAL_FOOD_RADIUS * 2, NORMAL_FOOD_RADIUS * 2);
				} else {
				
					dead_snake_food.RemoveAt(i--);
				}
			}
				
			//Draw AISnakes
			for (int i = 0; i < snakebots.Count; i++) {
				for (int j = 0; j < snakebots[i].body.Count; j++) {
					if (isInCameraView(snakebots[i].body[j], viewStartX, viewEndX, viewStartY, viewEndY, 40)) {
						snakebots[i].Draw(g, viewStartX, viewEndX, viewStartY, viewEndY);
						break;
					}
				}
			}
			
			
			Image img =(Bitmap) rm.GetObject("coffee");
			g.DrawImage(img,0,0,50,50);
		}
		
		
		
		
		public static double distance(Point p1, Point p2)
		{
			return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
		}

		
		void generateNewFood(int viewStartX, int viewEndX, int viewStartY, int viewEndY)
		{
			Point f;
			//south-east
			if (player.forceX > 2 && player.forceX < 9 && player.forceY > 2 && player.forceY < 9) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewEndX + 5, viewEndX + 35), randomizer.Next(viewStartY, viewEndY)); //east
							
				} else {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 5, viewEndY + 35));//south
							
				}
			}
					//north-east
					else if (player.forceX > 2 && player.forceX < 9 && player.forceY < -2 && player.forceY > -9) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 35, viewStartY - 5));//north
							
				} else {
					f = new Point(randomizer.Next(viewEndX + 5, viewEndX + 35), randomizer.Next(viewStartY, viewEndY));//east
							
				}
					
			}
					//south-west
					else if (player.forceX < -2 && player.forceX > -9 && player.forceY > 2 && player.forceY < 9) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 5, viewEndY + 35));//south
							
				} else {
					f = new Point(randomizer.Next(viewStartX - 35, viewStartX - 5), randomizer.Next(viewStartY, viewEndY));//west
							
				}
					
			}
					//north-west
					else if (player.forceX < -2 && player.forceX > -9 && player.forceY < -2 && player.forceY > -9) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 35, viewStartY - 5));//north
							
				} else {
					f = new Point(randomizer.Next(viewStartX - 35, viewStartX - 5), randomizer.Next(viewStartY, viewEndY));//west
							
				}
			}
					//east
					else if (player.forceX == 9 && player.forceY <= 4 && player.forceY >= -4) {
				f = new Point(randomizer.Next(viewEndX + 5, viewEndX + 35), randomizer.Next(viewStartY, viewEndY));
						
			}
					//west
					else if (player.forceX == -9 && player.forceY <= 4 && player.forceY >= -4) {
				f = new Point(randomizer.Next(viewStartX - 35, viewStartX - 5), randomizer.Next(viewStartY, viewEndY));
						
			}
					//south
					else if (player.forceY == 9 && player.forceX <= 4 && player.forceX >= -4) {
				f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 5, viewEndY + 35));
						
			}
					//north
					else {
				f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 35, viewStartY - 5));
						
			}
					
					
			food.Add(f);
		}

		void MainFormMouseMove(object sender, MouseEventArgs e)
		{
			player.changeAngle(e.Location, Width, Height);
			
		}

		void generateNewSnakeAI(int viewStartX, int viewEndX, int viewStartY, int viewEndY)
		{
			int side = randomizer.Next(0, 4);
			int pointX, pointY;
			switch (side) {
				case 0:
					//west
					pointX = randomizer.Next(viewStartX - 3000, viewStartX - 1000);
					pointY = randomizer.Next(viewStartY, viewEndY);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));
					break;
				
				case 1:
				//north
					pointX = randomizer.Next(viewStartX, viewEndX);
					pointY = randomizer.Next(viewStartY - 3000, viewStartY - 1000);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));
					break;
				case 2:
				//east
					pointX = randomizer.Next(viewEndX + 1000, viewEndX + 3000);
					pointY = randomizer.Next(viewStartY, viewEndY);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));
					break;
				case 3:
				//north
					pointX = randomizer.Next(viewStartX, viewEndX);
					pointY = randomizer.Next(viewEndY + 1000, viewEndY + 3000);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));
					break;					
			}
		}
		
		
		
		
		void MoveSnakes(object sender, EventArgs e)
		{
			Point cameraCenter = new Point(Width / 2, Height / 2);
			int viewStartX = player.body[0].X - cameraCenter.X;
			int viewStartY = player.body[0].Y - cameraCenter.Y;
			int viewEndX = player.body[0].X + cameraCenter.X;
			int viewEndY = player.body[0].Y + cameraCenter.Y;
			
			//move player
			player.Move();
			
			//move ai snakes
			for (int i = 0; i < snakebots.Count; i++) {
				snakebots[i].Move(food, player);
				if (distance(snakebots[i].body[0], player.body[0]) > 3000) {
					snakebots.RemoveAt(i--);
					generateNewSnakeAI(viewStartX, viewEndX, viewStartY, viewEndY);
				}
			}
			
			Invalidate(true);
			
			//Check if player hits other snakes
			for (int i = 0; i < snakebots.Count; i++) {
				for (int j = 0; j < snakebots[i].body.Count; j++) {
					if (distance(player.body[0], snakebots[i].body[j]) <= (2 * player.SNAKEHEAD_RADIUS)) {
						stopGame();
						return;
					}
				}
			}
			
			//Check if snakes hit player
			for (int i = 0; i < snakebots.Count; i++) {
				for (int j = 0; j < player.body.Count; j++) {
					if (distance(snakebots[i].body[0], player.body[j]) <= (2 * player.SNAKEHEAD_RADIUS)) {
						for (int k = 0; k < snakebots[i].body.Count; k++)
							dead_snake_food.Add(snakebots[i].body[k]);
						snakebots.RemoveAt(i--);
						generateNewSnakeAI(viewStartX, viewEndX, viewStartY, viewEndY);
						break;
					}
				}
			}
			
			//Check if ai snakes hit eachother
			
			bool snakeDead = false;
			
			for (int i = 0; i < snakebots.Count; i++) {
				snakeDead = false;
				
				for (int j = 0; j < snakebots.Count; j++) {
					
					
					if (i == j)
						continue;
					for (int l = 0; l < snakebots[j].body.Count; l++) {
						if (i < 0 || j < 0 || l < 0 || i >= snakebots.Count || j >= snakebots.Count || l >= snakebots[j].body.Count)
							break;
						if (distance(snakebots[i].body[0], snakebots[j].body[l]) <= (2 * player.SNAKEHEAD_RADIUS)) {
							for (int k = 0; k < snakebots[i].body.Count; k++)
								dead_snake_food.Add(snakebots[i].body[k]);
							snakebots.RemoveAt(i--);
							generateNewSnakeAI(viewStartX, viewEndX, viewStartY, viewEndY);
							snakeDead = true;
							break;
							
						}
						if (snakeDead)
							break;
					}
					
				}
			}
			
			
			//check if player eats food pill
			for (int i = 0; i < food.Count; i++) {
				
				if (distance(player.body[0], food[i]) <= (player.SNAKEHEAD_RADIUS + NORMAL_FOOD_RADIUS)) {
					food.RemoveAt(i--);
					player.Eat("normal");
					generateNewFood(viewStartX, viewEndX, viewStartY, viewEndY);
				}
			}
			
			//check if player eats dead snake food pill
			
			for (int i = 0; i < dead_snake_food.Count; i++) {
				
				if (distance(player.body[0], dead_snake_food[i]) <= (player.SNAKEHEAD_RADIUS + NORMAL_FOOD_RADIUS)) {
					dead_snake_food.RemoveAt(i--);
					player.Eat("normal");
				}
			}
			
			//check if snakebot eats food
			for (int s = 0; s < snakebots.Count; s++) {
				for (int i = 0; i < food.Count; i++) {
				
					if (distance(snakebots[s].body[0], food[i]) <= (player.SNAKEHEAD_RADIUS + NORMAL_FOOD_RADIUS)) {
						food.RemoveAt(i--);
						snakebots[s].Eat("normal");
						generateNewFood(viewStartX, viewEndX, viewStartY, viewEndY);
					}
				}
			}
			
			
			//check if snakebot eats dead snake food pill
			for (int s = 0; s < snakebots.Count; s++) {
				for (int i = 0; i < dead_snake_food.Count; i++) {
				
					if (distance(snakebots[s].body[0], dead_snake_food[i]) <= (player.SNAKEHEAD_RADIUS + NORMAL_FOOD_RADIUS)) {
						dead_snake_food.RemoveAt(i--);
						snakebots[s].Eat("normal");
					}
				}
			}
			
		}
		
		
	}
}
