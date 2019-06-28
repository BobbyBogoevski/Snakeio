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
	public class AISnake
	{
		public List<Point> body {get;set;}
		public int force {get;set;}
		public readonly int SNAKEHEAD_RADIUS=20;
		public double angle {get;set;}
		public int forceX {get;set;}
		public int forceY {get;set;}
		public int newBodyParts {get;set;}
		static Random randomizer {get;set;}
		
		public AISnake(int worldWidth, int worldHeight)
		{
			if(randomizer==null) randomizer=new Random();
			
			int X=randomizer.Next((int)(worldWidth*0.3),(int)(worldWidth*0.6));
			int Y=randomizer.Next((int)(worldHeight*0.3),(int)(worldHeight*0.6));
			body=new List<Point>();
			for(int i=0;i<20;i++){
				body.Add(new Point(X-i*10,Y));
			}
			force=10;
			newBodyParts=0;	
		}
		
		
		public void Move(){
			
			
			Point tail=new Point(body[body.Count-1].X,body[body.Count-1].Y);
			
			for(int i=body.Count-1;i>=1;i--){
				body[i]=body[i-1];
			}
			if(newBodyParts>0)
			{
				body.Add(tail);
				newBodyParts--;
			}
			
			forceX=-(int)(force*Math.Cos(angle));
			forceY=(int)(force*Math.Sin(angle));
			int posX,posY;
			if(forceX>0&&body[0].X>=int.MaxValue-forceX){ posX=forceX-(int.MaxValue-body[0].X);}
			else {posX=body[0].X+forceX;}
			
			if(forceY>0&&body[0].Y>=int.MaxValue-forceY){ posY=forceY-(int.MaxValue-body[0].Y);}
			else {posY=body[0].Y+forceY;}
			
			if(posX<0) posX=int.MaxValue+forceX+body[0].X;
			if(posY<0) posY=int.MaxValue+forceY+body[0].Y;
			
			body[0]=new Point(posX,posY);
		}
		
		
	}
}
