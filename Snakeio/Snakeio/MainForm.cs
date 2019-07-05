/*
 * Created by SharpDevelop.
 * User: User
 * Date: 19/06/2019
 * Time: 20:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
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
		Timer effectTimer { get; set; }
		Random randomizer { get; set; }
		List<Point> food { get; set; }
		List<SpecialFood> special_food { get; set; }
		readonly int NORMAL_FOOD_RADIUS = 5;
		List<AISnake> snakebots { get; set; }
		List<Point> dead_snake_food { get; set; }
		ResourceManager rm { get; set; }
		Hashtable image_table { get; set; }
		
		
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			DoubleBuffered = true;
			randomizer = new Random();
			rm = new ResourceManager("Snakeio.foodResources", Assembly.GetExecutingAssembly());
			image_table = new Hashtable();
			image_table["coffee"] = (Bitmap)rm.GetObject("coffee");
			image_table["shield"] = (Bitmap)rm.GetObject("shield");
			
			initTimers();
			startGame();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		void generateNewSpecialFood(string effectName, int viewStartX, int viewEndX, int viewStartY, int viewEndY)
		{
			SpecialFood fd;
			Point f;
			double angle = (player.angle * 180 / Math.PI) + 180;
			//south-east
			if (angle <= 340 && angle >= 290) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewEndX + 1000, viewEndX + 4000), randomizer.Next(viewStartY, viewEndY)); //east
					
				} else {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 1000, viewEndY + 3000));//south
					
				}
			}
			//north-east
			else if (angle >= 20 && angle <= 70) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 3000, viewStartY - 300));//north
					
				} else {
					f = new Point(randomizer.Next(viewEndX + 300, viewEndX + 3000), randomizer.Next(viewStartY, viewEndY));//east
					
				}
				
			}
			//south-west
			else if (angle >= 200 && angle <= 250) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 300, viewEndY + 3000));//south
					
				} else {
					f = new Point(randomizer.Next(viewStartX - 3000, viewStartX - 300), randomizer.Next(viewStartY, viewEndY));//west
					
				}
				
			}
			//north-west
			else if (angle >= 110 && angle <= 160) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 3000, viewStartY - 300));//north
					
				} else {
					f = new Point(randomizer.Next(viewStartX - 3000, viewStartX - 300), randomizer.Next(viewStartY, viewEndY));//west
					
				}
			}
			//east
			else if (angle < 20 || angle > 340) {
				f = new Point(randomizer.Next(viewEndX + 300, viewEndX + 3000), randomizer.Next(viewStartY, viewEndY));
				
			}
			//west
			else if (angle > 160 && angle < 200) {
				f = new Point(randomizer.Next(viewStartX - 3000, viewStartX - 300), randomizer.Next(viewStartY, viewEndY));
				
			}
			//south
			else if (angle > 250 && angle < 290) {
				f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 300, viewEndY + 3000));
				
			}
			//north
			else {
				f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 3000, viewStartY - 300));
				
			}
			
			fd = new SpecialFood(f, effectName);
			special_food.Add(fd);
		}
		
		void EffectTick(object sender, EventArgs e)
		{
			int viewStartX = player.body[0].X - (Width / 2) - 20;
			int viewStartY = player.body[0].Y - (Height / 2) - 20;
			
			int viewEndX = player.body[0].X + (Width / 2) + 20;
			int viewEndY = player.body[0].Y + (Height / 2) + 20;
			
			for (int i = 0; i < special_food.Count; i++) {
				if (special_food[i].eaten) {
					special_food[i].duration--;
					switch (special_food[i].effectName) {
						case "coffee":
							if (special_food[i].duration <= 0)
								special_food[i].affectedSnake.force = 10;
							break;
						case "shield":
							if (special_food[i].duration > 0) {
								special_food[i].affectedSnake.invincible = true;
								if (special_food[i].duration % 2 == 0)
									special_food[i].affectedSnake.color = Color.Blue;
								else
									special_food[i].affectedSnake.color = Color.White;
							} else {
								special_food[i].affectedSnake.invincible = false;
								special_food[i].affectedSnake.color = special_food[i].affectedSnake.defColor;
							}
							;
							break;
					}
					
					if (special_food[i].affectedSnake == player && special_food[i].duration > 0)
						label1.Text = String.Format("{0}", special_food[i].duration / 10 + 1);
					else if (special_food[i].affectedSnake == player && special_food[i].duration <= 0)
						label1.Text = "";
					
					if (special_food[i].duration <= 0) {
						special_food[i].affectedSnake.hasEffect = false;
						generateNewSpecialFood(special_food[i].effectName, viewStartX, viewEndX, viewStartY, viewEndY);
						special_food.RemoveAt(i--);
						
						
					}
					
				}
				
			}
		}
		
		
		void initTimers()
		{
			mainTimer = new Timer();
			mainTimer.Interval = 40;
			mainTimer.Enabled = true;
			mainTimer.Tick += MoveSnakes;
			effectTimer = new Timer();
			effectTimer.Interval = 100;
			effectTimer.Enabled = true;
			effectTimer.Tick += EffectTick;
		}
		
		void generateFood()
		{
			
			int viewStartX = player.body[0].X - (Width / 2) - 20;
			int viewStartY = player.body[0].Y - (Height / 2) - 20;
			
			int viewEndX = player.body[0].X + (Width / 2) + 20;
			int viewEndY = player.body[0].Y + (Height / 2) + 20;
			
			
			for (int i = 0; i < 20; i++) {
				Point f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY, viewEndY));
				if (!food.Contains(f))
					food.Add(f);
			}
			
			int x, y;
			SpecialFood fd;
			
			for (int i = 0; i < 8; i++) {
				x = randomizer.Next(viewStartX - 3000, viewEndX + 3000);
				y = randomizer.Next(viewStartY - 3000, viewEndY + 3000);
				fd = new SpecialFood(new Point(x, y), "coffee");
				special_food.Add(fd);
			}
			
			for (int i = 0; i < 2; i++) {
				x = randomizer.Next(viewStartX - 5000, viewEndX + 5000);
				y = randomizer.Next(viewStartY - 5000, viewEndY + 5000);
				fd = new SpecialFood(new Point(x, y), "shield");
				special_food.Add(fd);
			}
		}
		
		void startGame()
		{
			player = new Snake(int.MaxValue / 2, int.MaxValue / 2);
			food = new List<Point>();
			snakebots = new List<AISnake>();
			dead_snake_food = new List<Point>();
			special_food = new List<SpecialFood>();
			generateAISnakes();
			generateFood();
			
			mainTimer.Start();
			//effectTimer.Start();
		}
		
		void generateAISnakes()
		{
			for (int i = 0; i < 4; i++) {
				snakebots.Add(new AISnake(int.MaxValue, int.MaxValue));
			}
		}
		
		void stopGame()
		{
			mainTimer.Stop();
			DialogResult res = MessageBox.Show(String.Format("You died. Your score: {0}. Play again?", player.score), "GAME OVER", MessageBoxButtons.YesNo);
			if (res == DialogResult.No)
				Close();
			else
				startGame();
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
			
			//Draw special food
			
			for (int i = 0; i < special_food.Count; i++) {
				switch (special_food[i].effectName) {
					case "coffee":
					case "shield":
						if (!special_food[i].eaten && isInCameraView(special_food[i].position, viewStartX, viewEndX, viewStartY, viewEndY, special_food[i].foodRadius * 2)) {
							g.DrawImage((Bitmap)image_table[special_food[i].effectName], special_food[i].position.X - viewStartX - special_food[i].foodRadius, special_food[i].position.Y - viewStartY - special_food[i].foodRadius, 2 * special_food[i].foodRadius, 2 * special_food[i].foodRadius);
						} else if (distance(player.body[0], special_food[i].position) > 5000) {
							generateNewSpecialFood(special_food[i].effectName, viewStartX, viewEndX, viewStartY, viewEndY);
							special_food.RemoveAt(i--);
						}
						;
						break;
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
			
		}
		
		
		
		
		public static double distance(Point p1, Point p2)
		{
			return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
		}

		
		void generateNewFood(int viewStartX, int viewEndX, int viewStartY, int viewEndY)
		{
			Point f;
			//south-east
			double angle = (player.angle * 180 / Math.PI) + 180;
			if (angle <= 340 && angle >= 290) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewEndX + 5, viewEndX + 35), randomizer.Next(viewStartY, viewEndY)); //east
					
				} else {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 5, viewEndY + 35));//south
					
				}
			}
			//north-east
			else if (angle >= 20 && angle <= 70) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 35, viewStartY - 5));//north
					
				} else {
					f = new Point(randomizer.Next(viewEndX + 5, viewEndX + 35), randomizer.Next(viewStartY, viewEndY));//east
					
				}
				
			}
			//south-west
			else if (angle >= 200 && angle <= 250) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewEndY + 5, viewEndY + 35));//south
					
				} else {
					f = new Point(randomizer.Next(viewStartX - 35, viewStartX - 5), randomizer.Next(viewStartY, viewEndY));//west
					
				}
				
			}
			//north-west
			else if (angle >= 110 && angle <= 160) {
				int side = randomizer.Next(0, 2);
				if (side == 0) {
					f = new Point(randomizer.Next(viewStartX, viewEndX), randomizer.Next(viewStartY - 35, viewStartY - 5));//north
					
				} else {
					f = new Point(randomizer.Next(viewStartX - 35, viewStartX - 5), randomizer.Next(viewStartY, viewEndY));//west
					
				}
			}
			//east
			else if (angle < 20 || angle > 340) {
				f = new Point(randomizer.Next(viewEndX + 5, viewEndX + 35), randomizer.Next(viewStartY, viewEndY));
				
			}
			//west
			else if (angle > 160 && angle < 200) {
				f = new Point(randomizer.Next(viewStartX - 35, viewStartX - 5), randomizer.Next(viewStartY, viewEndY));
				
			}
			//south
			else if (angle > 250 && angle < 290) {
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
			
			int pointX, pointY;
			double angle = (player.angle * 180 / Math.PI) + 180;
			int num = randomizer.Next(0, 3);
			
			//player facing east			
			if (angle < 45 || angle >= 315) {
				if (num == 0) {
					//generate north
					pointX = randomizer.Next(viewStartX, viewEndX);
					pointY = randomizer.Next(viewStartY - 3000, viewStartY - 1000);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));	
					
				} else if (num == 1) {
					//generate east
					pointX = randomizer.Next(viewEndX + 1000, viewEndX + 3000);
					pointY = randomizer.Next(viewStartY, viewEndY);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));
									
				} else {
					//generate south
					pointX = randomizer.Next(viewStartX, viewEndX);
					pointY = randomizer.Next(viewEndY + 1000, viewEndY + 3000);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));
									
				}
						
				//player facing north			
			} else if (angle < 135 && angle >= 45) {
				if (num == 0) {
					//generate west
					pointX = randomizer.Next(viewStartX - 3000, viewStartX - 1000);
					pointY = randomizer.Next(viewStartY, viewEndY);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));
							
				} else if (num == 1) {
					//generate north
					pointX = randomizer.Next(viewStartX, viewEndX);
					pointY = randomizer.Next(viewStartY - 3000, viewStartY - 1000);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));
									
				} else {
					//generate east
					pointX = randomizer.Next(viewEndX + 1000, viewEndX + 3000);
					pointY = randomizer.Next(viewStartY, viewEndY);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));
									
				}
				//player facing west
			} else if (angle < 225 && angle >= 135) {
				if (num == 0) {
					//generate south
					pointX = randomizer.Next(viewStartX, viewEndX);
					pointY = randomizer.Next(viewEndY + 1000, viewEndY + 3000);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));
						
				} else if (num == 1) {
					//generate west
					pointX = randomizer.Next(viewStartX - 3000, viewStartX - 1000);
					pointY = randomizer.Next(viewStartY, viewEndY);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));		
					
				} else {
					//generate north
					pointX = randomizer.Next(viewStartX, viewEndX);
					pointY = randomizer.Next(viewStartY - 3000, viewStartY - 1000);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));					
					
				}
				//player facing south
			} else if (angle < 315 && angle >= 225) {
				if (num == 0) {
					//generate east
					pointX = randomizer.Next(viewEndX + 1000, viewEndX + 3000);
					pointY = randomizer.Next(viewStartY, viewEndY);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));			
					
				} else if (num == 1) {
					//generate south
					pointX = randomizer.Next(viewStartX, viewEndX);
					pointY = randomizer.Next(viewEndY + 1000, viewEndY + 3000);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));		
					
				} else {
					//generate west
					pointX = randomizer.Next(viewStartX - 3000, viewStartX - 1000);
					pointY = randomizer.Next(viewStartY, viewEndY);
					snakebots.Add(new AISnake(new Point(pointX, pointY)));				
					
				}
			}
			
		}
		
		
		void destroyAISnake(int snake_index, int viewStartX, int viewEndX, int viewStartY, int viewEndY)
		{
			for (int k = 0; k < snakebots[snake_index].body.Count; k += 3)
				dead_snake_food.Add(snakebots[snake_index].body[k]);
			snakebots.RemoveAt(snake_index);
			generateNewSnakeAI(viewStartX, viewEndX, viewStartY, viewEndY);
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
				snakebots[i].Move(food, player, snakebots);
				if (distance(snakebots[i].body[0], player.body[0]) > 2000) {
					destroyAISnake(i, viewStartX, viewEndX, viewStartY, viewEndY);
					i--;
				}
			}
			
			Invalidate(true);
			
			//Check if player hits other snakes
			for (int i = 0; i < snakebots.Count; i++) {
				for (int j = 0; j < snakebots[i].body.Count; j++) {
					if (i < 0 || i >= snakebots.Count || j < 0 || j >= snakebots[i].body.Count)
						break;
					if (distance(player.body[0], snakebots[i].body[j]) <= (2 * player.SNAKEHEAD_RADIUS)) {
						if (player.invincible&&!snakebots[i].invincible) {
							destroyAISnake(i, viewStartX, viewEndX, viewStartY, viewEndY);
							i--;
							break;
						}
							stopGame();
							return;
						
					
					}
				}
			}
			
			//Check if snakes hit player
			for (int i = 0; i < snakebots.Count; i++) {
				for (int j = 0; j < player.body.Count; j++) {
					if (distance(snakebots[i].body[0], player.body[j]) <= (2 * player.SNAKEHEAD_RADIUS)) {
						if(snakebots[i].invincible){
							stopGame();
							return;
						}
						destroyAISnake(i--, viewStartX, viewEndX, viewStartY, viewEndY);
						break;
					}
				}
			}
			
			//Check if ai snakes hit eachother
			
			
			
			for (int i = 0; i < snakebots.Count; i++) {
				
				
				for (int j = 0; j < snakebots.Count; j++) {
					
					
					if (i == j)
						continue;
					for (int l = 0; l < snakebots[j].body.Count; l++) {
						if (i < 0 || j < 0 || l < 0 || i >= snakebots.Count || j >= snakebots.Count || l >= snakebots[j].body.Count)
							break;
						
						if (distance(snakebots[i].body[0], snakebots[j].body[l]) <= (2 * player.SNAKEHEAD_RADIUS)) {
							if(snakebots[i].invincible&&!snakebots[j].invincible){
								destroyAISnake(j--,viewStartX,viewEndX,viewStartY,viewEndY);
								break;
							}
							destroyAISnake(i--, viewStartX, viewEndX, viewStartY, viewEndY);
							break;
							
						}
					
					}
					
				}
			}
			
		
			
			//check if snakes eat special food
			
			for (int s = 0; s < snakebots.Count; s++) {
				for (int i = 0; i < special_food.Count; i++) {
					if (!player.hasEffect && distance(player.body[0], special_food[i].position) <= (player.SNAKEHEAD_RADIUS + special_food[i].foodRadius)) {
						special_food[i].affectedSnake = player;
						special_food[i].eaten = true;
						player.Eat(special_food[i].effectName);
						break;
					}
					if (!snakebots[s].hasEffect && distance(snakebots[s].body[0], special_food[i].position) <= (player.SNAKEHEAD_RADIUS + special_food[i].foodRadius)) {
						special_food[i].affectedSnake = snakebots[s];
						special_food[i].eaten = true;
						snakebots[s].Eat(special_food[i].effectName);
						break;
					}
				}
			}
			
			//check if snakes eat food pills
			for (int s = 0; s < snakebots.Count; s++) {
				for (int i = 0; i < food.Count; i++) {
					if (distance(player.body[0], food[i]) <= (player.SNAKEHEAD_RADIUS + NORMAL_FOOD_RADIUS)) {
						food.RemoveAt(i--);
						player.Eat("normal");
						generateNewFood(viewStartX, viewEndX, viewStartY, viewEndY);
					} else if (distance(snakebots[s].body[0], food[i]) <= (player.SNAKEHEAD_RADIUS + NORMAL_FOOD_RADIUS)) {
						food.RemoveAt(i--);
						snakebots[s].Eat("normal");
						generateNewFood(viewStartX, viewEndX, viewStartY, viewEndY);
					}
				}
			}
			
			
			//check if snakes eat dead snake food pill
			for (int s = 0; s < snakebots.Count; s++) {
				for (int i = 0; i < dead_snake_food.Count; i++) {
					if (distance(player.body[0], dead_snake_food[i]) <= (player.SNAKEHEAD_RADIUS + NORMAL_FOOD_RADIUS)) {
						dead_snake_food.RemoveAt(i--);
						player.Eat("normal");
					} else if (distance(snakebots[s].body[0], dead_snake_food[i]) <= (player.SNAKEHEAD_RADIUS + NORMAL_FOOD_RADIUS)) {
						dead_snake_food.RemoveAt(i--);
						snakebots[s].Eat("normal");
					}
				}
			}
			
		}
		
		
	}
}
