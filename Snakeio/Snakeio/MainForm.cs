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
using System.Windows.Forms;

namespace Snakeio
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Snake player{get;set;}
		Timer mainTimer{get;set;}
		Random randomizer {get;set;}
		List<Point> food {get;set;}
		readonly int NORMAL_FOOD_RADIUS = 5;
		List<AISnake> snakebots {get;set;}
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			DoubleBuffered=true;
			randomizer=new Random();

			mainTimer=new Timer();
			mainTimer.Interval=40;
			mainTimer.Enabled=true;
			mainTimer.Tick+=MoveSnakes;
			
			startGame();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		void generateFood()
		{
			
			int viewStartX=player.body[0].X-(Width/2)-20;
				int startY=player.body[0].Y-(Height/2)-20;
				
				int viewEndX=player.body[0].X+(Width/2)+20;
				int endY=player.body[0].Y+(Height/2)+20;
				
				
			for(int i = 0; i<20;i++){	
					Point f = new Point(randomizer.Next(viewStartX,viewEndX),randomizer.Next(startY,endY));
					if(!food.Contains(f)) food.Add(f);
			}
		}
		
		void startGame(){
			player=new Snake(int.MaxValue/2,int.MaxValue/2);
			food=new List<Point>();
			
			generateFood();
			
			mainTimer.Start();
		}
		
		void stopGame(){
			
		}

		bool isInCameraView(Point point, int viewStartX, int viewEndX, int viewStartY, int viewEndY)
		{
			if((point.X>(viewStartX-40)&&point.X<(viewEndX+40))&&(point.Y>(viewStartY-40)&&point.Y<(viewEndY+40))) return true;
			return false;
		}
		
		void MainFormPaint(object sender, PaintEventArgs e)
		{
			
			Graphics g = e.Graphics;
			g.Clear(Color.Black);
			player.Draw(g,Width,Height);
			Point cameraCenter=new Point(Width/2,Height/2);
			int viewStartX=player.body[0].X-cameraCenter.X;
			int viewStartY=player.body[0].Y-cameraCenter.Y;
			int viewEndX=player.body[0].X+cameraCenter.X;
			int viewEndY=player.body[0].Y+cameraCenter.Y;
			
			for(int i=0;i<food.Count;i++){
				if(isInCameraView(food[i],viewStartX,viewEndX,viewStartY,viewEndY))
				{
					g.FillEllipse(new SolidBrush(Color.Pink),food[i].X-viewStartX,food[i].Y-viewStartY,NORMAL_FOOD_RADIUS*2,NORMAL_FOOD_RADIUS*2);
				}
				
				else
				{
				
					food.RemoveAt(i--);
					generateNewFood(viewStartX,viewEndX,viewStartY,viewEndY);
				
				}
			}
			
		}
		
		double distance(Point p1, Point p2){
			return Math.Sqrt(Math.Pow(p1.X-p2.X,2)+Math.Pow(p1.Y-p2.Y,2));
		}

		
		void generateNewFood(int viewStartX, int viewEndX, int viewStartY, int viewEndY)
		{
				Point f;
					//south-east
					if(player.forceX>2&&player.forceX<9&&player.forceY>2&&player.forceY<9) 
					{
						int side=randomizer.Next(0,2);
						if(side==0)  
						{
							f=new Point(randomizer.Next(viewEndX+5,viewEndX+35),randomizer.Next(viewStartY,viewEndY)); //east
							
						}
						else 
						{
							f=new Point(randomizer.Next(viewStartX,viewEndX),randomizer.Next(viewEndY+5,viewEndY+35));//south
							
						}
					}
					//north-east
					else if(player.forceX>2&&player.forceX<9&&player.forceY<-2&&player.forceY>-9)
					{
						int side=randomizer.Next(0,2);
						if(side==0)  
						{
							f=new Point(randomizer.Next(viewStartX,viewEndX),randomizer.Next(viewStartY-35,viewStartY-5));//north
							
						}
						else 
						{
							f=new Point(randomizer.Next(viewEndX+5,viewEndX+35),randomizer.Next(viewStartY,viewEndY));//east
							
						}
					
					}
					//south-west
					else if(player.forceX<-2&&player.forceX>-9&&player.forceY>2&&player.forceY<9) 
					{
						int side=randomizer.Next(0,2);
						if(side==0)   
						{
							f=new Point(randomizer.Next(viewStartX,viewEndX),randomizer.Next(viewEndY+5,viewEndY+35));//south
							
						}
						else  
						{
							f=new Point(randomizer.Next(viewStartX-35,viewStartX-5),randomizer.Next(viewStartY,viewEndY));//west
							
						}
					
					}
					//north-west
					else if(player.forceX<-2&&player.forceX>-9&&player.forceY<-2&&player.forceY>-9)
					{
						int side=randomizer.Next(0,2);
						if(side==0)  
						{
							f=new Point(randomizer.Next(viewStartX,viewEndX),randomizer.Next(viewStartY-35,viewStartY-5));//north
							
						}
						else 
						{
							f=new Point(randomizer.Next(viewStartX-35,viewStartX-5),randomizer.Next(viewStartY,viewEndY));//west
							
						}
					}
					//east
					else if(player.forceX==9&&player.forceY<=4&&player.forceY>=-4) 
					{
						f=new Point(randomizer.Next(viewEndX+5,viewEndX+35),randomizer.Next(viewStartY,viewEndY));
						
					}
					//west
					else if(player.forceX==-9&&player.forceY<=4&&player.forceY>=-4) 
					{
						f=new Point(randomizer.Next(viewStartX-35,viewStartX-5),randomizer.Next(viewStartY,viewEndY));
						
					}
					//south
					else if(player.forceY==9&&player.forceX<=4&&player.forceX>=-4) 
					{
						f=new Point(randomizer.Next(viewStartX,viewEndX),randomizer.Next(viewEndY+5,viewEndY+35));
						
					}
					//north
					else 
					{
						f=new Point(randomizer.Next(viewStartX,viewEndX),randomizer.Next(viewStartY-35,viewStartY-5));
						
					}
					
					
					food.Add(f);
		}

		void MainFormMouseMove(object sender, MouseEventArgs e)
		{
			player.changeAngle(e.Location,Width,Height);
			
		}
		
		
		
		
		
		void MoveSnakes(object sender, EventArgs e)
		{
			Point cameraCenter=new Point(Width/2,Height/2);
			int viewStartX=player.body[0].X-cameraCenter.X;
			int viewStartY=player.body[0].Y-cameraCenter.Y;
			int viewEndX=player.body[0].X+cameraCenter.X;
			int viewEndY=player.body[0].Y+cameraCenter.Y;
			
			player.Move();
			for(int i=0;i<food.Count;i++){
				
				if(distance(player.body[0],food[i])<=(player.SNAKEHEAD_RADIUS+NORMAL_FOOD_RADIUS)){
					food.RemoveAt(i--);
					player.newBodyParts++;
					generateNewFood(viewStartX,viewEndX,viewStartY,viewEndY);
				}
			}
			Invalidate(true);
		}
		
		
	}
}
